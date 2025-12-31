using MongoDB.Bson;

namespace Data.DBModels;

public class TradeFactDB
{
    public BsonObjectId _id { get; set; }
    public int TIME_ID { get; set; }
    public int SOURCE_COUNTRY_ID { get; set; }
    public int MARKET_COUNTRY_ID { get; set; }
    public int TARIFF_ID { get; set; }
    public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public double WEIGHT { get; set; }
    public double MONETARY_VALUE { get; set; }
    public double YTD_WEIGHT { get; set; }
    public double YTD_MONETARY_VALUE { get; set; }
    public string ESTIMATED { get; set; }
    public string COO_GEO_CODE { get; set; }
    public string CWC_GEO_CODE { get; set; }
    public int MONTH { get; set; }
    public int YEAR { get; set; }
    public int QUARTER { get; set; }
    public int BATCH { get; set; }
}