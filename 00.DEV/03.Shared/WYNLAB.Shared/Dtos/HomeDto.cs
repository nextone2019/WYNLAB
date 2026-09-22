namespace WYNLAB.Shared.Dtos;

/// <summary>홈화면 공지사항 위젯 1건 - HomeController.Notices(USP_SM_BOARD_Q).</summary>
public class HomeNoticeItemDto
{
    public long BoardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImportantYn { get; set; } = "N";
    public DateTime? RegDt { get; set; }
}

/// <summary>홈화면 "오늘 일정" 위젯 1건 - HomeController.TodaySchedule(USP_SM_SCHEDULE_Q).</summary>
public class HomeScheduleItemDto
{
    public long ScheduleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string StartDt { get; set; } = string.Empty;
    public string? EndDt { get; set; }
    public string? StartTm { get; set; }
    public string? EndTm { get; set; }
    public string? ColorCd { get; set; }
}

/// <summary>홈화면 "읽지 않은 쪽지" 위젯 1건 - HomeController.UnreadMessages(USP_SM_MESSAGE_Q).</summary>
public class HomeMessageItemDto
{
    public long MsgId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FromEmpNm { get; set; } = string.Empty;
    public DateTime? RegDt { get; set; }
}
