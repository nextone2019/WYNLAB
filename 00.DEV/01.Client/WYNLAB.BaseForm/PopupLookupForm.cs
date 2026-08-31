using System.Data;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

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
public class PopupLookupForm : XtraForm
{
    private readonly PopupDefinitionDto _def;
    private readonly Dictionary<string, BaseEdit> _searchControls = new();
    private readonly GridControl grid = new();
    private readonly GridView gridView = new();
    private readonly TreeList tree = new();
    private DataTable _data = new();

    public PopupLookupResult? SelectedResult { get; private set; }

    private PopupLookupForm(PopupDefinitionDto def, string? initialKeyword)
    {
        _def = def;

        Text = def.PopupNm;
        Width = def.PopupWidth;
        Height = def.PopupHeight;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowIcon = false;

        // BuildContent를 먼저 불러야(Dock=Top인 트리 헤더 라벨 줄이 있으면 그것까지) 검색창
        // (BuildSearchPanel, 역시 Dock=Top)이 나중에 추가되어 항상 맨 위에 온다 - WinForms Dock
        // 규칙상 같은 Dock=Top끼리는 나중에 Controls.Add된 쪽이 가장자리(맨 위)에 온다.
        BuildContent();
        BuildFooter();
        BuildSearchPanel(initialKeyword);

        Load += async (s, e) => await SearchAsync();
    }

    /// <summary>정의(sysPopUpM/D)를 서버에서 받아와 팝업을 띄우고, 사용자가 고른 행을 돌려준다.
    /// 취소하거나 팝업 정의를 못 찾으면 null. initialKeyword를 주면(PopupLookupEditWyn의 멀티필드
    /// 모드가 Leave 시 정확히 하나로 못 좁혔을 때) 조회조건 입력창들을 그 값으로 미리 채우고
    /// 뜨자마자 그 값으로 자동 조회한다. WYNLAB.Base.ControlDataSources가 이 메서드를
    /// PopupLookupProvider.OpenPopup으로 등록해서, PopupLookupEditWyn은 이 클래스 이름조차
    /// 몰라도 된다(반대 방향 참조 금지 컨벤션 유지).</summary>
    public static async Task<PopupLookupResult?> ShowAsync(string popupKey, Control owner, string? initialKeyword)
    {
        var def = await ApiClient.GetAsync<PopupDefinitionDto>($"api/lookups/{Uri.EscapeDataString(popupKey)}/definition");
        if (def == null)
        {
            AppMessageBox.Show($"등록되지 않은 팝업입니다: {popupKey}", "확인");
            return null;
        }

        using var form = new PopupLookupForm(def, initialKeyword);
        var ownerForm = owner.FindForm();
        var result = ownerForm != null ? form.ShowDialog(ownerForm) : form.ShowDialog();
        return result == DialogResult.OK ? form.SelectedResult : null;
    }

    /// <summary>조회조건은 팝업마다 개수/파라미터명이 전부 다르다(sysPopUpS, frmSysPopup의
    /// "컬럼생성"이 프로시저 파라미터를 읽어서 채워준 것을 관리자가 직접 손본 결과) - 그래서
    /// 고정된 검색창 하나가 아니라 정의된 개수만큼 라벨+입력창을 왼쪽부터 순서대로 늘어놓는다.
    /// DATE 타입은 DateEdit, 그 외는 TextEdit. 조회조건이 하나도 없으면(아직 설정 전) 검색줄
    /// 자체가 안 보인다.</summary>
    private void BuildSearchPanel(string? initialKeyword)
    {
        var fields = _def.SearchFields.OrderBy(f => f.Sort).ToList();
        if (fields.Count == 0) return;

        var panel = new Panel { Dock = DockStyle.Top, Height = 40 };
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
            tree.Dock = DockStyle.Fill;
            tree.KeyFieldName = _def.KeyField;
            tree.ParentFieldName = _def.ParentField;
            tree.OptionsBehavior.Editable = false;
            tree.OptionsView.ShowIndicator = false;
            // DevExpress 자체 컬럼 헤더가 이 폼(런타임에 전부 코드로 구성)에서 끝내 안 떠서
            // (ShowColumns/ColumnHeaderAutoHeight/Appearance.HeaderPanel Font+ForeColor+
            // UseXxx까지 다 맞춰봤는데도 실패, 진단 로그로도 컬럼 설정 자체는 정상 확인함 -
            // project_wynlab_popup_lookup_framework 메모리 참고) 네이티브 헤더를 아예 끄고
            // (frmMenu.menuTree와 같은 상태) 그 대신 트리 위에 직접 그린 라벨 줄(headerPanel,
            // 아래)을 얹는 우회 방식으로 바꿨다.
            tree.OptionsView.ShowColumns = false;
            tree.OptionsView.ShowHorzLines = false;
            tree.OptionsView.ShowVertLines = false;
            // 기본 행높이가 아주 빡빡해서 셀 텍스트가 잘려 안 보이는 것처럼 보인다(frmMenu의
            // menuTree와 같은 이유로 명시적으로 넉넉하게 잡는다).
            tree.RowHeight = 26;
            tree.Appearance.Row.Font = AppFonts.Body;
            tree.Appearance.Row.Options.UseFont = true;
            tree.Appearance.Row.ForeColor = Color.Black;
            tree.Appearance.Row.Options.UseForeColor = true;
            tree.DoubleClick += (s, e) => Accept();

            // 수동 헤더 줄 - 각 라벨의 X좌표를 컬럼 폭 누적으로 맞춘다. 첫 컬럼만 트리의
            // 펼침아이콘/들여쓰기 자리(indentWidth)만큼 밀어서 최상위 행 기준으로 대략 맞춘다 -
            // 하위 노드는 더 들여써지므로 완벽히 안 맞을 수 있는데, 헤더 자체가 아예 안 보이던
            // 것보다는 훨씬 낫다.
            const int indentWidth = 24;
            var headerPanel = new Panel { Dock = DockStyle.Top, Height = 24, BackColor = UiTheme.GridHeaderBackColor };
            var x = indentWidth;

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

                var lbl = new LabelControl { Text = col.Caption, Location = new Point(x, 5), Size = new Size(col.Width, 15) };
                lbl.Appearance.Font = AppFonts.BodyBold;
                lbl.Appearance.Options.UseFont = true;
                lbl.Appearance.ForeColor = UiTheme.GridHeaderForeColor;
                lbl.Appearance.Options.UseForeColor = true;
                headerPanel.Controls.Add(lbl);
                x += col.Width;
            }

            Controls.Add(tree);
            Controls.Add(headerPanel);
        }
        else
        {
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
                // 안 보이는 문제가 있다(PopupLookupForm 트리 쪽에서 실제로 겪음) - 그리드도 동일하게 방지.
                column.VisibleIndex = visibleIndex++;
                if (col.ControlType == "DATE") column.ColumnEdit = new RepositoryItemDateEdit();
                // LOOKUP 컬럼(다른 프로시저로 표시값 치환)은 후속 작업 - 지금은 원본 값 그대로 표시.
            }

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

            var response = await ApiClient.PostAsync<Dictionary<string, string?>, DataQueryResponse>(
                $"api/lookups/{Uri.EscapeDataString(_def.PopupKey)}/search", conditions);

            _data = response?.Tables.Count > 0 ? ProcData.ToDataTable(response.Tables[0]) : new DataTable();

            if (_def.HierarchicalYn)
            {
                tree.DataSource = _data;
                tree.ExpandAll();
            }
            else
            {
                grid.DataSource = _data;
            }
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"조회 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
            row = _data.Columns.Cast<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => dataRow[c.ColumnName] == DBNull.Value ? null : Convert.ToString(dataRow[c.ColumnName]));
        }

        var code = row.TryGetValue(_def.KeyField, out var codeVal) ? codeVal : null;
        var display = row.TryGetValue(_def.DisplayField, out var displayVal) ? displayVal : null;

        // 행을 실제로 골랐는데도 code가 비어있으면 거의 항상 sysPopUpM.key_field 설정 실수다
        // (예: P_EMP가 key_field=''로 등록돼 있던 사고 - 컬럼명이 안 맞아 row에서 못 찾음).
        // 예전엔 여기서 그냥 return해서 더블클릭/선택 버튼이 아무 반응 없는 것처럼 보였다 -
        // 원인을 바로 알 수 있게 메시지로 알려준다.
        if (string.IsNullOrEmpty(code))
        {
            AppMessageBox.Show(
                $"이 팝업의 키 컬럼({_def.KeyField}) 값을 찾을 수 없습니다.\n메뉴등록의 팝업 설정(key_field)을 확인해주세요.",
                "확인");
            return;
        }

        SelectedResult = new PopupLookupResult { Code = code!, Display = display ?? string.Empty, Row = row };
        DialogResult = DialogResult.OK;
        Close();
    }
}
