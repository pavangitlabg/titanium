using MongoDB.Bson;

namespace Data.DBModels
{
    public class AssetDB
    {
        public BsonObjectId _id { get; set; }
        public int ID { get; set; }
        public BsonDateTime LAST_MODIFIED { get; set; }        public string DESCRIPTION { get; set; }        public string SERIAL_NO { get; set; }        public string MANUFACTURER { get; set; }        public string MODEL { get; set; }        public BsonDateTime PURCHASE_DATE { get; set; }        public double VALUE { get; set; }        public string LOCATION { get; set; }        public string USERNAME { get; set; }        public string PASSWORD { get; set; }        public string MEMO { get; set; }        public string CHECKED_BY { get; set; }        public BsonDateTime DATE_CHECKED { get; set; }
    }
}