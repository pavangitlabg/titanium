using MongoDB.Bson;

namespace Data.DBModels
{
    public class SystemControlDB
    {
        public BsonObjectId _id { get; set; }
        public long LastFileNumber { get; set; }
        public long ImportBatch { get; set; }
        public double HowlerWeight { get; set; }
        public double HowlerValue { get; set; }
        public string SMSNumber { get; set; }
        public string SMSMessage { get; set; }
        public int ScheduleHour { get; set; }

    }
}
