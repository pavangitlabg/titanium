using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISSB.Models
{
    public class IndReportBuilterModel
    {
        [Display(Name = "Forms")]
        public List<IndForms> Forms { get; set; }
        public List<IndTariffCodes> TariffCodes { get; set; }
        public List<IndustryRowModel> DataRows { get; set; }
        public List<IndCols> Cols { get; set; }
        public List<QueryDateMonthModel> Years { get; set; }
        public int ReportType { get; set; }
        [Display(Name = "Report Name")]
        public string ReportName { get; set; }
        [Display(Name = "Save For Later")]
        public bool SaveReport { get; set; }
        public List<ProductBroadFilterItemModel> HighTree { get; set; }
    }

    public class IndForms
    {
        public int Index { get; set; }
        public string FormNumber { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
    }

    public class IndCols
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
    }

    public class IndTariffCodes
    {
        public int Index { get; set; }
        public string TrariffCode { get; set; }
        public string Description { get; set; }
    }
}
