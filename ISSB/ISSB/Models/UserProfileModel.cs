using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ISSB.Models
{
    public class UserProfileModel
    {


        public string _id { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = "Address 3")]
        public string Address3 { get; set; }
        [Display(Name = "Address 4")]
        public string Address4 { get; set; }
        [Display(Name = "Post Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Telephone Number")]
        public string Telephone { get; set; }
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is required")]
        public string Mobile { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        [Display(Name = "Email Notifications")]
        public bool EmailNotification { get; set; }
        
        [Display(Name = "Upload Image")]
        public IFormFile File { get; set; }
        public string ImageURL { get; set; }

    }
}
