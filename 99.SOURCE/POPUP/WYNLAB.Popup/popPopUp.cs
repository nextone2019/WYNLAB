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

    // sysPopUpM.search_panel_class로 지정된 전용 검색패널(PopupSearchPanelBase) - 있으면 자동 생성
    // 검색창(_searchControls) 대신 이걸로 조건을 읽는다.
    private readonly PopupSearchPanelBase? _customPanel;

    public PopupLookupResult? SelectedResult { get; private set; }

    // 다중 선택 모드(ShowMultiAsync) - 그리드에 체크박스 열이 생기고, 체크한 행들(없으면 현재 행 1건)이 SelectedResults로 나간다.
    private readonly bool _multi;
    public List<PopupLookupResult> SelectedResults { get; } = new();

    private popPopUp(PopupDefinitionDto def, string? initialKeyword, DataTable? preloadedData, PopupSearchPanelBase? customPanel, bool multi = false)
    {
        _def = def;
        _multi = multi;
        _customPanel = customPanel;

        // 컨트롤 5벌을 한꺼번에 Controls.Add하는 동안 매번 레이아웃을 다시 계산하면, 폼이 아직
        // CenterParent로 자리잡기 전의 위치(또는 크기)로 한 번 그려졌다가 마지막에야 제 위치로
        // 정리되는 게 사용자 눈에 보일 수 있다 - "팝업이 열릴 때 폼이 두 개 떴다가 하나가 닫히는
        // 것처럼 보인다"는 지적(2026-09-06)의 원인으로 지목. SuspendLayout으로 묶어서 모든 컨트롤
        // 배치가 끝난 뒤 한 번만 레이아웃/페인트가 일어나게 한다.
        SuspendLayout();

        // Text를 OS 기본 제목표시줄에 그대로 주면, 바로 아래(BuildTitleBar)에서 아이콘+굵은
        // 이름+회색 [팝업키]로 사실상 같은 내용을 또 한 번 보여주는 커스텀 헤더가 붙는다 -
        // 제목표시줄 두 개가 같은 글자를 담고 위아래로 겹쳐 보여서 "팝업이 두 번 뜨는 것처럼
        // 부자연스럽다"는 지적(2026-09-23)의 실제 원인이었다. 여러 번 타이밍(레이아웃/데이터
        // 바인딩 순서)을 고쳐봐도 안 없어졌던 이유가 이거였다 - 타이밍 문제가 아니라애초에 헤더가
        // 시각적으로 두 벌이었다. OS 제목표시줄 자체(드래그로 옮기는 용도)는 남기되 텍스트/버튼은
        // 비워서 커스텀 헤더 하나만 실제 내용을 보여주게 한다.
        Text = string.Empty;
        ControlBox = false;
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
            // Load 이벤트까지 미루지 않고 생성자에서 바로 그리드/트리에 데이터를 채운다.
            // ShowAsync가 이미 이 데이터를 미리 조회해둔 상태로 폼을 만드는 경우인데(정확히
            // 1건이면 ShowAsync가 폼 자체를 안 만들고 여기까지 안 옴 - BindData의 "1건이면
            // 자동선택" 재확인은 그래서 여기선 절대 안 걸린다), Load에서 채우면 ShowDialog가
            // 창을 화면에 이미 한 번 보여준 뒤(빈 그리드) 그 다음에야 데이터가 채워져 보였다 -
            // "빈 창이 먼저 뜨고 다시 그려지는 것 같다"는 지적(2026-09-23)과 정확히 일치한다.
            // 여기서 미리 채워두면 ShowDialog가 창을 처음 그릴 때 이미 완성된 모습이다.
            _data = preloadedData;
            BindData();
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
    // 이 팝업을 여는 진입점은 항상 이 메서드 하나뿐이다(PopupLookupProvider.OpenPopup으로
    // 등록됨) - ShowDialog()로 실제 모달이 뜨기 전에 definition/프리페치 조회(await) 구간이
    // 있어서, 더블클릭처럼 짧은 시간 안에 두 번 클릭되면 첫 호출이 아직 그 await 구간(아직
    // ShowDialog 전이라 모달이 아직 없음)에 있는 사이 두 번째 호출이 또 들어와 팝업이 두 개
    // 뜨는 문제가 있었다(2026-09-23 실제 발견 - "더블클릭할때 실수로 한번더 누르면 팝업창이
    // 두개가 떠"). 앱 전체에서 이 팝업은 한 번에 하나만 뜨면 되므로, static 플래그로 이미 진행
    // 중인 호출이 있으면 새 호출은 조용히 무시한다.
    private static bool _isShowing;

    public static async Task<PopupLookupResult?> ShowAsync(string popupKey, Control owner, string? initialKeyword)
    {
        if (_isShowing) return null;
        _isShowing = true;
        try
        {
            var def = await ApiClient.GetAsync<PopupDefinitionDto>($"api/lookups/{Uri.EscapeDataString(popupKey)}/definition");
            if (def == null)
            {
                AppMessageBox.Show($"등록되지 않은 팝업입니다: {popupKey}", "확인");
                return null;
            }

            // 전용 검색패널이 지정된 팝업이면 패널을 먼저 만들어 초기 검색어를 채우고, 프리페치도 그
            // 패널이 읽어주는 조건으로 한다(자동 생성 검색창이 아니라서 sysPopUpS로는 조건을 알 수 없다).
            var customPanel = TryCreateCustomPanel(def);
            customPanel?.SetInitialKeyword(initialKeyword);
            customPanel?.ApplyExtraConditions(PopupLookupProvider.ExtraConditions);

            DataTable? preData = null;
            try
            {
                preData = await SearchRowsAsync(def, customPanel != null
                    ? customPanel.GetConditions()
                    : BuildInitialConditions(def, initialKeyword));
            }
            catch
            {
                preData = null; // 프리페치 실패는 무시 - 폼을 정상적으로 띄우면 Load에서 SearchAsync가
                                 // 같은 조건으로 다시 시도하고, 그래도 실패하면 그 안에서 에러를 보여준다.
            }

            if (preData != null && preData.Rows.Count == 1)
            {
                var result = BuildResult(def, RowToDict(preData, preData.Rows[0]));
                if (result != null)
                {
                    customPanel?.Dispose();
                    return result;
                }
                // key_field 설정 오류 등으로 자동선택을 못 하면 아래로 흘려보내 평소대로 팝업을 띄운다.
            }

            using var form = new popPopUp(def, initialKeyword, preData, customPanel);
            var ownerForm = owner.FindForm();
            var result2 = ownerForm != null ? form.ShowDialog(ownerForm) : form.ShowDialog();
            return result2 == DialogResult.OK ? form.SelectedResult : null;
        }
        finally
        {
            _isShowing = false;
        }
    }

    /// <summary>여러 건을 한 번에 고르는 팝업(2026-10-03) - 같은 팝업 정의(sysPopUpM/D)를 그대로 쓰되 그리드 맨 앞에 체크박스 열이 생긴다.
    /// 체크한 행들을 순서대로 돌려주고(아무것도 체크 안 했으면 현재 행 1건), 취소하면 null. 이미 담은 품목을 또 골라도 막지 않는다 - 호출한
    /// 화면이 고른 만큼 행을 추가한다. 조회 결과가 1건이어도 자동 선택하지 않는다(체크하고 [선택]을 눌러야 한다). 트리형 팝업은 지원하지 않는다.</summary>
    public static async Task<List<PopupLookupResult>?> ShowMultiAsync(string popupKey, Control owner)
    {
        if (_isShowing) return null;
        _isShowing = true;
        try
        {
            var def = await ApiClient.GetAsync<PopupDefinitionDto>($"api/lookups/{Uri.EscapeDataString(popupKey)}/definition");
            if (def == null)
            {
                AppMessageBox.Show($"등록되지 않은 팝업입니다: {popupKey}", "확인");
                return null;
            }
            if (def.HierarchicalYn)
            {
                AppMessageBox.Show($"트리형 팝업({popupKey})은 여러 건 선택을 지원하지 않습니다.", "확인");
                return null;
            }

            var customPanel = TryCreateCustomPanel(def);
            customPanel?.ApplyExtraConditions(PopupLookupProvider.ExtraConditions);

            using var form = new popPopUp(def, null, null, customPanel, multi: true);
            var ownerForm = owner.FindForm();
            var result = ownerForm != null ? form.ShowDialog(ownerForm) : form.ShowDialog();
            return result == DialogResult.OK ? form.SelectedResults : null;
        }
        finally
        {
            _isShowing = false;
        }
    }
    /// <summary>sysPopUpM.search_panel_class(예: "WYNLAB.Popup.pnlItemSearch")로 지정된 전용 검색패널을
    /// 만들고, 조회조건의 컨트롤 연결(sysPopUpS.control_nm)을 넘겨준다. 못 찾거나 만들다 실패하면
    /// 안내 후 null(자동 생성 검색창으로 계속) - 팝업이 아예 안 열리는 것보다 기본 검색창이라도 뜨는 편이
    /// 낫다.</summary>
    private static PopupSearchPanelBase? TryCreateCustomPanel(PopupDefinitionDto def)
    {
        var panel = PopupSearchPanelBase.TryCreate(def.SearchPanelClass, out var error);
        if (panel == null)
        {
            if (error != null) AppMessageBox.Show($"{error}\n기본 검색창으로 엽니다.", "확인");
            return null;
        }

        panel.SetMappings(def.SearchFields
            .Where(f => !string.IsNullOrWhiteSpace(f.ControlNm))
            .Select(f => new KeyValuePair<string, string>(f.ParamNm, f.ControlNm!.Trim())));
        return panel;
    }

    /// <summary>BuildSearchPanel이 initialKeyword를 채워 넣는 것과 똑같은 규칙(정렬순 첫 번째
    /// TEXT 조회조건에만) - 폼을 만들기 전 프리페치 조회에도 같은 조건을 넣어야 결과가 일치한다.</summary>
    private static Dictionary<string, string?> BuildInitialConditions(PopupDefinitionDto def, string? initialKeyword)
    {
        var fields = def.SearchFields.OrderBy(f => f.Sort).ToList();
        var conditions = fields.ToDictionary(f => f.ParamNm, f => (string?)null);
        if (!string.IsNullOrEmpty(initialKeyword))
        {
            var firstTextField = fields.FirstOrDefault(f => f.ControlType is not "DATE" and not "LOOKUP");
            if (firstTextField != null) conditions[firstTextField.ParamNm] = initialKeyword;
        }
        return conditions;
    }

    private static async Task<DataTable> SearchRowsAsync(PopupDefinitionDto def, Dictionary<string, string?> conditions)
    {
        // 팝업을 연 화면이 넘긴 추가 조건(PopupLookupProvider.ExtraConditions) - 조회조건에 같은 키가 없거나 비어 있을 때만 넣는다.
        if (PopupLookupProvider.ExtraConditions is { } extra)
            foreach (var kv in extra)
                if (!conditions.TryGetValue(kv.Key, out var current) || string.IsNullOrEmpty(current)) conditions[kv.Key] = kv.Value;

        var response = await ApiClient.PostAsync<Dictionary<string, string?>, DataQueryResponse>(
            $"api/lookups/{Uri.EscapeDataString(def.PopupKey)}/search", conditions);
        return response?.Tables.Count > 0 ? ProcData.ToDataTable(response.Tables[0]) : new DataTable();
    }

    // StringComparer.OrdinalIgnoreCase가 핵심이다 - sysPopUpM.key_field에 등록된 대소문자와
    // 실제 프로시저가 돌려주는 컬럼명의 대소문자가 다르면(예: P_ITEM은 key_field='ITEM_ID'인데
    // SSP_POP_ITEM_Q는 소스 테이블 그대로 'item_id'로 내려줌) 기본(대소문자 구분) Dictionary는
    // TryGetValue를 못 찾아 "키 컬럼 값을 찾을 수 없습니다" 오류가 뜨고 선택이 아예 씹힌다
    // (2026-09-23 실제 발견 - "그리드의 품목 팝업이 연결 되어있지 않아"). 대소문자 무시로
    // 바꾸면 이 클래스의 정의(key_field/display_field 표기)에 실제 컬럼명이 대소문자까지
    // 정확히 일치할 필요가 없어져서, 이런 종류의 등록 실수가 이 팝업 하나만이 아니라 전체에서
    // 재발하지 않는다.
    private static Dictionary<string, string?> RowToDict(DataTable data, DataRow row) =>
        data.Columns.Cast<DataColumn>().ToDictionary(
            c => c.ColumnName,
            c => row[c.ColumnName] == DBNull.Value ? null : Convert.ToString(row[c.ColumnName]),
            StringComparer.OrdinalIgnoreCase);

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

        // 제목표시줄 오른쪽 끝의 닫기(X) 버튼 - 취소와 같다(2026-09-25 요청). 마우스를 올리면 빨갛게.
        var btnClose = new Label
        {
            Text = "✕",
            Dock = DockStyle.Right,
            Width = 40,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI Symbol", 10F),
            ForeColor = Color.FromArgb(110, 110, 110),
            BackColor = Color.White,
            Cursor = Cursors.Hand
        };
        btnClose.MouseEnter += (s, e) => { btnClose.BackColor = Color.FromArgb(232, 17, 35); btnClose.ForeColor = Color.White; };
        btnClose.MouseLeave += (s, e) => { btnClose.BackColor = Color.White; btnClose.ForeColor = Color.FromArgb(110, 110, 110); };
        btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        header.Controls.Add(btnClose);

        // OS 제목표시줄은 ControlBox=false + 빈 Text라 잡을 자리가 사실상 없다 - 눈에 보이는 이 커스텀 헤더를
        // 제목표시줄처럼 끌어서 창을 옮길 수 있게 한다(2026-09-25 요청). 표준 트릭: 마우스를 놓고 "제목표시줄을
        // 눌렀다"는 메시지(WM_NCLBUTTONDOWN/HTCAPTION)를 폼에 보내면 Windows가 창 이동을 그대로 처리한다.
        foreach (Control c in new Control[] { header, icon, lblTitle, lblCode })
            c.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            };

        return header;
    }

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 2;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    /// <summary>조회조건 패널과 본문(그리드/트리) 사이의 작은 여백 - frmEmp 같은 업무화면의
    /// "목록" 섹션 제목 자리에 해당하지만, 팝업에는 그 제목까지는 필요 없어서 여백만 둔다.</summary>
    private static Panel BuildSpacer() => new() { Dock = DockStyle.Top, Height = 8, BackColor = Color.White };

    /// <summary>조회조건은 팝업마다 개수/파라미터명이 전부 다르다(sysPopUpS, frmSysPopup의
    /// "컬럼생성"이 프로시저 파라미터를 읽어서 채워준 것을 관리자가 직접 손본 결과) - 그래서
    /// 고정된 검색창 하나가 아니라 정의된 개수만큼 라벨+입력창을 왼쪽부터 순서대로 늘어놓는다.
    /// DATE는 DateEdit, LOOKUP은 LookUpEditWyn(field.LookupKey - sysLookupM 콤보 재사용), 그 외는
    /// TextEdit. 조회조건이 하나도 없으면(아직 설정 전) 검색줄 자체가 안 보인다.
    ///
    /// RowNo로 여러 줄에 나눠 배치한다(2026-09-23 요청 - "조회 조건을 한줄로 밖에 표현이
    /// 안되는데") - 같은 RowNo끼리는 Sort 순으로 왼쪽부터, RowNo가 다르면 줄을 바꾼다. 기존
    /// 팝업은 전부 row_no=1(기본값)이라 지금까지와 똑같이 한 줄로 보인다.</summary>
    private void BuildSearchPanel(string? initialKeyword)
    {
        if (_customPanel != null)
        {
            // 전용 패널은 자기 높이 그대로 - 자동 생성 검색창과 같은 테두리 패널 안에 담는다. 초기 검색어는
            // ShowAsync가 이미 채웠다.
            // 호스트 테두리가 안쪽 영역을 조금 깎으므로 여유(+6)를 준다 - 안 그러면 마지막 줄이 아래에서 잘린다.
            var host = new PanelWyn { Dock = DockStyle.Top, Height = _customPanel.Height + 6 };
            _customPanel.Dock = DockStyle.Fill;
            _customPanel.Initialize(() => _ = SearchAsync(fromUser: true));
            host.Controls.Add(_customPanel);                  // Fill이 먼저(맨 앞), 가장자리 도킹은 그 뒤에 추가한다
            host.Controls.Add(CreateQueryButtonHost(_customPanel.BackColor));
            Controls.Add(host);
            return;
        }

        var fields = _def.SearchFields.OrderBy(f => f.RowNo).ThenBy(f => f.Sort).ToList();
        if (fields.Count == 0) return;

        var rowNumbers = fields.Select(f => f.RowNo).Distinct().OrderBy(r => r).ToList();
        const int rowHeight = 40;

        // PanelWyn 기본 스타일(Style=None)이 곧 DevExpress PanelControl 기본 테두리라, 이거
        // 하나로 본문(흰 배경, 테두리 없음)과 구분되는 경계가 생긴다(2026-09-06 요청 - "조회조건은
        // 판넬의 보더를 default로 해서 구분").
        var panel = new PanelWyn { Dock = DockStyle.Top, Height = rowHeight * rowNumbers.Count };
        var isFirstTextField = true;

        // 줄이 여러 개면 같은 순번(왼쪽부터 n번째) 칸끼리 라벨 폭/입력칸 폭을 맞춰 세로로 열이 정렬되게 한다(2026-10-05 요청 - 품목 팝업의
        // 1줄 품번/품명·자산구분과 2줄 품목그룹이 들쭉날쭉했다). 한 줄뿐인 팝업은 지금과 똑같이 보인다.
        var labels = fields.ToDictionary(f => f, f => new LabelControl { Text = f.Caption, AutoSize = true });
        var colLabelW = new List<int>();
        var colEditW = new List<int>();
        foreach (var rowNo in rowNumbers)
        {
            var idx = 0;
            foreach (var field in fields.Where(f => f.RowNo == rowNo))
            {
                var ew = field.Width > 0 ? field.Width : 120;
                if (idx == colLabelW.Count) { colLabelW.Add(labels[field].Width); colEditW.Add(ew); }
                else { colLabelW[idx] = Math.Max(colLabelW[idx], labels[field].Width); colEditW[idx] = Math.Max(colEditW[idx], ew); }
                idx++;
            }
        }

        foreach (var rowNo in rowNumbers)
        {
            var y = rowNumbers.IndexOf(rowNo) * rowHeight;
            var x = 10;
            var col = 0;

            foreach (var field in fields.Where(f => f.RowNo == rowNo))
            {
                var lbl = labels[field];
                lbl.Location = new Point(x, y + 13);
                panel.Controls.Add(lbl);
                x += colLabelW[col] + 6;

                BaseEdit edit = field.ControlType switch
                {
                    "DATE" => new DateEdit(),
                    "LOOKUP" => new LookUpEditWyn { LookupKey = field.LookupKey },
                    _ => new TextEdit()
                };
                edit.Location = new Point(x, y + 9);
                edit.Size = new Size(colEditW[col], 20);
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
                // 팝업을 연 컨트롤이 넘긴 조건(PopupConditions/ConditionProvider)과 같은 이름의 LOOKUP 칸은 그 값으로 미리 채워 보여 준다
                // (조회 때 엔진이 같은 값을 넣으므로 - 보이는 값과 실제 조건이 일치하게).
                if (edit is LookUpEditWyn && PopupLookupProvider.ExtraConditions is { } pre
                    && pre.TryGetValue(field.ParamNm, out var preValue) && !string.IsNullOrEmpty(preValue))
                    edit.EditValue = preValue;
                edit.KeyDown += async (s, e) =>
                {
                    if (e.KeyCode != Keys.Enter) return;
                    e.Handled = true;
                    await SearchAsync(fromUser: true);
                };
                panel.Controls.Add(edit);
                _searchControls[field.ParamNm] = edit;

                x += edit.Width + 16;
                col++;
            }
        }

        WireLookupCascades(fields);

        panel.Controls.Add(CreateQueryButtonHost(Color.Transparent));
        Controls.Add(panel);
    }

    /// <summary>LOOKUP 조건의 연쇄(sysPopUpS.par_fields) - 부모 조건의 값이 바뀌면 그 값들을 (부모의 param_nm 그대로) 자식 콤보의 룩업
    /// 파라미터로 넘겨 목록을 다시 불러오고 자식의 선택값은 비운다(예: 품목그룹1을 고르면 품목그룹2는 그 아래 그룹만, 이미 골랐던 그룹2 값은 해제).
    /// 자식을 비우면 그 자식의 자식(그룹3 등)도 같은 방식으로 이어서 갱신된다. 팝업을 연 쪽이 넘긴 조건으로 부모가 미리 채워져 있으면
    /// 처음부터 그 값으로 좁혀서 보여준다(이때는 자식의 미리 채워진 값을 비우지 않는다).</summary>
    private void WireLookupCascades(List<PopupSearchFieldDto> fields)
    {
        foreach (var field in fields.Where(f => f.ControlType == "LOOKUP" && !string.IsNullOrWhiteSpace(f.ParFields)))
        {
            if (!_searchControls.TryGetValue(field.ParamNm, out var childEdit) || childEdit is not LookUpEditWyn child) continue;

            var parents = field.ParFields!.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => _searchControls.TryGetValue(p, out var pe) && pe is LookUpEditWyn)
                .ToList();
            if (parents.Count == 0) continue;

            void ApplyParents(bool clearChild)
            {
                child.SetParams(parents.Select(p => new KeyValuePair<string, string?>(p, ExtractValue(_searchControls[p]))));
                if (clearChild) child.EditValue = null!;
            }

            foreach (var p in parents)
                _searchControls[p].EditValueChanged += (s, e) => ApplyParents(clearChild: true);

            if (parents.Any(p => !string.IsNullOrEmpty(ExtractValue(_searchControls[p]))))
                ApplyParents(clearChild: false);
        }
    }

    /// <summary>조회 버튼을 조회조건 영역의 오른쪽 끝(세로 가운데)에 두는 작은 도킹 패널(2026-09-25 요청 - "조회는 위쪽,
    /// 선택/취소는 아래쪽"). 자동 생성 검색창과 전용 패널 둘 다 이걸 붙인다.</summary>
    private Panel CreateQueryButtonHost(Color backColor)
    {
        var btnQuery = new SimpleButton { Text = "조회", Size = new Size(84, 30) };
        btnQuery.Click += async (s, e) => await SearchAsync(fromUser: true);

        var host = new Panel { Dock = DockStyle.Right, Width = 104, BackColor = backColor };
        host.Controls.Add(btnQuery);
        void Position() => btnQuery.Location = new Point(host.Width - btnQuery.Width - 12, Math.Max(2, (host.Height - btnQuery.Height) / 2));
        host.Resize += (s, e) => Position();
        Position();
        return host;
    }

    /// <summary>선택/취소(우측 정렬)는 그대로 두고, 조회(좌측)는 별도로 추가한다 - 지금까지는
    /// 검색창에서 Enter를 치거나 팝업이 뜰 때(Load) 자동조회되는 것뿐이라, 검색조건을 입력한
    /// 뒤 마우스로 누를 수 있는 버튼이 아예 없었다(사장님 피드백, 2026-08-31) - Enter를 안 치고
    /// 다른 필드로 탭 이동만 해도 조회가 안 되는 게 답답하다는 지적과 같은 문제.</summary>
    private void BuildFooter()
    {
        var panel = new Panel { Dock = DockStyle.Bottom, Height = 44 };
        // 조회 버튼은 조회조건 영역 오른쪽(BuildSearchPanel/CreateQueryButtonHost)에 있다 - 여기는 선택/취소만.
        var btnCancel = new SimpleButton { Text = "취소", Size = new Size(84, 30) };
        var btnOk = new SimpleButton { Text = "선택", Size = new Size(84, 30) };

        btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        btnOk.Click += (s, e) => Accept();

        void Position()
        {
            btnCancel.Location = new Point(panel.Width - btnCancel.Width - 12, 7);
            btnOk.Location = new Point(btnCancel.Left - btnOk.Width - 8, 7);
        }
        panel.Resize += (s, e) => Position();

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
            GridSortSupport.Enable(gridView); // 헤더 클릭 정렬 - 모든 그리드 공통
            gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView.OptionsView.ColumnAutoWidth = false;
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
            if (_multi)
            {
                gridView.OptionsSelection.MultiSelect = true;
                gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                gridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
                gridView.OptionsSelection.CheckBoxSelectorColumnWidth = 34;
                // 체크한 행이 있을 땐 더블클릭(체크 칸을 빠르게 두 번 누르는 경우 포함)으로 닫지 않는다 - [선택]으로만 확정.
                gridView.DoubleClick += (s, e) => { if (gridView.GetSelectedRows().Length == 0) Accept(); };
            }
            else gridView.DoubleClick += (s, e) => Accept();

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
                // LOOKUP 컬럼은 코드값(G, EA...)을 sysLookupM의 명칭으로 바꿔 보여준다 - 팝업관리 "컨트롤타입=LOOKUP,
                // 룩업=L_xxx"로 정의한 것이 검색조건(LookUpEditWyn)에만 적용되고 결과 그리드엔 안 먹던 것을 고쳤다(2026-09-25).
                // 화면에 보이는 값만 바뀐다 - 선택 결과(RowToDict)는 여전히 원래 코드값이다.
                else if (col.ControlType == "LOOKUP" && !string.IsNullOrEmpty(col.LookupProcNm))
                    column.ColumnEdit = new LookUpColumnEdit { LookupKey = col.LookupProcNm };
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
    private async Task SearchAsync(bool fromUser = false)
    {
        try
        {
            var conditions = _customPanel != null
                ? _customPanel.GetConditions()
                : _searchControls.ToDictionary(kv => kv.Key, kv => ExtractValue(kv.Value));
            _data = await SearchRowsAsync(_def, conditions);
            BindData(autoAcceptSingle: !fromUser);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"조회 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>_data를 그리드/트리에 바인딩한다. 폼이 열리며 하는 자동 조회에서 결과가 정확히 1건이면 사용자가
    /// 고를 이유가 없으므로 그 한 건을 바로 선택된 것으로 처리하고 폼을 닫는다(2026-09-09 요청). 사용자가 직접
    /// 조회 버튼/Enter를 눌러 조회한 경우(autoAcceptSingle=false)는 결과가 1건이어도 닫지 않는다 - 조건을 바꿔
    /// 가며 찾아보는 중에 창이 갑자기 닫히는 것을 막는다(2026-09-25 지적).</summary>
    private void BindData(bool autoAcceptSingle = true)
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

        if (autoAcceptSingle && !_multi && _data.Rows.Count == 1)
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
        if (_multi) { AcceptMulti(); return; }

        Dictionary<string, string?> row;

        if (_def.HierarchicalYn)
        {
            var node = tree.FocusedNode;
            if (node == null) return;
            row = _data.Columns.Cast<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => (string?)Convert.ToString(node.GetValue(c.ColumnName)), StringComparer.OrdinalIgnoreCase); // 그리드 경로(RowToDict)와 같이 대소문자 무시 - 안 그러면 MapField("dept_id")가 실제 컬럼 DEPT_ID를 못 찾는다
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

    /// <summary>체크한 행들을 화면에 보이는 순서대로 담는다. 체크가 없으면 현재 행 1건. 키 컬럼 값이 비는 행이 있으면(BuildResult가 안내) 아무것도 확정하지 않고 폼을 그대로 둔다.</summary>
    private void AcceptMulti()
    {
        var handles = gridView.GetSelectedRows().Where(h => h >= 0).OrderBy(h => gridView.GetVisibleIndex(h)).ToList();
        if (handles.Count == 0 && gridView.FocusedRowHandle >= 0) handles.Add(gridView.FocusedRowHandle);
        if (handles.Count == 0)
        {
            Toast.Show("선택할 행을 체크해주세요.");
            return;
        }

        var results = new List<PopupLookupResult>();
        foreach (var handle in handles)
        {
            var dataRow = gridView.GetDataRow(handle);
            if (dataRow == null) continue;
            var result = BuildResult(_def, RowToDict(_data, dataRow));
            if (result == null) return;
            results.Add(result);
        }

        SelectedResults.Clear();
        SelectedResults.AddRange(results);
        DialogResult = DialogResult.OK;
        Close();
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
