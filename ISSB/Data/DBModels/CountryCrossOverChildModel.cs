using System;
namespace Data.DBModels
{
    public class CountryCrossOverChildModel
    {
        public string _id { get; set; }
        public int Source_Country_ID { get; set; }
        public string Source_Country_Value { get; set; }
        public string GEO_CODE { get; set; }
        public string NAME { get; set; }
    }
}
