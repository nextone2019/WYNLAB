// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-17.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SM;

public partial class frmMessage : BaseForm
{
    private DataTable _list = new();
    private string? _editingKey; // null이면 신규모드

    public frmMessage()
    {
        InitializeComponent();

        Text = "쪽지함";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        // 다른 마스터 행을 고를 때 panData에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용). 이름 있는
        // 메서드로 등록해야 QueryCore가 grd1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다(바로
        // 아래 Gvw1_FocusedRowObjectChanged 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 - 손으로 만든 화면 여러 개에서 이게 빠져있던 걸 발견하고 템플릿에도 추가함).
        txtDetailMsgId.Tag = new BindingFieldTag("msg_id");
        txtDetailBoxType.Tag = new BindingFieldTag("box_type");
        txtDetailFromEmpNm.Tag = new BindingFieldTag("from_emp_nm");
        txtDetailToEmpNo.Tag = new BindingFieldTag("to_emp_no");
        txtDetailToEmpNm.Tag = new BindingFieldTag("to_emp_nm");
        txtDetailTitle.Tag = new BindingFieldTag("title");
        memDetailContent.Tag = new BindingFieldTag("content");
        chkDetailReadYn.Tag = new BindingFieldTag("read_yn");
        txtDetailRegDt.Tag = new BindingFieldTag("reg_dt");

        // 발신자(보낸사람)/구분/받는사람명/보낸일시는 서버가 채우는 값이라(from_emp_id는 로그인
        // 세션에서, box_type/to_emp_nm/reg_dt는 조회 시 JOIN으로) 화면에서 직접 입력할 이유가
        // 없다 - 직접 입력 가능한 필드처럼 보이면 오히려 혼란스럽다(2026-09-17).
        txtDetailBoxType.Properties.ReadOnly = true;
        txtDetailFromEmpNm.Properties.ReadOnly = true;
        txtDetailToEmpNm.Properties.ReadOnly = true;
        txtDetailRegDt.Properties.ReadOnly = true;

        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>사용자가 직접 누른 조회(preserveSelection: false)는 새 검색이므로 0번 행부터,
    /// 저장/삭제 뒤의 내부 재조회(preserveSelection: true)는 방금 편집하던 행에 포커스를 되돌려
    /// panData까지 그 값 그대로 다시 채운다 - 안 그러면 저장 직후 조회했을 때 panData가 안 채워진
    /// 것처럼 보인다(2026-09-06 요청, frmCust/frmMinorCode/frmAcc 등과 같은 패턴).</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_emp_no"] = Session.EmpNo, // 내가 보낸 것+받은 것만(USP_SM_MESSAGE_Q가 이 값으로 필터)
            ["p_title"] = txtTitle.Text,
        };
        _list = await QueryAsync("USP_SM_MESSAGE_Q", p);

        var editingKey = preserveSelection ? _editingKey : null;

        // 저장 직후(SaveClick -> QueryCore)엔 아직 IsDirty가 true로 남아있을 수 있다(DataTable.
        // AcceptChanges()는 RowChanged를 안 냄) - 이 재바인딩이 그 상태에서 자동으로 0번 행에
        // 포커스를 주면 ConfirmMasterRowSwitch가 "변경사항이 있다"고 또 확인창을 띄우는 오작동이
        // 생긴다. 아래에서 직접 EnterNewMode/OnMasterSelected를 호출해 최종 상태를 맞추므로
        // 이 재바인딩 구간만 조용히 지나가면 된다.
        //
        // 구독을 잠깐 끊는 이유(2026-09-08 실제 발견): SuppressMasterRowSwitchConfirm은 "확인창"만
        // 막지, 안에서 gvw1.FocusedRowHandle을 바꾸면 FocusedRowObjectChanged 자체는 그대로
        // 발생한다 - 그 이벤트가 다시 OnMasterSelected를 부르고, 바로 아래에서 같은 행에 대해 또
        // 한 번 OnMasterSelected를 부른다. 명시적 호출 하나만으로 충분하므로, 재바인딩 구간에서는
        // 이벤트를 끊어 중복 호출 자체를 없앤다(frmAcc.cs가 이미 쓰던 패턴).
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;
            if (editingKey != null)
            {
                var handle = FindRowHandle(editingKey);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingKey == null ? null : FindRow(editingKey);
        if (row != null) OnMasterSelected(row);
        // editingKey가 없으면(최초 조회, preserveSelection=false) grd1을 바인딩한 직후 DevExpress가
        // 스스로 0번 행에 포커스를 준다 - 그 자동 포커스 행을 그대로 panData에 채운다. 여기서
        // EnterNewMode()로 바로 넘어가면(예전 버전의 실제 버그) 목록엔 데이터가 있는데 처음 눌러보기
        // 전까지 panData는 계속 비어있는 것처럼 보인다(2026-09-08 실제 발견 - frmItem 최초 조회 시
        // panData 안 채워짐). frmAcc.cs가 이미 쓰던 패턴 그대로 가져온다.
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) OnMasterSelected(focusedView.Row);
        else EnterNewMode();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => OnMasterSelected(row.Row));

    /// <summary>조회된 목록에서 키(msg_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["msg_id"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키(msg_id)로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로
    /// 찾는 방식(gvw1.Columns[fieldName] + GetRowCellValue)은 그 키 컬럼이 화면에 안 보이는 숨김
    /// 컬럼(Visible=false)일 때 안 먹힌다(2026-09-11 실제 발견 - 저장 후 재조회하면 선택된 행이
    /// 안 돌아오고 항상 0번 행으로 가던 버그, frmEMP의 emp_id처럼 PK 컬럼을 그리드에 안 보이게
    /// 둔 화면에서 재현됨). FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로
    /// 표시 행 핸들로 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다.</summary>
    private int? FindRowHandle(string key)
    {
        var row = FindRow(key);
        if (row == null) return null;

        var rowIndex = _list.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    private void OnMasterSelected(DataRow row)
    {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않게 감싼다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["msg_id"]?.ToString();
        txtDetailMsgId.Text = row["msg_id"]?.ToString() ?? string.Empty;
        txtDetailBoxType.Text = row["box_type"]?.ToString() ?? string.Empty;
        txtDetailFromEmpNm.Text = row["from_emp_nm"]?.ToString() ?? string.Empty;
        txtDetailToEmpNo.Text = row["to_emp_no"]?.ToString() ?? string.Empty;
        txtDetailToEmpNm.Text = row["to_emp_nm"]?.ToString() ?? string.Empty;
        txtDetailTitle.Text = row["title"]?.ToString() ?? string.Empty;
        memDetailContent.Text = row["content"]?.ToString() ?? string.Empty;
        chkDetailReadYn.Checked = row["read_yn"]?.ToString() == "Y";
        txtDetailRegDt.Text = row["reg_dt"]?.ToString() ?? string.Empty;
        });
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
        txtDetailMsgId.Text = string.Empty;
        txtDetailBoxType.Text = string.Empty;
        txtDetailFromEmpNm.Text = Session.UserNm; // 새 쪽지는 항상 내가 보낸 사람
        txtDetailToEmpNo.Text = string.Empty;
        txtDetailToEmpNm.Text = string.Empty;
        txtDetailTitle.Text = string.Empty;
        memDetailContent.Text = string.Empty;
        chkDetailReadYn.Checked = false;
        txtDetailRegDt.Text = string.Empty;
        });
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다(TplSingleGrid.cs와 같은 가드).
        if (string.IsNullOrEmpty("USP_SM_MESSAGE_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtDetailToEmpNo.Text))
        {
            AppMessageBox.Show("받는사람(사번)을 입력해주세요.", "확인");
            return;
        }

        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_msg_id"] = txtDetailMsgId.Text,
            ["p_to_emp_no"] = txtDetailToEmpNo.Text,
            ["p_title"] = txtDetailTitle.Text,
            ["p_content"] = memDetailContent.Text,
            ["p_read_yn"] = chkDetailReadYn.Checked ? "Y" : "N",
        };

        var headerResult = await SaveAsync("USP_SM_MESSAGE_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        _editingKey ??= headerResult.GeneratedCode;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_msg_id"] = _editingKey,
        };
        var result = await SaveAsync("USP_SM_MESSAGE_S", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        _editingKey = null;
        Toast.Show("삭제되었습니다.");
        await QueryClick();
    }
}
