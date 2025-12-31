using MongoDB.Bson;

namespace Data.DBModels;

public class TopHeaderDB
{
    public BsonObjectId _id { get; set; }
    public bool IsActive { get; set; }
    public bool IsFooter { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string HTML { get; set; }
}