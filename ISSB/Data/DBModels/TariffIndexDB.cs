using MongoDB.Bson;
namespace Data.DBModels
{
    public class TariffIndexDB
    {
        public BsonObjectId _id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
