using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.DBModels;

public class IndDataDB
{
    public BsonObjectId _id { get; set; }
    public DateTime UploadDate { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ReportingPeriod { get; set; }

    public int Month { get; set; }
    public int Year { get; set; }
    public string FormNumber { get; set; }
    public string FormLink { get; set; }
    public string TabName { get; set; }
    public List<DataColsDB> Datas { get; set; }
}

public class DataColsDB
{
    public string Code { get; set; }
    public string Description { get; set; }
    public string ColCode { get; set; }
    public double ColValue { get; set; }
}