/*
  Data/Models/ChartModel.cs

  model for transferring choice of chart
*/

namespace ISSB.Models
{
    /// <summary>model for transferring choice of chart</summary>

    public class ChartModel
    {
        /// <summary>chart type</summary>

        public int Type { get ; set ; }

        /// <summary>saved query ID</summary>

        public string Query { get ; set ; }

        /// <summary>reporting engine model</summary>

        public ReportEngineModel EngineModel { get ; set ; }
    }
}
