namespace WYNLAB.Shared.Dtos;

/// <summary>레코드셋 1개의 컬럼 구조 - DescribeProcMultiResultDto.ResultSets의 원소.
/// 컬럼 자체의 모양(ProcColumnInfoDto)은 기존 describe-proc과 같은 걸 재사용한다.</summary>
public class ProcResultSetInfoDto
{
    public int Index { get; set; }
    public List<ProcColumnInfoDto> Columns { get; set; } = new();
}

/// <summary>api/screen-builder/describe-proc-multi 응답 - 프로시저 하나가 반환하는 레코드셋을
/// 순서대로 전부 읽는다. 기존 describe-proc(sys.dm_exec_describe_first_result_set, 실행 없이
/// 정적분석이라 첫 레코드셋만 보임)과 달리, 이건 트랜잭션 안에서 실제로 EXECUTE한 뒤 ROLLBACK한다 -
/// 정적분석 방식으로는 두 번째 이후 레코드셋을 볼 방법이 없기 때문(SQL Server 제약).</summary>
public class DescribeProcMultiResultDto
{
    public List<ProcParamInfoDto> Params { get; set; } = new();
    public List<ProcResultSetInfoDto> ResultSets { get; set; } = new();
}

/// <summary>api/screen-builder/proc-work-types 응답 - 프로시저 소스(OBJECT_DEFINITION)에서
/// "@p_work_type = 'X'" 형태로 비교하는 리터럴 값을 전부 찾아 등장 순서대로 돌려준다. 실행하지
/// 않는 정적 텍스트 검색이라 실행 위험이 없다 - 이 코드베이스의 SQL 프로시저 관례
/// (feedback_sql_proc_style: "@p_work_type 디스패치, IF/ELSE IF 분기")를 그대로 이용한다.
/// AI Builder "레코드셋 조회"가 계획 그리드에 WorkType을 비워둔 행을 만나면 이걸 먼저 불러서
/// Q/Q1/... 각각을 자동으로 describe한다(2026-09-09 - "프로시저 이름만 넣으면 Q/Q1을 알아서
/// 찾아달라"는 요청).</summary>
public class ProcWorkTypesResultDto
{
    public List<string> WorkTypes { get; set; } = new();
}
