using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IUserManageRepository
{
    Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null);
    Task<bool> ExistsAsync(string userId);
    Task CreateAsync(string userId, string userNm, string passwordHash, string? empNo,
        string? deptCd, string? positionNm, string? email, string? mobileNo, bool isAdminYn);
    Task UpdateAsync(string userId, string userNm, string? empNo, string? deptCd,
        string? positionNm, string? email, string? mobileNo, bool useYn, bool isAdminYn);
    Task SetUseYnAsync(string userId, bool useYn);
}

/// <summary>DB 조회 전용 - 목록 화면 표시용 (비밀번호 해시는 절대 포함하지 않음)</summary>
public class UserManageRow
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string? DeptCd { get; set; }
    public string? DeptNm { get; set; }
    public string? PositionNm { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public string UseYn { get; set; } = "Y";
    public string IsAdminYn { get; set; } = "N";
    public DateTime? LastLoginDt { get; set; }
}

public class UserManageRepository : IUserManageRepository
{
    private readonly IDapperContext _context;

    public UserManageRepository(IDapperContext context) => _context = context;

    /// <summary>userId/userNm 둘 다 부분일치(LIKE) 검색 - 값이 없으면(null/공백) 해당 조건은 무시</summary>
    public async Task<List<UserManageRow>> GetAllAsync(string? userId = null, string? userNm = null)
    {
        const string sql = @"
            SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, U.EMP_NO AS EmpNo,
                   U.DEPT_CD AS DeptCd, D.DEPT_NM AS DeptNm, U.POSITION_NM AS PositionNm,
                   U.EMAIL AS Email, U.MOBILE_NO AS MobileNo, U.USE_YN AS UseYn,
                   U.IS_ADMIN_YN AS IsAdminYn, U.LAST_LOGIN_DT AS LastLoginDt
            FROM TSMUSER U
            LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
            WHERE (@UserId IS NULL OR U.USER_ID LIKE '%' + @UserId + '%')
              AND (@UserNm IS NULL OR U.USER_NM LIKE '%' + @UserNm + '%')
            ORDER BY U.REG_DT DESC";

        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<UserManageRow>(sql, new
        {
            UserId = string.IsNullOrWhiteSpace(userId) ? null : userId,
            UserNm = string.IsNullOrWhiteSpace(userNm) ? null : userNm
        });
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string userId)
    {
        const string sql = "SELECT COUNT(1) FROM TSMUSER WHERE USER_ID = @UserId";
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        return count > 0;
    }

    public async Task CreateAsync(string userId, string userNm, string passwordHash, string? empNo,
        string? deptCd, string? positionNm, string? email, string? mobileNo, bool isAdminYn)
    {
        const string sql = @"
            INSERT INTO TSMUSER (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, DEPT_CD, POSITION_NM, EMAIL, MOBILE_NO, IS_ADMIN_YN, USE_YN)
            VALUES (@UserId, @UserNm, @PasswordHash, @EmpNo, @DeptCd, @PositionNm, @Email, @MobileNo, @IsAdminYn, 'Y')";

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new
        {
            UserId = userId,
            UserNm = userNm,
            PasswordHash = passwordHash,
            EmpNo = empNo,
            DeptCd = deptCd,
            PositionNm = positionNm,
            Email = email,
            MobileNo = mobileNo,
            IsAdminYn = isAdminYn ? "Y" : "N"
        });
    }

    public async Task UpdateAsync(string userId, string userNm, string? empNo, string? deptCd,
        string? positionNm, string? email, string? mobileNo, bool useYn, bool isAdminYn)
    {
        const string sql = @"
            UPDATE TSMUSER SET
                USER_NM = @UserNm, EMP_NO = @EmpNo, DEPT_CD = @DeptCd, POSITION_NM = @PositionNm,
                EMAIL = @Email, MOBILE_NO = @MobileNo, USE_YN = @UseYn, IS_ADMIN_YN = @IsAdminYn
            WHERE USER_ID = @UserId";

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new
        {
            UserId = userId,
            UserNm = userNm,
            EmpNo = empNo,
            DeptCd = deptCd,
            PositionNm = positionNm,
            Email = email,
            MobileNo = mobileNo,
            UseYn = useYn ? "Y" : "N",
            IsAdminYn = isAdminYn ? "Y" : "N"
        });
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 이력/참조무결성 보존을 위한 표준 삭제 방식</summary>
    public async Task SetUseYnAsync(string userId, bool useYn)
    {
        const string sql = "UPDATE TSMUSER SET USE_YN = @UseYn WHERE USER_ID = @UserId";
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new { UserId = userId, UseYn = useYn ? "Y" : "N" });
    }
}
