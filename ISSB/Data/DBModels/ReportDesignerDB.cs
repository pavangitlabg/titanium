using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ReportDesignerDB
    {
        public BsonObjectId _id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifedDate { get; set; }
        public string Name { get; set; }
        public string ReportID { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string MarginTop { get; set; }
        public string MarginBottom { get; set; }
        public string MarginLeft { get; set; }
        public string MarginRight { get; set; }
        public bool Landscape { get; set; }
        public string PageSize { get; set; }
        public int Scale { get; set; }
        public string FileName { get; set; }
        public string PageRanges { get; set; }
        public bool IncludeHeader { get; set; }
        public string FileType { get; set; }
        public string ValueFormat { get; set; }
        public string WeightFormat { get; set; }
        public bool PivotTable { get; set; }
        public string FieldSort { get; set; }
        public string User { get; set; }
        public string FontSize { get; set; }
        public string TableHeader { get; set; }
        public string HTML { get; set; }
        public bool RunFromHTML { get; set; }
        public bool ShowTotals { get; set; }
        public string TableStyle { get; set; }
        public bool PageBreak { get; set; }
        public string PageBreakType { get; set; }
        public string CSS { get; set; }
        public string JQUERY { get; set; }
        public bool ShowWeight { get; set; }
        public bool ShowWeightVpt { get; set; }
        public bool ShowWeightValueVpt { get; set; }
        public bool GroupByFlow { get; set; }
        public string BuildStatus { get; set; }
        public List<ReportKeysDB> Keys { get; set; }
    }

}