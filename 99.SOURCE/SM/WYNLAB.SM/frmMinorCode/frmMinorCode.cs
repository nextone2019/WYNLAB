using System.Data;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM;

/// <summary>
/// 기초코드등록 화면(TSMMAJOR 대분류/TSMMINOR 소분류). grd1에 대분류 리스트, panData에 대분류
/// 상세 입력폼(관리항목1~10 포함), grd2에 소분류 그리드(인라인 편집).
///
/// 관리항목1~10의 구분 콤보(cborel_cd_type1~10)는 대분류 SM00001(소분류참조구분)의 소분류
/// 목록으로 채운다 - LookUpEditWyn.ProcName/Where(SSP_CBO_CODE_Q, major_cd)를 그대로 쓰는
/// 시스템 공용 콤보 패턴이라(WYNLAB.BaseForm.ControlDataSources 참고), 이 화면이 하는 일은
/// Where에 그 대분류코드를 지정하는 것뿐이다 - 다른 화면도 자기 대분류코드만 바꿔서 똑같이
/// 쓸 수 있다. 값 자체가 바뀔 수 있으니(관리자가 SM00001 소분류를 늘리거나 줄일 수 있음)
/// 화면 코드에 하드코딩된 고정 목록 대신 DB를 참조하게 한 것.
///
/// panData에는 대분류의 사용여부(sys_yn)/비고(remark)를 편집할 컨트롤이 아직 없어서, 저장 시
/// 각각 false/빈 문자열로 고정 전송한다(화면에 해당 입력 UI가 추가되면 그때 실제 값으로 바꾸면 됨).
/// </summary>
public partial class frmMinorCode : BaseForm
{
    private const int RelCount = 10;

    /// <summary>관리항목1~10 구분 콤보가 참조하는 대분류코드("소분류참조구분"). 이 대분류의
    /// 소분류 목록(TSMMINOR)이 곧 콤보에 뜨는 선택지다 - 코드를 고치는 게 아니라 기초코드등록
    /// 화면에서 SM00001의 소분류를 추가/수정하면 그대로 반영된다.</summary>
    private const string RelCdTypeMajorCd = "SM00001";

    // 조회 결과를 DTO가 아니라 DataTable로 들고 있다. 이 화면은 범용 데이터 통로(api/data/*)로
    // 프로시저를 직접 부르는 첫 화면이라, 화면별 DTO를 만들지 않는 게 그 구조의 요점이다
    // (설계 배경은 저장소 루트의 GENERIC_DATA_API.md 참고).
    //
    // 그리드 컬럼이 디자이너에서 FieldName 문자열("minor_cd")로 잡혀 있어서 타입 없는 행을
    // 그대로 바인딩해도 표시가 똑같다. DataTable은 IBindingList도 구현해서 grd2의 행추가/행삭제
    // (gvw2.AddNewRow/DeleteRow)도 그대로 동작한다 - 예전에 BindingList를 써야 했던 이유가
    // 그것이었는데 DataTable도 같은 조건을 만족한다.
    private DataTable _majors = new();
    private DataTable _minors = new();
    private string? _editingMajorCd; // null이면 신규모드

    // 대분류가 바뀔 때마다 RebuildDynamicMinorColumnsAsync가 만들었다가 지우는 grd2의 동적
    // 컬럼/리포지토리 아이템 - 다음 대분류로 넘어가기 전에 여기 담긴 것부터 전부 지워야
    // 대분류를 옮겨다닐수록 컬럼이 계속 누적되는 걸 막을 수 있다.
    private readonly List<GridColumn> _dynamicMinorColumns = new();
    private readonly List<RepositoryItem> _dynamicMinorRepositoryItems = new();

    // 관리항목1~10 컨트롤 - Designer에서 2열x5행으로 이미 배치돼 있어서(참조1~5=왼쪽, 참조6~10=오른쪽),
    // 인덱스 순서로 접근할 수 있게 배열로만 묶는다(런타임에 새로 만들지 않음).
    private TextEditWyn[] TxtRelTitle => new[] { txtrel_title1, txtrel_title2, txtrel_title3, txtrel_title4, txtrel_title5, txtrel_title6, txtrel_title7, txtrel_title8, txtrel_title9, txtrel_title10 };
    private LookUpEditWyn[] CboRelCdType => new[] { cborel_cd_type1, cborel_cd_type2, cborel_cd_type3, cborel_cd_type4, cborel_cd_type5, cborel_cd_type6, cborel_cd_type7, cborel_cd_type8, cborel_cd_type9, cborel_cd_type10};
    private TextEditWyn[] TxtRelCd => new[] { txtrel_cd1, txtrel_cd2, txtrel_cd3, txtrel_cd4, txtrel_cd5, txtrel_cd6, txtrel_cd7, txtrel_cd8, txtrel_cd9, txtrel_cd10 };

    public frmMinorCode()
    {
        InitializeComponent();

        Text = "기초코드등록";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw2.InitNewRow += Gvw2_InitNewRow;

        // 소분류 그리드 헤더의 +/x 버튼 클릭 연결은 Designer.cs(InitializeComponent)가 이미
        // 하고 있다 - 여기서 또 구독하면 클릭 한 번에 NewRowClick/DeleteRowClick이 두 번씩
        // 불려서(행이 2개 추가되거나, 삭제가 어긋나는 등) 문제가 생긴다(실제로 겪음).

        // grd1(대분류 목록)은 조회전용 - 셀 편집/행추가삭제 전부 막는다.
        gvw1.Role = GridRoleWyn.Query;

        // grd2(소분류)는 편집 가능 - 네비게이터의 추가/삭제 버튼도 이미 있는 검증 로직
        // (NewRowClick/DeleteRowClick, 대분류 미저장 시 막는 등)을 그대로 태운다. 헤더의 +/x
        // 버튼과 네비게이터 버튼 둘 다 같은 메서드를 부르므로 동작이 어긋나지 않는다.
        gvw2.Role = GridRoleWyn.Edit;
        gvw2.RowAdd += async (s, e) => await NewRowClick();
        gvw2.RowDelete += async (s, e) => await DeleteRowClick();

        foreach (var cbo in CboRelCdType)
        {
            cbo.ProcName = "SSP_CBO_CODE_Q";
            cbo.Where = RelCdTypeMajorCd;
        }

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync) - grd2(소분류)는 편집 가능한
        // 그리드라 panData뿐 아니라 그 DataTable도 걸어야 한다(EnterNewMode/LoadMinors에서
        // _minors가 새 인스턴스로 교체될 때마다 TrackDirty(_minors)를 다시 호출해야 함).
        TrackDirty(panData);

        EnterNewMode();
        //Load += async (s, e) => await QueryClick();
    }

    /// <summary>
    /// 대분류 목록 조회. 검색조건을 하나 늘리려면 아래 익명 객체에 p_ 파라미터를 한 줄 추가하고
    /// 프로시저에 같은 이름의 파라미터(기본값 NULL)와 WHERE 조건을 넣으면 된다 - 서버 API는
    /// 손대지 않으므로 배포도, 서비스 중단도 없다.
    /// </summary>
    // 사용자가 조회 버튼을 누른 경우(preserveSelection: false, 항상 0번 행부터 새로 시작)와
    // 저장/삭제 뒤 내부 재조회(preserveSelection: true, 방금 편집하던 행 유지)는 다른 동작이어야
    // 한다 - 지금까지는 _editingMajorCd를 무조건 복원해서, 목록 중간 행을 보다가 조회를 눌러도
    // 그 행이 계속 선택된 채로 남아있었다(2026-09-02 실제 발견, [[feedback_query_refocus_after_save]]).
    public override async Task QueryClick() => await QueryCore(preserveSelection: false);

    private async Task QueryCore(bool preserveSelection)
    {
        // panHeader의 검색창(txtminor_cd_q) 하나로 대분류코드/명을 같이 검색한다("대분류코드/명" 라벨).
        var keyword = txtminor_cd_q.Text.Trim();

        _majors = await QueryAsync("USP_SM_MINORCODE_Q", new
        {
            p_work_type = "Q",
            p_major_cd = keyword,
            p_major_nm = keyword
        });

        var editingMajorCd = preserveSelection ? _editingMajorCd : null;

        if (editingMajorCd == null)
        {
            // 편집 중이던 대분류가 없으면(최초 조회 등) DevExpress 기본 동작(재바인딩 시 자동으로
            // 첫 행에 포커스 -> FocusedRowObjectChanged -> EnterEditMode)에 그대로 맡긴다.
            grd1.DataSource = _majors;
            return;
        }

        var targetRow = FindMajorRow(editingMajorCd);

        // 편집 중이던 대분류가 있으면 그 행에 포커스를 정확히 복원한다(예: 저장 버튼을 누르면
        // SaveClick이 QueryClick을 다시 부르는데, 저장 전 보고 있던 행 그대로 유지되어야 한다).
        // grd1을 재바인딩하면 DevExpress가 자동으로 한 행(대개 0번)에 먼저 포커스를 주면서
        // FocusedRowObjectChanged가 발생하는데, 그게 복원하려는 행이 아니면 EnterEditMode가
        // 잠깐 엉뚱한 대분류로 실행되어(우측 패널이 깜빡이고, 뒤이어 우리가 다시 포커스를
        // 옮기는 것과 경합해 소분류 응답 순서가 엇갈릴 수도 있다). 그래서 재바인딩 + 포커스
        // 복원이 끝날 때까지 이벤트를 끊어두고, 최종적으로 남길 행 하나에 대해서만 아래에서
        // 명시적으로 EnterEditMode/EnterNewMode를 호출한다(실제로 겪음 - 저장 후 재조회하면
        // 방금 편집하던 대분류에서 포커스가 풀렸다).
        gvw1.FocusedRowObjectChanged -= Gvw1_FocusedRowObjectChanged;
        try
        {
            grd1.DataSource = _majors;
            if (targetRow != null)
            {
                var handle = FindMajorRowHandle(editingMajorCd);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }
        }
        finally
        {
            gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        }

        // 편집 중이던 대분류가 조회 결과에서 사라졌으면(검색어가 바뀌었거나 지워졌으면) 신규모드로,
        // 남아 있으면 그 행으로 우측 패널/소분류를 맞춘다.
        if (targetRow != null) EnterEditMode(targetRow);
        else EnterNewMode();
    }

    /// <summary>조회된 대분류 목록에서 코드로 행을 찾는다. 없으면 null.</summary>
    private DataRow? FindMajorRow(string majorCd) =>
        _majors.Rows.Cast<DataRow>()
            .FirstOrDefault(r => string.Equals(Convert.ToString(r["major_cd"]), majorCd, StringComparison.OrdinalIgnoreCase));

    /// <summary>major_cd로 grd1의 행 핸들을 찾는다. 없으면 null.
    ///
    /// GridView.LocateByValue를 안 쓰는 이유: 이 화면의 DataTable 컬럼은 전부 object 타입인데
    /// (ProcData.ToDataTable 참고), 실제로 그 안에 들어있는 값은 System.Text.Json이 만든
    /// JsonElement이지 순수 string이 아니다. LocateByValue는 셀의 실제 값과 넘겨준 값(순수
    /// string)을 그대로 값비교(Equals)하는데 JsonElement와 string은 절대 같다고 판정되지 않아서
    /// 늘 못 찾은 것처럼 실패한다(실제로 겪음 - 저장 후 포커스가 정확한 행으로 갔다고 우측
    /// 패널은 맞는데 노란 포커스 표시만 첫 행에 남아있었다). Convert.ToString으로 비교하는
    /// FindMajorRow와 같은 방식으로 직접 순회하면 이 문제가 없다.</summary>
    private int? FindMajorRowHandle(string majorCd)
    {
        var column = gvw1.Columns["major_cd"];
        for (var handle = 0; handle < gvw1.RowCount; handle++)
        {
            if (string.Equals(Convert.ToString(gvw1.GetRowCellValue(handle, column)), majorCd, StringComparison.OrdinalIgnoreCase))
                return handle;
        }
        return null;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    /// <summary>
    /// grd1에서 선택한 대분류를 삭제한다. 신규 입력 중(_editingMajorCd == null)일 땐 아직 저장된
    /// 게 없으므로 삭제할 대상 자체가 없다.
    ///
    /// 소분류(TSMMINOR)를 어떻게 할지는 서버 프로시저(USP_SM_MINORCODE_S의 'D' 분기)가 정한다 -
    /// 화면에서 소분류를 먼저 지우고 대분류를 지우는 식으로 나눠 처리하면 중간에 실패했을 때
    /// 반쪽만 지워진 상태가 남는다. 프로시저 한 번의 호출로 끝내야 트랜잭션이 보장된다.
    /// 프로시저가 "소분류가 있어 삭제 불가"로 막으면 그 사유(ReturnMsg)가 그대로 표시된다.
    /// </summary>
    public override async Task DeleteClick()
    {
        if (_editingMajorCd == null)
        {
            AppMessageBox.Show("삭제할 대분류코드를 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"선택하신 대분류코드를 삭제 하시겠습니까?\n\n[{_editingMajorCd}] {txtmajor_nm.Text}",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_SM_MINORCODE_S", new
        {
            p_work_type = "D",
            p_major_cd = _editingMajorCd
        });

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "삭제 실패");
            return;
        }

        // 지워진 대분류를 계속 편집 상태로 두면 안 되므로 편집 대상을 먼저 놓아준다.
        _editingMajorCd = null;
        gvw2.ClearDirtyMarks();
        await QueryClick();

        // 여기서 EnterNewMode()를 부르면 안 된다. QueryClick이 grd1을 다시 바인딩하는 순간
        // 포커스 행이 새로 잡히면서 FocusedRowObjectChanged -> EnterEditMode가 돌아 우측 패널을
        // 이미 채워놓는데, 그 뒤에 신규 모드로 비워버리면 그리드에는 행이 선택되어 있고
        // 소분류까지 조회됐는데 패널만 빈 상태가 된다(실제로 겪음).
        // 남은 대분류가 하나도 없을 때만 신규 입력 상태로 둔다.
        if (_majors.Rows.Count == 0) EnterNewMode();

        Toast.Show("삭제되었습니다.");
    }

    public override Task NewRowClick()
    {
        // 대분류가 아직 저장 안 된 신규 입력 상태(_editingMajorCd == null)에서 소분류부터
        // 추가하면, 저장 시 대분류코드가 없는 소분류가 되어 정합성이 깨진다 - 대분류를 먼저
        // 저장해야 소분류를 추가할 수 있게 막는다.
        if (_editingMajorCd == null)
        {
            AppMessageBox.Show("대분류를 먼저 등록한 뒤에 소분류를 추가할 수 있습니다.", "안내");
            return Task.CompletedTask;
        }

        gvw2.AddNewRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        // 방금 AddNewRow()로 추가한 행에 포커스가 그대로 있는 상태에서 바로 삭제를 누르면,
        // 그 행은 아직 "새 행 편집 중" 상태라 FocusedRowHandle이 실제 행 번호가 아니라
        // DevExpress의 가상 핸들(NewItemRowHandle, 음수)을 가리킨다 - 아래 handle >= 0 검사에
        // 걸려 조용히 아무 일도 안 일어난다(실제로 겪음 - 다른 행을 한 번 선택했다 오면 그
        // 사이에 새 행이 커밋되어 정상 동작하는 것처럼 보였다). SaveClick과 같은 방법으로
        // 먼저 편집 중인 셀을 확정해서 진짜 행 번호를 받아온다.
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        var handle = gvw2.FocusedRowHandle;
        if (handle >= 0) gvw2.DeleteRow(handle);
        return Task.CompletedTask;
    }

    /// <summary>DataTable에 바인딩하면 포커스 행 객체는 DataRowView로 온다(예전엔 DTO였다).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is DataRowView view) EnterEditMode(view.Row);
    }

    /// <summary>소분류 그리드(grd2)에 새 행을 추가하면 사용여부를 기본 'Y'로 채운다 - 비워두면
    /// 매번 체크박스를 직접 눌러줘야 하는데, 신규 등록은 대개 바로 쓰는 코드라 기본이
    /// "사용"인 편이 자연스럽다.</summary>
    private void Gvw2_InitNewRow(object? sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
    {
        gvw2.SetRowCellValue(e.RowHandle, "use_yn", "Y");
    }

    /// <summary>panData/그리드를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안 그러면
    /// 코드가 값을 채우는 것뿐인데 TrackDirty가 "사용자가 고쳤다"로 오인해서, 조회/행 선택
    /// 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingMajorCd = null;
            txtmajor_cd.Text = string.Empty;
            txtmajor_cd.ReadOnly = false;
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

            // 신규 대분류는 아직 관리항목1~10 참조명이 전부 비어있으므로 동적 컬럼도 없어야 한다 -
            // 이전에 보고 있던 대분류의 동적 컬럼이 남아있으면 지운다.
            ClearDynamicMinorColumns();

            // 소분류 그리드는 비우되 컬럼 구조는 유지해야 한다 - 새 DataTable()로 갈아끼우면
            // 컬럼이 하나도 없는 상태가 되어 그리드에 행을 추가해도 넣을 자리가 없다.
            _minors = _minors.Clone();
            TrackDirty(_minors);
            grd2.DataSource = _minors;
        });
        txtmajor_cd.Focus();
    }

    private void EnterEditMode(DataRow major)
    {
        // grd1.DataSource를 다시 세팅할 때마다(QueryClick 안) 포커스 행이 새로 잡히면서
        // FocusedRowObjectChanged가 또 발생 -> 같은 대분류인데도 여기가 또 불려서 소분류를
        // 계속 반복 재조회하는 문제가 있었다(실제로 겪음 - 우측 상세 패널이 몇 초씩 걸려 보이던
        // 원인). 실제로 "다른" 대분류로 옮겨간 경우에만 재조회한다.
        var majorCd = Str(major, "major_cd");
        var isSameMajor = _editingMajorCd == majorCd;

        SuppressDirtyTracking(() =>
        {
            _editingMajorCd = majorCd;
            txtmajor_cd.Text = majorCd;
            txtmajor_cd.ReadOnly = true; // 대분류코드는 PK라 수정 불가
            txtmajor_nm.Text = Str(major, "major_nm");

            // 관리항목1~10은 컬럼명이 번호만 다르므로 이름을 만들어서 읽는다 - DTO였을 때는 30개
            // 프로퍼티를 손으로 나열해야 했지만, 컬럼명으로 접근하니 반복문으로 끝난다.
            var titles = TxtRelTitle;
            var types = CboRelCdType;
            var codes = TxtRelCd;

            for (var i = 0; i < RelCount; i++)
            {
                var n = i + 1;
                titles[i].Text = Str(major, $"rel_title{n}");
                types[i].EditValue = Str(major, $"rel_cd_type{n}") is { Length: > 0 } t ? t : null;
                codes[i].Text = Str(major, $"rel_cd{n}");
            }
        });

        // 소분류는 이 대분류 기준으로 다시 받아와야 정확하다(그리드에서 편집 중인 내용을
        // 잃더라도, 대분류를 바꿔 선택했다는 건 이전 미저장 소분류 변경은 버린다는 뜻).
        // QueryClick() 대신 소분류 전용 API를 쓴다 - QueryClick은 대분류(grd1)까지 같이
        // 다시 받아와서 grd1.DataSource를 매번 재할당하는 바람에, grd1에서 행을 클릭할
        // 때마다 grd1 자체도 리프레시되는 것처럼 보이는 문제가 있었다(실제로 겪음).
        if (!isSameMajor)
        {
            _ = RefreshMinorSectionAsync(majorCd);
        }
    }

    /// <summary>대분류를 바꿔 선택했을 때 소분류 섹션을 전부(동적 컬럼 + 데이터) 새로 맞춘다.
    /// 동적 컬럼을 먼저 만들고 나서 데이터를 채운다 - 순서를 반대로 하면 아주 짧은 순간 방금
    /// 로드된 값이 아직 없는 컬럼에 표시를 시도하는 상태가 될 수 있다.</summary>
    private async Task RefreshMinorSectionAsync(string majorCd)
    {
        await RebuildDynamicMinorColumnsAsync();
        await LoadMinors(majorCd);
    }

    /// <summary>
    /// 관리항목1~10 패널(TxtRelTitle/CboRelCdType/TxtRelCd)에 지금 표시된 값 기준으로, 참조명이
    /// 채워진 슬롯만큼 grd2에 "사용여부"와 "비고" 사이에 동적 컬럼을 만든다. DataRow가 아니라
    /// 패널 컨트롤을 직접 읽는 이유: SaveClick에서 대분류의 관리항목 정의 자체를 방금 새로
    /// 저장한 직후에도(그 경우 major_cd는 안 바뀌어서 EnterEditMode의 isSameMajor 최적화가
    /// 소분류 재조회를 건너뛴다) 다시 불러야 하는데, 그때는 아직 서버에서 다시 조회한
    /// DataRow가 없고 패널에 방금 입력한 값만 있기 때문이다.
    ///
    /// 구분(rel_cd_type{n})이 SM00001의 "C"(CheckBox)면 체크박스로, "L"(LOOKUP)이면 그 슬롯의
    /// 참조코드(rel_cd{n})를 대분류코드로 쓰는 SSP_CBO_CODE_Q 결과를 팝업 목록으로 갖는
    /// LookUp으로 그린다. 매번 이전 컬럼/리포지토리 아이템을 먼저 전부 지운다.
    /// </summary>
    private async Task RebuildDynamicMinorColumnsAsync()
    {
        ClearDynamicMinorColumns();

        var titles = TxtRelTitle;
        var types = CboRelCdType;
        var codes = TxtRelCd;
        var insertIndex = gvw2.Columns["use_yn"].VisibleIndex + 1;

        for (var i = 0; i < RelCount; i++)
        {
            var title = titles[i].Text;
            if (title.Length == 0) continue;

            var n = i + 1;
            var column = gvw2.Columns.AddField($"rel_cd{n}");
            column.Caption = title;
            column.Visible = true;
            column.VisibleIndex = insertIndex++;
            column.Width = 90;
            _dynamicMinorColumns.Add(column);

            var relCdType = (string?)types[i].EditValue;
            if (relCdType == "C")
            {
                // 체크박스는 소분류 그리드 전체가 이미 쓰고 있는 "Y"/"N" 규약을 그대로
                // 따라야(사용여부와 동일 규약) SSP_CBO_CODE_Q 등 다른 곳과 값이 어긋나지
                // 않는다 - use_yn 컬럼과 같은 리포지토리 아이템을 그대로 재사용한다.
                column.ColumnEdit = repositoryItemCheckEdit1;
            }
            else if (relCdType == "L")
            {
                var relCd = codes[i].Text;

                // 참조코드가 아직 실제 대분류코드가 아니거나(입력 중 임시값 등) 서버가 잠깐
                // 오류를 내도 목록 하나 때문에 저장 전체가 실패한 것처럼 보이면 안 된다(실제로
                // 겪음 - SaveClick 안에서 이 메서드를 부르는데, 목록 조회가 예외를 던지니 방금
                // 성공한 대분류 저장까지 "[저장] 처리 중 오류가 발생했습니다"로 잘못 보였다).
                // LookUpEditWyn.LoadAsync도 같은 이유로 조용히 무시한다 - 여기도 맞춘다.
                List<CodeLookupItem> items;
                try
                {
                    items = CodeLookupProvider.Providers.TryGetValue("SSP_CBO_CODE_Q", out var fetch)
                        ? (await fetch(relCd)).ToList()
                        : new List<CodeLookupItem>();
                }
                catch
                {
                    items = new List<CodeLookupItem>();
                }

                // LookUpEditWyn.BindCodeList와 같은 "코드+명" 2열 팝업 모양으로 맞춘다.
                var lookup = new RepositoryItemLookUpEdit { NullText = string.Empty };
                lookup.Columns.Add(new LookUpColumnInfo(nameof(CodeLookupItem.Value), "코드", 80));
                lookup.Columns.Add(new LookUpColumnInfo(nameof(CodeLookupItem.Display), "명칭"));
                lookup.ValueMember = nameof(CodeLookupItem.Value);
                lookup.DisplayMember = nameof(CodeLookupItem.Display);
                lookup.DataSource = items;
                lookup.PopupWidth = 260;
                lookup.AutoSearchColumnIndex = 1;
                lookup.ShowFooter = false;

                grd2.RepositoryItems.Add(lookup);
                _dynamicMinorRepositoryItems.Add(lookup);
                column.ColumnEdit = lookup;
            }
        }
    }

    private void ClearDynamicMinorColumns()
    {
        foreach (var column in _dynamicMinorColumns) gvw2.Columns.Remove(column);
        _dynamicMinorColumns.Clear();

        foreach (var item in _dynamicMinorRepositoryItems) grd2.RepositoryItems.Remove(item);
        _dynamicMinorRepositoryItems.Clear();
    }

    /// <summary>DataRow에서 문자열 컬럼을 안전하게 읽는다 - 없는 컬럼이거나 NULL이면 빈 문자열.
    /// 컬럼이 없어도 예외를 내지 않는 이유: 프로시저가 돌려주는 컬럼이 나중에 바뀌어도 화면이
    /// 통째로 죽지 않게 하기 위함이다(타입이 없는 대신 감수하는 부분).</summary>
    private static string Str(DataRow row, string columnName) =>
        row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? Convert.ToString(row[columnName]) ?? string.Empty
            : string.Empty;

    private async Task LoadMinors(string majorCd)
    {
        _minors = await QueryAsync("USP_SM_MINORCODE_Q", new
        {
            p_work_type = "Q1",
            p_major_cd = majorCd
        });
        TrackDirty(_minors);

        grd2.DataSource = _minors;
    }

    /// <summary>
    /// 그리드에서 바뀐 소분류 행마다 USP_SM_MINORCODE_S_1을 순서대로 한 번씩 호출한다.
    /// USP_SM_MINORCODE_S(대분류)와 같은 단일 레코드 CRUD 구조라 그리드 전체를 한 번에 보낼
    /// 방법이 없어서, 바뀐 행 수만큼 서버를 왕복한다.
    ///
    /// 이 프로시저는 major_cd처럼 minor_cd도 한 번 등록되면 안 바뀌는 키로 다룬다(U 분기가
    /// minor_cd를 WHERE에서만 쓰고 SET하지 않음). 그래서 그리드에서 소분류코드 자체를 고친
    /// 행은 "원래 코드 삭제(D) + 새 코드 등록(N)" 두 번의 호출로 표현한다.
    ///
    /// 호출 하나가 실패하면 그 자리에서 멈추고 실패 행을 알려준다. 이 시점까지 성공한 호출은
    /// 각자 독립된 트랜잭션으로 이미 서버에 반영된 상태이므로(007과 달리 그리드 전체를 묶는
    /// 트랜잭션이 없다), 재조회하면 그때까지 성공한 변경사항은 남아있고 실패한 것부터 다시
    /// 시도하면 된다.
    /// </summary>
    private async Task<string?> SaveMinorRowsAsync(string majorCd)
    {
        foreach (DataRow row in _minors.Rows)
        {
            string? error = row.RowState switch
            {
                DataRowState.Added => await SaveNewMinorAsync(majorCd, row),
                DataRowState.Modified => await SaveModifiedMinorAsync(majorCd, row),
                DataRowState.Deleted => await SaveDeletedMinorAsync(majorCd, row),
                _ => null // Unchanged - 보낼 필요 없음
            };
            if (error != null) return error;
        }

        return null; // 전부 성공
    }

    private async Task<string?> SaveNewMinorAsync(string majorCd, DataRow row)
    {
        var minorCd = ProcData.Str(row, "minor_cd", DataRowVersion.Current);
        if (minorCd.Length == 0) return null; // 코드 없이 행만 추가해둔 빈 행은 건너뜀

        var result = await SaveAsync("USP_SM_MINORCODE_S_1", MinorParams("N", majorCd, row, DataRowVersion.Current));
        return result.Success ? null : $"[{minorCd}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveModifiedMinorAsync(string majorCd, DataRow row)
    {
        var origMinorCd = ProcData.Str(row, "minor_cd", DataRowVersion.Original);
        var minorCd = ProcData.Str(row, "minor_cd", DataRowVersion.Current);

        if (minorCd != origMinorCd)
        {
            // 코드 자체가 바뀌었다 - 이 프로시저는 코드를 바꾸는 수정을 표현할 수 없으므로
            // (클래스 설명 참고), 원래 코드를 지우고 새 코드로 다시 등록한다.
            var delResult = await SaveAsync("USP_SM_MINORCODE_S_1",
                new { p_work_type = "D", p_major_cd = majorCd, p_minor_cd = origMinorCd });
            if (!delResult.Success) return $"[{origMinorCd}] {FormatSaveFailMessage(delResult)}";

            var addResult = await SaveAsync("USP_SM_MINORCODE_S_1", MinorParams("N", majorCd, row, DataRowVersion.Current));
            return addResult.Success ? null : $"[{minorCd}] {FormatSaveFailMessage(addResult)}";
        }

        var result = await SaveAsync("USP_SM_MINORCODE_S_1", MinorParams("U", majorCd, row, DataRowVersion.Current));
        return result.Success ? null : $"[{minorCd}] {FormatSaveFailMessage(result)}";
    }

    private async Task<string?> SaveDeletedMinorAsync(string majorCd, DataRow row)
    {
        var minorCd = ProcData.Str(row, "minor_cd", DataRowVersion.Original);
        var result = await SaveAsync("USP_SM_MINORCODE_S_1",
            new { p_work_type = "D", p_major_cd = majorCd, p_minor_cd = minorCd });
        return result.Success ? null : $"[{minorCd}] {FormatSaveFailMessage(result)}";
    }

    /// <summary>신규(N)/수정(U) 호출에 필요한 파라미터를 DataRow에서 뽑아 담는다.
    /// use_yn이 비어 있으면(그리드에 값이 없는 신규 행 등) 기본값 'Y'를 보낸다.
    ///
    /// rel_cd1~10은 동적 컬럼(RebuildDynamicMinorColumnsAsync)이 채운 값이다 - USP_SM_MINORCODE_S_1은
    /// 이미 이 파라미터들을 받게 되어 있었지만, 지금까지 화면이 안 보내고 있었을 뿐이다.
    ///
    /// 익명 객체 대신 Dictionary를 직접 만드는 이유: ProcData.ToParams가 Dictionary는 반사 없이
    /// 그대로 통과시키기 때문이다(SaveClick의 대분류 저장과 같은 이유) - 반복문으로 10개
    /// 파라미터를 채워야 해서 처음부터 Dictionary가 더 자연스럽기도 하다.</summary>
    private static Dictionary<string, string?> MinorParams(string workType, string majorCd, DataRow row, DataRowVersion version)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_major_cd"] = majorCd,
            ["p_minor_cd"] = ProcData.Str(row, "minor_cd", version),
            ["p_minor_nm"] = ProcData.Str(row, "minor_nm", version),
            ["p_sort"] = ProcData.Str(row, "sort", version),
            ["p_use_yn"] = ProcData.Str(row, "use_yn", version) is { Length: > 0 } useYn ? useYn : "Y",
            ["p_remark"] = ProcData.Str(row, "remark", version)
        };

        for (var n = 1; n <= RelCount; n++)
            p[$"p_rel_cd{n}"] = ProcData.Str(row, $"rel_cd{n}", version);

        return p;
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

        // 프로시저 파라미터를 이름으로 직접 담는다. 관리항목1~10은 이름 규칙이 일정해서
        // 반복문으로 채운다 - 예전엔 30개 필드를 손으로 나열한 DTO가 필요했다.
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingMajorCd == null ? "N" : "U",
            ["p_major_cd"] = txtmajor_cd.Text.ToUpper(),
            ["p_major_nm"] = txtmajor_nm.Text,
            // panData에 사용여부/비고 입력 UI가 아직 없어서 고정값을 보낸다(클래스 설명 참고).
            ["p_sys_yn"] = "N",
            ["p_remark"] = string.Empty
        };

        for (var i = 0; i < RelCount; i++)
        {
            var n = i + 1;
            p[$"p_rel_cd{n}"] = codes[i].Text;
            p[$"p_rel_title{n}"] = titles[i].Text;
            p[$"p_rel_cd_type{n}"] = (string?)types[i].EditValue;
        }

        var wasNew = _editingMajorCd == null;
        var result = await SaveAsync("USP_SM_MINORCODE_S", p);

        if (!result.Success)
        {
            AppMessageBox.Show(FormatSaveFailMessage(result), "저장 실패");
            return;
        }

        var savedMajorCd = wasNew ? (result.GeneratedCode ?? p["p_major_cd"]!) : _editingMajorCd!;

        // 그리드에서 편집 중이던 셀 값을 먼저 확정해야 아래 저장에 마지막 수정이 포함된다.
        gvw2.CloseEditor();
        gvw2.UpdateCurrentRow();

        // 관리항목1~10 정의(참조명/구분/참조코드) 자체가 방금 바뀌었을 수 있다 - 새 대분류든
        // 기존 대분류의 정의를 고친 것이든 major_cd는 안 바뀌므로, EnterEditMode의 isSameMajor
        // 최적화가 재조회 후에도 동적 컬럼을 다시 안 그린다(실제로 겪음). 그래서 여기서 패널에
        // 지금 표시된 값 기준으로 명시적으로 한 번 다시 그린다.
        await RebuildDynamicMinorColumnsAsync();

        var minorSaveError = await SaveMinorRowsAsync(savedMajorCd);
        if (minorSaveError != null)
        {
            AppMessageBox.Show(minorSaveError, "저장 실패");
            return;
        }

        // 방금 보낸 신규/수정/삭제가 그대로 서버에 반영됐으니, 로컬 DataTable도 여기서
        // AcceptChanges로 확정한다 - 이게 없으면 각 행이 여전히 Added/Modified/Deleted로
        // 남아있어서, 재조회 없이 곧바로 다시 저장을 누르면(아래 QueryClick이 같은 대분류를
        // "안 바뀐 것"으로 보고 소분류를 다시 안 불러오므로) 방금 넣은 신규 행을 또
        // INSERT하려다 PK 중복 오류가 난다(실제로 겪음).
        _minors.AcceptChanges();

        _editingMajorCd = savedMajorCd;
        gvw2.ClearDirtyMarks();
        await QueryCore(preserveSelection: true); // 방금 저장한 행 유지 - QueryClick(사용자 조회)과 다른 경로
        Toast.Show(wasNew ? "대분류코드가 등록되었습니다." : "수정되었습니다.");
    }

    /// <summary>ApiResult.ErrorCode는 SQL 예외(ERROR_NUMBER())일 때만 채워진다(0이면 업무로직
    /// 판단만으로 실패 - 예: 필수값 누락) - 그럴 때만 메시지에 오류번호를 같이 보여준다.</summary>
    private static string FormatSaveFailMessage(ApiResult? result)
    {
        var message = result?.Message ?? "저장에 실패했습니다.";
        return result is { ErrorCode: not 0 } ? $"{message} (오류코드: {result.ErrorCode})" : message;
    }

    // 소분류 그리드 헤더의 +/- 버튼(Designer.cs가 이 두 메서드에 직접 연결)이 NewRowClick/
    // DeleteRowClick을 그대로 호출한다 - 생성자에서 또 구독하면 클릭 한 번에 두 번씩 불리므로
    // (생성자 주석 참고) 연결은 여기 이 두 메서드 하나로만 존재해야 한다.
    private async void btnAddRow2_Click(object sender, EventArgs e)
    {
        await NewRowClick();
    }

    private async void btnDeletRow2_Click(object sender, EventArgs e)
    {
        await DeleteRowClick();
    }
}
