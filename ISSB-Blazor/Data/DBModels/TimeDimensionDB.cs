using MongoDB.Bson;

namespace Data.DBModels;

public class TimeDimensionDB
{
    public BsonObjectId _id { get; set; }
    public int TIME_ID { get; set; }
    public int YEAR { get; set; }
    public int QUARTER { get; set; }
    public int MONTH { get; set; }
    public int WEEK { get; set; }
    public string PERIOD_NAME_OLD { get; set; }
    public DateTime PERIOD_END_DATE { get; set; }
    public int NUMBER_OF_WEEKS_IN_PERIOD { get; set; }
    public int CUMULATIVE_NUMBER_OF_CALENDAR_DAYS { get; set; }
    public int CUMULATIVE_WEEKS { get; set; }
    public int NUMBER_OF_CALENDAR_DAYS { get; set; }
    public int QUARTER_IN_FINANCIAL_YEAR { get; set; }
    public int MONTH_IN_FINANCIAL_YEAR { get; set; }
    public int ORDINAL_MONTH_OF_QUARTER { get; set; }
    public int ORDINAL_WEEK_OF_MONTH { get; set; }
    public string WEEK_POSITION_IN_MONTH { get; set; }
    public string PERIOD_GRANULARITY { get; set; }
    public string PERIOD_SHORT_LEGEND { get; set; }
    public string PERIOD_LONG_LEGEND { get; set; }
    public DateTime MONTH_DATE { get; set; }
    public string PERIOD_NAME { get; set; }
    public string FINANCIAL_YEAR { get; set; }
}