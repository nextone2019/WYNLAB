namespace WYNLAB.Shared.Dtos;

/// <summary>사용자그룹 목록/상세 조회용</summary>
public class UserGroupListItemDto
{
    public string UserGrpCd { get; set; } = string.Empty;
    public string UserGrpNm { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool UseYn { get; set; }
    public int MemberCount { get; set; }
}

public class UserGroupCreateRequest
{
    public string UserGrpCd { get; set; } = string.Empty;
    public string UserGrpNm { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}

/// <summary>USER_GRP_CD는 PK라 수정 불가 - 나머지 항목만 변경 가능</summary>
public class UserGroupUpdateRequest
{
    public string UserGrpNm { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool UseYn { get; set; }
}

/// <summary>그룹 소속 배정 화면용 - 전체 사용자 목록에 현재 이 그룹 소속 여부(IsMember)를 같이 내려줌</summary>
public class UserGroupMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserNm { get; set; } = string.Empty;
    public string? DeptNm { get; set; }
    public bool IsMember { get; set; }
}

/// <summary>그룹 소속 배정 저장 요청 - 화면에서 체크된 UserId 전체 목록을 그대로 보내면
/// 서버가 기존 매핑을 전부 지우고 이 목록으로 다시 채운다(치환 방식)</summary>
public class UpdateGroupMembersRequest
{
    public List<string> UserIds { get; set; } = new();
}
