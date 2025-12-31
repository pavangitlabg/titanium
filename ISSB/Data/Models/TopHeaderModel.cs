using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class TopHeaderModel
    {
        public string _id { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Is Footer")]
        public bool IsFooter { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Decription")]
        public string Description { get; set; }
        [Display(Name = "HTML")]
        public string HTML { get; set; }
    }
}
