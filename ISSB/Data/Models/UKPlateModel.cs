
using System.Collections.Generic;

namespace Data.Models
{
    public class UKPlateModel
    {
     
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string GEO { get; set; }
        public string Country { get; set; }
        public double Tonnes1 { get; set; }
        public double Tonnes2 { get; set; }
        public double Tonnes3 { get; set; }
        public double Tonnes4 { get; set; }
        public double Tonnes5 { get; set; }
        public double Tonnes6 { get; set; }
        public double Tonnes7 { get; set; }
        public double Tonnes8 { get; set; }
        public double Tonnes9 { get; set; }
        public double Tonnes10 { get; set; }
        public double Tonnes11 { get; set; }
        public double Tonnes12 { get; set; }
        public double TonnesYTD { get; set; }

        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public double Weight3 { get; set; }
        public double Weight4 { get; set; }
        public double Weight5 { get; set; }
        public double Weight6 { get; set; }
        public double Weight7 { get; set; }
        public double Weight8 { get; set; }
        public double Weight9 { get; set; }
        public double Weight10 { get; set; }
        public double Weight11 { get; set; }
        public double Weight12 { get; set; }
        public double WeightYTD { get; set; }

    }

    public class UKPLateJsonModel
    {
        public List<UKPlateModel> data { get; set; }
    }

  
}
