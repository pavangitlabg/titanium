using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.DBModels
{
    public class UserDB
    {
        public BsonObjectId _id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsSystemUser { get; set; }
        public bool IsAdvancedUser { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsTradeInquiry { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExpiryDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateJoined { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }        
        public string Mobile { get; set; }          
        public double Costs { get; set; }
        public string AccountStatus { get; set; }
        public bool TwoFactorAuth { get; set; }
        public bool EmailNotification { get; set; }
        public string ImageURL { get; set; }
        public List<object> Permissions { get; set; }
        public List<SourceCountryDB> AllowedSourceCountries { get; set; }
        public List<string> AllowedMarketCountries { get; set; }
        public List<ProductAllFilterDB> AllowedTariffCodes { get; set; }
        public string NavBar { get; set; }
        public string BrandLink { get; set; }
        public string SideBar { get; set; }
        public string AllowedYears { get; set; }
        public bool IsAPI { get; set; }
        public List<ProductCustomFilterDB> AllowedCustomFilters { get; set; }

    }
}
