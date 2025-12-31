using MongoDB.Bson;

namespace Data.DBModels;

public class MarketCountryExceptionDB
{
    public BsonObjectId _id { get; set; }
    public int MARKET_COUNTRY_ID { get; set; }
    public string GEO_CODE { get; set; }
    public string NAME_OLD { get; set; }
    public string SHORT_LEGEND { get; set; }
    public string LONG_LEGEND { get; set; }
    public string REGION_NAME { get; set; }
    public string SOURCE_COUNTRY_INDICATOR { get; set; }
    public string REPLACED_GEO_CODE { get; set; }
    public BsonDateTime START_DATE { get; set; }
    public BsonDateTime DISCONTINUED_DATE { get; set; }
    public string NAME { get; set; }
    public bool ACTIVE { get; set; }
}