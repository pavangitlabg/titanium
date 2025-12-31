using System;
namespace Data.Models
{
    public class TariffSearchModel
    {
        public int ID { get; set; }
        public string LongTariffCode { get; set; }
        public string ShortTariffCode { get; set; }
        public string SideOfTrade { get; set; }
        public string ReigionCode { get; set; }
    }
}
