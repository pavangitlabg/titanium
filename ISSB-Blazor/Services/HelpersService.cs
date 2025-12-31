using Data.Models;

namespace Services;

public class HelpersService
{
    public List<SaveQueryDateModel> ConvertToTimeDimension(string TimeArray)
    {
        var TimeDimenstionList = new List<SaveQueryDateModel>();

        TimeArray = TimeArray.Remove(TimeArray.Length - 1);

        var Time = string.Empty;
        var Year = string.Empty;
        var TimeParts = TimeArray.Split('|');
        foreach (var word in TimeParts)
        {
            if (word.Length == 4) Year = word;

            if (word.Length <= 2)
            {
                if (word.Length == 1)
                    Time += GetQuarter(word) + word.PadLeft(word.Length + 1, '0') + "00";
                else
                    Time += GetQuarter(word) + word + "00";
            }

            if (Time.Length >= 5)
            {
                var tDimenstion = Year + Time;
                var m = new SaveQueryDateModel { TimeID = int.Parse(tDimenstion) };
                TimeDimenstionList.Add(m);
                Time = string.Empty;
            }
        }


        return TimeDimenstionList;
    }

    public List<DashBoardSaveQueryDateModel> DashBoardConvertToTimeDimension(string TimeArray)
    {
        var TimeDimenstionList = new List<DashBoardSaveQueryDateModel>();

        TimeArray = TimeArray.Remove(TimeArray.Length - 1);

        var Time = string.Empty;
        var Year = string.Empty;
        var TimeParts = TimeArray.Split('|');
        foreach (var word in TimeParts)
        {
            if (word.Length == 4) Year = word;

            if (word.Length <= 2)
            {
                if (word.Length == 1)
                    Time += GetQuarter(word) + word.PadLeft(word.Length + 1, '0') + "00";
                else
                    Time += GetQuarter(word) + word + "00";
            }

            if (Time.Length >= 5)
            {
                var tDimenstion = Year + Time;
                var m = new DashBoardSaveQueryDateModel { TimeID = int.Parse(tDimenstion) };
                TimeDimenstionList.Add(m);
                Time = string.Empty;
            }
        }


        return TimeDimenstionList;
    }

    private int GetQuarter(string item)
    {
        var Q = int.Parse(item);
        var R = 0;

        if (Q >= 1 && Q <= 3) R = 1;

        if (Q >= 4 && Q <= 6) R = 2;

        if (Q >= 7 && Q <= 9) R = 3;

        if (Q >= 10 && Q <= 12) R = 4;

        return R;
    }
}