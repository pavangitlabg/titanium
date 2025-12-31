using System;
using Data.Enums;

namespace Data.Models
{
    public class IndustryMasterModel
    {
        public string _id { get; set; }
        public int DataType { get; set; }
        public string DataName { get; set; }
        public DateTime dateTime { get; set; }
        public string FileName { get; set; }
        public string Subject { get; set; }
    }
}
