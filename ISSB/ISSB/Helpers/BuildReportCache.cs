using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using ISSB.Models;
using Services;

namespace ISSB.Helpers
{
    public class BuildReportCache
    {
        private static readonly Lazy<BuildReportCache> lazy = new Lazy<BuildReportCache>(() => new BuildReportCache());
        public static BuildReportCache Instance { get => lazy.Value; }

        public BuildReportCache()
        {
        }

        public async Task<FileReturnModel> BuildFromCache(ReportDesignerModel model)
        {
            bool bByMonthHorizontal = true;
            var rCacheSrv = ReportCacheService.Instance;
            int DocCount = await rCacheSrv.Count(model.ReportID);
            var selectedCurrency = "GBP";

            var dbReturnList = await rCacheSrv.GetRange(model.ReportID, 0, DocCount);
            var outList = dbReturnList.Select(s => (ReportWriterByMonthOutModel)s).ToList();

            var fList = new List<string>();

            if (!string.IsNullOrEmpty(model.FieldSort))
            {
                string[] words = model.FieldSort.Split(',');

                foreach (var f in words)
                {
                    if (!string.IsNullOrEmpty(f))
                    {
                        fList.Add(f);
                    }
                }
            }

            if (bByMonthHorizontal)
            {
                var ExportList = new List<CrossTabMultiMonthModel>();
                var ImportList = new List<CrossTabMultiMonthModel>();
                var NewKey = string.Empty;
                var outListByHor = outList.GroupBy(x => x.YEAR).ToList();

                outListByHor = outListByHor.OrderBy(x => x.Key).ToList();

                if (model.FileType.Equals("TXT"))
                {

                    var outLine = string.Empty;
                    var headerLine = string.Empty;
                    var SearchKey = string.Empty;
                    var LastKey = string.Empty;

                    var searchList = new List<SearchPivotModel>();

                    foreach (var keyItem in outListByHor)
                    {


                        foreach (var Item in keyItem)
                        {
                            if (keyItem.Key != Item.YEAR)
                                break;


                            SearchKey = string.Empty;
                            foreach (var kItem in fList)
                            {
                                if (kItem.Equals("SC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO))
                                    {
                                        SearchKey += Item.SC_GEO + "|";
                                    }
                                }
                                if (kItem.Equals("SC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_NAME))
                                    {

                                        SearchKey += Item.SC_NAME + "|";
                                    }
                                }
                                if (kItem.Equals("SC_GEO_REGION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                    {

                                        SearchKey += Item.SC_GEO_REGION + "|";
                                    }
                                }
                                if (kItem.Equals("MC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_GEO))
                                    {

                                        SearchKey += Item.MC_GEO + "|";
                                    }
                                }

                                if (kItem.Equals("MC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_NAME))
                                    {

                                        SearchKey += Item.MC_NAME + "|";
                                    }
                                }

                                if (kItem.Equals("PORT"))
                                {

                                    SearchKey += Item.PORT_ID + "|";

                                }

                                if (kItem.Equals("PORT_NAME"))
                                {

                                    SearchKey += Item.PORT_NAME + "|";

                                }

                                if (kItem.Equals("TARIFF_CODE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {

                                        SearchKey += Item.TARIFF_CODE + "|";
                                    }
                                }

                                if (kItem.Equals("TARIFF_DESCRIPTION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {

                                        SearchKey += Item.TARIFF_LEGEND + "|";
                                    }
                                }

                                if (kItem.Equals("TWO_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                    {

                                        SearchKey += Item.TWO_DIGIT + "|";
                                    }
                                }

                                if (kItem.Equals("SIX_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                    {
                                        SearchKey += Item.SIX_DIGIT + "|";
                                    }
                                }

                                if (kItem.Equals("TARIFF_CATEGORY"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                    {
                                        SearchKey += Item.TARIFF_CATEGORY + "|";
                                    }
                                }


                                if (kItem.Equals("SIDE_OF_TRADE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                    {

                                        SearchKey += Item.SIDE_OF_TRADE + "|";
                                    }
                                }
                            }


                            headerLine = string.Empty;
                            outLine = string.Empty;

                            foreach (var kItem in fList)
                            {
                                if (kItem.Equals("SC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO))
                                    {
                                        headerLine += "SC_GEO,";
                                        outLine += Item.SC_GEO + ",";

                                    }
                                }
                                if (kItem.Equals("SC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_NAME))
                                    {
                                        headerLine += "SC_NAME,";
                                        outLine += Item.SC_NAME + ",";

                                    }
                                }
                                if (kItem.Equals("SC_GEO_REGION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                    {
                                        headerLine += "SC_GEO_REGION,";
                                        outLine += Item.SC_GEO_REGION + ",";

                                    }
                                }
                                if (kItem.Equals("MC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_GEO))
                                    {
                                        headerLine += "MC_GEO,";
                                        outLine += Item.MC_GEO + ",";

                                    }
                                }

                                if (kItem.Equals("MC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_NAME))
                                    {
                                        headerLine += "MC_NAME,";
                                        outLine += Item.MC_NAME + ",";

                                    }
                                }

                                if (kItem.Equals("PORT"))
                                {

                                    headerLine += "PORT,";
                                    outLine += Item.PORT_ID + ",";


                                }

                                if (kItem.Equals("PORT_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.PORT_NAME))
                                    {
                                        headerLine += "PORT_NAME,";
                                        outLine += Item.PORT_NAME + ",";

                                    }
                                }

                                if (kItem.Equals("TARIFF_CODE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_CODE,";
                                        outLine += Item.TARIFF_CODE + ",";

                                    }
                                }

                                if (kItem.Equals("TARIFF_DESCRIPTION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_DESCRIPTION,";
                                        outLine += Item.TARIFF_LEGEND + ",";

                                    }
                                }

                                if (kItem.Equals("TWO_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                    {
                                        headerLine += "TWO_DIGIT,";
                                        outLine += Item.TWO_DIGIT + ",";

                                    }
                                }

                                if (kItem.Equals("SIX_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                    {
                                        headerLine += "SIX_DIGIT,";
                                        outLine += Item.SIX_DIGIT + ",";

                                    }
                                }

                                if (kItem.Equals("TARIFF_CATEGORY"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                    {
                                        headerLine += "TARIFF_CATEGORY,";
                                        outLine += Item.TARIFF_CATEGORY + ",";

                                    }
                                }

                                if (kItem.Equals("SIDE_OF_TRADE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                    {
                                        headerLine += "SIDE_OF_TRADE,";
                                        outLine += Item.SIDE_OF_TRADE + ",";

                                    }
                                }

                            }

                            var hearderConcat = string.Empty;
                            var dataConcat = string.Empty;
                            if (model.ShowWeight && !model.ShowWeightValueVpt && !model.ShowWeightVpt)
                            {
                                int iResult = searchList.FindIndex(x => x.Key.Equals(SearchKey) && x.Year == Item.YEAR);

                                if (iResult == -1)
                                {
                                    if (Item.JAN_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JAN_WEIGHT,";
                                        outLine += Item.JAN_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " JAN_WEIGHT,";
                                        dataConcat += Item.JAN_WEIGHT + ",";
                                    }

                                    if (Item.FEB_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " FEB_WEIGHT,";
                                        outLine += Item.FEB_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " FEB_WEIGHT,";
                                        dataConcat += Item.FEB_WEIGHT + ",";
                                    }
                                    if (Item.MAR_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " MAR_WEIGHT,";
                                        outLine += Item.MAR_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " MAR_WEIGHT,";
                                        dataConcat += Item.MAR_WEIGHT + ",";
                                    }
                                    if (Item.APR_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " APR_WEIGHT,";
                                        outLine += Item.APR_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " APR_WEIGHT,";
                                        dataConcat += Item.APR_WEIGHT + ",";
                                    }
                                    if (Item.MAY_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " MAY_WEIGHT,";
                                        outLine += Item.MAY_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " MAY_WEIGHT,";
                                        dataConcat += Item.MAY_WEIGHT + ",";
                                    }
                                    if (Item.JUN_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JUN_WEIGHT,";
                                        outLine += Item.JUN_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " JUN_WEIGHT,";
                                        dataConcat += Item.JUN_WEIGHT + ",";
                                    }
                                    if (Item.JLY_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JLY_WEIGHT,";
                                        outLine += Item.JLY_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " JLY_WEIGHT,";
                                        dataConcat += Item.JLY_WEIGHT + ",";
                                    }
                                    if (Item.AUG_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + "AUG_WEIGHT,";
                                        outLine += Item.AUG_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " AUG_WEIGHT,";
                                        dataConcat += Item.AUG_WEIGHT + ",";
                                    }
                                    if (Item.SEP_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " SEP_WEIGHT,";
                                        outLine += Item.SEP_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " SEP_WEIGHT,";
                                        dataConcat += Item.SEP_WEIGHT + ",";
                                    }
                                    if (Item.OCT_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " OCT_WEIGHT,";
                                        outLine += Item.OCT_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " OCT_WEIGHT,";
                                        dataConcat += Item.OCT_WEIGHT + ",";
                                    }
                                    if (Item.NOV_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " NOV_WEIGHT,";
                                        outLine += Item.NOV_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " NOV_WEIGHT,";
                                        dataConcat += Item.NOV_WEIGHT + ",";
                                    }
                                    if (Item.DEC_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " DEC_WEIGHT,";
                                        outLine += Item.DEC_WEIGHT + ",";
                                        hearderConcat += Item.YEAR + " DEC_WEIGHT,";
                                        dataConcat += Item.DEC_WEIGHT + ",";
                                    }
                                }

                                var searchModel = new SearchPivotModel { Key = SearchKey, Year = Item.YEAR };
                                searchList.Add(searchModel);

                            }

                            if (model.ShowWeight && !model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                int iResult = searchList.FindIndex(x => x.Key.Equals(SearchKey) && x.Year == Item.YEAR);

                                if (iResult == -1)
                                {
                                    if (Item.JAN_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JAN_WEIGHT,VPT,";
                                        outLine += Item.JAN_WEIGHT + "," + Item.JAN_VPT + ",";
                                        hearderConcat += Item.YEAR + " JAN_WEIGHT,VPT,";
                                        dataConcat += Item.JAN_WEIGHT + "," + Item.JAN_VPT + ",";
                                    }
                                    if (Item.FEB_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " FEB_WEIGHT,VPT,";
                                        outLine += Item.FEB_WEIGHT + "," + Item.FEB_VPT + ",";
                                        hearderConcat += Item.YEAR + " FEB_WEIGHT,VPT,";
                                        dataConcat += Item.FEB_WEIGHT + "," + Item.FEB_VPT + ",";
                                    }
                                    if (Item.MAR_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " MAR_WEIGHT,VPT,";
                                        outLine += Item.MAR_WEIGHT + "," + Item.MAR_VPT + ",";
                                        hearderConcat += Item.YEAR + " MAR_WEIGHT,VPT,";
                                        dataConcat += Item.MAR_WEIGHT + "," + Item.MAR_VPT + ",";
                                    }
                                    if (Item.APR_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " APR_WEIGHT,VPT,";
                                        outLine += Item.APR_WEIGHT + "," + Item.APR_VPT + ",";
                                        hearderConcat += Item.YEAR + " APR_WEIGHT,VPT,";
                                        dataConcat += Item.APR_WEIGHT + "," + Item.APR_VPT + ",";
                                    }
                                    if (Item.MAY_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " MAY_WEIGHT,VPT,";
                                        outLine += Item.MAY_WEIGHT + "," + Item.MAY_VPT + ",";
                                        hearderConcat += Item.YEAR + " MAY_WEIGHT,VPT,";
                                        dataConcat += Item.MAY_WEIGHT + "," + Item.MAY_VPT + ",";
                                    }
                                    if (Item.JUN_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JUN_WEIGHT,VPT,";
                                        outLine += Item.JUN_WEIGHT + "," + Item.JUN_VPT + ",";
                                        hearderConcat += Item.YEAR + " JUN_WEIGHT,VPT,";
                                        dataConcat += Item.JUN_WEIGHT + "," + Item.JUN_VPT + ",";
                                    }
                                    if (Item.JLY_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JLY_WEIGHT,VPT,";
                                        outLine += Item.JLY_WEIGHT + "," + Item.JLY_VPT + ",";
                                        hearderConcat += Item.YEAR + " JLY_WEIGHT,VPT,";
                                        dataConcat += Item.JLY_WEIGHT + "," + Item.JLY_VPT + ",";
                                    }
                                    if (Item.AUG_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + "AUG_WEIGHT,VPT,";
                                        outLine += Item.AUG_WEIGHT + "," + Item.AUG_VPT + ",";
                                        hearderConcat += Item.YEAR + " AUG_WEIGHT,VPT,";
                                        dataConcat += Item.AUG_WEIGHT + "," + Item.AUG_VPT + ",";
                                    }
                                    if (Item.SEP_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " SEP_WEIGHT,VPT,";
                                        outLine += Item.SEP_WEIGHT + "," + Item.SEP_VPT + ",";
                                        hearderConcat += Item.YEAR + " SEP_WEIGHT,VPT,";
                                        dataConcat += Item.SEP_WEIGHT + "," + Item.SEP_VPT + ",";
                                    }
                                    if (Item.OCT_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " OCT_WEIGHT,VPT,";
                                        outLine += Item.OCT_WEIGHT + "," + Item.OCT_VPT + ",";
                                        hearderConcat += Item.YEAR + " OCT_WEIGHT,VPT,";
                                        dataConcat += Item.OCT_WEIGHT + "," + Item.OCT_VPT + ",";
                                    }
                                    if (Item.NOV_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " NOV_WEIGHT,VPT,";
                                        outLine += Item.NOV_WEIGHT + "," + Item.NOV_VPT + ",";
                                        hearderConcat += Item.YEAR + " NOV_WEIGHT,VPT,";
                                        dataConcat += Item.NOV_WEIGHT + "," + Item.NOV_VPT + ",";
                                    }
                                    if (Item.DEC_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " DEC_WEIGHT,VPT,";
                                        outLine += Item.DEC_WEIGHT + "," + Item.DEC_VPT + ",";
                                        hearderConcat += Item.YEAR + " DEC_WEIGHT,VPT,";
                                        dataConcat += Item.DEC_WEIGHT + "," + Item.DEC_VPT + ",";
                                    }
                                }
                            }

                            //CHAS
                            if (model.ShowWeight && model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                int iResult = searchList.FindIndex(x => x.Key.Equals(SearchKey) && x.Year == Item.YEAR);

                                if (iResult == -1)
                                {
                                    if (Item.JAN_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JAN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.JAN_WEIGHT + "," + Item.JAN_VPT + "," + Item.JAN_VALUE + ",";
                                        hearderConcat += Item.YEAR + " JAN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.JAN_WEIGHT + "," + Item.JAN_VPT + "," + Item.JAN_VALUE + ",";
                                    }
                                    if (Item.FEB_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " FEB_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.FEB_WEIGHT + "," + Item.FEB_VPT + "," + Item.FEB_VALUE + ",";
                                        hearderConcat += Item.YEAR + " FEB_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.FEB_WEIGHT + "," + Item.FEB_VPT + "," + Item.FEB_VALUE + ",";
                                    }
                                    if (Item.MAR_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " MAR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.MAR_WEIGHT + "," + Item.MAR_VPT + "," + Item.MAR_VALUE + ",";
                                        hearderConcat += Item.YEAR + " MAR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.MAR_WEIGHT + "," + Item.MAR_VPT + "," + Item.MAR_VALUE + ",";
                                    }
                                    if (Item.APR_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " APR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.APR_WEIGHT + "," + Item.APR_VPT + "," + Item.APR_VALUE + ",";
                                        hearderConcat += Item.YEAR + " APR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.APR_WEIGHT + "," + Item.APR_VPT + "," + Item.APR_VALUE + ",";
                                    }
                                    if (Item.MAY_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " MAY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.MAY_WEIGHT + "," + Item.MAY_VPT + "," + Item.MAY_VALUE + ",";
                                        hearderConcat += Item.YEAR + " MAY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.MAY_WEIGHT + "," + Item.MAY_VPT + "," + Item.MAY_VALUE + ",";
                                    }
                                    if (Item.JUN_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JUN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.JUN_WEIGHT + "," + Item.JUN_VPT + "," + Item.JUN_VALUE + ",";
                                        hearderConcat += Item.YEAR + " JUN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.JUN_WEIGHT + "," + Item.JUN_VPT + "," + Item.JUN_VALUE + ",";
                                    }
                                    if (Item.JLY_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " JLY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.JLY_WEIGHT + "," + Item.JLY_VPT + "," + Item.JLY_VALUE + ",";
                                        hearderConcat += Item.YEAR + " JLY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.JLY_WEIGHT + "," + Item.JLY_VPT + "," + Item.JLY_VALUE + ",";
                                    }
                                    if (Item.AUG_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " AUG_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.AUG_WEIGHT + "," + Item.AUG_VPT + "," + Item.AUG_VALUE + ",";
                                        hearderConcat += Item.YEAR + " AUG_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.AUG_WEIGHT + "," + Item.AUG_VPT + "," + Item.AUG_VALUE + ",";
                                    }
                                    if (Item.SEP_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " SEP_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.SEP_WEIGHT + "," + Item.SEP_VPT + "," + Item.SEP_VALUE + ",";
                                        hearderConcat += Item.YEAR + " SEP_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.SEP_WEIGHT + "," + Item.SEP_VPT + "," + Item.SEP_VALUE + ",";
                                    }
                                    if (Item.OCT_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " OCT_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.OCT_WEIGHT + "," + Item.OCT_VPT + "," + Item.OCT_VALUE + ",";
                                        hearderConcat += Item.YEAR + " OCT_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.OCT_WEIGHT + "," + Item.OCT_VPT + "," + Item.OCT_VALUE + ",";
                                    }
                                    if (Item.NOV_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " NOV_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.NOV_WEIGHT + "," + Item.NOV_VPT + "," + Item.NOV_VALUE + ",";
                                        hearderConcat += Item.YEAR + " NOV_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.NOV_WEIGHT + "," + Item.NOV_VPT + "," + Item.NOV_VALUE + ",";
                                    }
                                    if (Item.DEC_WEIGHT >= 0)
                                    {
                                        headerLine += Item.YEAR + " DEC_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        outLine += Item.DEC_WEIGHT + "," + Item.DEC_VPT + "," + Item.DEC_VALUE + ",";
                                        hearderConcat += Item.YEAR + " DEC_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                        dataConcat += Item.DEC_WEIGHT + "," + Item.DEC_VPT + "," + Item.DEC_VALUE + ",";
                                    }
                                }
                            }

                            if (Item.SIDE_OF_TRADE.Equals("Export"))
                            {
                                var iFind = ExportList.FindIndex(x => x.Key.Equals(SearchKey));

                                if (iFind >= 0)
                                {
                                    var foundModel = ExportList[iFind];
                                    foundModel.Header += hearderConcat;
                                    foundModel.Data += dataConcat;

                                    ExportList[iFind] = foundModel;
                                    hearderConcat = string.Empty;
                                    dataConcat = string.Empty;
                                }
                                else
                                {
                                    var exportModel = new CrossTabMultiMonthModel
                                    {
                                        Key = SearchKey,
                                        Data = outLine,
                                        Header = headerLine
                                    };

                                    ExportList.Add(exportModel);
                                }
                            }

                            if (Item.SIDE_OF_TRADE.Equals("Import"))
                            {
                                var iFind = ImportList.FindIndex(x => x.Key.Equals(SearchKey));

                                if (iFind >= 0)
                                {
                                    var foundModel = ImportList[iFind];
                                    foundModel.Header += hearderConcat;
                                    foundModel.Data += dataConcat;

                                    ImportList[iFind] = foundModel;
                                    hearderConcat = string.Empty;
                                    dataConcat = string.Empty;
                                }
                                else
                                {
                                    var importModel = new CrossTabMultiMonthModel
                                    {
                                        Key = SearchKey,
                                        Data = outLine,
                                        Header = headerLine
                                    };

                                    ImportList.Add(importModel);
                                }
                            }
                        }


                        //End
                    }
                    //END
                }

                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);

                if (ExportList.Count > 0)
                {
                    var sModel = ExportList.OrderByDescending(s => s.Header.Length).First();
                    var hItem = sModel.Header.Remove(sModel.Header.Length - 1);
                    tw.WriteLine(hItem);

                    foreach (var Item in ExportList)
                    {
                        var dataItem = Item.Data.Remove(Item.Data.Length - 1);

                        string[] dt = dataItem.Split(',');
                        var listOfNumber = new List<int>();
                        Array.Reverse(dt);

                        foreach (var dtItem in dt)
                        {
                            bool res = int.TryParse(dtItem, out int iNumber);
                            if (res)
                            {
                                listOfNumber.Add(iNumber);
                            }
                            else
                            {
                                break;
                            }
                        }
                        var sum = listOfNumber.Sum();

                        if (sum > 0)
                            tw.WriteLine(dataItem);
                    }
                }

                if (ImportList.Count > 0)
                {
                    int cnt = ImportList.Count;
                    ImportList.RemoveAt(cnt - 1);

                    if (ExportList.Count == 0)
                    {

                        var sModel = ImportList.OrderByDescending(s => s.Header.Length).First();
                        var hItem = sModel.Header.Remove(sModel.Header.Length - 1);
                        tw.WriteLine(hItem);
                    }
                    foreach (var Item in ImportList)
                    {
                        var dataItem = Item.Data.Remove(Item.Data.Length - 1);
                        string[] dt = dataItem.Split(',');
                        var listOfNumber = new List<int>();
                        Array.Reverse(dt);

                        foreach (var dtItem in dt)
                        {
                            bool res = int.TryParse(dtItem, out int iNumber);
                            if (res)
                            {
                                listOfNumber.Add(iNumber);
                            }
                            else
                            {
                                break;
                            }
                        }
                        var sum = listOfNumber.Sum();

                        if (sum > 0)
                            tw.WriteLine(dataItem);
                    }
                }

                tw.Flush();

                var FileName = model.FileName + ".csv";

                var length = ms.Length;
                tw.Close();
                var toWrite = new byte[length];
                Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                ms.Close();

                var fileModel = new FileReturnModel
                {
                     FileName = FileName,
                     ToWrite = toWrite
                };

                return fileModel;
              //  return File(toWrite, "text/plain", FileName);

            }

            var rfileModel = new FileReturnModel
            {
                FileName = null,
                ToWrite = null
            };

            return rfileModel;
        }
    }
}
