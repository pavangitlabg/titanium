using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class SystemControlModel
{
    public string _id { get; set; }

    [Display(Name = "Last File Number")] public long LastFileNumber { get; set; }

    [Display(Name = "Last Batch Number")] public long ImportBatch { get; set; }

    [Display(Name = "Howler Upper Weight (KG)")]
    [Range(0, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
    public double HowlerWeight { get; set; }

    [Display(Name = "Howler Upper Value")]
    [Range(0, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
    public double HowlerValue { get; set; }

    [Display(Name = "SMS Gateway Number")] public string SMSNumber { get; set; }

    [Display(Name = "SMS Prefix Message")] public string SMSMessage { get; set; }

    [Display(Name = "Shedule Hour Execution")]
    public int ScheduleHour { get; set; }
}