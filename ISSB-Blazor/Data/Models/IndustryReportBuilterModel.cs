namespace Data.Models;

public class IndustryReportBuilterModel
{
    public List<IndFormsModel> FormNumber { get; set; }
    public List<string> ColNumber { get; set; }
    public List<string> TariffCodes { get; set; }
    public List<YearMonthModel> YM { get; set; }
    public bool IsTariff { get; set; }
}