// AI Builder가 마스터-폼-탭그리드 템플릿을 복제해서 자동 생성 - 2026-09-04.
// 디자인(제목영역/여백/색상)을 바꾸려면 이 파일이 아니라 원본 템플릿(99.SOURCE/TEMPLATE/WYNLAB.TEMPLATE)을 고치세요.
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Popup;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.BA;

public partial class frmCust : BaseForm
{
    /// <summary>TSMFILE.doc_type 값 - 이 화면(거래처)이 등록하는 첨부파일을 다른 화면의
    /// 첨부파일과 구분하는 용도. doc_id는 CUST_ID를 그대로 쓴다.</summary>
    private const string FileDocType = "BACUST";

    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private DataTable _detail3 = new(); // 거래처분류(BA0003) 체크박스 목록 - grd4
    private List<FileListItemDto> _files = new();
    private long? _editingKey; // null이면 신규모드

    public frmCust()
    {
        InitializeComponent();

        Text = "거래처등록";


        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        // 다른 거래처를 고를 때 panData/grd2/grd3에 저장 안 된 변경이 있으면 먼저 확인한다
        // (BaseForm.ConfirmMasterRowSwitch 참고, 2026-09-06 - 모든 화면 공통 적용). 이름 있는
        // 메서드로 등록해야 QueryCore가 grd1을 다시 그리는 동안 잠깐 구독을 끊을 수 있다
        // (Gvw1_FocusedRowObjectChanged 설명 참고) - 람다로 등록하면 나중에 뗄 방법이 없다.
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // grd2/grd3는 MasterFormSubGrid의 grd2(조회전용)와 달리 편집 가능하다 - 각자 자기
        // 저장프로시저(USP_BA_CUST_S_1/USP_BA_CUST_S_2)로 저장되기 때문. Role=Edit이면 그리드
        // 자신의 EmbeddedNavigator에도 추가/삭제 버튼이 뜬다(탭당 하나씩, 독립 동작).
        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.RowAdd += (s, e) => gvw2.AddNewRow();
        gvw2.RowDelete += (s, e) =>
        {
            try { if (gvw2.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        gvw3.Role = GridRoleWyn.Edit;
        gvw3.HighlightFocusedRow = true;
        gvw3.RowAdd += (s, e) => gvw3.AddNewRow();
        gvw3.RowDelete += (s, e) =>
        {
            try { if (gvw3.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // grd4(거래처분류)는 BA0003 코드가 행을 추가/삭제하는 목록이 아니라 항상 전체 코드를
        // 보여주고 체크박스(is_member)만 토글하는 고정 목록이라 grd2/grd3와 달리 Role=Edit(행
        // 추가/삭제)을 안 쓴다 - 체크박스 클릭 자체는 GridViewWynBehavior가 앱 전역에서 처리한다
        // ([[feedback_grid_checkbox_double_toggle_fix]]).
        gvw4.Role = GridRoleWyn.Query;
        gvw4.HighlightFocusedRow = true;

        // panelWyn1의 공용 추가/삭제 버튼 - 현재 활성 탭의 그리드에 적용(각 그리드 자체
        // EmbeddedNavigator와 별개로, 탭을 안 넘나들어도 되는 지름길).
        btnAddRow2.Click += (s, e) => ActiveDetailView().AddNewRow();
        btnDeletRow2.Click += (s, e) =>
        {
            try { if (ActiveDetailView().GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // 부가세유형(cboDetailVatType, L_CM0004)을 고르면 그 LookUp의 rel_cd1(관리항목1 - 세율)을
        // 세율(txtDetailVatRate)에 그대로 채워준다(2026-09-06 요청). rel_cd1은 sysLookupC에
        // width=0으로 등록해서 팝업에는 안 보이지만 데이터소스 컬럼으로는 남아있다(LookUpEditWyn.
        // LoadFromLookupKeyAsync/ComboLookupColumnBuilder 참고) - GetColumnValue로 그대로 읽는다.
        // OnMasterSelectedAsync/EnterNewMode는 이 콤보 값을 채운 바로 다음 줄에서 txtDetailVatRate를
        // DB에 저장된 실제 값으로 다시 덮어쓰므로, 조회/신규 진입 시에는 이 핸들러 결과가 그대로
        // 유지되지 않는다 - 사용자가 직접 콤보를 바꿀 때만 실질적으로 적용된다.
        cboDetailVatType.EditValueChanged += (s, e) =>
        {
            var relCd1 = cboDetailVatType.GetColumnValue("rel_cd1");
            txtDetailVatRate.Text = relCd1 == null || relCd1 == DBNull.Value ? string.Empty : relCd1.ToString();
        };

        // FILE SIZE를 바이트 그대로 안 보여주고 KB/MB 단위로 바꿔 보여준다(2026-09-06 요청 -
        // popFileUpload와 같은 형식을 쓰도록 FileSizeFormatter로 공용화함).
        gvwFile.CustomColumnDisplayText += (s, e) =>
        {
            if (e.Column == gridColumn3 && e.Value is long bytes)
                e.DisplayText = FileSizeFormatter.Format(bytes);
        };

        // 첨부파일(grdFile) - 공통 팝업(popFileUpload)을 doc_type="BACUST"/doc_id=CUST_ID로
        // 열고, 닫히면 목록을 다시 조회한다(팝업 안에서 업로드/삭제가 몇 번 있었는지 몰라도
        // 되게 - popFileUpload.ShowAsync 설명 참고). 저장 전(신규모드, CUST_ID가 아직 없음)에는
        // 첨부할 대상 자체가 없으므로 먼저 저장하라고 안내한다.
        btnFileAttach.Click += async (s, e) =>
        {
            if (_editingKey == null)
            {
                AppMessageBox.Show("먼저 거래처를 저장한 뒤 첨부파일을 등록할 수 있습니다.", "확인");
                return;
            }
            popFileUpload.ShowAsync(FileDocType, _editingKey.Value, txtDetailCustNm.Text, 0, this);
            await LoadFileListAsync();
        };

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 그리드 컬럼은 FieldName이 이미 DB
        // 컬럼명이라 자동 적용되지만, panData 개별 컨트롤은 Tag에 미리 넣어둬야 잡힌다
        // (2026-09-12 감사 - 이 화면엔 원래 빠져있었음).
        txtDetailCustId.Tag = new BindingFieldTag("CUST_ID");
        txtDetailCustNm.Tag = new BindingFieldTag("cust_nm");
        txtDetailBizNo.Tag = new BindingFieldTag("biz_no");
        txtDetailTel.Tag = new BindingFieldTag("tel");
        cboDetailCurCd.Tag = new BindingFieldTag("cur_cd");
        txtDetailOwnerNm.Tag = new BindingFieldTag("owner_nm");
        txtDetailZipCode.Tag = new BindingFieldTag("zip_code");
        txtDetailAddr1.Tag = new BindingFieldTag("addr1");
        txtDetailAddr2.Tag = new BindingFieldTag("addr2");
        txtDetailHomepage.Tag = new BindingFieldTag("homepage");
        txtDetailEmail.Tag = new BindingFieldTag("email");
        txtDetailFax.Tag = new BindingFieldTag("fax");
        txtDetailBizKind.Tag = new BindingFieldTag("biz_kind");
        txtDetailBizType.Tag = new BindingFieldTag("biz_type");
        ymdDetailTransOpenDate.Tag = new BindingFieldTag("trans_open_date");
        cboDetailVatType.Tag = new BindingFieldTag("vat_type");
        txtDetailVatRate.Tag = new BindingFieldTag("vat_rate");
        memoEditWyn1.Tag = new BindingFieldTag("remark");
        cboDetailStatCd.Tag = new BindingFieldTag("stat_cd");
        txtDetailEmpNo.Tag = new BindingFieldTag("EMP_ID");

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync)과 다른 거래처로 옮길 때 확인
        // (ConfirmMasterRowSwitch) 둘 다 이 추적에 기대므로 걸어둔다 - grd2/grd3(편집 가능한
        // 하위 그리드)는 EnterNewMode/LoadDetailAsync에서 새 DataTable로 바뀔 때마다 그때그때
        // TrackDirty(_detail1)/TrackDirty(_detail2)를 다시 걸어야 한다(2026-09-06 요청 전까지는
        // 이 화면에 dirty 추적이 아예 없었다).
        TrackDirty(panData);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    private GridViewWyn ActiveDetailView() => ReferenceEquals(tabDetailGrids.SelectedTabPage, tabDetail2) ? gvw3 : gvw2;

    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    /// <summary>사용자가 직접 누른 조회(preserveSelection: false)는 새 검색이므로 0번 행부터,
    /// 저장/삭제 뒤의 내부 재조회(preserveSelection: true)는 방금 편집하던 거래처에 포커스를
    /// 되돌려 panData/grd2/grd3까지 그 값 그대로 다시 채운다(2026-09-06 요청 - "저장 후 조회하면
    /// panData에 바인딩이 안 된다"). frmMinorCode/frmAcc 등 다른 화면과 같은 패턴.</summary>
    private async Task QueryCore(bool preserveSelection)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_cust_nm"] = txtCustNm.Text,
        };
        _list = await QueryAsync("USP_BA_CUST_Q", p);

        var editingKey = preserveSelection ? _editingKey : null;

        // 저장 직후(SaveClick -> QueryCore)엔 아직 IsDirty가 true로 남아있을 수 있다(DataTable.
        // AcceptChanges()는 RowChanged를 안 냄) - 이 재바인딩이 그 상태에서 자동으로 0번 행에
        // 포커스를 주면 ConfirmMasterRowSwitch가 "변경사항이 있다"고 또 확인창을 띄우는 오작동이
        // 생긴다(frmMinorCode에서 실제로 겪음). 아래에서 직접 EnterNewMode/OnMasterSelectedAsync를
        // 호출해 최종 상태를 맞추므로 이 재바인딩 구간만 조용히 지나가면 된다.
        //
        // 구독을 잠깐 끊는 이유(2026-09-08 실제 발견): SuppressMasterRowSwitchConfirm은 "확인창"만
        // 막지, 안에서 gvw1.FocusedRowHandle을 바꾸면 FocusedRowObjectChanged 자체는 그대로
        // 발생한다 - 그 이벤트가 다시 OnMasterSelectedAsync를 fire-and-forget으로 부르고, 바로
        // 아래에서 같은 행에 대해 또 한 번(이번엔 await로) OnMasterSelectedAsync를 부른다. 저장
        // 직후처럼 그 두 호출이 겹치면 나중에 끝나는 쪽이 먼저 끝난 쪽의 TrackDirty 구독/그리드
        // 바인딩을 덮어써서, panData를 닫아도 될 상태로 다시 정리했다고 여겼던 IsDirty가 조용히
        // 다시 true가 되는 경우가 있었다(저장 버튼을 누르고 바로 탭을 닫아도 "변경 내역이
        // 존재합니다" 확인창이 뜸). 아래 명시적 호출 하나만으로 충분하므로, 재바인딩 구간에서는
        // 이벤트를 끊어 중복 호출 자체를 없앤다(frmAcc.cs가 이미 쓰던 패턴).
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        SuppressMasterRowSwitchConfirm(gvw1, () =>
        {
            grd1.DataSource = _list;
            if (editingKey != null)
            {
                var handle = FindRowHandle(editingKey.Value);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        });
        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        var row = editingKey == null ? null : FindRow(editingKey.Value);
        if (row != null) await OnMasterSelectedAsync(row);
        // editingKey가 없으면(최초 조회) grd1 바인딩 직후 DevExpress가 스스로 0번 행에 포커스를
        // 준다 - 그 자동 포커스 행을 그대로 panData에 채운다(2026-09-08 실제 발견/수정 - AI
        // Builder 템플릿 자체의 누락이었고 frmAcc.cs가 이미 쓰던 패턴을 가져왔다).
        else if (gvw1.GetFocusedRow() is DataRowView focusedView) await OnMasterSelectedAsync(focusedView.Row);
        else EnterNewMode();
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, row => _ = OnMasterSelectedAsync(row.Row));

    /// <summary>조회된 목록에서 CUST_ID로 행을 찾는다. 없으면 null. grd1에 CUST_ID 그리드컬럼이
    /// 없어서(거래처ID는 화면에 안 보임) 다른 화면처럼 컬럼 셀 값으로 찾을 수 없다 - _list를
    /// 직접 순회한다.</summary>
    private DataRow? FindRow(long custId) =>
        _list.Rows.Cast<DataRow>().FirstOrDefault(r => r["CUST_ID"] != DBNull.Value && Convert.ToInt64(r["CUST_ID"]) == custId);

    /// <summary>CUST_ID로 grd1의 행 핸들을 찾는다 - FindRow로 찾은 DataRow의 DataTable상 인덱스를
    /// GridView.GetRowHandle로 표시 행 핸들로 변환한다(그리드에 바인딩된 컬럼이 없어도 동작함).</summary>
    private int? FindRowHandle(long custId)
    {
        var row = FindRow(custId);
        if (row == null) return null;
        var rowIndex = _list.Rows.IndexOf(row);
        var handle = gvw1.GetRowHandle(rowIndex);
        return handle >= 0 ? handle : null;
    }

    private async Task OnMasterSelectedAsync(DataRow row)
    {
        // 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인하지 않도록
        // 감싼다(frmMinorCode/frmDept 등과 같은 이유) - 2026-09-06 TrackDirty(panData) 추가 전까지는
        // 이 화면에 dirty 추적이 아예 없어서 필요 없었다.
        SuppressDirtyTracking(() =>
        {
            _editingKey = row["CUST_ID"] == DBNull.Value ? null : Convert.ToInt64(row["CUST_ID"]);
            txtDetailCustId.Text = row["CUST_ID"]?.ToString() ?? string.Empty;
            txtDetailCustId.ReadOnly = true; // CUST_ID는 항상 읽기전용
            txtDetailCustNm.Text = row["cust_nm"]?.ToString() ?? string.Empty;
            txtDetailBizNo.Text = row["biz_no"]?.ToString() ?? string.Empty;
            txtDetailTel.Text = row["tel"]?.ToString() ?? string.Empty;
            cboDetailCurCd.EditValue = row["cur_cd"]?.ToString() ?? string.Empty;
            txtDetailOwnerNm.Text = row["owner_nm"]?.ToString() ?? string.Empty;
            txtDetailZipCode.Text = row["zip_code"]?.ToString() ?? string.Empty;
            txtDetailAddr1.Text = row["addr1"]?.ToString() ?? string.Empty;
            txtDetailAddr2.Text = row["addr2"]?.ToString() ?? string.Empty;
            txtDetailHomepage.Text = row["homepage"]?.ToString() ?? string.Empty;
            txtDetailEmail.Text = row["email"]?.ToString() ?? string.Empty;
            txtDetailFax.Text = row["fax"]?.ToString() ?? string.Empty;
            txtDetailBizKind.Text = row["biz_kind"]?.ToString() ?? string.Empty;
            txtDetailBizType.Text = row["biz_type"]?.ToString() ?? string.Empty;
            ymdDetailTransOpenDate.YyyyMmDd = row["trans_open_date"]?.ToString();
            cboDetailVatType.EditValue = row["vat_type"]?.ToString() ?? string.Empty;
            txtDetailVatRate.Text = row["vat_rate"]?.ToString() ?? string.Empty;
            memoEditWyn1.Text = row["remark"]?.ToString() ?? string.Empty;
            cboDetailStatCd.EditValue = row["stat_cd"]?.ToString() ?? string.Empty;
            txtDetailEmpNo.Text = row["EMP_ID"]?.ToString() ?? string.Empty;
        });

        await LoadDetailAsync();
        await LoadFileListAsync();
    }

    /// <summary>grdFile(읽기전용 요약 그리드) 갱신 - 실제 업로드/다운로드/삭제는 popFileUpload
    /// 팝업에서 하고, 여기서는 "이 거래처에 첨부파일이 몇 건 있는지"만 보여준다.</summary>
    private async Task LoadFileListAsync()
    {
        if (_editingKey == null)
        {
            _files = new List<FileListItemDto>();
        }
        else
        {
            try
            {
                var url = $"api/files?docType={FileDocType}&docId={_editingKey.Value}&docSerl=0";
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

    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingKey = null;
            txtDetailCustId.Text = string.Empty; // 신규는 아직 ID가 없음(저장 시 서버가 발급)
            txtDetailCustId.ReadOnly = true; // CUST_ID는 항상 서버가 발급 - 직접 입력 불가
            txtDetailCustNm.Text = string.Empty;
            txtDetailBizNo.Text = string.Empty;
            txtDetailTel.Text = string.Empty;
            cboDetailCurCd.EditValue = string.Empty;
            txtDetailOwnerNm.Text = string.Empty;
            txtDetailZipCode.Text = string.Empty;
            txtDetailAddr1.Text = string.Empty;
            txtDetailAddr2.Text = string.Empty;
            txtDetailHomepage.Text = string.Empty;
            txtDetailEmail.Text = string.Empty;
            txtDetailFax.Text = string.Empty;
            txtDetailBizKind.Text = string.Empty;
            txtDetailBizType.Text = string.Empty;
            ymdDetailTransOpenDate.YyyyMmDd = null;
            cboDetailVatType.EditValue = string.Empty;
            txtDetailVatRate.Text = string.Empty;
            memoEditWyn1.Text = string.Empty;
            cboDetailStatCd.EditValue = "0"; // 신규 입력 기본값(2026-09-06 요청)
            txtDetailEmpNo.Text = string.Empty;
            _detail1 = _detail1.Clone();
            _detail2 = _detail2.Clone();
            _detail3 = _detail3.Clone();
            TrackDirty(_detail1);
            TrackDirty(_detail2);
            TrackDirty(_detail3);
            grd2.DataSource = _detail1;
            grd3.DataSource = _detail2;
            grd4.DataSource = _detail3;
            _files = new List<FileListItemDto>();
            grdFile.DataSource = null;
            grdFile.DataSource = _files;
        });

        // 신규 입력 진입 시 커서(포커스)를 거래처명 칸으로 보낸다(2026-09-06 요청).
        // - Focus()는 SuppressDirtyTracking(위 블록) "밖"에서 호출해야 한다: 저 블록은 "코드가
        //   EditValue를 채우는 동안엔 TrackDirty가 사용자가 고친 걸로 오인하지 않게" 감싸는
        //   용도일 뿐이고, Focus()는 EditValueChanged를 발생시키지 않으므로 dirty 추적과는
        //   애초에 무관하다 - 그래도 "값 채우기가 다 끝난 뒤에 커서를 놓는다"는 순서를 코드
        //   구조로도 분명히 보여주려고 바깥에 둔다.
        // - EnterNewMode()는 화면이 처음 뜰 때(생성자)와 "신규" 버튼을 눌렀을 때 둘 다 호출된다 -
        //   그래서 여기 한 줄만 추가하면 두 경우 모두 자동으로 커서가 거래처명으로 간다.
        // - grd1(거래처 목록) 쪽 이벤트(FocusedRowObjectChanged 등)가 이 Focus() 호출 뒤에
        //   포커스를 다시 grd1으로 가져가는 경우는 없다 - EnterNewMode는 grd1의 포커스 변경에
        //   "반응"해서 불리는 쪽(OnMasterSelectedAsync)이 아니라 독립적으로 호출되는 메서드라서
        //   순환/경합이 생기지 않는다.
        txtDetailCustNm.Focus();
    }

    /// <summary>선택된 마스터 행의 하위 목록 3개(grd2/grd3/grd4)를 한 번의 호출로 같이 조회한다 -
    /// USP_BA_CUST_Q가 work_type='Q1'일 때 레코드셋을 3개(순서대로 grd2/grd3/grd4용) 반환하기
    /// 때문에 QueryMultiAsync를 쓴다(QueryAsync는 첫 레코드셋만 받음). grd4(거래처분류)는
    /// BA0003 전체 코드를 항상 반환하고(추가/삭제 없음), 이 거래처가 이미 속한 분류만
    /// is_member=1로 표시된다.</summary>
    private async Task LoadDetailAsync()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_cust_id"] = _editingKey?.ToString(),
        };
        var tables = await QueryMultiAsync("USP_BA_CUST_Q", p);
        _detail1 = tables.Count > 0 ? tables[0] : new DataTable();
        _detail2 = tables.Count > 1 ? tables[1] : new DataTable();
        _detail3 = tables.Count > 2 ? tables[2] : new DataTable();
        TrackDirty(_detail1);
        TrackDirty(_detail2);
        TrackDirty(_detail3);
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
        grd4.DataSource = _detail3;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // ---- 1) 헤더(panData -> USP_BA_CUST_S) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            ["p_cust_id"] = _editingKey?.ToString(),
            ["p_cust_nm"] = txtDetailCustNm.Text,
            ["p_biz_no"] = txtDetailBizNo.Text,
            ["p_tel"] = txtDetailTel.Text,
            ["p_cur_cd"] = cboDetailCurCd.EditValue?.ToString() ?? string.Empty,
            ["p_owner_nm"] = txtDetailOwnerNm.Text,
            ["p_zip_code"] = txtDetailZipCode.Text,
            ["p_addr1"] = txtDetailAddr1.Text,
            ["p_addr2"] = txtDetailAddr2.Text,
            ["p_homepage"] = txtDetailHomepage.Text,
            ["p_email"] = txtDetailEmail.Text,
            ["p_fax"] = txtDetailFax.Text,
            ["p_biz_kind"] = txtDetailBizKind.Text,
            ["p_biz_type"] = txtDetailBizType.Text,
            ["p_trans_open_date"] = ymdDetailTransOpenDate.YyyyMmDd,
            ["p_vat_type"] = cboDetailVatType.EditValue?.ToString(),
            ["p_vat_rate"] = string.IsNullOrWhiteSpace(txtDetailVatRate.Text) ? null : txtDetailVatRate.Text,
            ["p_remark"] = memoEditWyn1.Text,
            ["p_stat_cd"] = cboDetailStatCd.EditValue?.ToString() ?? string.Empty,
            ["p_emp_id"] = long.TryParse(txtDetailEmpNo.Text, out var peid) ? peid.ToString() : null,
        };

        var headerResult = await SaveAsync("USP_BA_CUST_S", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey?.ToString() ?? headerResult.GeneratedCode;

        // ---- 2) 명세1(grd2 -> USP_BA_CUST_S_1) ----
        var detail1Ok = await SaveDetailRowsAsync(gvw2, _detail1, "USP_BA_CUST_S_1", headerKey, (row, version) => new Dictionary<string, string?>
        {
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_prsn_nm"] = ProcData.Str(row, "prsn_nm", version),
            ["p_grade"] = ProcData.Str(row, "grade", version),
            ["p_tel1"] = ProcData.Str(row, "tel1", version),
            ["p_tel2"] = ProcData.Str(row, "tel2", version),
            ["p_fax"] = ProcData.Str(row, "fax", version),
            ["p_email"] = ProcData.Str(row, "email", version),
        });
        if (!detail1Ok) return;

        // ---- 3) 명세2(grd3 -> USP_BA_CUST_S_2) ----
        var detail2Ok = await SaveDetailRowsAsync(gvw3, _detail2, "USP_BA_CUST_S_2", headerKey, (row, version) => new Dictionary<string, string?>
        {
            ["p_serl"] = ProcData.Str(row, "serl", version),
            ["p_bank_cd"] = ProcData.Str(row, "bank_cd", version),
            ["p_acnt_no"] = ProcData.Str(row, "acnt_no", version),
            ["p_remark"] = ProcData.Str(row, "remark", version),
        });
        if (!detail2Ok) return;

        // ---- 4) 거래처분류(grd4 -> USP_BA_CUST_S_3) - 체크박스 토글이라 행이 새로 생기거나
        // 지워지는 게 아니라 is_member 값만 바뀌므로 RowState는 항상 Modified('U')다. 프로시저가
        // is_member 값을 보고 추가/삭제를 알아서 나눠 처리한다. ----
        var detail3Ok = await SaveDetailRowsAsync(gvw4, _detail3, "USP_BA_CUST_S_3", headerKey, (row, version) => new Dictionary<string, string?>
        {
            ["p_class_cd"] = ProcData.Str(row, "class_cd", version),
            ["p_is_member"] = ProcData.Str(row, "is_member", version),
        });
        if (!detail3Ok) return;

        _editingKey ??= long.TryParse(headerResult.GeneratedCode, out var newId) ? newId : null;
        Toast.Show("저장되었습니다.");
        await QueryCore(preserveSelection: true); // 방금 저장한 거래처 유지 - QueryClick(사용자 조회)과 다른 경로
    }

    /// <summary>grd2/grd3 공통 저장 루프 - 변경된 행마다 N/U/D로 나눠 저장한다(SingleGrid.SaveClick과
    /// 같은 RowState 판정 방식). extraParams는 화면마다 다른 컬럼->파라미터 매핑을 행 하나 기준으로
    /// 만들어주는 콜백(생성기가 컬럼 목록으로 채워넣음).</summary>
    private async Task<bool> SaveDetailRowsAsync(GridViewWyn gvw, DataTable table, string saveProc, string? masterKey,
        Func<DataRow, DataRowVersion, Dictionary<string, string?>> extraParams)
    {
        gvw.CloseEditor();
        gvw.UpdateCurrentRow();

        foreach (DataRow row in table.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            // Deleted 행에서 DataRowVersion.Current를 읽으면 DeletedRowInaccessibleException이 난다
            // (ProcData.Str 주석 참고) - 그래서 컬럼 매핑에도 이 버전을 그대로 넘겨준다.
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_cust_id"] = masterKey,
            };
            foreach (var kv in extraParams(row, version)) p[kv.Key] = kv.Value;

            var result = await SaveAsync(saveProc, p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return false;
            }
        }
        return true;
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p_cust_id"] = _editingKey?.ToString(),
        };
        var result = await SaveAsync("USP_BA_CUST_S", p);
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
