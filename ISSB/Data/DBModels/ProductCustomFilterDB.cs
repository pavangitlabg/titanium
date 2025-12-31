using System.Collections.Generic;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ProductCustomFilterDB
    {
        public BsonObjectId _id { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string SearchType { get; set; }
        public List<ProductCustomFilterItemDB> Items { get; set; }
    }

    public class ProductCustomFilterItemDB
    {
        public int Sort { get; set; }
        public string TariffCode { get; set; }
        public string Name { get; set; }
    }
}
