using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SYS;

/// <summary>
/// 프로시저 빌더 - 테이블 구조를 읽어서 컬럼을 체크로 골라 조회(Q) 또는 저장(N/U/D) 프로시저
/// 코드를 만드는 도구. AI Builder(frmAIBuilder, 화면 코드 생성)와 발상은 같지만 입출력이 완전히
/// 달라(화면 코드 vs SQL 텍스트) 별도 화면/컨트롤러로 뒀다(00.DEV/MODULE_ARCHITECTURE.md의
/// "별도 도구, 공용 유틸리티만 공유" 판단 - 2026-09-13, "AI Builder에 통합하기엔 너무
/// 복잡하지 않을까"라는 사용자 우려에 따라 처음부터 이렇게 결정함).
///
/// 2026-09-14 재설계 - 실제로 써보고 나온 피드백 반영:
/// - 모듈은 자유입력 대신 AI Builder(frmAIBuilder.cboModule)와 같은 룩업(L_SM0003)으로.
/// - "기능명+패턴" 대신 "프로시저 이름(편집 가능, 모듈+기능명으로 자동 제안)" + "조회/저장 선택".
/// - 가져온 컬럼을 전부 쓰는 대신 체크박스로 골라서 쓴다(조회는 WHERE 필터 여부도 같이 체크,
///   저장은 Insert/Update 포함 여부를 같이 체크).
/// - "프로시저 생성" 버튼이 미리보기 저장뿐 아니라 실제로 지금 접속한 서비스의 DB에 실행까지
///   한다(사용자 확인) - 실행 전 어느 서비스인지 반드시 확인 대화상자를 띄운다. 그리고 나서도
///   재현 가능성을 위해 마이그레이션 파일로도 같이 남긴다(이 저장소의 "새 마이그레이션 파일만
///   추가" 관례 유지 - 운영/다른 PC에도 같은 프로시저를 재현할 수 있어야 하므로).
/// - 화면 배치는 2026-09-14부터 이 파일이 아니라 frmProcBuilder.Designer.cs가 맡는다(Visual
///   Studio 폼 디자이너로 컨트롤을 직접 드래그해서 조정할 수 있게 해달라는 요청) - 이 파일은
///   InitializeComponent() 호출 이후의 값 채우기 + 이벤트 + 실제 로직만 담는다.
///
/// 실제 조립 로직은 ProcTemplateGenerator.cs 참고 - 이 화면은 입력을 모아 그걸 부르고
/// 결과를 미리보기에 보여준 뒤, 확인되면 DB 실행 + 마이그레이션 파일 저장을 한다.
/// </summary>
public partial class frmProcBuilder : BaseForm
{
    // AI Builder(frmAIBuilder.cs)와 같은 이유 - 이 저장소 전용 개발 도구라 배포 대상마다
    // 달라질 이유가 없다.
    private const string RepoRoot = @"D:\01. SOURCE\00. WYNLAB";

    private List<SelectedColumn> _masterColumns = new();
    private List<SelectedColumn> _detailColumns = new();

    public frmProcBuilder()
    {
        Text = "프로시저 빌더";
        InitializeComponent();

        cboModule.LookupKey = "L_SM0003";

        cboModule.EditValueChanged += (s, e) => UpdateSuggestedProcName();
        cboType.EditValueChanged += (s, e) => { UpdateColumnVisibility(); UpdateSuggestedProcName(); };

        btnDescribeMaster.Click += async (s, e) => await DescribeAsync(isDetail: false);
        btnDescribeDetail.Click += async (s, e) => await DescribeAsync(isDetail: true);
        btnSelectAllMaster.Click += (s, e) => SelectAll(_masterColumns, grdMasterColumns);
        btnSelectAllDetail.Click += (s, e) => SelectAll(_detailColumns, grdDetailColumns);
        btnGenerate.Click += (s, e) => GenerateSql();
        btnCreateProc.Click += async (s, e) => await CreateProcAsync();

        UpdateColumnVisibility();
    }

    // cboType이 L_SM0007(Procedure종류, minor_cd: Q="Query"/S="Save") 룩업으로 바뀌면서
    // (2026-09-14, 사용자가 디자이너에서 직접 ComboBoxEdit -> LookUpEditWyn으로 교체) SelectedIndex
    // 대신 실제 선택된 코드값(EditValue)으로 판단한다 - cboModule과 같은 방식(LookUpEditWyn.EditValue
    // override는 미선택 상태에서 null 대신 빈 문자열을 돌려주므로 안전하게 비교할 수 있다).
    private bool IsQuery => cboType.EditValue?.ToString() == "Q";

    /// <summary>지금 고른 종류(조회/저장)에 맞는 체크박스 컬럼만 보여준다 - WHERE 필터는 조회에서만,
    /// Insert/Update는 저장에서만 의미가 있다(둘 다 항상 보이게 두면 헷갈린다는 판단). 상세 테이블
    /// 조회(Q_1)도 조회 프로시저에서만 의미가 있다.</summary>
    private void UpdateColumnVisibility()
    {
        colMasterIsWhereFilter.Visible = IsQuery;
        colMasterIncludeInsert.Visible = !IsQuery;
        colMasterIncludeUpdate.Visible = !IsQuery;

        // 상세 테이블 섹션(제목/입력/그리드)이 사용자가 디자이너에서 panelWyn6 하나로 합쳐놨다
        // (2026-09-14) - 그 컨테이너 하나만 토글하면 안의 컨트롤이 다 같이 숨겨진다. 단, 링크
        // 컬럼 라벨/콤보는 디자이너 기본값이 Visible=false라서(사용자가 그렇게 둠) 부모가
        // 다시 보여도 자기 자신은 그대로 숨은 채라 따로 켜줘야 한다.
        panelWyn6.Visible = IsQuery;
        lblLinkColumn.Visible = IsQuery;
        cboLinkColumn.Visible = IsQuery;
    }

    /// <summary>모듈+종류로 "USP_{모듈}_Q"(또는 _S) 형태를 제안한다 - AI
    /// Builder(frmAIBuilder.UpdateSuggestedProcPrefix)와 같은 패턴. 자유입력 필드라 사용자가
    /// 나머지(기능명 부분)를 이어서 직접 채워 넣거나 통째로 고쳐 쓸 수 있다(2026-09-14 - "기능명은
    /// 프로시저명 생성용일 뿐이니 프로시저명 칸 하나면 된다"는 요청으로 별도 기능명 입력칸을 없앰).</summary>
    private void UpdateSuggestedProcName()
    {
        var moduleCd = cboModule.EditValue?.ToString()?.Trim().ToUpperInvariant() ?? string.Empty;
        var suffix = IsQuery ? "Q" : "S";

        txtProcName.Text = moduleCd.Length == 0 ? string.Empty : $"USP_{moduleCd}_{suffix}";
    }

    private async Task DescribeAsync(bool isDetail)
    {
        var tableName = (isDetail ? txtDetailTable.Text : txtMasterTable.Text).Trim();
        if (string.IsNullOrWhiteSpace(tableName))
        {
            AppMessageBox.Show("테이블명을 입력해주세요.", "확인");
            return;
        }

        DescribeTableResultDto? result;
        try
        {
            result = await ApiClient.GetAsync<DescribeTableResultDto>($"api/proc-builder/describe-table?tableName={Uri.EscapeDataString(tableName)}");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            return;
        }

        if (result == null || result.Columns.Count == 0)
        {
            AppMessageBox.Show($"'{tableName}' 테이블을 찾을 수 없습니다.", "확인");
            return;
        }

        // PK 컬럼은 기본으로 체크해둔다 - 조회(Q)의 SELECT 목록에도, 저장(S)의 WHERE/필수 키에도
        // 거의 항상 필요하므로 매번 손으로 체크하는 수고를 던다.
        var columns = result.Columns.Select(c => new SelectedColumn { Column = c, IsSelected = c.IsPrimaryKey }).ToList();

        if (isDetail)
        {
            _detailColumns = columns;
            grdDetailColumns.DataSource = null;
            grdDetailColumns.DataSource = _detailColumns;

            cboLinkColumn.Properties.Items.Clear();
            foreach (var col in _detailColumns) cboLinkColumn.Properties.Items.Add(col.ColumnNm);
        }
        else
        {
            _masterColumns = columns;
            grdMasterColumns.DataSource = null;
            grdMasterColumns.DataSource = _masterColumns;
        }
    }

    /// <summary>"전체선택" - 컬럼이 많은 테이블(수십 개)을 하나씩 체크하는 게 번거롭다는
    /// 요청(2026-09-14)으로 추가. IsSelected는 SelectedColumn(POCO)의 평범한 속성이라
    /// DataSource에 다시 대입해줘야 그리드 체크박스가 갱신된다.</summary>
    private static void SelectAll(List<SelectedColumn> columns, GridControlWyn grid)
    {
        if (columns.Count == 0) return;
        foreach (var c in columns) c.IsSelected = true;
        grid.RefreshDataSource();
    }

    private ProcGenSpec BuildSpec()
    {
        return new ProcGenSpec
        {
            ProcName = txtProcName.Text.Trim(),
            Type = IsQuery ? ProcType.Query : ProcType.Save,
            MasterTable = txtMasterTable.Text.Trim(),
            MasterColumns = _masterColumns,
            DetailTable = string.IsNullOrWhiteSpace(txtDetailTable.Text) ? null : txtDetailTable.Text.Trim(),
            DetailColumns = _detailColumns,
            DetailLinkColumn = cboLinkColumn.Text.Trim(),
        };
    }

    private void GenerateSql()
    {
        try
        {
            memoPreview.Text = ProcTemplateGenerator.Generate(BuildSpec());
        }
        catch (ArgumentException ex)
        {
            AppMessageBox.Show(ex.Message, "확인");
        }
    }

    /// <summary>"프로시저 생성" - 미리보기를 그대로 지금 접속한 서비스의 DB에 실행하고, 재현
    /// 가능성을 위해 마이그레이션 파일로도 남긴다(2026-09-14 요청). 실행 전 지금 어느 서비스에
    /// 붙어있는지 반드시 확인시킨다 - 개발PC/운영서버를 실수로 헷갈리면 되돌리기 어려운 사고라,
    /// 이 화면이 지금까지 겪은 여러 "환경 헷갈림" 사고와 같은 종류의 위험이다.</summary>
    private async Task CreateProcAsync()
    {
        if (string.IsNullOrWhiteSpace(memoPreview.Text))
        {
            AppMessageBox.Show("먼저 '스크립트 생성'으로 미리보기를 만들어주세요.", "확인");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtProcName.Text))
        {
            AppMessageBox.Show("프로시저 이름을 입력해주세요.", "확인");
            return;
        }

        if (AppMessageBox.Show(
                $"지금 접속 서비스: '{AppConfig.CurrentEnvironment}' ({AppConfig.ApiBaseUrl})\n\n" +
                $"이 서비스의 DB에 '{txtProcName.Text.Trim()}' 프로시저를 생성/변경하시겠습니까?",
                "확인", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        var sql = memoPreview.Text;

        ApiResult? result;
        try
        {
            result = await ApiClient.PostAsync<ExecuteProcRequest, ApiResult>("api/proc-builder/execute", new ExecuteProcRequest { Sql = sql });
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"실행 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            return;
        }

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "실행에 실패했습니다.", "오류");
            return;
        }

        // 다른 PC/운영서버에도 같은 프로시저를 재현할 수 있어야 하므로, 실행 성공 후에도
        // 마이그레이션 파일로 남긴다(이 저장소의 "새 마이그레이션 파일만 추가, 사람이 검토 후
        // 직접 적용" 관례와는 살짝 다르게 - 여기는 이미 검토 후 실행까지 끝난 결과물을 기록만
        // 남기는 것).
        var path = ProcTemplateGenerator.NextMigrationPath(RepoRoot, txtProcName.Text.Trim());
        try
        {
            File.WriteAllText(path, sql, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }
        catch (Exception ex)
        {
            Toast.Show($"프로시저는 생성됐지만 마이그레이션 파일 저장에 실패했습니다: {ex.Message}");
            return;
        }

        Toast.Show($"프로시저 생성 완료. 마이그레이션 파일: {Path.GetFileName(path)}");
    }
}
