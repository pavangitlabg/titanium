using MongoDB.Bson;

namespace Data.DBModels;

public class IssbExchangeRatesDB
{
    public BsonObjectId _id { get; set; }
    public string Currency { get; set; }
    public string Country { get; set; }
    public double Value { get; set; }
    public string Date { get; set; }
    public string ReportDate { get; set; }
    public List<IssbExchangeListDB> Rates { get; set; }
}

public class IssbExchangeListDB
{
    public string Currency { get; set; }
    public string Country { get; set; }
    public double Value { get; set; }
    public string Date { get; set; }
    public string ReportDate { get; set; }
}