namespace WYNLAB.Shared.Dtos;

/// <summary>프로시저 빌더(frmProcBuilder, SYS_PROC_BUILDER) 전용 - AI Builder(ScreenBuilderDto)가
/// "프로시저를 실행해서 결과셋 모양을 본다"는 것과 달리, 이건 테이블 하나의 컬럼 구조를 직접
/// 읽어서(INFORMATION_SCHEMA/sys.columns) 마스터/마스터+상세 CRUD 프로시저(Q+S) 코드를 만드는 데
/// 쓴다. AI Builder와는 입출력 모양이 달라 별도 화면/DTO로 뒀다(00.DEV/MODULE_ARCHITECTURE.md
/// 판단 기준과 별개로, 같은 SYS 모듈 안에서도 화면 단위로는 서로 독립적인 게 자연스럽다).</summary>
public class TableColumnInfoDto
{
    public string ColumnNm { get; set; } = string.Empty;
    public string SqlType { get; set; } = string.Empty;
    public int? MaxLength { get; set; }
    public bool IsNullable { get; set; }
    public bool IsIdentity { get; set; }
    public bool IsPrimaryKey { get; set; }
}

public class DescribeTableResultDto
{
    public List<TableColumnInfoDto> Columns { get; set; } = new();
}

/// <summary>frmProcBuilder가 조립한 CREATE OR ALTER PROCEDURE 텍스트를 그대로 실행 요청할 때
/// 쓴다(2026-09-14 - "생성 버튼을 누르면 실제로 DB에 반영돼야 한다"는 요청으로 추가). 컨트롤러가
/// 텍스트 맨 앞이 CREATE [OR ALTER] PROCEDURE인지만 확인하고 그대로 실행한다 - 임의 SQL 실행
/// 엔드포인트가 되지 않도록 그 검사가 핵심이다.</summary>
public class ExecuteProcRequest
{
    public string Sql { get; set; } = string.Empty;
}
