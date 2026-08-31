using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 부서등록 화면 - 왼쪽은 부서 계층을 보여주는 tree1(dept_cd/par_dept_cd), 오른쪽 위는 상세
/// 입력(panData), 오른쪽 아래 grd2는 tree1에서 고른 부서의 소속 사원 목록(조회 전용)이다.
///
/// USP_BA_DEPT_Q 하나를 work_type으로 나눠 쓴다 - 'Q'는 부서 목록(트리), 'Q1'은 그 중 한
/// 부서의 소속 사원 목록. @p_dept_cd 의미가 work_type마다 다르다(Q=검색어, Q1=정확히 일치하는
/// 부서코드) - USP_SM_MINORCODE_Q의 Q/Q1 분리와 같은 방식.
/// </summary>
public partial class frmDept : BaseForm
{
    private DataTable _depts = new();
    private DataTable _employees = new();
    private string? _editingDeptCd; // null이면 신규모드

    public frmDept()
    {
        InitializeComponent();

        Text = "부서등록";
        MenuCd = "BA_DEPT";

        tree1.KeyFieldName = "dept_cd";
        tree1.ParentFieldName = "par_dept_cd";
        tree1.FocusedNodeChanged += Tree1_FocusedNodeChanged;

        // grd2(소속 사원)는 조회 전용이다 - 사원 등록/수정은 frmEmp에서 한다. Role=Query가 셀
        // 편집과 네비게이터 추가/삭제/편집 버튼까지 전부 막아준다. 헤더의 +/x 버튼(panelWyn7)은
        // 그리드 네비게이터와 별개 UI라 여기서도 따로 숨겨야 한다.
        gvw2.Role = GridRoleWyn.Query;
        panelWyn7.Visible = false;

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync) - grd2는 조회 전용이라 DataTable은
        // 추적할 필요 없이 panData만 걸면 된다.
        TrackDirty(panData);

        ConfigureBindingFieldTags();

        EnterNewMode();
    }

    /// <summary>개발자용 마우스오버 툴팁이 읽어갈 BindingField를 컨트롤마다 심어둔다 - 실제
    /// 툴팁 적용은 BaseForm.ApplyBindingFieldTooltips가 모든 화면에 공통으로 처리한다(원래는
    /// 이 화면(frmDept)에만 시험 적용했던 EnableBindingTooltips()가 직접 .ToolTip을 채웠는데,
    /// "모든 화면에 공통기능으로 적용해달라"는 지시로 base로 옮기면서 여기는 Tag만 세팅하는
    /// 역할로 바뀌었다, 2026-08-31). WYNLAB 화면은 실제 WinForms 데이터바인딩을 안 쓰고
    /// 코드로 직접 값을 채우는 방식이라(txtDeptCd.Text = row["dept_cd"]) 이 매핑 정보가
    /// 컨트롤 자체엔 원래 안 남는다 - 그래서 컨트롤 선언 시(또는 여기처럼 생성자에서) Tag에
    /// BindingFieldTag로 한 번 심어둬야 한다.
    ///
    /// 그리드/트리 컬럼(colTreeDeptCd, colEmpNo 등)은 여기서 손댈 게 없다 - FieldName이 이미
    /// 실제 DB 컬럼명이라 BaseForm이 전부 자동으로 잡는다.</summary>
    private void ConfigureBindingFieldTags()
    {
        txtDeptCd.Tag = new BindingFieldTag("dept_cd");
        txtDeptNm.Tag = new BindingFieldTag("dept_nm");
        txtParDeptCd.Tag = new BindingFieldTag("par_dept_cd");
        txtParDeptNm.Tag = new BindingFieldTag("dept_nm"); // 부모 부서명(popup MapField로 채워짐)
        txtDeptType.Tag = new BindingFieldTag("dept_type");
        txtRemark.Tag = new BindingFieldTag("remark");
    }

    /// <summary>부서 트리 조회. 검색조건을 늘리려면 USP_BA_DEPT_Q의 Q 분기에 파라미터를
    /// 추가하면 된다.
    ///
    /// 저장 후 재조회(SaveClick -> QueryClick)에서도 방금 편집하던 부서가 그대로 선택돼 있어야
    /// 한다 - 무조건 EnterNewMode로 비우면 저장 직후 우측 패널이 빈 채로 보인다(frmMinorCode에서
    /// 실제로 겪은 문제와 같은 패턴). tree1.DataSource를 다시 세팅하면 DevExpress가 자동으로
    /// 첫 노드에 포커스를 주면서 FocusedNodeChanged가 발생하는데, 그게 복원하려는 노드가
    /// 아니면 잠깐 엉뚱한 부서로 EnterEditMode가 실행되므로, 포커스 복원이 끝날 때까지
    /// 이벤트를 끊어두고 마지막에 한 번만 명시적으로 처리한다.</summary>
    public override async Task QueryClick()
    {
        var detpCd = txtDeptCd_Q.Text.Trim();

        _depts = await QueryAsync("USP_BA_DEPT_Q", new
        {
            p_work_type = "Q",
            p_dept_cd = detpCd
        });

        var editingDeptCd = _editingDeptCd;

        tree1.FocusedNodeChanged -= Tree1_FocusedNodeChanged;
        try
        {
            tree1.DataSource = _depts;

            if (editingDeptCd != null)
            {
                var node = tree1.FindNodeByFieldValue("dept_cd", editingDeptCd);
                if (node != null) tree1.FocusedNode = node;
            }
        }
        finally
        {
            tree1.FocusedNodeChanged += Tree1_FocusedNodeChanged;
        }

        var row = editingDeptCd == null ? null : FindDeptRow(editingDeptCd);
        if (row != null) EnterEditMode(row);
        else EnterNewMode();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (_editingDeptCd == null)
        {
            AppMessageBox.Show("삭제할 부서를 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 부서를 삭제 하시겠습니까?\n\n[{_editingDeptCd}] {txtDeptNm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_BA_DEPT_S", new
        {
            p_work_type = "D",
            p_dept_cd = _editingDeptCd
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    // grd2는 조회 전용이라 여기 두 개는 쓸 일이 없다(생성자에서 버튼도 숨김).
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtDeptCd.Text) || string.IsNullOrWhiteSpace(txtDeptNm.Text))
        {
            AppMessageBox.Show("부서코드와 부서명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingDeptCd == null;

        var result = await SaveAsync("USP_BA_DEPT_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_dept_cd = txtDeptCd.Text.ToUpper(),
            p_dept_nm = txtDeptNm.Text,
            p_par_dept_cd = string.IsNullOrWhiteSpace(txtParDeptCd.Text) ? null : txtParDeptCd.Text,
            p_dept_type = txtDeptType.Text,
            p_remark = txtRemark.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        await QueryClick();
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    private void Tree1_FocusedNodeChanged(object? sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
    {
        if (e.Node == null) return;
        var deptCd = e.Node.GetValue("dept_cd") as string;
        if (string.IsNullOrEmpty(deptCd)) return;

        var row = FindDeptRow(deptCd!);
        if (row != null) EnterEditMode(row);
    }

    /// <summary>조회된 부서 목록에서 코드로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindDeptRow(string deptCd) =>
        _depts.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["dept_cd"]), deptCd, StringComparison.OrdinalIgnoreCase));

    /// <summary>panData를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안 그러면 코드가
    /// 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인해서, 조회/트리
    /// 클릭 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingDeptCd = null;
            txtDeptCd.Text = string.Empty;
            txtDeptCd.ReadOnly = false;
            txtDeptNm.Text = string.Empty;
            txtParDeptCd.Text = string.Empty;
            txtParDeptNm.Text = string.Empty;
            txtDeptType.Text = string.Empty;
            txtRemark.Text = string.Empty;

            _employees = _employees.Clone();
            grd2.DataSource = _employees;
        });
        txtDeptCd.Focus();
    }

    private void EnterEditMode(DataRow dept)
    {
        var deptCd = Str(dept, "dept_cd");
        var isSameDept = _editingDeptCd == deptCd;

        SuppressDirtyTracking(() =>
        {
            _editingDeptCd = deptCd;
            txtDeptCd.Text = deptCd;
            txtDeptCd.ReadOnly = true; // 부서코드는 키라 수정 불가
            txtDeptNm.Text = Str(dept, "dept_nm");
            var parDeptCd = Str(dept, "par_dept_cd");
            txtParDeptCd.Text = parDeptCd;
            // par_dept_cd는 있지만 그 이름은 이 행에 안 담겨 있다 - 이미 받아둔 부서 목록(_depts,
            // 트리 전체)에서 같은 코드를 찾아 채운다(추가 조회 없이 즉시 가능).
            var parDeptRow = parDeptCd.Length == 0 ? null : FindDeptRow(parDeptCd);
            txtParDeptNm.Text = parDeptRow == null ? string.Empty : Str(parDeptRow, "dept_nm");
            txtDeptType.Text = Str(dept, "dept_type");
            txtRemark.Text = Str(dept, "remark");
        });

        if (!isSameDept) _ = LoadEmployeesAsync(deptCd);
    }

    private async Task LoadEmployeesAsync(string deptCd)
    {
        _employees = await QueryAsync("USP_BA_DEPT_Q", new
        {
            p_work_type = "Q1",
            p_dept_cd = deptCd
        });

        grd2.DataSource = _employees;
    }

    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? Convert.ToString(row[columnName]) ?? string.Empty
            : string.Empty;

    /// <summary>ApiResult.ErrorCode는 SQL 예외(ERROR_NUMBER())일 때만 채워진다(0이면 업무로직
    /// 판단만으로 실패 - 예: 필수값 누락) - 그럴 때만 메시지에 오류번호를 같이 보여준다.</summary>
    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }

    // grd2는 조회 전용이라 아래 두 핸들러는 실질적으로 안 불린다(panelWyn7 숨김) - Designer.cs가
    // 이 이름으로 이벤트를 연결해두고 있어서 시그니처만 유지한다.
    private async void btnAddRow2_Click(object sender, EventArgs e)
    {
        await NewRowClick();
    }

    private async void btnDeletRow2_Click(object sender, EventArgs e)
    {
        await DeleteRowClick();
    }
}
