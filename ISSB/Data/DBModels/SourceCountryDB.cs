using MongoDB.Bson;

namespace Data.DBModels
{
    public class SourceCountryDB
    {
        public BsonObjectId _id { get; set; }
        public int SOURCE_COUNTRY_ID { get; set; }        public string REGION_NAME { get; set; }        public string GEO_CODE { get; set; }        public string NAME_OLD { get; set; }        public string SHORT_LEGEND { get; set; }        public string LONG_LEGEND { get; set; }        public string ACTIVE { get; set; }        public string FREQUENCY { get; set; }        public BsonDateTime LATEST_DATE { get; set; }        public BsonDateTime FIRST_DATE { get; set; }        public string SIDE_OF_TRADE { get; set; }        public string CURRENCY_CODE { get; set; }        public string DATA_FORMAT { get; set; }        public string OLD_GEO_CODE { get; set; }        public BsonDateTime GEO_CODE_DISCONTINUED_DATE { get; set; }        public int INDUSTRY_REPORTING_CENTRE_ID { get; set; }        public string NAME { get; set; }
    }
}
