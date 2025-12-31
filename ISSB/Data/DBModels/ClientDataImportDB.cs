using System;
using System.Collections.Generic;
using Data.Enums;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ClientDataImportDB
    {
        public BsonObjectId _id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string PostCode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public string Notes { get; set; }
        public string SearchString { get; set; }
        public string FileExtenstion { get; set; }
        public DateTime LastModified { get; set; }
        public string Header { get; set; }
        public string Data { get; set; }
        public string Trailer { get; set; }
        public string WeightType { get; set; }
        public string CurrencyCode { get; set; }
        public string RegionCode { get; set; }
        public bool CountryMapping { get; set; }
        public string ImportType { get; set; }
        public string SourceGEO { get; set; }
        public List<ClientDataImportTransactionsDB> Transactions { get; set; }
    }

    public class ClientDataImportTransactionsDB
    {
        public int BatchNumber { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string FileName { get; set; }
        public ImportEnums FileType { get; set; }
    }
}
