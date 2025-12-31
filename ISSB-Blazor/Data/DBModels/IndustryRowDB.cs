using MongoDB.Bson;

namespace Data.DBModels;

public class IndustryRowDB
{
    public BsonObjectId _id { get; set; }
    public string Number { get; set; }
    public string Description { get; set; }
}