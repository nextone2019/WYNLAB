using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

/// <summary>
/// 사업장등록 화면 - TEMPLATE을 복사해서 만듦(GENERIC_DATA_API.md의 범용 데이터 통로 사용,
/// 서버 Controller/Repository 없음 - 메뉴등록(TSMMENU)의 PROC_PREFIX=USP_BA_ACC_ 만으로 동작).
///
/// 지금은 사업장코드/사업장명 2개 필드만 있다 - 자세한 컬럼정의(주소/사업자번호 등)는 나중에
/// 추가 예정. 필드를 늘릴 때는 DB(TBAACC 컬럼 + USP_BA_ACC_Q/S)와 화면(panData 컨트롤 +
/// EnterNewMode/EnterEditMode/SaveClick의 값 채우기/읽기) 양쪽을 같이 늘리면 된다.
///
/// grd1(목록)에서 행을 고르면 그 상세를 panData에 채우고(EnterEditMode), 저장 전이면
/// EnterNewMode로 비워둔다 - frmMinorCode/frmUserAuth가 전부 따르는 표준 패턴.
/// </summary>
public partial class frmAcc : BaseForm
{
    /// <summary>TSMFILE.doc_type 값 - 이 화면(사업장)이 등록하는 첨부파일을 다른 화면의
    /// 첨부파일과 구분하는 용도(frmCust의 FileDocType="BACUST"와 같은 이유). doc_id는 ACC_ID를
    /// 그대로 쓴다.</summary>
    private const string FileDocType = "BAACC";

    private DataTable _list = new();
    private string? _editingCd; // null이면 신규모드
    private List<FileListItemDto> _files = new();

    public frmAcc()
    {
        InitializeComponent();

        Text = "사업장등록";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd1은 조회전용 - 이 화면엔 편집 가능한 그리드가 따로 없다(등록/수정은 우측 panData).
        gvw1.Role = GridRoleWyn.Query;

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. gridColumn1/2는 FieldName이
        // 이미 실제 DB 컬럼명이라 손댈 것 없이 자동 적용된다.
        txtAccId.Tag = new BindingFieldTag("acc_id");
        txtAccNm.Tag = new BindingFieldTag("acc_nm");
        txtBizNo.Tag = new BindingFieldTag("biz_no");
        lookUpEditWyn1.Tag = new BindingFieldTag("cur_cd");
        txtOwnerNm.Tag = new BindingFieldTag("owner_nm");
        textEditWyn1.Tag = new BindingFieldTag("owner_nm_eng");
        txtTel.Tag = new BindingFieldTag("tel");
        txtFax.Tag = new BindingFieldTag("fax");
        txtZipCode.Tag = new BindingFieldTag("zip_code");
        txtAddr1.Tag = new BindingFieldTag("addr1");
        txtAddr2.Tag = new BindingFieldTag("addr2");
        txtAddr1Eng.Tag = new BindingFieldTag("addr1_eng");
        txtAddr2Eng.Tag = new BindingFieldTag("addr2_eng");
        textEditWyn2.Tag = new BindingFieldTag("homepage");
        textEditWyn3.Tag = new BindingFieldTag("email");
        ymdOpenDate.Tag = new BindingFieldTag("open_date");
        txtDetailBizKind.Tag = new BindingFieldTag("biz_kind");
        txtDetailBizType.Tag = new BindingFieldTag("biz_type");
        cboDetailVatType.Tag = new BindingFieldTag("vat_type");
        txtDetailVatRate.Tag = new BindingFieldTag("vat_rate");
        picLogo.Tag = new BindingFieldTag("logo");
        picStamp.Tag = new BindingFieldTag("stamp");

        // 부가세유형(cboDetailVatType, L_CM0004)을 고르면 그 LookUp의 rel_cd1(관리항목1 - 세율)을
        // 세율(txtDetailVatRate)에 그대로 채워준다(frmCust.cboDetailVatType과 같은 이유/패턴).
        // EnterNewMode/EnterEditModeAsync는 이 콤보 값을 채운 바로 다음 줄에서 txtDetailVatRate를
        // DB에 저장된 실제 값으로 다시 덮어쓰므로, 조회/신규 진입 시에는 이 핸들러 결과가 그대로
        // 유지되지 않는다 - 사용자가 직접 콤보를 바꿀 때만 실질적으로 적용된다.
        cboDetailVatType.EditValueChanged += (s, e) =>
        {
            var relCd1 = cboDetailVatType.GetColumnValue("rel_cd1");
            txtDetailVatRate.Text = relCd1 == null || relCd1 == DBNull.Value ? string.Empty : relCd1.ToString();
        };

        // FILE SIZE를 바이트 그대로 안 보여주고 KB/MB 단위로 바꿔 보여준다(frmCust.gvwFile와 같은 이유).
        gvwFile.CustomColumnDisplayText += (s, e) =>
        {
            if (e.Column == gridColumn5 && e.Value is long bytes)
                e.DisplayText = FileSizeFormatter.Format(bytes);
        };

        // 첨부파일(grdFile) - 공통 팝업(popFileUpload)을 doc_type="BAACC"/doc_id=ACC_ID로 열고,
        // 닫히면 목록을 다시 조회한다(frmCust.btnFileAttach와 같은 패턴). 저장 전(신규모드,
        // ACC_ID가 아직 없음)에는 첨부할 대상 자체가 없으므로 먼저 저장하라고 안내한다.
        btnFileAttach.Click += async (s, e) =>
        {
            if (_editingCd == null)
            {
                AppMessageBox.Show("먼저 사업장을 저장한 뒤 첨부파일을 등록할 수 있습니다.", "확인");
                return;
            }
            popFileUpload.ShowAsync(FileDocType, long.Parse(_editingCd), txtAccNm.Text, 0, this);
            await LoadFileListAsync();
        };

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)이 panData의 값 변경을 감지할 수 있도록.
        TrackDirty(panData);

        EnterNewMode();
    }

    /// <summary>사용자가 툴바에서 직접 누른 조회 - 새 검색이므로 이전 선택은 무시하고 1행부터
    /// 보여준다(preserveSelection: false). 저장/삭제 뒤 재조회는 QueryCore를 직접 호출해서
    /// 방금 편집하던 행을 유지한다(feedback_query_refocus_after_save 메모리 참고).</summary>
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>목록 조회 - acc_id가 사람이 입력하는 코드가 아니라 IDENTITY 자동번호로 바뀌면서
    /// (2026-09-08) 검색어로는 더 이상 의미가 없다 - 사업장명으로만 찾는다.</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var keyword = txtAccNm_Q.Text.Trim();

        _list = await QueryAsync("USP_BA_ACC_Q", new
        {
            p_work_type = "Q",
            p_acc_nm = keyword
        });

        var editingCd = preserveSelection ? _editingCd : null;

        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;

            if (editingCd != null)
            {
                var handle = FindRowHandle(editingCd);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingCd == null ? null : FindRow(editingCd);
        if (row != null) await EnterEditModeAsync(row);
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) await EnterEditModeAsync(focusedView.Row);
        else EnterNewMode();
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (_editingCd == null)
        {
            AppMessageBox.Show("삭제할 항목을 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 사업장을 삭제 하시겠습니까?\n\n[{_editingCd}]",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_BA_ACC_S", new
        {
            p_work_type = "D",
            p_acc_id = _editingCd
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        _editingCd = null;
        await QueryClick();
        Toast.Show("삭제되었습니다.");
    }

    // 이 화면엔 하위 그리드가 없다 - BaseForm 추상 멤버라 구현만 비워둔다.
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;

    public override async Task SaveClick()
    {
        // 사업장ID(acc_id)는 이제 IDENTITY 자동번호라(2026-09-08) 사업장명만 필수값이다.
        if (string.IsNullOrWhiteSpace(txtAccNm.Text))
        {
            AppMessageBox.Show("사업장명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingCd == null;

        var result = await SaveAsync("USP_BA_ACC_S", new
        {
            p_work_type = wasNew ? "N" : "U",
            p_acc_id = wasNew ? null : _editingCd,
            p_acc_nm = txtAccNm.Text,
            p_biz_no = txtBizNo.Text,
            p_tel = txtTel.Text,
            p_cur_cd = lookUpEditWyn1.EditValue?.ToString(),
            p_owner_nm = txtOwnerNm.Text,
            p_owner_nm_eng = textEditWyn1.Text,
            p_zip_code = txtZipCode.Text,
            p_addr1 = txtAddr1.Text,
            p_addr2 = txtAddr2.Text,
            p_addr1_eng = txtAddr1Eng.Text,
            p_addr2_eng = txtAddr2Eng.Text,
            p_homepage = textEditWyn2.Text,
            p_email = textEditWyn3.Text,
            p_fax = txtFax.Text,
            p_open_date = ymdOpenDate.YyyyMmDd,
            p_biz_kind = txtDetailBizKind.Text,
            p_biz_type = txtDetailBizType.Text,
            p_vat_type = cboDetailVatType.EditValue?.ToString(),
            p_vat_rate = string.IsNullOrWhiteSpace(txtDetailVatRate.Text) ? null : txtDetailVatRate.Text,
            // 저장 전이면 안 바뀐 것 - USP_BA_ACC_S가 빈 문자열/NULL이면 기존 이미지를 그대로 둔다.
            p_logo = picLogo.ImageBytes is { Length: > 0 } logoBytes ? Convert.ToBase64String(logoBytes) : null,
            p_stamp = picStamp.ImageBytes is { Length: > 0 } stampBytes ? Convert.ToBase64String(stampBytes) : null
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        _editingCd = wasNew ? result.GeneratedCode : _editingCd;
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지
        Toast.Show(wasNew ? "등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>다른 행을 고를 때 panData에 저장 안 된 변경이 있으면 먼저 확인한다
    /// (BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = EnterEditModeAsync(row.Row));

    /// <summary>조회된 목록에서 키(acc_id)로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindRow(string cd) =>
        _list.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["acc_id"]), cd, StringComparison.OrdinalIgnoreCase));

    /// <summary>acc_id로 grd1의 행 핸들을 찾는다. 없으면 null. 컬럼 셀 값으로 찾는 방식은 그 키
    /// 컬럼이 화면에 안 보이는 숨김 컬럼일 때 안 먹힌다(2026-09-11 실제 발견, frmEMP에서 재현) -
    /// FindRow로 찾은 DataRow의 DataTable상 인덱스를 GridView.GetRowHandle로 표시 행 핸들로
    /// 변환하면 키 컬럼이 보이든 안 보이든 항상 동작한다.</summary>
    private int? FindRowHandle(string cd)
    {
        var row = FindRow(cd);
        if (row == null) return null;

        var rowIndex = _list.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    /// <summary>panData를 채우는 부분은 반드시 SuppressDirtyTracking으로 감쌀 것 - 코드가 값을
    /// 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하면 재조회/신규모드 진입
    /// 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = null;
            txtAccId.Text = string.Empty; // IDENTITY라 저장 후에야 채워진다(ReadOnly는 Designer에서 항상 true)
            txtAccNm.Text = string.Empty;
            txtBizNo.Text = string.Empty;
            txtTel.Text = string.Empty;
            lookUpEditWyn1.EditValue = string.Empty;
            txtOwnerNm.Text = string.Empty;
            textEditWyn1.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            txtAddr1.Text = string.Empty;
            txtAddr2.Text = string.Empty;
            txtAddr1Eng.Text = string.Empty;
            txtAddr2Eng.Text = string.Empty;
            textEditWyn2.Text = string.Empty;
            textEditWyn3.Text = string.Empty;
            txtFax.Text = string.Empty;
            ymdOpenDate.YyyyMmDd = null;
            txtDetailBizKind.Text = string.Empty;
            txtDetailBizType.Text = string.Empty;
            cboDetailVatType.EditValue = string.Empty;
            txtDetailVatRate.Text = string.Empty;
            picLogo.ImageBytes = null;
            picStamp.ImageBytes = null;
            _files = new List<FileListItemDto>();
            grdFile.DataSource = null;
            grdFile.DataSource = _files;
        });
        txtAccNm_Q.Focus();
    }

    private async Task EnterEditModeAsync(DataRow row)
    {
        SuppressDirtyTracking(() =>
        {
            _editingCd = Str(row, "acc_id");
            txtAccId.Text = _editingCd;
            txtAccNm.Text = Str(row, "acc_nm");
            txtBizNo.Text = Str(row, "biz_no");
            txtTel.Text = Str(row, "tel");
            lookUpEditWyn1.EditValue = Str(row, "cur_cd");
            txtOwnerNm.Text = Str(row, "owner_nm");
            textEditWyn1.Text = Str(row, "owner_nm_eng");
            txtZipCode.Text = Str(row, "zip_code");
            txtAddr1.Text = Str(row, "addr1");
            txtAddr2.Text = Str(row, "addr2");
            txtAddr1Eng.Text = Str(row, "addr1_eng");
            txtAddr2Eng.Text = Str(row, "addr2_eng");
            textEditWyn2.Text = Str(row, "homepage");
            textEditWyn3.Text = Str(row, "email");
            txtFax.Text = Str(row, "fax");
            ymdOpenDate.YyyyMmDd = Str(row, "open_date");
            txtDetailBizKind.Text = Str(row, "biz_kind");
            txtDetailBizType.Text = Str(row, "biz_type");
            cboDetailVatType.EditValue = Str(row, "vat_type");
            txtDetailVatRate.Text = Str(row, "vat_rate");
            SetImageFromBase64(picLogo, Str(row, "logo"));
            SetImageFromBase64(picStamp, Str(row, "stamp"));
        });

        await LoadFileListAsync();
    }

    /// <summary>grdFile(읽기전용 요약 그리드) 갱신 - 실제 업로드/다운로드/삭제는 popFileUpload
    /// 팝업에서 하고, 여기서는 "이 사업장에 첨부파일이 몇 건 있는지"만 보여준다(frmCust.
    /// LoadFileListAsync와 같은 패턴).</summary>
    private async Task LoadFileListAsync()
    {
        if (_editingCd == null)
        {
            _files = new List<FileListItemDto>();
        }
        else
        {
            try
            {
                var url = $"api/files?docType={FileDocType}&docId={_editingCd}&docSerl=0";
                _files = await ApiClient.GetAsync<List<FileListItemDto>>(url) ?? new List<FileListItemDto>();
            }
            catch (Exception ex)
            {
                AppMessageBox.Show($"첨부파일 목록 조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
                _files = new List<FileListItemDto>();
            }
        }

        grdFile.DataSource = null;
        grdFile.DataSource = _files;
    }

    /// <summary>서버가 돌려준 Base64 문자열(logo/stamp 컬럼, image -> JSON 직렬화 시 자동으로
    /// Base64가 됨)을 PictureEditWyn.ImageBytes에 그대로 되돌린다(frmEMP.SetPhotoFromBase64와
    /// 같은 이유/패턴 - 값이 없거나 유효한 이미지가 아니면 조용히 비운다).</summary>
    private static void SetImageFromBase64(PictureEditWyn control, string? base64)
    {
        if (string.IsNullOrEmpty(base64))
        {
            control.ImageBytes = null;
            return;
        }

        try
        {
            control.ImageBytes = Convert.FromBase64String(base64);
        }
        catch
        {
            control.ImageBytes = null;
        }
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
}
