namespace Data.Models;

public class OpenExchangeRatesModel
{
    public string Name { get; set; }
    public double Rate { get; set; }
    public double Value { get; set; }
}

public class OpenExchangeRatesPeriodModel
{
    public string Name { get; set; }
    public List<OpenExchangeRatesModel> Rates { get; set; }
}

public class OpenExchangeRatesBaseModel
{
    public string _id { get; set; }
    public string Disclaimer { get; set; }
    public string License { get; set; }
    public string Start_date { get; set; }
    public string End_date { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string Base { get; set; }
    public List<OpenExchangeRatesPeriodModel> Rates { get; set; }
}