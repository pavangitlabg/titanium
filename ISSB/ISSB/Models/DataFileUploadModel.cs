using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ISSB.Models
{
    public class DataFileUploadModel
    {

		public DateTime Date { get; set; }
		public string Source { get; set; }
        [Required(ErrorMessage = "Please enter a valid Subject")]
        public string Subject { get; set; }
		public string Message { get; set; }
        [Required(ErrorMessage = "Empty File Name")]
        public IFormFile File { get; set; }
	}
}
