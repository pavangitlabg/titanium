using MongoDB.Bson;

namespace Data.DBModels;

public class OpenExchangeRatesBaseDB
{
    public BsonObjectId _id { get; set; }
    public string Disclaimer { get; set; }
    public string License { get; set; }
    public string Start_date { get; set; }
    public string End_date { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string Base { get; set; }
    public List<OpenExchangeRatesPeriodDB> Rates { get; set; }
}

public class OpenExchangeRatesPeriodDB
{
    public string Name { get; set; }
    public List<OpenExchangeRatesDB> Rates { get; set; }
}

public class OpenExchangeRatesDB
{
    public string Name { get; set; }
    public double Rate { get; set; }
    public double Value { get; set; }
}