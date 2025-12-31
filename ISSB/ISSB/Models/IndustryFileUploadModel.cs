using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISSB.Models
{
    public class IndustryFileUploadModel
    {
        public DateTime Date { get; set; }
        public List<SelectListItem> Source { get; set; }
        [Required(ErrorMessage = "Please enter a valid Subject")]
        public string Subject { get; set; }
        public string Message { get; set; }
        [Required(ErrorMessage = "Empty File Name")]
        public IFormFile File { get; set; }
    }


}
