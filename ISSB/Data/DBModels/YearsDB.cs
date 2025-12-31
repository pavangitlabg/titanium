using System.Collections.Generic;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class YearsDB
    {
        public BsonObjectId _id { get; set; }
        public int Idx { get; set; }
        public int Year { get; set; }
        public List<MonthDB> Months { get; set; }
    }

    public class MonthDB
    {
        public int MounthNumber { get; set; }
        public string Month { get; set; }
    }
}
