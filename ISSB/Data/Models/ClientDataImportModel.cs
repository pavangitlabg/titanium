using System;
using System.Collections.Generic;
using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ClientDataImportModel
    {
        public string _id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Client Name Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address Required")]
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        [Display(Name = "..")]
        public string Address2 { get; set; }
        [Display(Name = "..")]
        public string Address3 { get; set; }
        [Display(Name = "Region")]
        public string Address4 { get; set; }
        [Display(Name = "Country")]
        public string Address5 { get; set; }
        [Required(ErrorMessage = "Post Code Required")]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "Telephone Number")]
        public string Telephone { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Fax Number")]
        public string Fax { get; set; }
        [Display(Name = "Web Site Address")]
        public string WebSite { get; set; }
        [Display(Name = "Notes / Memo")]
        public string Notes { get; set; }
        [Display(Name = "ZIP File Search ID Prefix")]
        [Required(ErrorMessage = "ZIP File Search ID Prefix Required")]
        public string SearchString { get; set; }
        [Display(Name = "File Extention")]
        [Required(ErrorMessage = "File Ext Required")]
        public string FileExtenstion { get; set; }
        [Display(Name = "Header Structure")]
        [Required(ErrorMessage = "Header Structure Required")]
        public string Header { get; set; }
        [Display(Name = "Data Structure")]
        [Required(ErrorMessage = "Data Structure Required")]
        public string Data { get; set; }
        [Display(Name = "Trailer Structure")]
        [Required(ErrorMessage = "Trailer Structure Required")]
        public string Trailer { get; set; }
        public DateTime LastModified { get; set; }
        [Required(ErrorMessage = "Weigh Type is required")]
        [Display(Name = "Weight Type")]
        public string WeightType { get; set; }
        [Required(ErrorMessage = "Currency Code is required")]
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Region Code")]
        public string RegionCode { get; set; }

        [Display(Name = "Country Mapping")]
        public bool CountryMapping { get; set; }
        [Display(Name = "Import Type")]
        public string ImportType { get; set; }
        [Display(Name = "Source Country GEO")]
        public string SourceGEO { get; set; }
        public List<ClientDataImportTransactionsModel> Transactions { get; set; }
    }

    public class ClientDataImportTransactionsModel
    {
        public int BatchNumber { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string FileName { get; set; }
        public ImportEnums FileType { get; set; }
    }
}
