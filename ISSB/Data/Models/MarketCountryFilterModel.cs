
using System.Collections.Generic;

namespace Data.Models
{
    public class MarketCountryFilterModel
    {
        public string _id { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public bool CanEdit { get; set; }
        public List<MarketCountryFilterListModel> Items { get; set; }

    }

    public class MarketCountryFilterListModel
    {
        public int SortOrder { get; set; }
        public string GeoCode { get; set; }
        public string CountryName { get; set; }
    }
}
