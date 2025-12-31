using System;
using System.ComponentModel.DataAnnotations;
namespace Data.Models
{
    public class ImportFileTypeModel
    {
        public string _id { get; set; }
        [Display(Name = "Type Key")]
        [Required(ErrorMessage = "Key Field is required")]
        public string KEY { get; set; }
        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "Country Name is required")]
        public string NAME { get; set; }
    }
}
