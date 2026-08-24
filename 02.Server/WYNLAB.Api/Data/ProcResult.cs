using System.Data;
using Dapper;

namespace WYNLAB.Api.Data;

/// <summary>
/// 저장(쓰기)용 프로시저(USP_*_S*)가 공통으로 돌려주는 표준 출력값.
/// ReturnCode/ReturnMsg: 프로시저 안의 업무로직이 판단한 성공/실패.
/// ErrorCode/ErrorMsg: TRY/CATCH로 잡힌 SQL 예외(ERROR_NUMBER/ERROR_MESSAGE) - 이게 채워지면
/// DB 연결/스키마 문제 같은 예상 못한 오류라는 뜻(과거 DB명 오타로 빈 응답만 오던 문제를 이걸로 막는다).
/// GeneratedCode: 신규등록(Mode='C') 시 채번/확정된 PK값 - 지금은 화면에서 입력한 값을 그대로 돌려주지만,
/// 나중에 자동채번 화면이 생기면 같은 규칙으로 그 값을 돌려주면 된다.
/// </summary>
public class ProcResult
{
    public int ReturnCode { get; set; }
    public string? ReturnMsg { get; set; }
    public int ErrorCode { get; set; }
    public string? ErrorMsg { get; set; }
    public string? GeneratedCode { get; set; }

    public bool IsSuccess => ReturnCode == 0 && ErrorCode == 0;

    /// <summary>실패 시 화면에 보여줄 메시지 - SQL 예외가 있으면 그쪽을, 없으면 업무로직 메시지를 우선한다</summary>
    public string? FailMessage => ErrorCode != 0 ? ErrorMsg : ReturnMsg;
}

public static class ProcParamsExtensions
{
    /// <summary>
    /// 표준 출력 5종의 파라미터 키 이름. 새 프로시저 표준(@p_work_type 계열, CODE 모듈부터 적용)은
    /// 접두사 없는 PascalCase(GeneratedCode 등)를 쓰고, 아직 026에서 snake_case로만 리네임된
    /// 예전 모듈(USER/MENU/USERGRP/LOGIN/MENUAUTH)은 return_code 등 snake_case 그대로다 -
    /// 모듈별로 실제 DB 프로시저가 어느 쪽 이름을 쓰는지에 맞춰 pascalCase를 지정해야 한다.
    /// </summary>
    public static void AddStandardOutputs(this DynamicParameters p, bool withGeneratedCode = false, bool pascalCase = false)
    {
        if (withGeneratedCode)
            p.Add(pascalCase ? "GeneratedCode" : "generated_code", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

        p.Add(pascalCase ? "ReturnCode" : "return_code", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add(pascalCase ? "ReturnMsg" : "return_msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        p.Add(pascalCase ? "ErrorCode" : "error_code", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add(pascalCase ? "ErrorMsg" : "error_msg", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
    }

    public static ProcResult ReadStandardOutputs(this DynamicParameters p, bool withGeneratedCode = false, bool pascalCase = false) => new()
    {
        GeneratedCode = withGeneratedCode ? p.Get<string?>(pascalCase ? "GeneratedCode" : "generated_code") : null,
        ReturnCode = p.Get<int?>(pascalCase ? "ReturnCode" : "return_code") ?? 0,
        ReturnMsg = p.Get<string?>(pascalCase ? "ReturnMsg" : "return_msg"),
        ErrorCode = p.Get<int?>(pascalCase ? "ErrorCode" : "error_code") ?? 0,
        ErrorMsg = p.Get<string?>(pascalCase ? "ErrorMsg" : "error_msg")
    };
}
