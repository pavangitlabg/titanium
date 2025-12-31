using System;
namespace Data.Models
{
    public class ErrorModel
    {
        public string _id { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
    }
}
