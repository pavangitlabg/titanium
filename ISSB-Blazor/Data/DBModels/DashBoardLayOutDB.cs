using MongoDB.Bson;

namespace Data.DBModels;

public class DashBoardLayOutDB
{
    public BsonObjectId _id { get; set; }
    public int ID { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string HTML { get; set; }
}