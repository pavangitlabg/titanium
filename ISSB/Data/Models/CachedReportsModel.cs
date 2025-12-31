using System;
namespace Data.Models
{
    public class CachedReportsModel
    {
        public string _id { get; set; }
        public bool IsVisitorPage { get; set; }
        public string User { get; set; }
        public object Model { get; set; }
    }
}
