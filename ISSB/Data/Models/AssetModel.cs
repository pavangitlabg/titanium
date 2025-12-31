using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class AssetModel
    {
        public string _id { get; set; }
        [Display(Name = "Id")]
        public int ID { get; set; }
        [Display(Name = "Last Date Modified")]
        [DataType(DataType.Date)]
        public DateTime LAST_MODIFIED { get; set; }
        [Display(Name = "Description")]        public string DESCRIPTION { get; set; }        [Display(Name = "Serial Number")]        public string SERIAL_NO { get; set; }
        [Display(Name = "Manufacturer")]        public string MANUFACTURER { get; set; }        [Display(Name = "Model")]        public string MODEL_TYPE { get; set; }
        [Display(Name = "Purchased Date")]
        [DataType(DataType.Date)]        public DateTime PURCHASE_DATE { get; set; }
        [Display(Name = "Value")]        public double VALUE { get; set; }
        [Display(Name = "Location")]        public string LOCATION { get; set; }        [Display(Name = "Username")]        public string USERNAME { get; set; }
        [Display(Name = "Password")]        public string PASSWORD { get; set; }
        [Display(Name = "Memo")]        public string MEMO { get; set; }        [Display(Name = "Checked By")]        public string CHECKED_BY { get; set; }
        [Display(Name = "Date Checked")]        [DataType(DataType.Date)]        public DateTime DATE_CHECKED { get; set; }
    }

    public class AssetJsonModel
    {
        public List<AssetModel> data { get; set; }
    }
}
