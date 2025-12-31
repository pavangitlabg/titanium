using MongoDB.Bson;

namespace Data.DBModels;

public class CurrencyCodesDB
{
    public BsonObjectId _id { get; set; }
    public string CURRENCY_CODE { get; set; }
    public string CURRENCY_NAME { get; set; }
    public string CURRENCY_SYMBOL { get; set; }
}