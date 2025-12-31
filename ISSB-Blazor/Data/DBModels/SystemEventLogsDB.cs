using Data.Enums;
using MongoDB.Bson;

namespace Data.DBModels;

public class SystemEventLogsDB
{
    public BsonObjectId _id { get; set; }
    public DateTime DATE { get; set; }
    public string USER { get; set; }
    public string MESSAGE { get; set; }
    public DataTypeEnums DataType { get; set; }
    public TransactionTypeEnums TransactionType { get; set; }
    public object Model { get; set; }
}