using MongoDB.Bson;
namespace Data.DBModels
{
    public class ReportCurrencyDB
    {
        public BsonObjectId _id { get; set; }
        public int Sort { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
