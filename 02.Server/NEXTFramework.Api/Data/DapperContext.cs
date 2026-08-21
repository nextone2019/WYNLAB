using System.Data;
using Microsoft.Data.SqlClient;

namespace NEXTFramework.Api.Data;

/// <summary>
/// Dapper용 DB 커넥션 팩토리. Repository는 이 인터페이스만 의존한다.
/// (테스트 시 Mock 교체 용이하도록 인터페이스 분리)
/// </summary>
public interface IDapperContext
{
    IDbConnection CreateConnection();
}

public class DapperContext : IDapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NextFrameworkDb")
            ?? throw new InvalidOperationException("ConnectionStrings:NextFrameworkDb 설정이 없습니다.");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
