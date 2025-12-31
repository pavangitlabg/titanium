using System;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class PendingImportHeaderDB
    {
        public BsonObjectId _id { get; set; }
        public DateTime DATE { get; set; }
        public int BATCH_NO { get; set; }
        public string STATUS { get; set; }
        public string IMPORT_TYPE { get; set; }
        public bool FAIL { get; set; }
        public DateTime END_DATE { get; set; }
        public bool POSTED { get; set; }
    }
}
