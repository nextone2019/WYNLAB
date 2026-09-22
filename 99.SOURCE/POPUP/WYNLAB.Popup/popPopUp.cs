using System.Data;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Popup;

/// <summary>
/// PopupLookupEditWyn의 "..." 버튼이 여는 공용 팝업 - 엔티티(부서/품목/거래처...)마다 별도
/// 폼 클래스를 만들지 않고, sysPopUpM/sysPopUpD에 저장된 정의(api/lookups/{key}/definition)를
/// 읽어서 그 내용대로 자기 자신을 그린다. hierarchical_yn에 따라 트리 또는 그리드로 그려지고,
/// 컬럼은 sysPopUpD 설정(캡션/타입/폭/순서)을 그대로 반영한다 - 새 팝업을 추가하거나 컬럼을
/// 하나 더 보여주고 싶을 때 이 클래스는 전혀 손댈 필요가 없다(설계 배경은 프로젝트 메모리
/// project_wynlab_popup_lookup_framework 참고).
///
/// frmMenu.cs와 같은 이유로 Wyn 래퍼 컨트롤(GridControlWyn 등) 대신 순정 DevExpress 컨트롤을
/// 코드에서 직접 만든다 - 전부 런타임에 코드로만 구성되는 폼이라 VS 디자이너 연동이 필요 없다.
/// </summary>
public class popPopUp : XtraForm
{
    private readonly PopupDefinitionDto _def;
    private readonly Dictionary<string, BaseEdit> _searchControls = new();
    private readonly GridControl grid = new();
    private readonly GridView gridView = new();
    // 순정 TreeList 대신 TreeListWyn(메뉴관리 frmMenu.menuTree와 같은 컨트롤)을 써서, 이 팝업의
    // 데이터에도 "MenuType"/"MenuLevel" 필드가 있으면(P_MENU 등) 모듈/그룹/leaf 배경색 구분을
    // 그대로 물려받는다(2026-09-16 요청 - "메뉴 팝업의 트리도 우리가 만든 트리 디자인으로").
    // 그 필드가 없는 일반 팝업(부서 등)은 TreeListWyn 쪽 가드가 알아서 건너뛰어 영향 없다.
    private readonly TreeListWyn tree = new();
    private DataTable _data = new();

    public PopupLookupResult? SelectedResult { get; private set; }

    private popPopUp(PopupDefinitionDto def, string? initialKeyword, DataTable? preloadedData)
    {
        _def = def;

        // 컨트롤 5벌을 한꺼번에 Controls.Add하는 동안 매번 레이아웃을 다시 계산하면, 폼이 아직
        // CenterParent로 자리잡기 전의 위치(또는 크기)로 한 번 그려졌다가 마지막에야 제 위치로
        // 정리되는 게 사용자 눈에 보일 수 있다 - "팝업이 열릴 때 폼이 두 개 떴다가 하나가 닫히는
        // 것처럼 보인다"는 지적(2026-09-06)의 원인으로 지목. SuspendLayout으로 묶어서 모든 컨트롤
        // 배치가 끝난 뒤 한 번만 레이아웃/페인트가 일어나게 한다.
        SuspendLayout();

        Text = def.PopupNm;
        Width = def.PopupWidth;
        Height = def.PopupHeight;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowIcon = false;

        // 추가 순서가 곧 세로 배치다(Dock=Top끼리는 나중에 Controls.Add된 쪽이 가장자리(맨
        // 위)에 온다) - 위에서부터 타이틀바 / 조회조건(테두리 있는 PanelWyn) / 작은 여백 /
        // 본문(그리드·트리) 순으로 보이게 하려고, 실제로는 그 반대 순서로 추가한다
        // (2026-09-06 요청 - "모든 팝업폼의 디자인을 통일화" - frmEmp 등 업무화면의
        // BuildScreenHeader 타이틀바와 같은 모양으로 맞춤).
        BuildContent();
        BuildFooter();
        Controls.Add(BuildSpacer());
        BuildSearchPanel(initialKeyword);
        Controls.Add(BuildTitleBar());

        ResumeLayout(false);

        if (preloadedData != null)
        {
            _data = preloadedData;
            Load += (s, e) => BindData();
        }
        else
        {
            Load += async (s, e) => await SearchAsync();
        }
    }

    /// <summary>정의(sysPopUpM/D)를 서버에서 받아와 팝업을 띄우고, 사용자가 고른 행을 돌려준다.
    /// 취소하거나 팝업 정의를 못 찾으면 null. initialKeyword를 주면(PopupLookupEditWyn의 멀티필드
    /// 모드가 Leave 시 정확히 하나로 못 좁혔을 때) 조회조건 입력창들을 그 값으로 미리 채우고
    /// 뜨자마자 그 값으로 자동 조회한다. WYNLAB.Base.ControlDataSources가 이 메서드를
    /// PopupLookupProvider.OpenPopup으로 등록해서, PopupLookupEditWyn은 이 클래스 이름조차
    /// 몰라도 된다(반대 방향 참조 금지 컨벤션 유지).
    ///
    /// 폼을 만들기 전에 먼저 한 번 조회해본다 - initialKeyword로 좁혀지든(멀티필드 Leave) 조건
    /// 없이 전체가 나오든, 결과가 정확히 1건이면 사용자가 고를 이유가 없으므로 팝업 자체를 아예
    /// 띄우지 않고 바로 그 한 건을 돌려준다(2026-09-09 요청 - "데이터가 한건 밖에 없으면 팝업이
    /// 뜨는 과정 자체를 생략해야 한다". 실제로 겪은 사례: "Upper Menu"에 "조직" 입력 시 일치하는
    /// 행이 "조직관리" 1건뿐인데도, OnLeaveAsync의 exact-match 체크는 "조직" != "조직관리"라 통과
    /// 못 하고 팝업이 떴었다 - 여기서 한 번 더, 이번엔 문자열 일치가 아니라 "결과가 1건인지"만으로
    /// 판단하므로 그 경우도 잡힌다). 프리페치가 실패하거나 2건 이상/0건이면 평소대로 폼을 띄우고,
    /// 그 안에서도(BindData) 조회조건을 좁혀 다시 1건이 되면 같은 규칙이 한 번 더 적용된다.</summary>
    public static async Task<PopupLookupResult?> ShowAsync(string popupKey, Control owner, string? initialKeyword)
    {
        var def = await ApiClient.GetAsync<PopupDefinitionDto>($"api/lookups/{Uri.EscapeDataString(popupKey)}/definition");
        if (def == null)
        {
            AppMessageBox.Show($"등록되지 않은 팝업입니다: {popupKey}", "확인");
            return null;
        }

        DataTable? preData = null;
        try
        {
            preData = await SearchRowsAsync(def, BuildInitialConditions(def, initialKeyword));
        }
        catch
        {
            preData = null; // 프리페치 실패는 무시 - 폼을 정상적으로 띄우면 Load에서 SearchAsync가
                             // 같은 조건으로 다시 시도하고, 그래도 실패하면 그 안에서 에러를 보여준다.
        }

        if (preData != null && preData.Rows.Count == 1)
        {
            var result = BuildResult(def, RowToDict(preData, preData.Rows[0]));
            if (result != null) return result;
            // key_field 설정 오류 등으로 자동선택을 못 하면 아래로 흘려보내 평소대로 팝업을 띄운다.
        }

        using var form = new popPopUp(def, initialKeyword, preData);
        var ownerForm = owner.FindForm();
        var result2 = ownerForm != null ? form.ShowDialog(ownerForm) : form.ShowDialog();
        return result2 == DialogResult.OK ? form.SelectedResult : null;
    }

    /// <summary>BuildSearchPanel이 initialKeyword를 채워 넣는 것과 똑같은 규칙(정렬순 첫 번째
    /// TEXT 조회조건에만) - 폼을 만들기 전 프리페치 조회에도 같은 조건을 넣어야 결과가 일치한다.</summary>
    private static Dictionary<string, string?> BuildInitialConditions(PopupDefinitionDto def, string? initialKeyword)
    {
        var fields = def.SearchFields.OrderBy(f => f.Sort).ToList();
        var conditions = fields.ToDictionary(f => f.ParamNm, f => (string?)null);
        if (!string.IsNullOrEmpty(initialKeyword))
        {
            var firstTextField = fields.FirstOrDefault(f => f.ControlType != "DATE");
            if (firstTextField != null) conditions[firstTextField.ParamNm] = initialKeyword;
        }
        return conditions;
    }

    private static async Task<DataTable> SearchRowsAsync(PopupDefinitionDto def, Dictionary<string, string?> conditions)
    {
        var response = await ApiClient.PostAsync<Dictionary<string, string?>, DataQueryResponse>(
            $"api/lookups/{Uri.EscapeDataString(def.PopupKey)}/search", conditions);
        return response?.Tables.Count > 0 ? ProcData.ToDataTable(response.Tables[0]) : new DataTable();
    }

    private static Dictionary<string, string?> RowToDict(DataTable data, DataRow row) =>
        data.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => row[c.ColumnName] == DBNull.Value ? null : Convert.ToString(row[c.ColumnName]));

    /// <summary>선택된(또는 자동선택된) 한 행의 전체 컬럼값으로 PopupLookupResult를 만든다 -
    /// code가 비어있으면(sysPopUpM.key_field 설정 오류) 사용자에게 알리고 null을 돌려준다.</summary>
    private static PopupLookupResult? BuildResult(PopupDefinitionDto def, Dictionary<string, string?> row)
    {
        var code = row.TryGetValue(def.KeyField, out var codeVal) ? codeVal : null;
        var display = row.TryGetValue(def.DisplayField, out var displayVal) ? displayVal : null;

        // 행을 실제로 골랐는데도(또는 자동선택했는데도) code가 비어있으면 거의 항상
        // sysPopUpM.key_field 설정 실수다(예: P_EMP가 key_field=''로 등록돼 있던 사고 - 컬럼명이
        // 안 맞아 row에서 못 찾음). 원인을 바로 알 수 있게 메시지로 알려준다.
        if (string.IsNullOrEmpty(code))
        {
            AppMessageBox.Show(
                $"이 팝업의 키 컬럼({def.KeyField}) 값을 찾을 수 없습니다.\n메뉴등록의 팝업 설정(key_field)을 확인해주세요.",
                "확인");
            return null;
        }

        return new PopupLookupResult { Code = code!, Display = display ?? string.Empty, Row = row };
    }

    /// <summary>업무화면(BaseForm.BuildScreenHeader)과 똑같은 모양의 타이틀바 - 아이콘 +
    /// 굵은 제목(팝업명) + 회색 코드([팝업키]) + 아래쪽 1px 구분선, 흰 배경. popPopUp은
    /// BaseForm을 상속하지 않아(클래스 설명 참고) 그 protected 메서드를 그대로 못 불러서
    /// 같은 시각 결과를 여기 직접 다시 구성한다 - "목록" 같은 별도 섹션 제목은 팝업에는 안 둔다
    /// (2026-09-06 요청 - 조회조건 패널과 살짝 띄우는 정도면 충분).</summary>
    private Panel BuildTitleBar()
    {
        var header = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.White };
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        header.Controls.Add(bottomBorder);

        var icon = new PictureBox
        {
            Image = MenuIconPainters.Render(MenuIconPainters.Folder, 16, Color.FromArgb(120, 124, 132)),
            SizeMode = PictureBoxSizeMode.CenterImage,
            Location = new Point(14, 4),
            Size = new Size(20, 20),
            BackColor = Color.Transparent
        };

        var lblTitle = new LabelControl
        {
            Text = _def.PopupNm,
            Location = new Point(38, 6),
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = Color.FromArgb(55, 55, 55);

        var lblCode = new LabelControl
        {
            Text = $"[{_def.PopupKey}]",
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblCode.Appearance.Font = AppFonts.Caption;
        lblCode.Appearance.ForeColor = Color.FromArgb(150, 150, 150);

        header.Controls.Add(icon);
        header.Controls.Add(lblTitle);
        header.Controls.Add(lblCode);
        header.Layout += (s, e) => lblCode.Location = new Point(lblTitle.Right + 8, 8);
        lblCode.Location = new Point(lblTitle.Right + 8, 8);

        return header;
    }

    /// <summary>조회조건 패널과 본문(그리드/트리) 사이의 작은 여백 - frmEmp 같은 업무화면의
    /// "목록" 섹션 제목 자리에 해당하지만, 팝업에는 그 제목까지는 필요 없어서 여백만 둔다.</summary>
    private static Panel BuildSpacer() => new() { Dock = DockStyle.Top, Height = 8, BackColor = Color.White };

    /// <summary>조회조건은 팝업마다 개수/파라미터명이 전부 다르다(sysPopUpS, frmSysPopup의
    /// "컬럼생성"이 프로시저 파라미터를 읽어서 채워준 것을 관리자가 직접 손본 결과) - 그래서
    /// 고정된 검색창 하나가 아니라 정의된 개수만큼 라벨+입력창을 왼쪽부터 순서대로 늘어놓는다.
    /// DATE 타입은 DateEdit, 그 외는 TextEdit. 조회조건이 하나도 없으면(아직 설정 전) 검색줄
    /// 자체가 안 보인다.</summary>
    private void BuildSearchPanel(string? initialKeyword)
    {
        var fields = _def.SearchFields.OrderBy(f => f.Sort).ToList();
        if (fields.Count == 0) return;

        // PanelWyn 기본 스타일(Style=None)이 곧 DevExpress PanelControl 기본 테두리라, 이거
        // 하나로 본문(흰 배경, 테두리 없음)과 구분되는 경계가 생긴다(2026-09-06 요청 - "조회조건은
        // 판넬의 보더를 default로 해서 구분").
        var panel = new PanelWyn { Dock = DockStyle.Top, Height = 40 };
        var x = 10;
        var isFirstTextField = true;

        foreach (var field in fields)
        {
            var lbl = new LabelControl { Text = field.Caption, Location = new Point(x, 13), AutoSize = true };
            panel.Controls.Add(lbl);
            x += lbl.Width + 6;

            BaseEdit edit = field.ControlType == "DATE" ? new DateEdit() : new TextEdit();
            edit.Location = new Point(x, 9);
            edit.Size = new Size(field.Width > 0 ? field.Width : 120, 20);
            // PopupLookupEditWyn의 멀티필드 모드가 Leave 시 정확히 하나로 못 좁혔을 때, 방금
            // 타이핑한 값을 여기 다시 안 치게 미리 채워준다 - 단, sort 순서상 "맨 앞"(대표 조회
            // 조건, 보통 코드/명 통합검색) 칸 하나에만 채운다. 예전엔 조회조건 전부(부서코드/
            // 부서명 등)에 같은 값을 채웠는데, 그 필드들은 AND로 묶여서 "이름=박 그리고
            // 부서코드=박 그리고 부서명=박"이 되어 버려 실제로는 매칭될 리 없는 조건이 되고
            // 결과가 0건으로 나왔다(P_EMP 팝업에서 실제로 겪음, 2026-08-31). DATE 입력창은
            // 문자열을 그대로 넣으면 타입이 안 맞으므로 애초에 대상에서 제외.
            if (isFirstTextField && !string.IsNullOrEmpty(initialKeyword) && edit is TextEdit)
            {
                edit.EditValue = initialKeyword;
                isFirstTextField = false;
            }
            edit.KeyDown += async (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.Handled = true;
                await SearchAsync();
            };
            panel.Controls.Add(edit);
            _searchControls[field.ParamNm] = edit;

            x += edit.Width + 16;
        }

        Controls.Add(panel);
    }

    /// <summary>선택/취소(우측 정렬)는 그대로 두고, 조회(좌측)는 별도로 추가한다 - 지금까지는
    /// 검색창에서 Enter를 치거나 팝업이 뜰 때(Load) 자동조회되는 것뿐이라, 검색조건을 입력한
    /// 뒤 마우스로 누를 수 있는 버튼이 아예 없었다(사장님 피드백, 2026-08-31) - Enter를 안 치고
    /// 다른 필드로 탭 이동만 해도 조회가 안 되는 게 답답하다는 지적과 같은 문제.</summary>
    private void BuildFooter()
    {
        var panel = new Panel { Dock = DockStyle.Bottom, Height = 44 };
        var btnQuery = new SimpleButton { Text = "조회", Size = new Size(84, 30), Location = new Point(12, 7) };
        var btnCancel = new SimpleButton { Text = "취소", Size = new Size(84, 30) };
        var btnOk = new SimpleButton { Text = "선택", Size = new Size(84, 30) };

        btnQuery.Click += async (s, e) => await SearchAsync();
        btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        btnOk.Click += (s, e) => Accept();

        void Position()
        {
            btnCancel.Location = new Point(panel.Width - btnCancel.Width - 12, 7);
            btnOk.Location = new Point(btnCancel.Left - btnOk.Width - 8, 7);
        }
        panel.Resize += (s, e) => Position();

        panel.Controls.Add(btnQuery);
        panel.Controls.Add(btnOk);
        panel.Controls.Add(btnCancel);
        Controls.Add(panel);
        Position();
    }

    private void BuildContent()
    {
        var visibleColumns = _def.Columns.Where(c => c.VisibleYn).OrderBy(c => c.Sort).ToList();

        if (_def.HierarchicalYn)
        {
            // DevExpress 컴포넌트는 BeginInit/EndInit 구간을 안 거치면 내부 초기화가 덜 끝난 채로
            // 남는 경우가 있다(LookUpColumnEdit.EndInit 주석 참고 - 이 팀이 이미 겪은 같은 종류의
            // 버그: Designer가 자동으로 넣어주는 이 구간을, 전부 런타임 코드로만 구성되는 이 폼은
            // 지금까지 빠뜨리고 있었다). 네이티브 컬럼 헤더가 이 폼에서만 유독 안 뜨던 문제
            // (project_wynlab_popup_lookup_framework 메모리의 "알려진 미해결 이슈")의 유력한 원인으로
            // 보고 2026-09-16 재시도 - BeginInit/EndInit로 감싸고 네이티브 헤더(ShowColumns=true)를
            // 되살렸다. 그동안 써온 "트리 위에 직접 그린 라벨 줄" 우회는 제거.
            ((System.ComponentModel.ISupportInitialize)tree).BeginInit();

            tree.Dock = DockStyle.Fill;
            tree.KeyFieldName = _def.KeyField;
            tree.ParentFieldName = _def.ParentField;
            tree.OptionsBehavior.Editable = false;
            tree.OptionsView.ShowColumns = true;
            // ShowIndicator/RowHeight/Appearance.Row.Font는 TreeListWyn 생성자가 이미 같은 값으로
            // 잡아준다 - 중복 설정 안 함. ShowHorzLines/ShowVertLines는 TreeListWyn 기본값(true,
            // 2026-09-14 요청)과 달리 이 팝업은 계속 선 없는 모양을 쓰므로 명시적으로 꺼둔다.
            tree.OptionsView.ShowHorzLines = false;
            tree.OptionsView.ShowVertLines = false;
            // frmDept.tree1과 같은 헤더 모양(굵게, 회색 배경) - GridViewWynBehavior가 그리드
            // 헤더에 주는 톤과 통일한다.
            tree.Appearance.HeaderPanel.Font = AppFonts.BodyBold;
            tree.Appearance.HeaderPanel.Options.UseFont = true;
            tree.Appearance.HeaderPanel.BackColor = UiTheme.GridHeaderBackColor;
            tree.Appearance.HeaderPanel.Options.UseBackColor = true;
            tree.Appearance.HeaderPanel.ForeColor = UiTheme.GridHeaderForeColor;
            tree.Appearance.HeaderPanel.Options.UseForeColor = true;
            // Appearance.Row.Font는 TreeListWyn 생성자가 이미 잡아주지만, ForeColor는 안 건드리므로
            // 이 팝업의 원래 검정 글자색은 그대로 유지한다(MenuType이 있는 데이터는 NodeCellStyle이
            // 행마다 이 값 위에 그룹/leaf 색을 덮어 씌운다 - TreeListWyn 클래스 설명 참고).
            tree.Appearance.Row.ForeColor = Color.Black;
            tree.Appearance.Row.Options.UseForeColor = true;
            tree.DoubleClick += (s, e) => Accept();

            var visibleIndex = 0;
            foreach (var col in visibleColumns)
            {
                var column = tree.Columns.AddField(col.ColumnNm);
                column.Caption = col.Caption;
                column.Width = col.Width;
                column.Visible = true;
                // AddField만으로는 VisibleIndex가 안 잡혀서(기본 -1) 컬럼이 있어도 화면엔 하나도
                // 안 보였다(실제로 겪음) - frmMenu.colMenuNm처럼 명시적으로 순서를 지정해야 한다.
                column.VisibleIndex = visibleIndex++;
            }

            ((System.ComponentModel.ISupportInitialize)tree).EndInit();

            Controls.Add(tree);
        }
        else
        {
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView).BeginInit();

            grid.Dock = DockStyle.Fill;
            grid.MainView = gridView;
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView.OptionsView.ShowGroupPanel = false;
            gridView.RowHeight = 24;
            gridView.Appearance.Row.Font = AppFonts.Body;
            gridView.Appearance.Row.Options.UseFont = true;
            gridView.Appearance.Row.ForeColor = Color.Black;
            gridView.Appearance.Row.Options.UseForeColor = true;
            gridView.Appearance.HeaderPanel.Font = AppFonts.BodyBold;
            gridView.Appearance.HeaderPanel.Options.UseFont = true;
            gridView.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView.Appearance.HeaderPanel.Options.UseForeColor = true;
            gridView.DoubleClick += (s, e) => Accept();

            var visibleIndex = 0;
            foreach (var col in visibleColumns)
            {
                var column = gridView.Columns.AddField(col.ColumnNm);
                column.Caption = col.Caption;
                column.Width = col.Width;
                column.Visible = true;
                // AddField만으로는 VisibleIndex가 안 잡혀서(기본 -1) 컬럼이 있어도 화면엔 하나도
                // 안 보이는 문제가 있다(popPopUp 트리 쪽에서 실제로 겪음) - 그리드도 동일하게 방지.
                column.VisibleIndex = visibleIndex++;
                if (col.ControlType == "DATE") column.ColumnEdit = new RepositoryItemDateEdit();
                // LOOKUP 컬럼(다른 프로시저로 표시값 치환)은 후속 작업 - 지금은 원본 값 그대로 표시.
            }

            ((System.ComponentModel.ISupportInitialize)gridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();

            Controls.Add(grid);
        }
    }

    /// <summary>이 폼은 ShowDialog()로 뜨는 모달이라 자체 중첩 메시지 루프를 돈다 - 그 안에서
    /// 비동기 이어달리기 중 예외가 나면 Program.cs의 Application.ThreadException(메인 메시지
    /// 루프 훅)을 안 타고 조용히 사라질 수 있다(실제로 겪음 - 검색이 실패해도 그냥 빈 그리드로
    /// 보였다). 그래서 여기서 직접 잡아 보여준다.</summary>
    private async Task SearchAsync()
    {
        try
        {
            var conditions = _searchControls.ToDictionary(kv => kv.Key, kv => ExtractValue(kv.Value));
            _data = await SearchRowsAsync(_def, conditions);
            BindData();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"조회 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>_data를 그리드/트리에 바인딩한다. 조회 결과가 정확히 1건이면 사용자가 고를 이유가
    /// 없으므로 그 한 건을 바로 선택된 것으로 처리하고 폼을 닫는다(2026-09-09 요청 - 폼이 이미
    /// 떠 있는 상태에서 조회조건을 좁혀(Enter/조회버튼) 1건이 되는 경우를 위한 것. 폼을 아예 안
    /// 띄우는 최초 관문은 ShowAsync 쪽 프리페치이고, 여기는 그걸 통과해 폼이 열린 뒤에 다시
    /// 좁혀지는 경우를 커버한다).</summary>
    private void BindData()
    {
        if (_def.HierarchicalYn)
        {
            tree.DataSource = _data;
            tree.ExpandAll();
        }
        else
        {
            grid.DataSource = _data;
        }

        if (_data.Rows.Count == 1)
            AcceptRow(RowToDict(_data, _data.Rows[0]));
    }

    /// <summary>.Text가 아니라 EditValue를 쓴다 - 아무것도 입력 안 한 빈 상태에서 .Text를 읽으면
    /// 실제 값(빈 문자열/null)이 아니라 Properties.NullText(플레이스홀더)가 그대로 돌아온다
    /// (이 폼이 예전에 실제로 겪은 문제 - PopupLookupEditWyn 쪽에서도 같은 이유로 EditValue를 쓴다).</summary>
    private static string? ExtractValue(BaseEdit edit) => edit switch
    {
        DateEdit d => d.EditValue is DateTime dt ? dt.ToString("yyyy-MM-dd") : null,
        _ => edit.EditValue as string
    };

    /// <summary>선택된 행의 전체 컬럼값을 Row에 담는다(코드/명 2개뿐 아니라, 그 결과셋의 모든
    /// 컬럼) - PopupLookupEditWyn의 멀티필드 모드(MapField)가 코드/명 외의 임의 컬럼(예:
    /// par_dept_cd)도 다른 컨트롤에 채워 넣을 수 있어야 하기 때문이다. _data.Columns 기준으로
    /// 뽑으므로, sysPopUpD에서 화면에 안 보이게(visible_yn='N') 해둔 컬럼도 포함된다 - 그리드/
    /// 트리에 보이는 것과 실제로 매핑에 쓸 수 있는 컬럼은 별개다.</summary>
    private void Accept()
    {
        Dictionary<string, string?> row;

        if (_def.HierarchicalYn)
        {
            var node = tree.FocusedNode;
            if (node == null) return;
            row = _data.Columns.Cast<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => (string?)Convert.ToString(node.GetValue(c.ColumnName)));
        }
        else
        {
            var handle = gridView.FocusedRowHandle;
            if (handle < 0) return;
            var dataRow = gridView.GetDataRow(handle);
            if (dataRow == null) return;
            row = RowToDict(_data, dataRow);
        }

        AcceptRow(row);
    }

    private void AcceptRow(Dictionary<string, string?> row)
    {
        var result = BuildResult(_def, row);
        if (result == null) return; // 에러 메시지는 BuildResult가 이미 보여줬다 - 폼은 그대로 열어둔다.

        SelectedResult = result;
        DialogResult = DialogResult.OK;
        Close();
    }
}
