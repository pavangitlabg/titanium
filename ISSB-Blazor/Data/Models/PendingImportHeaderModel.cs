using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class PendingImportHeaderModel
{
    public string _id { get; set; }
    public DateTime DATE { get; set; }
    public int BATCH_NO { get; set; }

    [Display(Name = "Status")] public string STATUS { get; set; }

    public string IMPORT_TYPE { get; set; }
    public bool FAIL { get; set; }
    public DateTime END_DATE { get; set; }
    public bool POSTED { get; set; }
    public string TIME_TAKEN { get; set; }
}