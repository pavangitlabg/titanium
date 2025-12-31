using Data.DBModels;

namespace Data.Models;

public class CountryCrossOverModel
{
    public string _id { get; set; }
    public int Country_ID { get; set; }
    public int RedBrick_Market_Country_ID { get; set; }
    public int RedBrick_Source_Country_ID { get; set; }
    public int Region_ID { get; set; }
    public string GEO_CODE { get; set; }
    public string NAME { get; set; }
    public bool Source_Country_Indicator { get; set; }
    public bool Market_Country_Indicator { get; set; }
    public string Short_Legend { get; set; }
    public string Long_Legend { get; set; }
    public bool Active { get; set; }
    public DateTime Start_Date { get; set; }
    public string Source_Country_First_Date { get; set; }
    public string Tariff_Code_Table_Code { get; set; }
    public string Side_Of_Trade { get; set; }
    public string Data_Format { get; set; }
    public int Currency_ID { get; set; }
    public int Currency_Units { get; set; }
    public string Old_Geo_Code { get; set; }
    public string Geo_Code_Discontinued_Date { get; set; }
    public bool BITS_Processing_Indicator { get; set; }
    public List<CountryCrossOverChildModel> Countries { get; set; }
}