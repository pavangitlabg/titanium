namespace Data.Models;

public class IndustrySavedReportsModel
{
    public string _id { get; set; }
    public DateTime Date { get; set; }
    public int ReportType { get; set; }
    public string ReportName { get; set; }
    public bool SaveReport { get; set; }
    public List<IndFormsModel> Forms { get; set; }
    public List<string> TCodes { get; set; }
    public List<string> ReoprtCols { get; set; }
    public string ProductType { get; set; }
    public List<SaveQueryDateModel> SourceTime { get; set; }
}