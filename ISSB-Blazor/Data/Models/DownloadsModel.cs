namespace Data.Models;

public class DownloadsModel
{
    public string _id { get; set; }
    public string UserId { get; set; }
    public string ReportId { get; set; }
    public DateTime Date { get; set; }
    public string ReportName { get; set; }
    public string FilePath { get; set; }
}