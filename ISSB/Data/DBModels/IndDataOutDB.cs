using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class IndDataOutDB
    {
        public BsonObjectId _id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime ReportingPeriod { get; set; }
        public string FormNumber { get; set; }
        public string FormLink { get; set; }
        public string TabName { get; set; }
        public List<DataColsDB> Datas { get; set; }
  
    }

}
