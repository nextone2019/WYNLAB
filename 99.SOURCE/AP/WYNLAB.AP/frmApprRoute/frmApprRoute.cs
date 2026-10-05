using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.AP;

/// <summary>
/// 결재경로관리(개인별 저장 결재라인/수신라인 템플릿, TAPROUTE/TAPROUTEDETAIL) - 결재문서현황과 같은 구조(2026-10-04):
/// 조회조건(사업장/결재경로명) / 왼쪽 결재경로 리스트(grd1) / 오른쪽 결재경로상세(경로명·코드 + 조직도 + 승인부(grd2)·수신부(grd3) 구성).
/// 리스트에서 경로를 고르면 상세가 채워지고, [신규]로 새 경로를 만든다. 저장은 툴바 [저장] 하나로 경로 이름과 승인부/수신부 라인을 함께 저장한다
/// (프로시저가 사원 1명 추가(ADDDETAIL)만 지원해서 라인은 전체 삭제(CLEARDETAIL) 후 화면 순서 그대로 다시 쌓는다).
/// 조직도에서 사원을 고르고 [추가]/화살표 또는 드래그&amp;드롭으로 승인부/수신부에 넣는다. 본인은 추가할 수 없다.
/// </summary>
public partial class frmApprRoute : BaseForm
{
    private DataTable _list = new();
    private List<ApprovalPathDto> _lineRows = new(); // path_type='C'
    private List<ApprovalPathDto> _recvRows = new(); // path_type='R'
    private long? _editingRouteId;   // null이면 신규모드
    private bool _detailDirty;       // 승인부/수신부를 고쳤지만 아직 저장 안 한 상태

    /// <summary>경로 이름뿐 아니라 승인부/수신부 변경도 저장 확인(닫기/경로 이동) 대상이다.</summary>
    protected override bool HasUnsavedChanges => IsDirty || _detailDirty;

    public frmApprRoute()
    {
        InitializeComponent();

        Text = "결재경로관리";
        Controls.Add(BuildScreenHeader());

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");
        txtSearchRouteNm.KeyDown += async (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            await RunQueryAsync();
        };

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        TrackDirty(panData);

        treeEmp.SelectImageList = BuildEmpTreeImageList();
        treeEmp.MouseDown += TreeEmp_MouseDown;
        treeEmp.DoubleClick += (s, e) => AddSelectedEmployee("C");

        grd2.DragEnter += (s, e) => AcceptEmpDrag(e);
        grd3.DragEnter += (s, e) => AcceptEmpDrag(e);
        grd2.DragDrop += (s, e) => DropOnGrid(e, gvw2, _lineRows, "C");
        grd3.DragDrop += (s, e) => DropOnGrid(e, gvw3, _recvRows, "R");
        gvw2.MouseDown += (s, e) => StartRowDrag(gvw2, _lineRows, e);
        gvw3.MouseDown += (s, e) => StartRowDrag(gvw3, _recvRows, e);

        btnAddLine.Click += (s, e) => AddSelectedEmployee("C");
        btnAddRecv.Click += (s, e) => AddSelectedEmployee("R");
        btnToLine.Click += (s, e) => AddSelectedEmployee("C");
        btnToRecv.Click += (s, e) => AddSelectedEmployee("R");
        btnRemoveLine.Click += (s, e) => RemoveFocusedDetailRow(recv: false);
        btnRemoveRecv.Click += (s, e) => RemoveFocusedDetailRow(recv: true);

        EnterNewMode();
        Load += async (s, e) => { await LoadEmpTreeAsync(); await QueryCore(restoreRouteId: null); };
    }

    // ==================== 조회 ====================

    public override async Task QueryClick()
    {
        if (HasUnsavedChanges)
        {
            var confirm = AppMessageBox.Show("저장하지 않은 변경 내용이 있습니다.\n조회하면 변경 내용이 사라집니다. 계속하시겠습니까?",
                "조회 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
        }

        await QueryCore(restoreRouteId: null);
    }

    /// <summary>restoreRouteId가 있으면(저장 직후 재조회) 조회 후 그 경로로 포커스를 되돌린다.</summary>
    private async Task QueryCore(long? restoreRouteId)
    {
        var table = await QueryAsync("USP_AP_ROUTE_Q", new
        {
            p_work_type = "Q",
            p_emp_id = Session.EmpId,
            p_acc_id = cboSearchAccId.EditValue?.ToString(),
            p_route_nm = txtSearchRouteNm.Text.Trim(),
        });

        // 재바인딩 구간엔 행 전환 이벤트(저장 확인)를 끊고, 아래에서 최종 행에 대해 직접 한 번만 상세를 채운다.
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            _list = table;
            grd1.DataSource = _list;

            if (restoreRouteId != null)
            {
                var index = _list.Rows.Cast<DataRow>().ToList().FindIndex(r => Convert.ToInt64(r["route_id"]) == restoreRouteId);
                if (index >= 0) gvw1.FocusedRowHandle = gvw1.GetRowHandle(index);
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        if (gvw1.GetFocusedDataRow() is DataRow focused) await LoadRouteAsync(focused);
        else
        {
            // 조회 공통 규칙: 결과가 없으면 신규 입력 모드로 전환한다.
            EnterNewMode();
            if (restoreRouteId == null) Toast.Show("조회 결과가 없어 신규 입력 상태로 전환했습니다.");
        }
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = SafeExecuteAsync(() => LoadRouteAsync(row.Row), "결재경로 상세 조회"));

    /// <summary>리스트에서 고른 경로의 이름/코드를 상세에 채우고, 그 경로의 승인부/수신부(Q1)를 조회한다.</summary>
    private async Task LoadRouteAsync(DataRow row)
    {
        var routeId = Convert.ToInt64(row["route_id"]);
        _editingRouteId = routeId;

        SuppressDirtyTracking(() =>
        {
            txtRouteNm.Text = row["route_nm"]?.ToString() ?? string.Empty;
            txtRouteId.Text = routeId.ToString();
        });

        var detail = await QueryAsync("USP_AP_ROUTE_Q", new { p_work_type = "Q1", p_route_id = routeId });

        // 응답을 기다리는 사이 다른 경로로 옮겼으면 이 결과는 버린다.
        if (_editingRouteId != routeId) return;

        _lineRows = ToPathRows(detail, "C");
        _recvRows = ToPathRows(detail, "R");
        _detailDirty = false;
        BindDetailGrids();
    }

    private static List<ApprovalPathDto> ToPathRows(DataTable table, string pathType) =>
        table.Rows.Cast<DataRow>()
            .Where(r => r["path_type"]?.ToString() == pathType)
            .Select(r => new ApprovalPathDto
            {
                Sort = Convert.ToInt32(r["sort"]),
                EmpId = Convert.ToInt64(r["emp_id"]),
                EmpNo = r["emp_no"]?.ToString(),
                EmpNm = r["emp_nm"]?.ToString(),
                PathType = pathType,
            })
            .OrderBy(r => r.Sort)
            .ToList();

    private void BindDetailGrids()
    {
        grd2.DataSource = null; grd2.DataSource = _lineRows;
        grd3.DataSource = null; grd3.DataSource = _recvRows;
    }

    // ==================== 신규 / 저장 / 삭제 ====================

    private void EnterNewMode()
    {
        _editingRouteId = null;
        SuppressDirtyTracking(() =>
        {
            txtRouteNm.Text = string.Empty;
            txtRouteId.Text = string.Empty;
        });
        _lineRows = new();
        _recvRows = new();
        _detailDirty = false;
        BindDetailGrids();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        FocusFirstEntryField(panData);
        return Task.CompletedTask;
    }

    /// <summary>경로 이름(+사업장)을 저장하고, 이어서 승인부/수신부 라인을 전체 다시 저장한다. 신규면 방금 만든 경로의 코드를 받아 이어서 쓴다.</summary>
    public override async Task SaveClick()
    {
        var name = txtRouteNm.Text.Trim();
        var accId = cboSearchAccId.EditValue?.ToString();

        ApiResult result;
        long routeId;
        if (_editingRouteId is { } existing)
        {
            routeId = existing;
            result = await SaveAsync("USP_AP_ROUTE_S", new { p_work_type = "U", p_route_id = existing, p_route_nm = name, p_acc_id = accId });
        }
        else
        {
            result = await SaveAsync("USP_AP_ROUTE_S", new { p_work_type = "N", p_route_nm = name, p_acc_id = accId });
            if (!result.Success || !long.TryParse(result.GeneratedCode, out routeId))
            {
                AppMessageBox.Show(result.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        if (!result.Success)
        {
            AppMessageBox.Show(result.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        // 신규였다면 이제부터 이 경로를 편집 중으로 본다(라인 저장이 실패해도 경로는 이미 만들어졌다).
        _editingRouteId = routeId;
        if (!await SaveDetailCoreAsync(routeId)) return;

        Toast.Show("저장되었습니다.");
        IsDirty = false;
        _detailDirty = false;
        await QueryCore(restoreRouteId: routeId);
    }

    /// <summary>선택된 경로의 라인을 서버에 쓴다(전체 삭제 후 화면 순서대로 재삽입). 실패하면 안내하고 false.</summary>
    private async Task<bool> SaveDetailCoreAsync(long routeId)
    {
        var clearResult = await SaveAsync("USP_AP_ROUTE_S", new { p_work_type = "CLEARDETAIL", p_route_id = routeId });
        if (!clearResult.Success)
        {
            AppMessageBox.Show(clearResult.Message ?? "저장에 실패했습니다.", "저장 실패");
            return false;
        }

        foreach (var row in _lineRows.Concat(_recvRows))
        {
            var result = await SaveAsync("USP_AP_ROUTE_S", new
            {
                p_work_type = "ADDDETAIL",
                p_route_id = routeId,
                p_target_emp_no = row.EmpNo,
                p_path_type = row.PathType,
            });
            if (!result.Success)
            {
                AppMessageBox.Show(result.Message ?? "저장에 실패했습니다.", "저장 실패");
                return false;
            }
        }

        return true;
    }

    protected override bool ConfirmDeleteByDefault => false; // 삭제 확인창을 DeleteClick에서 직접 띄움(문서번호 등 상세 문구)

    public override async Task DeleteClick()
    {
        if (_editingRouteId is not { } routeId) return;

        var confirm = AppMessageBox.Show($"결재경로 '{txtRouteNm.Text}'을(를) 삭제하시겠습니까?", "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_AP_ROUTE_S", new { p_work_type = "D", p_route_id = routeId });
        if (!result.Success)
        {
            AppMessageBox.Show(result.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        Toast.Show("삭제되었습니다.");
        EnterNewMode();
        await QueryCore(restoreRouteId: null);
    }

    // ==================== 승인부/수신부 편집 ====================

    /// <summary>결재라인(recv=false) 또는 수신라인(recv=true) 그리드의 선택 행을 지운다 - 각 그리드 위 [삭제] 버튼. 저장은 툴바 [저장].</summary>
    private void RemoveFocusedDetailRow(bool recv)
    {
        DevExpress.XtraGrid.Views.Grid.GridView view = recv ? gvw3 : gvw2;
        var rows = recv ? _recvRows : _lineRows;
        if (view.GetFocusedRow() is not ApprovalPathDto row)
        {
            AppMessageBox.Show("삭제할 행을 먼저 선택해주세요.", "안내");
            return;
        }

        rows.Remove(row);
        RenumberSort(rows, row.PathType);
        _detailDirty = true;
        BindDetailGrids();
    }
    // ---- 조직도(부서트리+사원목록) - popApp.cs와 동일(여러 화면 공용 조회라 ApprovalClient 사용) ----

    private async Task LoadEmpTreeAsync()
    {
        var depts = await ApprovalClient.GetDeptTreeAsync() ?? new List<DeptTreeItemDto>();
        var emps = await ApprovalClient.GetEmployeesAsync() ?? new List<ApprovalEmpItemDto>();

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

    private void TreeEmp_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        var hit = treeEmp.CalcHitInfo(e.Location);
        if (hit.Node == null || hit.Node.GetValue("IsDept") is true) return;

        var emp = NodeToEmp(hit.Node);
        if (emp == null) return;

        treeEmp.DoDragDrop(emp, DragDropEffects.Copy);
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

    // ---- 결재라인/수신라인 그리드 - popApp.cs의 드래그&드롭/재정렬 로직과 동일 ----

    private static void AcceptEmpDrag(DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(typeof(ApprovalPathDto)) == true) e.Effect = DragDropEffects.Move;
        else if (e.Data?.GetDataPresent(typeof(ApprovalEmpItemDto)) == true) e.Effect = DragDropEffects.Copy;
        else e.Effect = DragDropEffects.None;
    }

    private void DropOnGrid(DragEventArgs e, DevExpress.XtraGrid.Views.Grid.GridView view, List<ApprovalPathDto> rows, string pathType)
    {
        if (e.Data?.GetData(typeof(ApprovalPathDto)) is ApprovalPathDto dragged)
        {
            if (!rows.Contains(dragged)) return;

            var pt = view.GridControl.PointToClient(new Point(e.X, e.Y));
            var hit = view.CalcHitInfo(pt);
            var targetIndex = hit.RowHandle >= 0 ? hit.RowHandle : rows.Count - 1;
            targetIndex = Math.Max(0, Math.Min(targetIndex, rows.Count - 1));

            rows.Remove(dragged);
            rows.Insert(targetIndex, dragged);
            RenumberSort(rows, pathType);
            _detailDirty = true;
            BindDetailGrids();
            return;
        }

        if (e.Data?.GetData(typeof(ApprovalEmpItemDto)) is ApprovalEmpItemDto emp)
            AddEmployeeToPath(emp, pathType);
    }

    private void StartRowDrag(DevExpress.XtraGrid.Views.Grid.GridView view, List<ApprovalPathDto> rows, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        var hit = view.CalcHitInfo(new Point(e.X, e.Y));
        if (hit.RowHandle < 0 || hit.RowHandle >= rows.Count) return;

        view.GridControl.DoDragDrop(rows[hit.RowHandle], DragDropEffects.Move);
    }

    private static void RenumberSort(List<ApprovalPathDto> rows, string pathType)
    {
        if (pathType != "C") return; // 수신라인(R)은 순서 개념이 없어(sort=0 고정) 번호를 안 매김
        for (var i = 0; i < rows.Count; i++) rows[i].Sort = i + 1;
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
            Sort = pathType == "C" ? target.Count + 1 : 0,
        });
        _detailDirty = true;
        BindDetailGrids();
    }
}
