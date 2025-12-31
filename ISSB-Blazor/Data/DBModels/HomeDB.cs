using MongoDB.Bson;

namespace Data.DBModels;

public class HomeDB

{
    public BsonObjectId _id { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string Company { get; set; }
    public string Info { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Comment { get; set; }
    public BsonDateTime ContactDate { get; set; }
    public bool Status { get; set; }
}