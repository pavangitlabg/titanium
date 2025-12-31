using System.Collections.Generic;
using Data.Models;
using MongoDB.Bson;
namespace Data.DBModels
{
    public class IndustrySavedReportsDB
    {
        public BsonObjectId _id { get; set; }
        public BsonDateTime Date { get; set; }
        public int ReportType { get; set; }
        public string ReportName { get; set; }
        public bool SaveReport { get; set; }
        public List<IndFormsModel> Forms { get; set; }
        public List<string> TCodes { get; set; }
        public List<string> ReoprtCols { get; set; }
        public string ProductType { get; set; }
        public List<SaveQueryDateModel> SourceTime { get; set; }

    }
}
