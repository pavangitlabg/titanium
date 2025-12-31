using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Data.Models
{
    public class ProductAllFilterModel
    {
        [JsonIgnore]
        public string _id { get; set; }
        [Display(Name = "Id")]
        public int Idx { get; set; }
        [Display(Name = "Sort Order")]        public int SortOrder { get; set; }
        [Display(Name = "Tariff Code")]        public string TariffCode { get; set; }
        [Display(Name = "Name")]        public string Name { get; set; }
        [Display(Name = "Cost")]
        public double Cost { get; set; }
    }

    //public class ProductJsonAllFilterModel
    //{
    //    public string Root { }
    //    public string _id { get; set; }
    //    public string TariffCode { get; set; }
    //    public string Name { get; set; }

    //}

    public class ProductJsonAllFilterModel
    {
        public List<ProductAllFilterModel> Products { get; set; }
    }
}
