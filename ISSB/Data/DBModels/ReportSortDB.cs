using MongoDB.Bson;

namespace Data.DBModels
{
    public class ReportCacheDB
    {
        public BsonObjectId _id { get; set; }
        public string ReportID { get; set; }
        public string Key { get; set; }
        public object Data { get; set; }
    }
}
