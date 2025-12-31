using Data.Enums;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ImportErrorLogDB
    {
        public BsonObjectId _id { get; set; }
        public DataTypeEnums Type { get; set; }
        public int BatchNumber { get; set; }
        public string Code { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
    }
}
