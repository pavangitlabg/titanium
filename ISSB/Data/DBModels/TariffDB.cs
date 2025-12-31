using MongoDB.Bson;

namespace Data.DBModels
{
    public class TariffDB
    {
        public BsonObjectId _id { get; set; }
        public int TARIFF_ID { get; set; }
        public string WTO_CODE { get; set; }
        public string WTO_CODE_LEGEND { get; set; }
        public string WTO_ALLOY_CODE { get; set; }
        public string WTO_ALLOY_LEGEND { get; set; }
        public string ISSB_STORED_CODE { get; set; }
        public string ISSB_STORED_LEGEND { get; set; }
        public string HARMONISED_TARIFF_CODE { get; set; }
        public string HARMONISED_TARIFF_SHORT_LEGEND_BACKUP { get; set; }
        public string HARMONISED_TARIFF_LONG_LEGEND { get; set; }
        public string SOURCE_COUNTRY_TARIFF_CODE { get; set; }
        public string SOURCE_COUNTRY_TARIFF_SHORT_LEGEND { get; set; }
        public string SOURCE_COUNTRY_TARIFF_LONG_LEGEND { get; set; }
        public string LOWER_VPT { get; set; }
        public string UPPER_VPT { get; set; }
        public BsonDateTime START_DATE { get; set; }
        public BsonDateTime DISCONTINUED_DATE { get; set; }
        public string SIDE_OF_TRADE { get; set; }
        public string TARIFF_CODE_TABLE_CODE { get; set; }
        public string HARMONISED_TARIFF_SHORT_LEGEND { get; set; }
    }
}
