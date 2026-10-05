using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Shell;

/// <summary>
/// 홈 "결재 리스트" 동작부 - 화면에 보이는 고정 컨트롤(제목줄/탭줄/검색조건/하위탭/그리드/기안서 작성
/// 영역)은 HomeForm.Designer.cs에 있고(VS 디자이너로 편집), 여기는 데이터 조회/바인딩/클릭 처리와
/// 개수가 가변적인 기안서 작성 칩·타일만 담당한다(2026-10-03 "최상위 사용자정보와 결재 정보를 통합").
///
/// 기안함 = 내가 상신한 문서를 미결재(진행중)/반려/결재완료로 구분. 결재함 = 미결재(내 차례인 문서),
/// 반려(내가 결재·수신라인에 포함된 문서 중 반려된 것), 결재완료(같은 조건 중 최종결재까지 끝난 것).
/// 기안서 작성 = 문서유형(TSMMINOR AP0002, rel_cd1=문서등록 화면) 바로가기 타일, 더블클릭하면 그 화면이
/// 신규입력 모드로 열린다.
/// </summary>
public partial class HomeForm
{
    private const int MainTabIndexDrafted = 0;
    private const int MainTabIndexInbox = 1;
    private const int MainTabIndexCompose = 2;

    private static readonly Color TileSelectedBg = Color.FromArgb(232, 240, 254);

    private List<ApprovalDashboardItemDto> _approvalRows = new();
    private int _approvalQueryVersion;
    private bool _suppressApprovalQuery;

    private List<ApprovalDocTypeDto>? _docTypes;
    private string _composeCategory = string.Empty;
    private Panel? _selectedTile;

    /// <summary>디자이너에서 정할 수 없는 런타임 값 - 버튼 아이콘 이미지, 기본 검색조건.</summary>
    private void InitializeApprovalCenter()
    {
        StyleTabHeaders(tabMain);
        StyleTabHeaders(tabSub);
        StyleTabHeaders(tabNotice);

        gvwApproval.Role = GridRoleWyn.Query;

        // 표준 - 사업장은 로그인 사업장이 기본값. 기간은 이번 달 1일 ~ 오늘.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        var today = DateTime.Today;
        dteSearchFrom.YyyyMmDd = new DateTime(today.Year, today.Month, 1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = today.ToString("yyyyMMdd");

        // P_EMP 팝업에서 고른 사원의 사번(emp_no)이 숨김 필드에 들어오게 매핑. 기안함(첫 탭)은 내가 올린
        // 문서만 보므로 기안자 조건은 결재함에서만 쓴다.
        popSearchReqEmp.MapField("emp_no", txtSearchReqEmpNo);
        popSearchReqEmp.Enabled = false;
        btnSearch.Image = SvgIcons.Load(SvgIcons.ToolbarSearch, 18, AccentBlue);
    }

    private void panSearch_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(CardBorder);
        e.Graphics.DrawRectangle(pen, 0, 0, panSearch.Width - 1, panSearch.Height - 1);
    }

    /// <summary>탭 머리글 글꼴(DevExpress 기본 8.25pt)이 화면 본문 글꼴보다 작아 보여서
    /// 앱 표준 글꼴로 맞춘다(활성 탭만 굵게).</summary>
    private static void StyleTabHeaders(XtraTabControl tabs)
    {
        tabs.AppearancePage.Header.Font = AppFonts.Body;
        tabs.AppearancePage.Header.Options.UseFont = true;
        tabs.AppearancePage.HeaderHotTracked.Font = AppFonts.Body;
        tabs.AppearancePage.HeaderHotTracked.Options.UseFont = true;
        tabs.AppearancePage.HeaderActive.Font = AppFonts.BodyBold;
        tabs.AppearancePage.HeaderActive.Options.UseFont = true;
    }

    // ---------------------------------------------------------------- 기안함/결재함 목록

    private void btnSearch_Click(object? sender, EventArgs e) => _ = QueryApprovalAsync(silent: false);

    private void txtSearchTitle_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        _ = QueryApprovalAsync(silent: false);
    }

    private void tabMain_SelectedPageChanged(object sender, TabPageChangedEventArgs e) => OnMainTabChanged();

    private void tabSub_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
    {
        if (!_homeReady || _suppressApprovalQuery) return;
        _ = QueryApprovalAsync(silent: false);
    }

    private void grdApproval_SizeChanged(object? sender, EventArgs e) => FitTitleColumn();

    private void gvwApproval_RowCellClick(object sender, RowCellClickEventArgs e)
    {
        if (e.Button != MouseButtons.Left || (e.Column != colAppNo && e.Column != colDocNo)) return;
        var row = gvwApproval.GetDataRow(e.RowHandle);
        if (row == null) return;
        var appId = Convert.ToInt64(row["app_id"]);
        var item = _approvalRows.FirstOrDefault(i => i.AppId == appId);
        if (item == null) return;

        // 문서번호 -> 원본 업무화면을 그 건에 포커스해서 열기, 결재번호 -> 전자결재 팝업(승인/반려 처리).
        if (e.Column == colDocNo) _ = SafeExecuteAsync(() => OpenOriginalDocumentAsync(item), "원본문서열기");
        else _ = SafeExecuteAsync(() => OpenApprovalPopupAsync(item), "전자결재");
    }

    /// <summary>제목 컬럼이 나머지 컬럼을 뺀 남는 폭을 다 차지하게 한다 - ColumnAutoWidth=false(표준)라 창
    /// 크기가 바뀌어도 폭이 저절로 안 맞춰져서 직접 계산한다.</summary>
    private void FitTitleColumn()
    {
        if (!_homeReady || grdApproval.ClientSize.Width <= 0) return;
        var others = gvwApproval.Columns.Cast<GridColumn>().Where(c => c != colAppTitle && c.Visible).Sum(c => c.Width);
        colAppTitle.Width = Math.Max(220, grdApproval.ClientSize.Width - others - SystemInformation.VerticalScrollBarWidth - 28);
    }

    private void OnMainTabChanged()
    {
        if (!_homeReady) return;
        var index = tabMain.SelectedTabPageIndex;
        panListView.Visible = index != MainTabIndexCompose;
        panComposeView.Visible = index == MainTabIndexCompose;
        if (index == MainTabIndexCompose) return;

        // 기안함은 내가 올린 문서만 보므로 기안자 조건을 쓰지 않는다(결재함에서만 입력 가능).
        popSearchReqEmp.Enabled = index == MainTabIndexInbox;
        if (index == MainTabIndexDrafted) { popSearchReqEmp.Text = string.Empty; txtSearchReqEmpNo.Text = string.Empty; }

        // 큰 탭을 바꾸면 하위탭은 항상 미결재부터 - 하위탭 변경 이벤트가 또 조회하지 않게 막고 한 번만 조회한다.
        _suppressApprovalQuery = true;
        tabSub.SelectedTabPageIndex = 0;
        _suppressApprovalQuery = false;
        _ = QueryApprovalAsync(silent: false);
    }

    /// <summary>현재 큰 탭(기안함/결재함) x 하위탭(미결재/반려/결재완료) x 검색조건으로 서버에서 목록을 받아
    /// 그리드를 다시 채운다. silent=true는 홈 탭 재활성화 때의 자동 새로고침 - 실패해도 메시지를 띄우지
    /// 않는다(HomeForm.RefreshDashboardAsync의 다른 위젯과 같은 이유).</summary>
    private async Task QueryApprovalAsync(bool silent)
    {
        if (tabMain.SelectedTabPageIndex == MainTabIndexCompose) return;

        var version = ++_approvalQueryVersion;
        var drafted = tabMain.SelectedTabPageIndex == MainTabIndexDrafted;
        var stat = tabSub.SelectedTabPageIndex switch { 1 => "R", 2 => "E", _ => "P" };

        var qs = new List<string> { $"box={(drafted ? "drafted" : "inbox")}", $"stat={stat}" };
        void Add(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value)) qs.Add($"{key}={Uri.EscapeDataString(value!.Trim())}");
        }
        Add("accId", cboSearchAccId.EditValue?.ToString());
        Add("docType", cboSearchDocType.EditValue?.ToString());
        Add("dateFrom", dteSearchFrom.YyyyMmDd);
        Add("dateTo", dteSearchTo.YyyyMmDd);
        Add("reqEmpNo", drafted ? null : txtSearchReqEmpNo.Text);
        Add("title", txtSearchTitle.Text);

        try
        {
            var list = await ApiClient.GetAsync<List<ApprovalDashboardItemDto>>("api/approvals/my-list?" + string.Join("&", qs)) ?? new();
            if (version != _approvalQueryVersion) return;
            _approvalRows = list;
        }
        catch (Exception ex)
        {
            if (version != _approvalQueryVersion) return;
            _approvalRows = new();
            if (!silent) AppMessageBox.Show($"결재 리스트를 불러오지 못했습니다.\n{ex.Message}", "오류");
        }
        BindApprovalRows(drafted);
    }

    private void BindApprovalRows(bool drafted)
    {
        var dt = new DataTable();
        dt.Columns.Add("app_id", typeof(long));
        foreach (var name in new[] { "kind", "app_no", "req_dt", "doc_type_nm", "doc_no", "app_title", "req_emp_nm", "stat_text", "cur_appr_emp_nm", "last_appr_emp_nm" })
            dt.Columns.Add(name, typeof(string));

        foreach (var i in _approvalRows)
        {
            dt.Rows.Add(
                i.AppId,
                drafted ? "기안" : (i.PathType == "R" ? "수신" : "결재"),
                i.AppNo,
                i.ReqDt?.ToString("yyyy-MM-dd HH:mm") ?? i.AppDate,
                ResolveDocTypeName(i.DocType),
                i.DocNo,
                i.AppTitle,
                i.ReqEmpNm ?? string.Empty,
                i.StatCd switch { "E" => "승인완료", "R" => "반려", "1" => "승인중", _ => "결재상신" },
                i.CurApprEmpNm ?? string.Empty,
                i.LastApprEmpNm ?? string.Empty);
        }

        grdApproval.DataSource = dt;
        FitTitleColumn();
    }

    // ---------------------------------------------------------------- 기안서 작성

    /// <summary>기안서 작성 타일 목록 - 문서유형 중 문서등록 화면이 지정돼 있고(rel_cd1) 내가 그 화면 권한도
    /// 가진 것만. 거의 안 바뀌는 값이라 처음 한 번만 불러온다.</summary>
    private async Task LoadDocTypesAsync()
    {
        if (_docTypes != null) return;
        try
        {
            var all = await ApiClient.GetAsync<List<ApprovalDocTypeDto>>("api/approvals/doc-types") ?? new();
            var menus = SessionManager.Current.Menus;
            _docTypes = all.Where(d => menus.Any(m => $"{m.Module}.{m.ScreenClassNm}" == d.FormId)).ToList();
        }
        catch { return; /* 못 불러오면 다음 새로고침 때 다시 시도 */ }
        RenderComposeView();
    }

    private void RenderComposeView()
    {
        if (_docTypes == null) return;
        var categories = _docTypes.Select(d => string.IsNullOrEmpty(d.Category) ? "기타" : d.Category).Distinct().ToList();
        if (!categories.Contains(_composeCategory)) _composeCategory = string.Empty;

        ClearAndDispose(flpChips);
        flpChips.SuspendLayout();
        flpChips.Controls.Add(BuildChip("전체", string.Empty));
        foreach (var c in categories) flpChips.Controls.Add(BuildChip(c, c));
        flpChips.ResumeLayout();

        ClearAndDispose(flpTiles);
        _selectedTile = null;
        flpTiles.SuspendLayout();
        var shown = _docTypes.Where(d => _composeCategory.Length == 0 || (string.IsNullOrEmpty(d.Category) ? "기타" : d.Category) == _composeCategory).ToList();
        foreach (var d in shown) flpTiles.Controls.Add(BuildDocTile(d));
        if (shown.Count == 0) flpTiles.Controls.Add(new LabelControl { Text = "상신 가능한 문서가 없습니다.", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        flpTiles.ResumeLayout();
    }

    private static void ClearAndDispose(Control host)
    {
        var old = host.Controls.Cast<Control>().ToList();
        host.Controls.Clear();
        foreach (var c in old) c.Dispose();
    }

    private Panel BuildChip(string text, string category)
    {
        var active = _composeCategory == category;
        var width = TextRenderer.MeasureText(text, AppFonts.Body).Width + 36;
        var chip = new Panel { Size = new Size(Math.Max(72, width), 30), Margin = new Padding(0, 0, 8, 0), Cursor = Cursors.Hand };
        chip.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, chip.Width - 1, chip.Height - 1);
            using (var path = RoundedRectPath(rect, 4))
            {
                using var fill = new SolidBrush(active ? Color.FromArgb(22, 98, 191) : CardBg);
                e.Graphics.FillPath(fill, path);
                using var pen = new Pen(active ? Color.FromArgb(22, 98, 191) : AccentBlue);
                e.Graphics.DrawPath(pen, path);
            }
            TextRenderer.DrawText(e.Graphics, text, AppFonts.Body, rect, active ? Color.White : AccentBlue,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        };
        chip.Click += (s, e) => { _composeCategory = category; RenderComposeView(); };
        return chip;
    }

    private Panel BuildDocTile(ApprovalDocTypeDto docType)
    {
        var painter = docType.DocType switch
        {
            "POREQ" or "PO" => MenuIconPainters.Cart,
            _ => (Action<Graphics, Rectangle, Color>)MenuIconPainters.Document,
        };
        var icon = MenuIconPainters.Render(painter, 26, AccentBlue);

        var tile = new Panel { Size = new Size(196, 58), Margin = new Padding(0, 0, 12, 12), Cursor = Cursors.Hand };
        tile.Disposed += (s, e) => icon.Dispose();
        tile.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var selected = ReferenceEquals(_selectedTile, tile);
            var rect = new Rectangle(0, 0, tile.Width - 1, tile.Height - 1);
            using (var path = RoundedRectPath(rect, 4))
            {
                using var fill = new SolidBrush(selected ? TileSelectedBg : CardBg);
                e.Graphics.FillPath(fill, path);
                using var pen = new Pen(selected ? AccentBlue : CardBorder);
                e.Graphics.DrawPath(pen, path);
            }
            e.Graphics.DrawImage(icon, 14, (tile.Height - icon.Height) / 2);
            TextRenderer.DrawText(e.Graphics, docType.DocTypeNm, AppFonts.Body, new Rectangle(50, 0, tile.Width - 50 - 14, tile.Height - 1),
                TextPrimary, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
        };
        tile.Click += (s, e) =>
        {
            var previous = _selectedTile;
            _selectedTile = tile;
            previous?.Invalidate();
            tile.Invalidate();
        };
        tile.DoubleClick += (s, e) => _ = SafeExecuteAsync(() => OpenNewDocumentAsync(docType), "기안서 작성");
        return tile;
    }

    /// <summary>문서유형의 문서등록 화면(FormId="{MODULE}.{화면클래스명}")을 열고 신규입력 모드로 전환한다 -
    /// OpenOriginalDocumentAsync(문서번호 클릭)와 같은 방식으로 화면을 만들되, 기존 건에 포커스하는 대신
    /// NewClick()으로 빈 문서를 연다.</summary>
    private async Task OpenNewDocumentAsync(ApprovalDocTypeDto docType)
    {
        var parts = docType.FormId.Split('.');
        if (parts.Length != 2)
        {
            AppMessageBox.Show($"문서 화면 정보 형식이 올바르지 않습니다: {docType.FormId}", "오류");
            return;
        }
        var module = parts[0];
        var className = parts[1];

        var assembly = ModuleLoader.EnsureLoaded($"WYNLAB.{module}");
        var formType = assembly?.GetType($"WYNLAB.{module}.{className}");
        if (formType == null || Activator.CreateInstance(formType) is not BaseForm form)
        {
            AppMessageBox.Show($"화면을 찾을 수 없습니다: {docType.FormId}", "오류");
            return;
        }

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == module && m.ScreenClassNm == className);
        form.MenuId = menu?.MenuId ?? 0;
        form.MdiParent = MdiParent;
        form.Show();

        await form.NewClick();
    }
}
