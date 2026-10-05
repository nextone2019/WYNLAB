// AI Builder가 마스터-폼-서브그리드 템플릿을 복제해서 자동 생성 - 2026-09-17.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM;

public partial class frmBoard : BaseForm
{
    /// <summary>TSMFILE.doc_type 값 - 이 화면(공지사항)의 첨부파일을 다른 화면의 첨부파일과
    /// 구분하는 용도(frmCust="BACUST", frmAcc="BAACC"와 같은 규칙). doc_id는 board_id.</summary>
    private const string FileDocType = "SMBOARD";

    private DataTable _list = new();
    private string? _editingKey; // null이면 신규모드
    private List<FileListItemDto> _files = new();

    public frmBoard()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "공지사항등록";


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
        txtDetailBoardId.Tag = new BindingFieldTag("board_id");
        cboDetailAccId.Tag = new BindingFieldTag("acc_id");
        txtDetailTitle.Tag = new BindingFieldTag("title");
        txtDetailContent.Tag = new BindingFieldTag("content");
        txtDetailEmpNm.Tag = new BindingFieldTag("emp_nm");
        chkDetailImportantYn.Tag = new BindingFieldTag("important_yn");
        chkDetailUseYn.Tag = new BindingFieldTag("use_yn");
        txtDetailRegDt.Tag = new BindingFieldTag("reg_dt");

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 마스터 행으로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기댄다 - grd2/grd3(편집 가능한 하위 그리드)는
        TrackDirty(panData);

        // FILE SIZE를 바이트 그대로 안 보여주고 KB/MB 단위로 바꿔 보여준다(frmAcc.gvwFile와 같은 이유).
        gvwFile.CustomColumnDisplayText += (s, e) =>
        {
            if (e.Column == colFFileSize && e.Value is long bytes)
                e.DisplayText = FileSizeFormatter.Format(bytes);
        };

        // 첨부파일 - 공통 팝업(popFileUpload)을 doc_type="SMBOARD"/doc_id=board_id로 열고, 닫히면
        // 목록을 다시 조회한다(frmAcc.btnFileAttach와 같은 패턴). 저장 전(신규모드, board_id가 아직
        // 없음)에는 첨부할 대상 자체가 없으므로 먼저 저장하라고 안내한다.
        btnFileAttach.Click += async (s, e) =>
        {
            if (_editingKey == null)
            {
                AppMessageBox.Show("먼저 공지사항을 저장한 뒤 첨부파일을 등록할 수 있습니다.", "확인");
                return;
            }
            popFileUpload.ShowAsync(FileDocType, long.Parse(_editingKey), txtDetailTitle.Text, 0, this);
            await LoadFileListAsync();
        };

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
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_title"] = txtTitle.Text,
        };
        _list = await QueryAsync("USP_SM_BOARD_Q", p);

        var editingKey = preserveSelection ? _editingKey : null;

        // 저장 직후(SaveClick -> QueryCore)엔 아직 IsDirty가 true로 남아있을 수 있다(DataTable.
        // AcceptChanges()는 RowChanged를 안 냄) - 이 재바인딩이 그 상태에서 자동으로 0번 행에
        // 포커스를 주면 ConfirmMasterRowSwitch가 "변경사항이 있다"고 또 확인창을 띄우는 오작동이
        // 생긴다. 아래에서 직접 EnterNewMode/OnMasterSelectedAsync를 호출해 최종 상태를 맞추므로
        // 이 재바인딩 구간만 조용히 지나가면 된다.
        //
        // 구독을 잠깐 끊는 이유(2026-09-08 실제 발견): SuppressMasterRowSwitchConfirm은 "확인창"만
        // 막지, 안에서 gvw1.FocusedRowHandle을 바꾸면 FocusedRowObjectChanged 자체는 그대로
        // 발생한다 - 그 이벤트가 다시 OnMasterSelectedAsync를 fire-and-forget으로 부르고, 바로
        // 아래에서 같은 행에 대해 또 한 번(이번엔 await로) OnMasterSelectedAsync를 부른다. 저장
        // 직후처럼 그 두 호출이 겹치면(둘 다 grd2/grd3용 LoadDetailAsync까지 비동기로 이어짐)
        // 나중에 끝나는 쪽이 먼저 끝난 쪽의 TrackDirty 구독/그리드 바인딩을 덮어써서, panData를
        // 닫아도 될 상태로 다시 정리했다고 여겼던 IsDirty가 조용히 다시 true가 되는 경우가
        // 있었다(저장 버튼을 누르고 바로 탭을 닫아도 "변경 내역이 존재합니다" 확인창이 뜸). 아래
        // 명시적 호출 하나만으로 충분하므로, 재바인딩 구간에서는 이벤트를 끊어 중복 호출 자체를
        // 없앤다(frmAcc.cs가 이미 쓰던 패턴).
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

    /// <summary>조회된 목록에서 키(board_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string key) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["board_id"]), key, StringComparison.OrdinalIgnoreCase));

    /// <summary>키(board_id)로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로
    /// 찾는 방식(gvw1.Columns[fieldName] + GetRowCellValue)은 그 키 컬럼이 화면에 안 보이는 숨김
    /// 컬럼(Visible=false)일 때 안 먹힌다(2026-09-11 실제 발견 - 저장 후 재조회하면 선택된 행이
    /// 안 돌아오고 항상 0번 행으로 가던 버그, frmEMP의 emp_id처럼 PK 컬럼을 그리드에 안 보이게
    /// 둔 화면에서 재현됨). FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로
    /// 표시 행 핸들로 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다 - 앞으로 이 템플릿에서
    /// 복제되는 모든 화면에 자동으로 적용된다.</summary>
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
            _editingKey = row["board_id"]?.ToString();
        txtDetailBoardId.Text = row["board_id"]?.ToString() ?? string.Empty;
        cboDetailAccId.EditValue = row["acc_id"]?.ToString() ?? string.Empty;
        txtDetailTitle.Text = row["title"]?.ToString() ?? string.Empty;
        txtDetailContent.Text = row["content"]?.ToString() ?? string.Empty;
        txtDetailEmpNm.Text = row["emp_nm"]?.ToString() ?? string.Empty;
        chkDetailImportantYn.Checked = row["important_yn"]?.ToString() == "Y";
        chkDetailUseYn.Checked = row["use_yn"]?.ToString() == "Y";
        txtDetailRegDt.Text = row["reg_dt"]?.ToString() ?? string.Empty;
        });

        _ = LoadFileListAsync();
    }

    /// <summary>grdFile(읽기전용 요약 그리드) 갱신 - 실제 업로드/다운로드/삭제는 popFileUpload
    /// 팝업에서 하고, 여기서는 "이 공지에 첨부파일이 몇 건 있는지"만 보여준다(frmAcc.
    /// LoadFileListAsync와 같은 패턴). 행을 빠르게 넘기면 늦게 끝난 이전 행의 응답이 현재 행의
    /// 목록을 덮어쓸 수 있어서, 응답이 왔을 때 아직 같은 공지를 보고 있는지 확인한다.</summary>
    private async Task LoadFileListAsync()
    {
        var key = _editingKey;
        var files = new List<FileListItemDto>();
        if (key != null)
        {
            try
            {
                var url = $"api/files?docType={FileDocType}&docId={key}&docSerl=0";
                files = await ApiClient.GetAsync<List<FileListItemDto>>(url) ?? new List<FileListItemDto>();
            }
            catch (Exception ex)
            {
                AppMessageBox.Show($"첨부파일 목록 조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            }
        }

        if (key != _editingKey) return;
        _files = files;
        grdFile.DataSource = null;
        grdFile.DataSource = _files;
    }

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
        txtDetailBoardId.Text = string.Empty;
        cboDetailAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        txtDetailTitle.Text = string.Empty;
        txtDetailContent.Text = string.Empty;
        txtDetailEmpNm.Text = string.Empty;
        chkDetailImportantYn.Checked = false;
        chkDetailUseYn.Checked = false;
        txtDetailRegDt.Text = string.Empty;
        });

        _files = new List<FileListItemDto>();
        grdFile.DataSource = null;
        grdFile.DataSource = _files;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다(TplSingleGrid.cs와 같은 가드).
        if (string.IsNullOrEmpty("USP_SM_BOARD_S"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        // ---- 1) 헤더(panData -> USP_SM_BOARD_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_board_id"] = txtDetailBoardId.Text,
            ["p_acc_id"] = cboDetailAccId.EditValue?.ToString() ?? string.Empty,
            ["p_title"] = txtDetailTitle.Text,
            ["p_content"] = txtDetailContent.Text,
            ["p_important_yn"] = chkDetailImportantYn.Checked ? "Y" : "N",
            ["p_use_yn"] = chkDetailUseYn.Checked ? "Y" : "N",
        };

        var headerResult = await SaveAsync("USP_SM_BOARD_S", headerParams);
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
            ["p_board_id"] = _editingKey,
        };
        var result = await SaveAsync("USP_SM_BOARD_S", p);
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
