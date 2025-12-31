using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ProductCustomFilterModel
    {
        public string _id { get; set; }
        
        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }
        [Display(Name = "Filter Name")]
        [Required(ErrorMessage = "Filter Name is required.")]
        public string Name { get; set; }
        public string SearchType { get; set; }
        public List<ProductCustomFilterItemModel> Items { get; set; }
    }

    public class ProductCustomFilterItemModel
    {
        public int Sort { get; set; }
        public string TariffCode { get; set; }
        public string Name { get; set; }
    }
}
