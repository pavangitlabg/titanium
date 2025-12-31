using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Models
{
    public class ReportDesignerModel
    {

        public string _id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifedDate { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is a required field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A valid report must be chosen")]
        public string ReportID { get; set; }
        [Display(Name = "Report Header")]
        public string Header { get; set; }
        [Display(Name = "Report Content")]
        public string Content { get; set; }
        [Display(Name = "Margin Top")]
        public string MarginTop { get; set; }
        [Display(Name = "Margin Bottom")]
        public string MarginBottom { get; set; }
        [Display(Name = "Margin Left")]
        public string MarginLeft { get; set; }
        [Display(Name = "Margin Right")]
        public string MarginRight { get; set; }
        [Display(Name = "Landscape Mode")]
        public bool Landscape { get; set; }
        [Display(Name = "Page Size")]
        public string PageSize { get; set; }
        [Display(Name = "Scale")]
        public int Scale { get; set; }
        [Display(Name = "File Name")]
        [Required(ErrorMessage = "File Name is a required field")]
        public string FileName { get; set; }
        [Display(Name = "Page Ranges")]
        public string PageRanges { get; set; }
        [Display(Name = "Include Header")]
        public bool IncludeHeader { get; set; }
        [Display(Name = "File Type")]
        public string FileType { get; set; }
        [Display(Name = "Value Format")]
        public string ValueFormat { get; set; }
        [Display(Name = "Weight Format")]
        public string WeightFormat { get; set; }
        [Display(Name = "Is Pivot Table")]
        public bool PivotTable { get; set; }
        [Display(Name = "Field Sort Order")]
        public string FieldSort { get; set; } 
        public List<SavedQueryModel> Data { get; set; }
        public string User { get; set; }
        [Display(Name = "Table Font Size")]
        public string FontSize { get; set; }
        [Display(Name = "Table Header")]
        public string TableHeader { get; set; }
        [Display(Name = "HTML Output")]
        public string HTML { get; set; }
        [Display(Name = "Run from HTML")]
        public bool RunFromHTML { get; set; }
        [Display(Name = "Display Totals")]
        public bool ShowTotals { get; set; }
        [Display(Name = "Table Style")]
        [Required(ErrorMessage = "Table Style is a required field and must be in the proper format")]
        public string TableStyle { get; set; }
        [Display(Name = "Section Page Break")]
        public bool PageBreak { get; set; }
        [Display(Name = "Page Break Type")]
        public string PageBreakType { get; set; }
        [Display(Name = "CSS")]
        public string CSS { get; set; }
        [Display(Name = "Jquery Script")]
        public string JQUERY { get; set; }

        [Display(Name = "CSV - Show Weight")]
        public bool ShowWeight { get; set; }
        [Display(Name = "CSV - Show Weight and VPT")]
        public bool ShowWeightVpt { get; set; }
        [Display(Name = "CSV - Show Weight, VPT and Value")]
        public bool ShowWeightValueVpt { get; set; }
        [Display(Name = "CSV - Group by Flow")]
        public bool GroupByFlow { get; set; }

        public string BuildStatus { get; set; }
        public List<ReportKeysModel> Keys { get; set; }

    }
}