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
