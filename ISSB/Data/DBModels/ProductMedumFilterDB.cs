using MongoDB.Bson;
using System.Collections.Generic;

namespace Data.DBModels
{
    public class ProductMedumFilterDB
    {
        public BsonObjectId _id { get; set; }
        public int Idx { get; set; }        public int SortOrder { get; set; }        public string Code { get; set; }        public string Name { get; set; }
        public List<ProductMedumFilterItemDB> Items { get; set; }
    }

    public class ProductMedumFilterItemDB    {        public int Sort { get; set; }        public string TariffCode { get; set; }        public string Name { get; set; }    }
}
