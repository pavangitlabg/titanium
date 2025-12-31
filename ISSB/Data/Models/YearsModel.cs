
using System.Collections.Generic;

namespace Data.Models
{
    public class YearsModel
    {
        public int Idx { get; set; }
        public int Year { get; set; }
        public List<MonthModel> Months { get; set; }
    }

    public class MonthModel
    {
        public int MounthNumber { get; set; }
        public string Month { get; set; }
    }
}
