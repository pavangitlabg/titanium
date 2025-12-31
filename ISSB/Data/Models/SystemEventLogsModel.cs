using System;
using Data.Enums;

namespace Data.Models
{
    public class SystemEventLogsModel
    {
        public string _id { get; set; }
        public DateTime DATE { get; set; }
        public string USER { get; set; }
        public string MESSAGE { get; set; }
        public DataTypeEnums DataType { get; set; }
        public TransactionTypeEnums TransactionType { get; set; }
        public string DataTypeString { get; set; }
        public string TransactionTypeString { get; set; }
        public object Model { get; set; }
    }
}
