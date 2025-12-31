using MongoDB.Bson;

namespace Data.DBModels
{
    public class TaxCodesDB
    {
        public BsonObjectId _id { get; set; }
        public string TaxCode { get; set; }
        public string Description { get; set; }
        public float VatRate { get; set; }
    }
}
