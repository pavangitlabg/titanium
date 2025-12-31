using MongoDB.Bson;

namespace Data.DBModels
{
    public class WorldCityDB
    {
        public BsonObjectId _id { get; set; }
        public int Idx { get; set; }
        public string GEO { get; set; }
        public string City { get; set; }
        public string CityAscii { get; set; }
        public float LAT { get; set; }
        public float LNG { get; set; }
        public float Population { get; set; }
        public string Country { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
        public string Province { get; set; }
    }
}
