using MongoDB.Bson;

namespace Data.DBModels;

public class CachedReportsDB
{
    public BsonObjectId _id { get; set; }
    public bool IsVisitorPage { get; set; }
    public string User { get; set; }
    public object Model { get; set; }
}