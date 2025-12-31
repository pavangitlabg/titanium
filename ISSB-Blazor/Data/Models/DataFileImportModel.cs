using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class DataFileImportModel
{
    public string _id { get; set; }
    public long Idx { get; set; }
    public DateTime Date { get; set; }
    public string Source { get; set; }

    [Display(Name = "Subject")] public string Subject { get; set; }

    [Display(Name = "Message")] public string Message { get; set; }

    public string FileLocation { get; set; }
    public string FileName { get; set; }
    public string FileStatus { get; set; }
}