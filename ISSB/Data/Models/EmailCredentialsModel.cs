using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class EmailCredentialsModel
    {
        [StringLength(60, MinimumLength = 5)]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter password"), MaxLength(30)]
        [Display(Name = "Email Password")]
        public string EmailPassword { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "File Storeage Path")]
        public string FilePath { get; set; }
        [Required]
        [Display(Name = "Download File Storeage Path")]
        public string DownloadFilePath { get; set; }
    }
}
