using MongoDB.Bson;

namespace Data.DBModels
{
    public class ImportFileTypeDB
    {
        public BsonObjectId _id { get; set; }
        public string KEY { get; set; }
        public string NAME { get; set; }

    }
}
