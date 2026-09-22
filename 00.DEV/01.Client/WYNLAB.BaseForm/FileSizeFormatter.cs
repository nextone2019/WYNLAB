namespace WYNLAB.Base;

/// <summary>바이트 값을 B/KB/MB/GB 단위로 사람이 읽기 좋게 바꾼다 - 첨부파일 관련 그리드
/// (popFileUpload의 grd1, frmCust의 grdFile 등)가 전부 같은 형식을 쓰도록 공용으로 뺐다.</summary>
public static class FileSizeFormatter
{
    public static string Format(long bytes)
    {
        string[] units = { "B", "KB", "MB", "GB" };
        double size = bytes;
        var unitIndex = 0;
        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }
        return unitIndex == 0 ? $"{size:0} {units[unitIndex]}" : $"{size:0.0} {units[unitIndex]}";
    }
}
