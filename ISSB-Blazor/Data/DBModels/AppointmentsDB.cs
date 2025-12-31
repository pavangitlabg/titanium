using MongoDB.Bson;

namespace Data.DBModels;

public class AppointmentsDB
{
    public BsonObjectId _id { get; set; }
    public BsonDateTime Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public bool IsPublished { get; set; }
}