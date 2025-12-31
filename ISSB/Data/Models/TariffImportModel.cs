using System;
namespace Data.Models
{
    public class TariffImportModel
    {
        public string _id { get; set; }
        public string HS { get; set; }
        public string TARIFF { get; set; }
        public string DESCRIPTION { get; set; }
        public string REGION_CODE { get; set; }
        public string SIDE_OF_TRADE { get; set; }
    }
}
