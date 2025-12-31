using MongoDB.Bson;

namespace Data.DBModels
{
    public class PortsDB
    {
        public BsonObjectId _id { get; set; }
        public int PortID { get; set; }        public string AlphaCode { get; set; }        public string GeoCode { get; set; }        public string Name { get; set; }
    }
}
