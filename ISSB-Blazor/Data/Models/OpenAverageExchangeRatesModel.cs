namespace Data.Models;

public class OpenAverageExchangeRatesModel
{
    public string _id { get; set; }
    public string Currency { get; set; }
    public string Country { get; set; }
    public double Value { get; set; }
    public string Date { get; set; }
    public string ReportDate { get; set; }
    public List<OpenAverageExchangeListModel> Rates { get; set; }
}

public class OpenAverageExchangeListModel
{
    public string Currency { get; set; }
    public string Country { get; set; }
    public double Value { get; set; }
    public string Date { get; set; }
    public string ReportDate { get; set; }
}