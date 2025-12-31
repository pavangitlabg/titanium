using MongoDB.Bson;

namespace Data.DBModels
{
    public class PendingImportAmendmentsDB
    {
        public BsonObjectId _id { get; set; }
        public int ID { get; set; }        public int TIME_ID { get; set; }        public int BATCH_NO { get; set; }        public string SC_GEO { get; set; }        public string MC_GEO { get; set; }
        public int SOURCE_COUNTRY_ID { get; set; }        public int MARKET_COUNTRY_ID { get; set; }        public string IMPORT_TARIFF { get; set; }        public string H_TARIFF { get; set; }        public string SIDE_OF_TRADE { get; set; }        public int PORT_ID { get; set; }        public string PORT_ALPHA { get; set; }        public double WEIGHT { get; set; }        public double MONETARY_VALUE { get; set; }        public double YTD_WEIGHT { get; set; }        public double YTD_MONETARY_VALUE { get; set; }        public string ESTIMATED { get; set; }        public string COO_GEO_CODE { get; set; }        public string CWC_GEO_CODE { get; set; }        public string IMP_UNIT { get; set; }        public string CURRENCY_CODE { get; set; }
        public int TARIFF_ID { get; set; }        public int YEAR { get; set; }        public int QUARTER { get; set; }        public int MONTH { get; set; }
    }
}
