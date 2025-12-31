using MongoDB.Bson;

namespace Data.DBModels
{
    public class ReportingOutDB
    {
        public BsonObjectId _id { get; set; }
        public int TIME_ID { get; set; }
        public int SIDE_OF_TRADE { get; set; }
        public int SOURCE_COUNTRY_ID { get; set; }
        public int MARKET_COUNTRY_ID { get; set; }
        public int WEIGHT { get; set; }
        public int MONETARY_VALUE { get; set; }
        public int YTD_WEIGHT { get; set; }
        public int YTD_MONETARY_VALUE { get; set; }
        public string GEO_CODE { get; set; }
        public string CITY { get; set; }
        public float LAT { get; set; }
        public float LNG { get; set; }
    }
}
