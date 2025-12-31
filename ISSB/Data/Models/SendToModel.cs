using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class SendToModel
    {
        public string FromEmail { get; set; }
        [Display(Name = "Email to Send To")]
        [Required(ErrorMessage = "Email required")]
        public string ToEmail { get; set; }
        public string RecordQueryId { get; set; }
        public string ReportId { get; set; }
    }
}
