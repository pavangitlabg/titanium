using Newtonsoft.Json;

namespace Data.Models;

public class ReportWriterModel
{
    public string KEY { get; set; }
    public int ID { get; set; }
    public int TIME_ID { get; set; }
    public string SC_GEO { get; set; }
    public string SC_GEO_REGION { get; set; }
    public string SC_NAME { get; set; }
    public string MC_GEO { get; set; }
    public string MC_NAME { get; set; }
    public string SIX_DIGIT { get; set; }
    public string TWO_DIGIT { get; set; }
    public string TARIFF_CODE { get; set; }
    public string TARIFF_LEGEND { get; set; }
    public string TARIFF_CATEGORY { get; set; }
    public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public string PORT_NAME { get; set; }
    public double WEIGHT { get; set; }
    public double MONETARY_VALUE { get; set; }
    public int YEAR { get; set; }
    public int QUARTER { get; set; }
    public int MONTH { get; set; }
    public double YTD_WEIGHT { get; set; }
    public double YTD_MONETARY_VALUE { get; set; }

    public List<ReportWriterByMonthModel> GroupByType(List<ReportWriterModel> ReportList)
    {
        var GrpList = new List<ReportWriterByMonthModel>();

        foreach (var Item in ReportList)
            try
            {
                var f = new ReportWriterByMonthModel();

                var CaseNumber = 0;
                if (string.IsNullOrEmpty(Item.TARIFF_CODE) && string.IsNullOrEmpty(Item.MC_GEO) &&
                    !string.IsNullOrEmpty(Item.SC_GEO)) CaseNumber = 1;
                if (!string.IsNullOrEmpty(Item.TARIFF_CODE) && !string.IsNullOrEmpty(Item.SC_GEO) &&
                    string.IsNullOrEmpty(Item.MC_GEO)) CaseNumber = 2;
                if (string.IsNullOrEmpty(Item.TARIFF_CODE) && string.IsNullOrEmpty(Item.SC_GEO) &&
                    !string.IsNullOrEmpty(Item.MC_GEO)) CaseNumber = 3;
                if (!string.IsNullOrEmpty(Item.TARIFF_CODE) && !string.IsNullOrEmpty(Item.MC_GEO) &&
                    string.IsNullOrEmpty(Item.SC_GEO)) CaseNumber = 4;
                if (!string.IsNullOrEmpty(Item.TARIFF_CODE) && !string.IsNullOrEmpty(Item.MC_GEO) &&
                    !string.IsNullOrEmpty(Item.SC_GEO)) CaseNumber = 5;
                if (!string.IsNullOrEmpty(Item.TARIFF_CODE) && string.IsNullOrEmpty(Item.MC_GEO) &&
                    string.IsNullOrEmpty(Item.SC_GEO)) CaseNumber = 6;

                switch (CaseNumber)
                {
                    case 1:
                        f = GrpList.Find(s =>
                            s.SC_GEO.Equals(Item.SC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                            s.YEAR.Equals(Item.YEAR));
                        break;
                    case 2:
                        f = GrpList.Find(s =>
                            s.SC_GEO.Equals(Item.SC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                            s.TARIFF_CODE.Equals(Item.TARIFF_CODE) && s.YEAR.Equals(Item.YEAR));
                        break;
                    case 3:
                        f = GrpList.Find(s =>
                            s.MC_GEO.Equals(Item.SC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                            s.YEAR.Equals(Item.YEAR));
                        break;
                    case 4:
                        f = GrpList.Find(s =>
                            s.MC_GEO.Equals(Item.MC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                            s.TARIFF_CODE.Equals(Item.TARIFF_CODE) && s.YEAR.Equals(Item.YEAR));
                        break;
                    case 5:
                        f = GrpList.Find(s =>
                            s.SC_GEO.Equals(Item.SC_GEO) && s.MC_GEO.Equals(Item.MC_GEO) &&
                            s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) && s.TARIFF_CODE.Equals(Item.TARIFF_CODE) &&
                            s.YEAR.Equals(Item.YEAR));
                        break;
                    case 6:
                        f = GrpList.Find(s =>
                            s.TARIFF_CODE.Equals(Item.TARIFF_CODE) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                            s.YEAR.Equals(Item.YEAR));
                        break;
                }


                if (f == null)
                {
                    var model = new ReportWriterByMonthModel
                    {
                        ID = Item.ID,
                        YEAR = Item.YEAR,
                        MC_GEO = Item.MC_GEO ?? string.Empty,
                        MC_NAME = Item.MC_NAME ?? string.Empty,
                        PORT_ID = Item.PORT_ID,
                        PORT_NAME = Item.PORT_NAME ?? string.Empty,
                        QUARTER = Item.QUARTER,
                        SC_GEO = Item.SC_GEO ?? string.Empty,
                        SC_NAME = Item.SC_NAME ?? string.Empty,
                        SC_GEO_REGION = Item.SC_GEO_REGION ?? string.Empty,
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE ?? string.Empty,
                        SIX_DIGIT = Item.SIX_DIGIT ?? string.Empty,
                        TARIFF_CATEGORY = Item.TARIFF_CATEGORY ?? string.Empty,
                        TWO_DIGIT = Item.TWO_DIGIT ?? string.Empty,
                        TARIFF_CODE = Item.TARIFF_CODE ?? string.Empty,
                        TARIFF_LEGEND = Item.TARIFF_LEGEND ?? string.Empty,
                        TIME_ID = Item.TIME_ID
                    };

                    for (var i = 1; i <= 12; i++)
                    {
                        var ValueList = new ReportWriterModel();

                        switch (CaseNumber)
                        {
                            case 1:
                                ValueList = ReportList.Find(s =>
                                    s.SC_GEO.Equals(Item.SC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                                    s.YEAR.Equals(Item.YEAR) && s.MONTH.Equals(i));
                                break;
                            case 2:
                                ValueList = ReportList.Find(s =>
                                    s.SC_GEO.Equals(Item.SC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                                    s.TARIFF_CODE.Equals(Item.TARIFF_CODE) && s.YEAR.Equals(Item.YEAR) &&
                                    s.MONTH.Equals(i));
                                break;
                            case 3:
                                ValueList = ReportList.Find(s =>
                                    s.MC_GEO.Equals(Item.MC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                                    s.YEAR.Equals(Item.YEAR) && s.MONTH.Equals(i));
                                break;
                            case 4:
                                ValueList = ReportList.Find(s =>
                                    s.MC_GEO.Equals(Item.MC_GEO) && s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                                    s.TARIFF_CODE.Equals(Item.TARIFF_CODE) && s.YEAR.Equals(Item.YEAR) &&
                                    s.MONTH.Equals(i));
                                break;
                            case 5:
                                ValueList = ReportList.Find(s =>
                                    s.SC_GEO.Equals(Item.SC_GEO) && s.MC_GEO.Equals(Item.MC_GEO) &&
                                    s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) &&
                                    s.TARIFF_CODE.Equals(Item.TARIFF_CODE) && s.YEAR.Equals(Item.YEAR) &&
                                    s.MONTH.Equals(i));
                                break;
                            case 6:
                                ValueList = ReportList.Find(s =>
                                    s.TARIFF_CODE.Equals(Item.TARIFF_CODE) &&
                                    s.SIDE_OF_TRADE.Equals(Item.SIDE_OF_TRADE) && s.YEAR.Equals(Item.YEAR) &&
                                    s.MONTH.Equals(i));
                                break;
                        }


                        if (ValueList != null)
                            switch (i)
                            {
                                case 1:
                                    model.WEIGHT_1 = ValueList.WEIGHT;
                                    model.VALUE_1 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_1 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 2:
                                    model.WEIGHT_2 = ValueList.WEIGHT;
                                    model.VALUE_2 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_2 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 3:
                                    model.WEIGHT_3 = ValueList.WEIGHT;
                                    model.VALUE_3 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_3 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 4:
                                    model.WEIGHT_4 = ValueList.WEIGHT;
                                    model.VALUE_4 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_4 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 5:
                                    model.WEIGHT_5 = ValueList.WEIGHT;
                                    model.VALUE_5 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_5 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 6:
                                    model.WEIGHT_6 = ValueList.WEIGHT;
                                    model.VALUE_6 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_6 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 7:
                                    model.WEIGHT_7 = ValueList.WEIGHT;
                                    model.VALUE_7 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_7 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 8:
                                    model.WEIGHT_8 = ValueList.WEIGHT;
                                    model.VALUE_8 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_8 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 9:
                                    model.WEIGHT_9 = ValueList.WEIGHT;
                                    model.VALUE_9 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_9 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 10:
                                    model.WEIGHT_10 = ValueList.WEIGHT;
                                    model.VALUE_10 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_10 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 11:
                                    model.WEIGHT_11 = ValueList.WEIGHT;
                                    model.VALUE_11 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_11 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                                case 12:
                                    model.WEIGHT_12 = ValueList.WEIGHT;
                                    model.VALUE_12 = ValueList.MONETARY_VALUE;
                                    if (ValueList.WEIGHT > 0 && ValueList.MONETARY_VALUE > 0)
                                        model.VPT_12 = ValueList.MONETARY_VALUE / ValueList.WEIGHT;
                                    break;
                            }

                        model.TOTAL_WEIGHT = model.WEIGHT_1
                                             + model.WEIGHT_2
                                             + model.WEIGHT_3
                                             + model.WEIGHT_4
                                             + model.WEIGHT_5
                                             + model.WEIGHT_6
                                             + model.WEIGHT_7
                                             + model.WEIGHT_8
                                             + model.WEIGHT_9
                                             + model.WEIGHT_10
                                             + model.WEIGHT_11
                                             + model.WEIGHT_12;

                        model.TOTAL_VALUE = model.VALUE_1
                                            + model.VALUE_2
                                            + model.VALUE_3
                                            + model.VALUE_4
                                            + model.VALUE_5
                                            + model.VALUE_6
                                            + model.VALUE_7
                                            + model.VALUE_8
                                            + model.VALUE_9
                                            + model.VALUE_10
                                            + model.VALUE_11
                                            + model.VALUE_12;

                        model.TOTAL_VPT = model.TOTAL_VALUE / model.TOTAL_WEIGHT;

                        if (double.IsInfinity(model.TOTAL_VPT)) model.TOTAL_VPT = 0;
                    }

                    GrpList.Add(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        return GrpList;
    }
}

public class ReportWriterByMonthModel
{
    public int ID { get; set; }
    public int TIME_ID { get; set; }
    public string SC_GEO { get; set; }
    public string SC_NAME { get; set; }
    public string SC_GEO_REGION { get; set; }
    public string MC_GEO { get; set; }
    public string MC_NAME { get; set; }
    public string TARIFF_CODE { get; set; }
    public string TWO_DIGIT { get; set; }
    public string SIX_DIGIT { get; set; }
    public string TARIFF_CATEGORY { get; set; }
    public string TARIFF_LEGEND { get; set; }
    public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public string PORT_NAME { get; set; }
    public int YEAR { get; set; }

    public int QUARTER { get; set; }

    // public int MONTH { get; set; }
    public double WEIGHT_1 { get; set; }
    public double VALUE_1 { get; set; }
    public double VPT_1 { get; set; }
    public double WEIGHT_2 { get; set; }
    public double VALUE_2 { get; set; }
    public double VPT_2 { get; set; }
    public double WEIGHT_3 { get; set; }
    public double VALUE_3 { get; set; }
    public double VPT_3 { get; set; }
    public double WEIGHT_4 { get; set; }
    public double VALUE_4 { get; set; }
    public double VPT_4 { get; set; }
    public double WEIGHT_5 { get; set; }
    public double VALUE_5 { get; set; }
    public double VPT_5 { get; set; }
    public double WEIGHT_6 { get; set; }
    public double VALUE_6 { get; set; }
    public double VPT_6 { get; set; }
    public double WEIGHT_7 { get; set; }
    public double VALUE_7 { get; set; }
    public double VPT_7 { get; set; }
    public double WEIGHT_8 { get; set; }
    public double VALUE_8 { get; set; }
    public double VPT_8 { get; set; }
    public double WEIGHT_9 { get; set; }
    public double VALUE_9 { get; set; }
    public double VPT_9 { get; set; }
    public double WEIGHT_10 { get; set; }
    public double VALUE_10 { get; set; }
    public double VPT_10 { get; set; }
    public double WEIGHT_11 { get; set; }
    public double VALUE_11 { get; set; }
    public double VPT_11 { get; set; }
    public double WEIGHT_12 { get; set; }
    public double VALUE_12 { get; set; }
    public double VPT_12 { get; set; }
    public double TOTAL_WEIGHT { get; set; }
    public double TOTAL_VALUE { get; set; }
    public double TOTAL_VPT { get; set; }
}

public class ReportPivotMonthModel
{
    [JsonProperty("KEY")] public string KEY { get; set; }

    [JsonProperty("1")] public string JAN { get; set; }

    [JsonProperty("2")] public string FEB { get; set; }

    [JsonProperty("3")] public string MAR { get; set; }

    [JsonProperty("4")] public string APR { get; set; }

    [JsonProperty("5")] public string MAY { get; set; }

    [JsonProperty("6")] public string JUN { get; set; }

    [JsonProperty("7")] public string JLY { get; set; }

    [JsonProperty("8")] public string AUG { get; set; }

    [JsonProperty("9")] public string SEP { get; set; }

    [JsonProperty("10")] public string OCT { get; set; }

    [JsonProperty("11")] public string NOV { get; set; }

    [JsonProperty("12")] public string DEC { get; set; }
}

public class ReportWriterByMonthOutModel
{
    public string KEY { get; set; }
    public int ID { get; set; }
    public int TIME_ID { get; set; }
    public string SC_GEO { get; set; }
    public string SC_NAME { get; set; }
    public string SC_GEO_REGION { get; set; }
    public string MC_GEO { get; set; }
    public string MC_NAME { get; set; }
    public string TARIFF_CODE { get; set; }
    public string TWO_DIGIT { get; set; }
    public string SIX_DIGIT { get; set; }
    public string TARIFF_CATEGORY { get; set; }
    public string TARIFF_LEGEND { get; set; }
    public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public string PORT_NAME { get; set; }
    public int YEAR { get; set; }

    public int QUARTER { get; set; }

    // public int MONTH { get; set; }
    public double JAN_WEIGHT { get; set; }
    public double JAN_VALUE { get; set; }
    public double JAN_VPT { get; set; }
    public double FEB_WEIGHT { get; set; }
    public double FEB_VALUE { get; set; }
    public double FEB_VPT { get; set; }
    public double MAR_WEIGHT { get; set; }
    public double MAR_VALUE { get; set; }
    public double MAR_VPT { get; set; }
    public double APR_WEIGHT { get; set; }
    public double APR_VALUE { get; set; }
    public double APR_VPT { get; set; }
    public double MAY_WEIGHT { get; set; }
    public double MAY_VALUE { get; set; }
    public double MAY_VPT { get; set; }
    public double JUN_WEIGHT { get; set; }
    public double JUN_VALUE { get; set; }
    public double JUN_VPT { get; set; }
    public double JLY_WEIGHT { get; set; }
    public double JLY_VALUE { get; set; }
    public double JLY_VPT { get; set; }
    public double AUG_WEIGHT { get; set; }
    public double AUG_VALUE { get; set; }
    public double AUG_VPT { get; set; }
    public double SEP_WEIGHT { get; set; }
    public double SEP_VALUE { get; set; }
    public double SEP_VPT { get; set; }
    public double OCT_WEIGHT { get; set; }
    public double OCT_VALUE { get; set; }
    public double OCT_VPT { get; set; }
    public double NOV_WEIGHT { get; set; }
    public double NOV_VALUE { get; set; }
    public double NOV_VPT { get; set; }
    public double DEC_WEIGHT { get; set; }
    public double DEC_VALUE { get; set; }
    public double DEC_VPT { get; set; }
    public double TOTAL_WEIGHT { get; set; }
    public double TOTAL_VALUE { get; set; }
    public double TOTAL_VPT { get; set; }
}

public class ReportPivotQuarterModel
{
    [JsonProperty("KEY")] public string KEY { get; set; }

    [JsonProperty("1")] public string Q1 { get; set; }

    [JsonProperty("2")] public string Q2 { get; set; }

    [JsonProperty("3")] public string Q3 { get; set; }

    [JsonProperty("4")] public string Q4 { get; set; }
}

public class ReportWriterByQuarterOutModel
{
    public string KEY { get; set; }
    public int ID { get; set; }
    public int TIME_ID { get; set; }
    public string SC_GEO { get; set; }
    public string SC_NAME { get; set; }
    public string SC_GEO_REGION { get; set; }
    public string MC_GEO { get; set; }
    public string MC_NAME { get; set; }
    public string TARIFF_CODE { get; set; }
    public string TWO_DIGIT { get; set; }
    public string SIX_DIGIT { get; set; }
    public string TARIFF_CATEGORY { get; set; }
    public string TARIFF_LEGEND { get; set; }
    public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public string PORT_NAME { get; set; }
    public int YEAR { get; set; }

    public int QUARTER { get; set; }

    // public int MONTH { get; set; }
    public double Q1_WEIGHT { get; set; }
    public double Q1_VALUE { get; set; }
    public double Q1_VPT { get; set; }
    public double Q2_WEIGHT { get; set; }
    public double Q2_VALUE { get; set; }
    public double Q2_VPT { get; set; }
    public double Q3_WEIGHT { get; set; }
    public double Q3_VALUE { get; set; }
    public double Q3_VPT { get; set; }
    public double Q4_WEIGHT { get; set; }
    public double Q4_VALUE { get; set; }
    public double Q4_VPT { get; set; }

    public double TOTAL_WEIGHT { get; set; }
    public double TOTAL_VALUE { get; set; }
    public double TOTAL_VPT { get; set; }
}

public class ReportPivotFlowModel
{
    [JsonProperty("KEY")] public string KEY { get; set; }

    [JsonProperty("Import")] public string Import { get; set; }

    [JsonProperty("Export")] public string Export { get; set; }
}

public class ReportWriterByFlowOutModel
{
    public string KEY { get; set; }
    public int ID { get; set; }
    public int TIME_ID { get; set; }
    public string SC_GEO { get; set; }
    public string SC_NAME { get; set; }
    public string SC_GEO_REGION { get; set; }
    public string MC_GEO { get; set; }
    public string MC_NAME { get; set; }
    public string TARIFF_CODE { get; set; }
    public string TWO_DIGIT { get; set; }
    public string SIX_DIGIT { get; set; }
    public string TARIFF_CATEGORY { get; set; }

    public string TARIFF_LEGEND { get; set; }

    //  public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public string PORT_NAME { get; set; }
    public int YEAR { get; set; }
    public int QUARTER { get; set; }
    public int MONTH { get; set; }

    public double EXPORT_WEIGHT { get; set; }
    public double EXPORT_VALUE { get; set; }
    public double EXPORT_VPT { get; set; }

    public double IMPORT_WEIGHT { get; set; }
    public double IMPORT_VALUE { get; set; }
    public double IMPORT_VPT { get; set; }

    public double TOTAL_WEIGHT { get; set; }
    public double TOTAL_VALUE { get; set; }
    public double TOTAL_VPT { get; set; }
}

public class ReportPivotYearModel
{
    [JsonProperty("KEY")] public string KEY { get; set; }

    [JsonProperty("2030")] public string Y2030 { get; set; }

    [JsonProperty("2029")] public string Y2029 { get; set; }

    [JsonProperty("2028")] public string Y2028 { get; set; }

    [JsonProperty("2027")] public string Y2027 { get; set; }

    [JsonProperty("2026")] public string Y2026 { get; set; }

    [JsonProperty("2025")] public string Y2025 { get; set; }

    [JsonProperty("2024")] public string Y2024 { get; set; }

    [JsonProperty("2023")] public string Y2023 { get; set; }

    [JsonProperty("2022")] public string Y2022 { get; set; }

    [JsonProperty("2021")] public string Y2021 { get; set; }

    [JsonProperty("2020")] public string Y2020 { get; set; }


    [JsonProperty("2019")] public string Y2019 { get; set; }

    [JsonProperty("2018")] public string Y2018 { get; set; }

    [JsonProperty("2017")] public string Y2017 { get; set; }

    [JsonProperty("2016")] public string Y2016 { get; set; }

    [JsonProperty("2015")] public string Y2015 { get; set; }

    [JsonProperty("2014")] public string Y2014 { get; set; }

    [JsonProperty("2013")] public string Y2013 { get; set; }

    [JsonProperty("2012")] public string Y2012 { get; set; }

    [JsonProperty("2011")] public string Y2011 { get; set; }

    [JsonProperty("2010")] public string Y2010 { get; set; }

    [JsonProperty("2009")] public string Y2009 { get; set; }

    [JsonProperty("2008")] public string Y2008 { get; set; }

    [JsonProperty("2007")] public string Y2007 { get; set; }

    [JsonProperty("2006")] public string Y2006 { get; set; }

    [JsonProperty("2005")] public string Y2005 { get; set; }

    [JsonProperty("2004")] public string Y2004 { get; set; }

    [JsonProperty("2003")] public string Y2003 { get; set; }

    [JsonProperty("2002")] public string Y2002 { get; set; }

    [JsonProperty("2001")] public string Y2001 { get; set; }

    [JsonProperty("2000")] public string Y2000 { get; set; }

    [JsonProperty("1999")] public string Y1999 { get; set; }

    [JsonProperty("1998")] public string Y1998 { get; set; }

    [JsonProperty("1997")] public string Y1997 { get; set; }

    [JsonProperty("1996")] public string Y1996 { get; set; }

    [JsonProperty("1995")] public string Y1995 { get; set; }

    [JsonProperty("1994")] public string Y1994 { get; set; }

    [JsonProperty("1993")] public string Y1993 { get; set; }

    [JsonProperty("1992")] public string Y1992 { get; set; }

    [JsonProperty("1991")] public string Y1991 { get; set; }

    [JsonProperty("1990")] public string Y1990 { get; set; }
}

public class ReportWriterByYearOutModel
{
    public string KEY { get; set; }
    public int ID { get; set; }
    public int TIME_ID { get; set; }
    public string SC_GEO { get; set; }
    public string SC_NAME { get; set; }
    public string SC_GEO_REGION { get; set; }
    public string MC_GEO { get; set; }
    public string MC_NAME { get; set; }
    public string TARIFF_CODE { get; set; }
    public string TWO_DIGIT { get; set; }
    public string SIX_DIGIT { get; set; }
    public string TARIFF_CATEGORY { get; set; }
    public string TARIFF_LEGEND { get; set; }
    public string SIDE_OF_TRADE { get; set; }
    public int PORT_ID { get; set; }
    public string PORT_NAME { get; set; }
    public int YEAR { get; set; }

    public int QUARTER { get; set; }
    // public int MONTH { get; set; }

    public double Y1990_WEIGHT { get; set; }
    public double Y1990_VALUE { get; set; }
    public double Y1990_VPT { get; set; }
    public double Y1991_WEIGHT { get; set; }
    public double Y1991_VALUE { get; set; }
    public double Y1991_VPT { get; set; }
    public double Y1992_WEIGHT { get; set; }
    public double Y1992_VALUE { get; set; }
    public double Y1992_VPT { get; set; }
    public double Y1993_WEIGHT { get; set; }
    public double Y1993_VALUE { get; set; }
    public double Y1993_VPT { get; set; }
    public double Y1994_WEIGHT { get; set; }
    public double Y1994_VALUE { get; set; }
    public double Y1994_VPT { get; set; }
    public double Y1995_WEIGHT { get; set; }
    public double Y1995_VALUE { get; set; }
    public double Y1995_VPT { get; set; }
    public double Y1996_WEIGHT { get; set; }
    public double Y1996_VALUE { get; set; }
    public double Y1996_VPT { get; set; }
    public double Y1997_WEIGHT { get; set; }
    public double Y1997_VALUE { get; set; }
    public double Y1997_VPT { get; set; }
    public double Y1998_WEIGHT { get; set; }
    public double Y1998_VALUE { get; set; }
    public double Y1998_VPT { get; set; }
    public double Y1999_WEIGHT { get; set; }
    public double Y1999_VALUE { get; set; }
    public double Y1999_VPT { get; set; }

    public double Y2000_WEIGHT { get; set; }
    public double Y2000_VALUE { get; set; }
    public double Y2000_VPT { get; set; }
    public double Y2001_WEIGHT { get; set; }
    public double Y2001_VALUE { get; set; }
    public double Y2001_VPT { get; set; }
    public double Y2002_WEIGHT { get; set; }
    public double Y2002_VALUE { get; set; }
    public double Y2002_VPT { get; set; }
    public double Y2003_WEIGHT { get; set; }
    public double Y2003_VALUE { get; set; }
    public double Y2003_VPT { get; set; }
    public double Y2004_WEIGHT { get; set; }
    public double Y2004_VALUE { get; set; }
    public double Y2004_VPT { get; set; }
    public double Y2005_WEIGHT { get; set; }
    public double Y2005_VALUE { get; set; }
    public double Y2005_VPT { get; set; }
    public double Y2006_WEIGHT { get; set; }
    public double Y2006_VALUE { get; set; }
    public double Y2006_VPT { get; set; }
    public double Y2007_WEIGHT { get; set; }
    public double Y2007_VALUE { get; set; }
    public double Y2007_VPT { get; set; }
    public double Y2008_WEIGHT { get; set; }
    public double Y2008_VALUE { get; set; }
    public double Y2008_VPT { get; set; }
    public double Y2009_WEIGHT { get; set; }
    public double Y2009_VALUE { get; set; }
    public double Y2009_VPT { get; set; }

    public double Y2010_WEIGHT { get; set; }
    public double Y2010_VALUE { get; set; }
    public double Y2010_VPT { get; set; }
    public double Y2011_WEIGHT { get; set; }
    public double Y2011_VALUE { get; set; }
    public double Y2011_VPT { get; set; }
    public double Y2012_WEIGHT { get; set; }
    public double Y2012_VALUE { get; set; }
    public double Y2012_VPT { get; set; }
    public double Y2013_WEIGHT { get; set; }
    public double Y2013_VALUE { get; set; }
    public double Y2013_VPT { get; set; }
    public double Y2014_WEIGHT { get; set; }
    public double Y2014_VALUE { get; set; }
    public double Y2014_VPT { get; set; }
    public double Y2015_WEIGHT { get; set; }
    public double Y2015_VALUE { get; set; }
    public double Y2015_VPT { get; set; }
    public double Y2016_WEIGHT { get; set; }
    public double Y2016_VALUE { get; set; }
    public double Y2016_VPT { get; set; }
    public double Y2017_WEIGHT { get; set; }
    public double Y2017_VALUE { get; set; }
    public double Y2017_VPT { get; set; }
    public double Y2018_WEIGHT { get; set; }
    public double Y2018_VALUE { get; set; }
    public double Y2018_VPT { get; set; }
    public double Y2019_WEIGHT { get; set; }
    public double Y2019_VALUE { get; set; }
    public double Y2019_VPT { get; set; }

    public double Y2020_WEIGHT { get; set; }
    public double Y2020_VALUE { get; set; }
    public double Y2020_VPT { get; set; }
    public double Y2021_WEIGHT { get; set; }
    public double Y2021_VALUE { get; set; }
    public double Y2021_VPT { get; set; }
    public double Y2022_WEIGHT { get; set; }
    public double Y2022_VALUE { get; set; }
    public double Y2022_VPT { get; set; }
    public double Y2023_WEIGHT { get; set; }
    public double Y2023_VALUE { get; set; }
    public double Y2023_VPT { get; set; }
    public double Y2024_WEIGHT { get; set; }
    public double Y2024_VALUE { get; set; }
    public double Y2024_VPT { get; set; }
    public double Y2025_WEIGHT { get; set; }
    public double Y2025_VALUE { get; set; }
    public double Y2025_VPT { get; set; }
    public double Y2026_WEIGHT { get; set; }
    public double Y2026_VALUE { get; set; }
    public double Y2026_VPT { get; set; }
    public double Y2027_WEIGHT { get; set; }
    public double Y2027_VALUE { get; set; }
    public double Y2027_VPT { get; set; }
    public double Y2028_WEIGHT { get; set; }
    public double Y2028_VALUE { get; set; }
    public double Y2028_VPT { get; set; }
    public double Y2029_WEIGHT { get; set; }
    public double Y2029_VALUE { get; set; }
    public double Y2029_VPT { get; set; }

    public double Y2030_WEIGHT { get; set; }
    public double Y2030_VALUE { get; set; }
    public double Y2030_VPT { get; set; }

    public double TOTAL_WEIGHT { get; set; }
    public double TOTAL_VALUE { get; set; }
    public double TOTAL_VPT { get; set; }
}

public class ReportOutModelJsonModel
{
    public string CSS { get; set; }
    public string JQUERY { get; set; }
    public string HTML { get; set; }
    public string Title { get; set; }
    public bool IsPivotData { get; set; }
    public List<ReportWriterModel> data { get; set; }
}