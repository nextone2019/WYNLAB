using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Popup;

/// <summary>
/// 전자결재 공용 팝업 - 명함신청서 등 어느 업무화면이든 저장 후 이 화면 하나를 띄워서 결재를
/// 상신/처리한다. popFileUpload.cs/popPopUp.cs와 같은 이유로 BaseForm이 아니라 XtraForm을
/// 직접 상속한다(여러 화면이 공용으로 띄우는 독립 팝업이라 MDI/메뉴 컨텍스트가 없음). 컨트롤
/// 배치는 popApp.Designer.cs(VS 디자이너로 편집 가능) - 여기는 로직/이벤트 연결만.
///
/// 모드 두 가지:
///  - 작성모드(app_id 없음, 아직 상신 전) - 부서트리+사원목록으로 결재라인/수신라인을 구성하고
///    "결재상신" 버튼만 활성화된다.
///  - 처리모드(app_id 있음, 상신됨) - 부서트리/결재경로 영역은 숨기고, 로그인 사용자 자신의
///    결재라인/수신라인 행 상태에 따라 승인/반려/승인취소/수신확인 버튼이 활성화된다. 실제 허용
///    여부는 서버(USP_AP_APPR_S)가 다시 검증하므로, 여기 계산은 버튼 활성/비활성을 위한 힌트일 뿐이다.
/// </summary>
public partial class popApp : XtraForm
{
    private readonly string _docType;
    private readonly long _docId;
    private readonly string _docNo;
    private readonly string? _formId;

    private long? _appId;
    private bool _changed;
    private bool _composing;
    private List<ApprovalPathDto> _lineRows = new(); // path_type='A'
    private List<ApprovalPathDto> _recvRows = new(); // path_type='F'
    private List<ApprovalRouteItemDto> _routes = new();

    /// <summary>VS 디자이너 전용 생성자 - 디자인 서페이스가 미리보기를 그리려면 매개변수 없는
    /// 생성자가 있어야 한다(frmAcc 등 메뉴화면은 ShellForm이 리플렉션으로 생성할 때도 필요해서
    /// public 기본 생성자가 원래 있지만, 이 화면은 공용 팝업이라 진입점을 ShowAsync 하나로
    /// 막아뒀다 - 그래서 디자이너용 생성자를 따로 둔다). 실제 코드에서는 절대 호출하지 않는다 -
    /// 아래 실제 생성자로 위임하고 더미 값을 채운다.</summary>
    private popApp() : this(string.Empty, 0, string.Empty, string.Empty, string.Empty, null) { }

    private popApp(string docType, long docId, string docNo, string title, string text, string? formId)
    {
        InitializeComponent();

        _docType = docType;
        _docId = docId;
        _docNo = docNo;
        _formId = formId;

        txtTitle.Text = title;
        memoOpinion.Text = text;

        treeEmp.SelectImageList = BuildEmpTreeImageList();
        treeEmp.MouseDown += TreeEmp_MouseDown;
        grdLine.AllowDrop = true;
        grdRecv.AllowDrop = true;
        grdLine.DragEnter += (s, e) => AcceptEmpDrag(e);
        grdRecv.DragEnter += (s, e) => AcceptEmpDrag(e);
        grdLine.DragDrop += (s, e) => DropOnGrid(e, gvwLine, _lineRows, "C");
        grdRecv.DragDrop += (s, e) => DropOnGrid(e, gvwRecv, _recvRows, "R");
        gvwLine.MouseDown += (s, e) => StartRowDrag(gvwLine, _lineRows, e);
        gvwRecv.MouseDown += (s, e) => StartRowDrag(gvwRecv, _recvRows, e);
        gvwLine.CustomColumnDisplayText += (s, e) => FormatPathStatus(e, isRecv: false);
        gvwRecv.CustomColumnDisplayText += (s, e) => FormatPathStatus(e, isRecv: true);

        btnRefresh.Click += async (s, e) => await RefreshAsync();
        btnSubmit.Click += async (s, e) => await OnPrimaryActionClick();
        btnReject.Click += async (s, e) => await ProcessApproveOrRejectAsync(approve: false);
        btnCancelApprove.Click += async (s, e) => await CancelApproveAsync();
        btnAck.Click += async (s, e) => await AcknowledgeAsync();
        btnAddLine.Click += (s, e) => AddSelectedEmployee("C");
        btnAddRecv.Click += (s, e) => AddSelectedEmployee("R");
        btnApplyRoute.Click += async (s, e) => await ApplyRouteAsync();
        btnSaveRoute.Click += async (s, e) => await SaveRouteAsync();
        btnClose.Click += (s, e) => Close();

        // OS 제목표시줄이 비어 있어 잡을 곳이 없으므로, 눈에 보이는 제목 영역을 끌어서 창을 옮길 수 있게 한다(popPopUp과 같은 방식).
        foreach (Control c in new Control[] { paTitle, sectionHeaderWyn1 })
            c.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            };

        Load += async (s, e) => await RefreshAsync();
    }

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 2;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    // popPopUp.ShowAsync(2026-09-23)와 같은 문제 - 빠르게 두 번 클릭(또는 링크 클릭 한 번이
    // DevExpress 쪽에서 중복 발생)하면 첫 호출이 아직 모달을 띄우기 전인 순간 두 번째 호출이
    // 들어와 같은 문서의 결재창이 두 개 뜰 수 있다. 이 팝업은 한 번에 하나만 뜨면 되므로 static
    // 플래그로 이미 진행 중인 호출이 있으면 새 호출은 조용히 무시한다.
    private static bool _isShowing;

    /// <summary>doc_type/doc_id로 이 문서의 전자결재 화면을 모달로 띄운다. 반환값이 true면 결재상태가
    /// 바뀐 것이므로(상신/승인/반려/취소/확인 중 하나라도 실제로 일어남) 호출측이 재조회해야 한다.</summary>
    public static Task<bool> ShowAsync(string docType, long docId, string docNo, string title, string text, Control owner)
    {
        if (_isShowing) return Task.FromResult(false);
        _isShowing = true;
        try
        {
            var ownerForm = owner.FindForm();
            var formId = ownerForm != null ? ResolveFormId(ownerForm.GetType()) : null;

            // ShowDialog는 원래 동기 호출(닫힐 때까지 블록)이라 await할 게 없다 - 그래도 다른
            // ShowAsync류(popFileUpload 등)와 호출부 모양을 맞추려고 Task<bool>로 감싸서 돌려준다.
            using var form = new popApp(docType, docId, docNo, title, text, formId);
            if (ownerForm != null) form.ShowDialog(ownerForm);
            else form.ShowDialog();
            return Task.FromResult(form._changed);
        }
        finally
        {
            _isShowing = false;
        }
    }

    /// <summary>owner 폼의 실제 타입에서 "{MODULE}.{클래스명}"을 뽑아낸다(예: WYNLAB.AP.frmNameCardReq
    /// -> "AP.frmNameCardReq") - 결재함이 TAPDOC.form_id로 원본 화면을 되찾을 때 쓸 값이다.
    /// 네임스페이스가 "WYNLAB."로 시작하지 않는 화면(공용 팝업 자신 등)은 그 화면으로는 다시
    /// 돌아갈 방법이 없으므로 null을 돌려준다.</summary>
    private static string? ResolveFormId(Type ownerType)
    {
        const string prefix = "WYNLAB.";
        var ns = ownerType.Namespace;
        if (ns == null || !ns.StartsWith(prefix, StringComparison.Ordinal)) return null;

        var module = ns.Substring(prefix.Length);
        return $"{module}.{ownerType.Name}";
    }

    private async Task RefreshAsync()
    {
        var history = await ApprovalClient.GetHistoryAsync(_docType, _docId);
        var header = history?.Headers.FirstOrDefault();

        if (header == null)
        {
            // 작성모드 - 아직 상신 전
            _appId = null;
            txtAppNo.Text = string.Empty;
            txtAppId.Text = string.Empty;
            ymdAppDate.Text = string.Empty;
            txtReqEmpNm.Text = string.Empty;
            cboAppStatCd.EditValue = string.Empty;
            cboDocType.EditValue = _docType;
            txtDocNo.Text = _docNo;

            // 상신 즉시 서버(USP_AP_APPR_S work type 'N')가 기안자 본인을 sort=1/승인완료로 자동
            // 삽입한다(클래스 주석 참고) - 상신 전에도 그리드에 미리 보여주되 아직 대기("N")로
            // 두고, 실제 승인상태/일시는 SubmitAsync 이후 RefreshAsync가 서버값으로 덮어쓴다.
            _lineRows = new List<ApprovalPathDto>
            {
                new() { EmpId = Session.EmpId ?? 0, EmpNo = Session.EmpNo, EmpNm = Session.EmpNm, PathType = "C", StatCd = "0", Sort = 1 },
            };
            _recvRows = new();
            BindGrids();

            txtTitle.ReadOnly = false;
            memoOpinion.ReadOnly = false;

            await LoadEmpTreeAsync();
            await LoadRoutesAsync();

            SetMode(composing: true);
            return;
        }

        _appId = header.AppId;
        txtAppNo.Text = header.AppNo;
        txtAppId.Text = header.AppId.ToString();
        ymdAppDate.Text = header.AppDate;
        txtReqEmpNm.Text = header.ReqEmpNm ?? string.Empty;
        cboAppStatCd.EditValue = header.StatCd ?? string.Empty;
        cboDocType.EditValue = header.DocType;
        txtDocNo.Text = header.DocNo;
        txtTitle.ReadOnly = true;
        memoOpinion.ReadOnly = true;

        var paths = history!.Paths.Where(p => p.AppId == header.AppId).ToList();
        _lineRows = paths.Where(p => p.PathType == "C").OrderBy(p => p.Sort).ToList();
        _recvRows = paths.Where(p => p.PathType == "R").ToList();
        BindGrids();

        SetMode(composing: false);
        UpdateActionButtons();
    }

    private void BindGrids()
    {
        grdLine.DataSource = null; grdLine.DataSource = _lineRows;
        grdRecv.DataSource = null; grdRecv.DataSource = _recvRows;
    }

    private void SetMode(bool composing)
    {
        // 조직도(승인자/수신자 추가 버튼 + 부서·사원 트리)와 저장된 결재경로 적용은 최초 상신 때 결재경로를 만들 때만 쓴다.
        panelWyn8.Visible = composing;
        splitterWyn1.Visible = composing;
        cboRoute.Enabled = composing;
        btnApplyRoute.Enabled = composing;
        _composing = composing;
        btnSubmit.Text = composing ? "결재상신" : "승인";
        btnReject.Visible = !composing;
        btnCancelApprove.Visible = !composing;
        btnAck.Visible = !composing;

        if (composing)
        {
            btnSubmit.Enabled = true;
            btnReject.Enabled = btnCancelApprove.Enabled = btnAck.Enabled = false;
        }
    }

    /// <summary>결재상신/승인 버튼을 하나로 합친 것 - 작성모드(상신 전)면 상신, 아니면(상신 후) 내
    /// 차례의 승인 처리. 텍스트/활성화는 SetMode·UpdateActionButtons가 미리 맞춰둔다.</summary>
    private async Task OnPrimaryActionClick()
    {
        if (_composing) await SubmitAsync();
        else await ProcessApproveOrRejectAsync(approve: true);
    }

    /// <summary>로그인 사용자 자신의 결재라인/수신라인 행 상태로 버튼 활성/비활성을 계산한다(힌트일
    /// 뿐 - 실제 허용 여부는 서버가 다시 검증). Session.EmpNo(TSMUSER.EMP_NO)로 내 행을 찾는다.</summary>
    private void UpdateActionButtons()
    {
        var myEmpNo = Session.EmpNo;

        var mine = _lineRows.FirstOrDefault(p => p.EmpNo == myEmpNo);
        var canActNow = mine != null && mine.StatCd == "0"
            && !_lineRows.Any(o => o.PathType == "C" && o.Sort < mine.Sort && o.StatCd != "E");
        btnSubmit.Enabled = canActNow;
        btnReject.Enabled = canActNow;

        var canCancel = mine != null && mine.StatCd == "E"
            && !_lineRows.Any(o => o.PathType == "C" && o.Sort > mine.Sort && o.StatCd == "E");
        btnCancelApprove.Enabled = canCancel;

        var myRecv = _recvRows.FirstOrDefault(p => p.EmpNo == myEmpNo);
        btnAck.Enabled = myRecv != null && myRecv.StatCd == "0";
    }

    /// <summary>부서(TBADEPT)+사원(TBAEMP)을 한 번에 받아 treeEmp 하나에 합쳐 그린다 - 부서는
    /// "D"+dept_id, 사원은 "E"+emp_id로 키를 합성해서(<see cref="EmpTreeNode"/> 참고) 서로
    /// 다른 두 엔티티를 KeyFieldName/ParentFieldName 한 쌍으로 묶는다.</summary>
    private async Task LoadEmpTreeAsync()
    {
        var depts = await ApprovalClient.GetDeptTreeAsync() ?? new List<DeptTreeItemDto>();
        var emps = await ApprovalClient.GetEmployeesAsync() ?? new List<ApprovalEmpItemDto>();

        // 같은 부서 밑에서 형제 노드 순서는 TreeList가 이 리스트에 담긴 순서를 그대로 따른다
        // (별도 정렬을 걸지 않았으므로) - 그래서 부서 소속 사원을 먼저 추가하고 하위부서를
        // 나중에 추가해야, 화면에서도 "그 부서 인원이 먼저, 하위부서는 그다음"으로 보인다.
        var nodes = new List<EmpTreeNode>(depts.Count + emps.Count);
        foreach (var e in emps)
        {
            nodes.Add(new EmpTreeNode
            {
                NodeKey = "E" + e.EmpId,
                ParentKey = "D" + e.DeptId,
                Nm = e.EmpNm,
                EmpNo = e.EmpNo,
                EmpId = e.EmpId,
                JobGrade = e.JobGrade,
                IsDept = false,
                ImgIdx = 1,
            });
        }
        foreach (var d in depts)
        {
            nodes.Add(new EmpTreeNode
            {
                NodeKey = "D" + d.DeptId,
                ParentKey = d.ParDeptId.HasValue ? "D" + d.ParDeptId.Value : null,
                Nm = d.DeptNm,
                IsDept = true,
                ImgIdx = 0,
            });
        }

        treeEmp.DataSource = nodes;
        treeEmp.ExpandAll();
    }

    /// <summary>부서 노드는 드래그 시작 대상에서 제외한다 - 결재라인/수신라인에 추가할 수 있는 건
    /// 사원뿐이라(부서 자체는 승인자/수신자가 될 수 없음), 마우스가 눌린 노드가 사원 노드일 때만
    /// DoDragDrop을 시작한다.</summary>
    private void TreeEmp_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        var hit = treeEmp.CalcHitInfo(e.Location);
        if (hit.Node == null || hit.Node.GetValue("IsDept") is true) return;

        var emp = NodeToEmp(hit.Node);
        if (emp == null) return;

        treeEmp.DoDragDrop(emp, DragDropEffects.Copy);
    }

    private static void AcceptEmpDrag(DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(typeof(ApprovalPathDto)) == true) e.Effect = DragDropEffects.Move;
        else if (e.Data?.GetDataPresent(typeof(ApprovalEmpItemDto)) == true) e.Effect = DragDropEffects.Copy;
        else e.Effect = DragDropEffects.None;
    }

    /// <summary>조직도에서 드래그해온 사원(ApprovalEmpItemDto)이면 새로 추가, 같은 그리드 안의 행
    /// (ApprovalPathDto)이면 결재순서 변경 - grdLine/grdRecv가 공유하는 DragDrop 핸들러라 페이로드
    /// 타입으로 분기한다. 다른 그리드에서 넘어온 행(rows에 없음)은 무시 - 결재라인/수신라인 간
    /// 이동은 지원하지 않는다.</summary>
    private void DropOnGrid(DragEventArgs e, DevExpress.XtraGrid.Views.Grid.GridView view, List<ApprovalPathDto> rows, string pathType)
    {
        if (e.Data?.GetData(typeof(ApprovalPathDto)) is ApprovalPathDto dragged)
        {
            if (!_composing || !rows.Contains(dragged)) return;

            var pt = view.GridControl.PointToClient(new Point(e.X, e.Y));
            var hit = view.CalcHitInfo(pt);
            var targetIndex = hit.RowHandle >= 0 ? hit.RowHandle : rows.Count - 1;
            // 본인(기안자) 행은 서버가 항상 sort=1로 고정 삽입하므로 1번 자리 밑으로만 옮길 수 있다.
            var minIndex = rows.Count > 0 && rows[0].EmpNo == Session.EmpNo ? 1 : 0;
            targetIndex = Math.Max(minIndex, Math.Min(targetIndex, rows.Count - 1)); // net48엔 Math.Clamp가 없음

            rows.Remove(dragged);
            rows.Insert(targetIndex, dragged);
            RenumberSort(rows, pathType);
            BindGrids();
            return;
        }

        if (e.Data?.GetData(typeof(ApprovalEmpItemDto)) is ApprovalEmpItemDto emp)
            AddEmployeeToPath(emp, pathType);
    }

    /// <summary>그리드 행(ApprovalPathDto)을 드래그 시작해서 같은 그리드 안에서 놓으면 DropOnGrid가
    /// 순서를 바꾼다. 상신 후(처리모드)엔 순서를 바꿔도 반영할 재정렬 API가 없으므로 작성모드에서만
    /// 허용하고, 서버가 위치를 고정하는 본인 행은 드래그 시작 자체를 막는다.</summary>
    private void StartRowDrag(DevExpress.XtraGrid.Views.Grid.GridView view, List<ApprovalPathDto> rows, MouseEventArgs e)
    {
        if (!_composing || e.Button != MouseButtons.Left) return;

        var hit = view.CalcHitInfo(new Point(e.X, e.Y));
        if (hit.RowHandle < 0 || hit.RowHandle >= rows.Count) return;
        if (rows[hit.RowHandle].EmpNo == Session.EmpNo) return;

        view.GridControl.DoDragDrop(rows[hit.RowHandle], DragDropEffects.Move);
    }

    private static void RenumberSort(List<ApprovalPathDto> rows, string pathType)
    {
        if (pathType != "C") return; // 수신라인(R)은 서버도 순서 개념이 없어(sort=0 고정) 번호를 안 매김
        for (var i = 0; i < rows.Count; i++) rows[i].Sort = i + 1;
    }

    /// <summary>결재/수신라인 그리드의 상태코드(Y/N/R)를 화면 문구로 바꿔 보여준다 - 데이터는 그대로
    /// 두고 표시 텍스트만 바꾸는 DevExpress 표준 방식(CustomColumnDisplayText)이라 별도 LookUp
    /// 컬럼 없이 처리된다. 같은 "Y"도 결재라인은 승인, 수신라인은 확인으로 뜻이 달라 isRecv로
    /// 구분한다.</summary>
    private static void FormatPathStatus(DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e, bool isRecv)
    {
        if (e.Column.FieldName != "StatCd") return;
        e.DisplayText = e.Value as string switch
        {
            "E" => isRecv ? "확인" : "승인",
            "R" => "반려",
            _ => "대기",
        };
    }

    private static ApprovalEmpItemDto? NodeToEmp(DevExpress.XtraTreeList.Nodes.TreeListNode node)
    {
        if (node.GetValue("EmpId") is not long empId) return null;
        return new ApprovalEmpItemDto
        {
            EmpId = empId,
            EmpNo = node.GetValue("EmpNo") as string ?? string.Empty,
            EmpNm = node.GetValue("Nm") as string ?? string.Empty,
            JobGrade = node.GetValue("JobGrade") as string,
        };
    }

    /// <summary>부서/사원 아이콘 - 회사별 커스터마이즈 대상이 아니라 리소스 파일 없이 코드로 직접
    /// 그린다(ToolbarIcons처럼 파일로 두지 않는 이유: 이 트리 전용의 아주 단순한 도형 2개뿐이라
    /// 별도 리소스/의존성을 늘릴 이유가 없음). 인덱스는 LoadEmpTreeAsync의 ImgIdx(0=부서,1=사원)와
    /// 반드시 같은 순서로 맞춰야 한다.</summary>
    private static ImageList BuildEmpTreeImageList()
    {
        var images = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };
        images.Images.Add(DrawDeptIcon());
        images.Images.Add(DrawEmpIcon());
        return images;
    }

    private static Image DrawDeptIcon()
    {
        var bmp = new Bitmap(16, 16);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using var tab = new SolidBrush(Color.FromArgb(255, 224, 178, 97));
        using var body = new SolidBrush(Color.FromArgb(255, 201, 154, 61));
        g.FillRectangle(tab, 1, 3, 6, 3);
        g.FillRectangle(body, 1, 5, 14, 9);
        return bmp;
    }

    private static Image DrawEmpIcon()
    {
        var bmp = new Bitmap(16, 16);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using var brush = new SolidBrush(Color.FromArgb(255, 74, 144, 226));
        g.FillEllipse(brush, 5, 1, 6, 6);
        g.FillPie(brush, 2, 7, 12, 12, 180, 180);
        return bmp;
    }

    /// <summary>treeEmp 하나에 부서(TBADEPT)+사원(TBAEMP)을 같이 그리기 위한 합성 노드 - TreeList의
    /// KeyFieldName/ParentFieldName은 원래 한 엔티티 타입만 상정하므로, 부서는 "D"+dept_id, 사원은
    /// "E"+emp_id로 접두어를 붙여 키 충돌 없이 한 트리로 합친다(사원의 ParentKey는 자신이 속한
    /// 부서의 "D"+dept_id).</summary>
    private class EmpTreeNode
    {
        public string NodeKey { get; set; } = string.Empty;
        public string? ParentKey { get; set; }
        public string Nm { get; set; } = string.Empty;
        public string? EmpNo { get; set; }
        public long? EmpId { get; set; }
        public string? JobGrade { get; set; }
        public bool IsDept { get; set; }
        public int ImgIdx { get; set; }
    }

    /// <summary>내가 저장한 결재경로(결재경로관리 화면에서 만든 것) 목록을 "저장된 결재경로" 콤보에 채운다.</summary>
    private async Task LoadRoutesAsync()
    {
        _routes = await ApprovalClient.GetRoutesAsync() ?? new List<ApprovalRouteItemDto>();
        cboRoute.BindCodeList(_routes, nameof(ApprovalRouteItemDto.RouteId), nameof(ApprovalRouteItemDto.RouteNm), "코드", "결재경로명", 330);
        cboRoute.EditValue = null;
    }
    private void AddSelectedEmployee(string pathType)
    {
        var node = treeEmp.FocusedNode;
        var emp = node != null ? NodeToEmp(node) : null;
        if (emp == null)
        {
            AppMessageBox.Show("사원을 먼저 선택해주세요.", "안내");
            return;
        }

        AddEmployeeToPath(emp, pathType);
    }

    private void AddEmployeeToPath(ApprovalEmpItemDto emp, string pathType)
    {
        // 결재라인/수신라인에 기안자(로그인 사용자) 본인은 넣을 수 없다 - 결재라인의 본인 행은 상신할 때 서버가 자동으로 넣는다.
        if (!string.IsNullOrEmpty(Session.EmpNo) && emp.EmpNo == Session.EmpNo)
        {
            AppMessageBox.Show("본인은 결재라인/수신라인에 추가할 수 없습니다.", "안내");
            return;
        }

        var target = pathType == "C" ? _lineRows : _recvRows;
        if (target.Any(r => r.EmpNo == emp.EmpNo))
        {
            AppMessageBox.Show("이미 추가된 사원입니다.", "안내");
            return;
        }

        target.Add(new ApprovalPathDto
        {
            EmpId = emp.EmpId,
            EmpNo = emp.EmpNo,
            EmpNm = emp.EmpNm,
            PathType = pathType,
            StatCd = "0",
            Sort = pathType == "C" ? target.Count(r => r.PathType == "C") + 1 : 0,
        });
        BindGrids();
    }

    /// <summary>"저장된 결재경로" 콤보에서 고른 경로의 결재라인/수신라인을 지금 상신 화면에 적용한다. 기안자 본인 행(결재라인 1번)은 남기고,
    /// 나머지 결재라인/수신라인은 그 경로의 구성으로 바꾼다(경로에 본인이 들어 있으면 건너뛴다).</summary>
    private async Task ApplyRouteAsync()
    {
        if (cboRoute.EditValue == null || !long.TryParse(cboRoute.EditValue.ToString(), out var routeId))
        {
            AppMessageBox.Show("적용할 결재경로를 먼저 선택해주세요.", "안내");
            return;
        }

        var detail = await ApprovalClient.GetRouteDetailAsync(routeId) ?? new List<ApprovalRoutePathItemDto>();

        _lineRows = _lineRows.Where(r => r.EmpNo == Session.EmpNo).ToList();   // 기안자 본인 행만 남긴다
        _recvRows = new List<ApprovalPathDto>();

        foreach (var d in detail.OrderBy(x => x.PathType).ThenBy(x => x.Sort))
        {
            if (string.IsNullOrEmpty(d.EmpNo) || d.EmpNo == Session.EmpNo) continue;

            var target = d.PathType == "C" ? _lineRows : _recvRows;
            if (target.Any(r => r.EmpNo == d.EmpNo)) continue;
            target.Add(new ApprovalPathDto
            {
                EmpId = d.EmpId,
                EmpNo = d.EmpNo,
                EmpNm = d.EmpNm,
                PathType = d.PathType,
                StatCd = "0",
                Sort = d.PathType == "C" ? target.Count + 1 : 0,
            });
        }

        BindGrids();
    }
    private async Task SaveRouteAsync()
    {
        if (string.IsNullOrWhiteSpace(txtNewRouteNm.Text))
        {
            AppMessageBox.Show("저장할 결재경로 이름을 입력해주세요.", "안내");
            return;
        }
        if (_lineRows.Count == 0 && _recvRows.Count == 0)
        {
            AppMessageBox.Show("결재라인/수신라인에 최소 1명 이상 추가한 뒤 저장해주세요.", "안내");
            return;
        }

        var result = await ApprovalClient.SaveRouteAsync(txtNewRouteNm.Text);
        if (result == null || !result.Success || result.GeneratedCode == null)
        {
            AppMessageBox.Show(result?.Message ?? "결재경로 저장에 실패했습니다.", "저장 실패");
            return;
        }

        var routeId = long.Parse(result.GeneratedCode);
        foreach (var row in _lineRows.Concat(_recvRows))
            await ApprovalClient.AddRouteDetailAsync(routeId, row.EmpNo ?? string.Empty, row.PathType);

        txtNewRouteNm.Text = string.Empty;
        await LoadRoutesAsync();
        Toast.Show("결재경로가 저장되었습니다.");
    }

    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            AppMessageBox.Show("문서제목을 입력해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show("결재상신 하시겠습니까?", "결재상신 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await ApprovalClient.SubmitAsync(new ApprovalSubmitRequest
        {
            DocType = _docType,
            DocId = _docId,
            DocNo = _docNo,
            Title = txtTitle.Text,
            Text = memoOpinion.Text,
            FormId = _formId,
        });

        if (result == null || !result.Success || result.GeneratedCode == null)
        {
            AppMessageBox.Show(result?.Message ?? "결재상신에 실패했습니다.", "결재상신 실패");
            return;
        }

        var appId = long.Parse(result.GeneratedCode);
        var pathFailed = false;   // 결재/수신라인 추가에 실패했으면 창을 닫지 않고 남겨서 사용자가 상태를 확인하게 한다

        // 기안자 본인은 서버(USP_AP_APPR_S work type 'N')가 상신 시점에 이미 자동 삽입하므로
        // (RefreshAsync 작성모드의 self-row는 그 미리보기일 뿐) 다시 보내면 중복 행이 생긴다.
        foreach (var row in _lineRows.Where(r => r.EmpNo != Session.EmpNo))
        {
            var r = await ApprovalClient.AddPathAsync(new ApprovalAddPathRequest { AppId = appId, TargetEmpNo = row.EmpNo ?? string.Empty, PathType = "C" });
            if (r == null || !r.Success)
            {
                AppMessageBox.Show(r?.Message ?? "결재라인 추가 중 오류가 발생했습니다.", "오류");
                pathFailed = true;
                break;
            }
        }
        foreach (var row in _recvRows)
        {
            var r = await ApprovalClient.AddPathAsync(new ApprovalAddPathRequest { AppId = appId, TargetEmpNo = row.EmpNo ?? string.Empty, PathType = "R" });
            if (r == null || !r.Success)
            {
                AppMessageBox.Show(r?.Message ?? "수신라인 추가 중 오류가 발생했습니다.", "오류");
                pathFailed = true;
                break;
            }
        }

        // 라인 구성이 끝났으니 상신 마무리 - 결재자가 없어 이미 완료인 문서(본인 전결)의 문서 확정 후처리를 서버가 실행한다.
        if (!pathFailed)
        {
            var fin = await ApprovalClient.FinalizeAsync(appId);
            if (fin != null && !fin.Success) AppMessageBox.Show(fin.Message ?? "결재 완료 처리 중 오류가 발생했습니다.", "오류");
        }

        _changed = true;
        Toast.Show("결재상신되었습니다.");
        if (!pathFailed) { Close(); return; }   // 정상 상신이면 결재창을 자동으로 닫는다(호출 화면이 _changed를 보고 다시 조회)
        await RefreshAsync();
    }

    private async Task ProcessApproveOrRejectAsync(bool approve)
    {
        if (_appId == null) return;
        var label = approve ? "승인" : "반려";
        var confirm = AppMessageBox.Show($"{label} 처리하시겠습니까?", $"{label} 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = approve
            ? await ApprovalClient.ApproveAsync(_appId.Value, memoOpinion.Text)
            : await ApprovalClient.RejectAsync(_appId.Value, memoOpinion.Text);

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? $"{label} 처리에 실패했습니다.", $"{label} 실패");
            return;
        }

        _changed = true;
        Toast.Show($"{label} 처리되었습니다.");
        Close();   // 승인/반려도 정상 처리되면 결재창을 자동으로 닫는다
        return;
    }

    private async Task CancelApproveAsync()
    {
        if (_appId == null) return;
        var confirm = AppMessageBox.Show("방금 하신 승인을 취소하시겠습니까?", "승인취소 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await ApprovalClient.CancelApproveAsync(_appId.Value);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "승인취소에 실패했습니다.", "승인취소 실패");
            return;
        }

        _changed = true;
        Toast.Show("승인이 취소되었습니다.");
        await RefreshAsync();
    }

    private async Task AcknowledgeAsync()
    {
        if (_appId == null) return;

        var result = await ApprovalClient.AcknowledgeAsync(_appId.Value, memoOpinion.Text);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "확인 처리에 실패했습니다.", "확인 실패");
            return;
        }

        _changed = true;
        Toast.Show("확인되었습니다.");
        await RefreshAsync();
    }


    //저장된 결재경로 적용 
    private void btnApplyRoute_Click(object sender, EventArgs e)
    {

    }
}
