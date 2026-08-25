using System.ComponentModel;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM.frmMinorCode;

/// <summary>
/// 기초코드등록 화면(TSMMAJOR 대분류/TSMMINOR 소분류). grd1에 대분류 리스트, panData에 대분류
/// 상세 입력폼(관리항목1~10 포함), grd2에 소분류 그리드(인라인 편집).
///
/// 관리항목1~10의 구분(CODE/TEXT) 콤보(cborel_cd_type1, lookUpEditWyn1~9)는 DB 조회가 아니라
/// 고정 값 2개뿐이라, ProcName/Where 대신 BindCodeList로 정적 목록을 직접 채운다.
///
/// panData에는 대분류의 사용여부(sys_yn)/비고(remark)를 편집할 컨트롤이 아직 없어서, 저장 시
/// 각각 false/빈 문자열로 고정 전송한다(화면에 해당 입력 UI가 추가되면 그때 실제 값으로 바꾸면 됨).
/// </summary>
public partial class frmMinorCode : BaseForm
{
    private const int RelCount = 10;

    private List<MajorListItemDto> _majors = new();
    // BindingList여야 grd2에서 행추가/행삭제(gvw2.AddNewRow/DeleteRow)가 실제로 동작한다 -
    // 일반 List<T>는 IBindingList를 구현하지 않아서 DevExpress 그리드가 바인딩된 목록에
    // 행을 추가/제거하지 못한다(버튼을 눌러도 아무 반응이 없던 원인 - 실제로 겪음).
    private BindingList<MinorItemDto> _minors = new();
    private string? _editingMajorCd; // null이면 신규모드

    // 관리항목1~10 컨트롤 - Designer에서 2열x5행으로 이미 배치돼 있어서(참조1~5=왼쪽, 참조6~10=오른쪽),
    // 인덱스 순서로 접근할 수 있게 배열로만 묶는다(런타임에 새로 만들지 않음).
    private TextEditWyn[] TxtRelTitle => new[] { txtrel_title1, txtrel_title2, txtrel_title3, txtrel_title4, txtrel_title5, txtrel_title6, txtrel_title7, txtrel_title8, txtrel_title9, txtrel_title10 };
    private LookUpEditWyn[] CboRelCdType => new[] { cborel_cd_type1, cborel_cd_type2, cborel_cd_type3, cborel_cd_type4, cborel_cd_type5, cborel_cd_type6, cborel_cd_type7, cborel_cd_type8, cborel_cd_type9, cborel_cd_type10};
    private TextEditWyn[] TxtRelCd => new[] { txtrel_cd1, txtrel_cd2, txtrel_cd3, txtrel_cd4, txtrel_cd5, txtrel_cd6, txtrel_cd7, txtrel_cd8, txtrel_cd9, txtrel_cd10 };

    public frmMinorCode()
    {
        InitializeComponent();

        Text = "기초코드등록";
        MenuCd = "SM_MINOR_CODE";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;

        // 소분류 그리드 헤더의 +/x 버튼 클릭 연결은 Designer.cs(InitializeComponent)가 이미
        // 하고 있다 - 여기서 또 구독하면 클릭 한 번에 NewRowClick/DeleteRowClick이 두 번씩
        // 불려서(행이 2개 추가되거나, 삭제가 어긋나는 등) 문제가 생긴다(실제로 겪음).

        var relCdTypeItems = new[]
        {
            new CodeLookupItem { Value = "CODE", Display = "CODE" },
            new CodeLookupItem { Value = "TEXT", Display = "TEXT" }
        };
        foreach (var cbo in CboRelCdType)
            cbo.BindCodeList(relCdTypeItems, nameof(CodeLookupItem.Value), nameof(CodeLookupItem.Display), popupWidth: 100);

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        // panHeader의 검색창(textEditWyn1) 하나로 대분류코드/명을 같이 검색한다("대분류코드/명" 라벨).
        var keyword = txtminor_cd_q.Text.Trim();
        var query = $"api/minor-codes?majorCd={Uri.EscapeDataString(keyword)}&majorNm={Uri.EscapeDataString(keyword)}&selectedMajorCd={Uri.EscapeDataString(_editingMajorCd ?? string.Empty)}";
        var result = await ApiClient.GetAsync<MinorCodeQueryResponse>(query) ?? new();
        _majors = result.Majors;
        _minors = new BindingList<MinorItemDto>(result.Minors);
        grd1.DataSource = _majors;
        grd2.DataSource = _minors;

        if (_editingMajorCd != null && _majors.All(m => m.major_cd != _editingMajorCd))
        {
            EnterNewMode();
        }
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override Task DeleteClick()
    {
        AppMessageBox.Show("대분류코드는 삭제(사용중지)를 지원하지 않습니다.", "안내");
        return Task.CompletedTask;
    }

    public override Task NewRowClick()
    {
        gvw2.AddNewRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        var handle = gvw2.FocusedRowHandle;
        if (handle >= 0) gvw2.DeleteRow(handle);
        return Task.CompletedTask;
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is MajorListItemDto major) EnterEditMode(major);
    }

    private void EnterNewMode()
    {
        _editingMajorCd = null;
        txtmajor_cd.Text = string.Empty;
        txtmajor_cd.Enabled = true;
        txtmajor_nm.Text = string.Empty;

        var titles = TxtRelTitle;
        var types = CboRelCdType;
        var codes = TxtRelCd;
        for (var i = 0; i < RelCount; i++)
        {
            titles[i].Text = string.Empty;
            types[i].EditValue = null;
            codes[i].Text = string.Empty;
        }

        _minors = new();
        grd2.DataSource = _minors;
        txtmajor_cd.Focus();
    }

    private void EnterEditMode(MajorListItemDto major)
    {
        // grd1.DataSource를 다시 세팅할 때마다(QueryClick 안) 포커스 행이 새로 잡히면서
        // FocusedRowObjectChanged가 또 발생 -> 같은 대분류인데도 여기가 또 불려서 QueryClick을
        // 또 부르면(아래) 같은 대분류를 계속 반복 재조회하는 루프가 생긴다(실제로 겪음 - 우측
        // 상세 패널이 몇 초씩 걸려 보이던 원인). 실제로 "다른" 대분류로 옮겨간 경우에만 재조회한다.
        var isSameMajor = _editingMajorCd == major.major_cd;
        _editingMajorCd = major.major_cd;
        txtmajor_cd.Text = major.major_cd;
        txtmajor_cd.Enabled = false; // 대분류코드는 PK라 수정 불가
        txtmajor_nm.Text = major.major_nm;

        var relTitles = new[] { major.rel_title1, major.rel_title2, major.rel_title3, major.rel_title4, major.rel_title5, major.rel_title6, major.rel_title7, major.rel_title8, major.rel_title9, major.rel_title10 };
        var relTypes = new[] { major.rel_cd_type1, major.rel_cd_type2, major.rel_cd_type3, major.rel_cd_type4, major.rel_cd_type5, major.rel_cd_type6, major.rel_cd_type7, major.rel_cd_type8, major.rel_cd_type9, major.rel_cd_type10 };
        var relCodes = new[] { major.rel_cd1, major.rel_cd2, major.rel_cd3, major.rel_cd4, major.rel_cd5, major.rel_cd6, major.rel_cd7, major.rel_cd8, major.rel_cd9, major.rel_cd10 };

        var titles = TxtRelTitle;
        var types = CboRelCdType;
        var codes = TxtRelCd;

        for (var i = 0; i < RelCount; i++)
        {
            titles[i].Text = relTitles[i] ?? string.Empty;
            types[i].EditValue = relTypes[i];
            codes[i].Text = relCodes[i] ?? string.Empty;
        }

        // 소분류는 이 대분류 기준으로 다시 받아와야 정확하다(그리드에서 편집 중인 내용을
        // 잃더라도, 대분류를 바꿔 선택했다는 건 이전 미저장 소분류 변경은 버린다는 뜻).
        // QueryClick() 대신 소분류 전용 API를 쓴다 - QueryClick은 대분류(grd1)까지 같이
        // 다시 받아와서 grd1.DataSource를 매번 재할당하는 바람에, grd1에서 행을 클릭할
        // 때마다 grd1 자체도 리프레시되는 것처럼 보이는 문제가 있었다(실제로 겪음).
        if (!isSameMajor)
        {
            _ = LoadMinors(major.major_cd);
        }
    }

    private async Task LoadMinors(string majorCd)
    {
        var minors = await ApiClient.GetAsync<List<MinorItemDto>>($"api/minor-codes/{majorCd}/minors") ?? new();
        _minors = new BindingList<MinorItemDto>(minors);
        grd2.DataSource = _minors;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtmajor_cd.Text) || string.IsNullOrWhiteSpace(txtmajor_nm.Text))
        {
            AppMessageBox.Show("대분류코드와 대분류명은 필수입니다.", "확인");
            return;
        }

        var titles = TxtRelTitle;
        var types = CboRelCdType;
        var codes = TxtRelCd;

        var request = new MajorSaveRequest
        {
            major_cd = txtmajor_cd.Text,
            major_nm = txtmajor_nm.Text,
            sys_yn = false,
            remark = string.Empty,
            rel_cd1 = codes[0].Text, rel_title1 = titles[0].Text, rel_cd_type1 = (string?)types[0].EditValue,
            rel_cd2 = codes[1].Text, rel_title2 = titles[1].Text, rel_cd_type2 = (string?)types[1].EditValue,
            rel_cd3 = codes[2].Text, rel_title3 = titles[2].Text, rel_cd_type3 = (string?)types[2].EditValue,
            rel_cd4 = codes[3].Text, rel_title4 = titles[3].Text, rel_cd_type4 = (string?)types[3].EditValue,
            rel_cd5 = codes[4].Text, rel_title5 = titles[4].Text, rel_cd_type5 = (string?)types[4].EditValue,
            rel_cd6 = codes[5].Text, rel_title6 = titles[5].Text, rel_cd_type6 = (string?)types[5].EditValue,
            rel_cd7 = codes[6].Text, rel_title7 = titles[6].Text, rel_cd_type7 = (string?)types[6].EditValue,
            rel_cd8 = codes[7].Text, rel_title8 = titles[7].Text, rel_cd_type8 = (string?)types[7].EditValue,
            rel_cd9 = codes[8].Text, rel_title9 = titles[8].Text, rel_cd_type9 = (string?)types[8].EditValue,
            rel_cd10 = codes[9].Text, rel_title10 = titles[9].Text, rel_cd_type10 = (string?)types[9].EditValue
        };

        var wasNew = _editingMajorCd == null;
        var result = wasNew
            ? await ApiClient.PostAsync<MajorSaveRequest, ApiResult>("api/minor-codes", request)
            : await ApiClient.PutAsync<MajorSaveRequest, ApiResult>($"api/minor-codes/{_editingMajorCd}", request);

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        var savedMajorCd = wasNew ? (result.GeneratedCode ?? request.major_cd) : _editingMajorCd!;

        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();
        var minorResult = await ApiClient.PutAsync<SaveMinorsRequest, ApiResult>(
            $"api/minor-codes/{savedMajorCd}/minors", new SaveMinorsRequest { Items = _minors.ToList() });

        if (minorResult == null || !minorResult.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(minorResult), "저장 실패");
            return;
        }

        _editingMajorCd = savedMajorCd;
        gvw2.ClearDirtyMarks();
        await QueryClick();
        Toast.Show(wasNew ? "대분류코드가 등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>ApiResult.ErrorCode는 SQL 예외(ERROR_NUMBER())일 때만 채워진다(0이면 업무로직
    /// 판단만으로 실패 - 예: 필수값 누락) - 그럴 때만 메시지에 오류번호를 같이 보여준다.</summary>
    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }

    private void textEditWyn1_EditValueChanged(object sender, EventArgs e)
    {
    }

    private async void btnAddRow2_Click(object sender, EventArgs e)
    {
        await NewRowClick();
    }

    private async void btnDeletRow2_Click(object sender, EventArgs e)
    {
        await DeleteRowClick();
    }
}
