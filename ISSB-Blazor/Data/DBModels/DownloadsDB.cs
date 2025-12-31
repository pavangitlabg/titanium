using MongoDB.Bson;

namespace Data.DBModels;

public class DownloadsDB
{
    public BsonObjectId _id { get; set; }
    public BsonObjectId UserId { get; set; }
    public BsonObjectId ReportId { get; set; }
    public BsonDateTime Date { get; set; }
    public string ReportName { get; set; }
    public string FilePath { get; set; }
}