using MongoDB.Bson;

namespace Data.DBModels
{
    public class IndustryColumnDB
    {
        public BsonObjectId _id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
