using System.Collections.Generic;

namespace Data.Models
{
    public class ProductBroadFilterModel
    {
        public int Idx { get; set; }        public int SortOrder { get; set; }        public string Name { get; set; }        public string Deleted { get; set; }        public List<ProductBroadFilterItemModel> Items { get; set; }
    }

    public class ProductBroadFilterItemModel    {        public int Sort { get; set; }        public string TariffCode { get; set; }        public string Name { get; set; }
        public string Description { get; set; }    }


}

