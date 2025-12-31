using MongoDB.Bson;
namespace Data.DBModels
{
    public class ReportKeysDB
    {
        public BsonObjectId _id { get; set; }
        public int Sort { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
        public string Width { get; set; }
    }
}
