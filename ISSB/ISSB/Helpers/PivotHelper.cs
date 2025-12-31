using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using ISSB.Models;

namespace ISSB.Helpers
{
    public class PivotHelper
    {
        private static readonly Lazy<PivotHelper> lazy = new Lazy<PivotHelper>(() => new PivotHelper());
        public static PivotHelper Instance { get => lazy.Value; }

        public List<ReportWriterByMonthOutModel> GroupByMonth(List<string> fieldList, List<ReportPivotMonthModel> reportByWeight, List<ReportPivotMonthModel> reportByValue, ReportEngineModel keyData)
        {
            var outList = new List<ReportWriterByMonthOutModel>();
            int Id = 0;

            foreach (var Item in reportByWeight)
            {
                string[] words = Item.KEY.Split('|');

                var monthModel = new ReportWriterByMonthOutModel();
                var cnt = 0;
                monthModel.KEY = Item.KEY;

                foreach (var fItem in fieldList)
                {
                    if (fItem.Equals("SC_GEO"))
                    {
                        monthModel.SC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_NAME"))
                    {
                        monthModel.SC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_GEO_REGION"))
                    {
                        monthModel.SC_GEO_REGION = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_GEO"))
                    {
                        monthModel.MC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_NAME"))
                    { 
                        monthModel.MC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SIDE_OF_TRADE"))
                    {
                        monthModel.SIDE_OF_TRADE = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("PORT"))
                    {
                        monthModel.PORT_ID = int.Parse(words[cnt]);
                        cnt++;

                    }

                    if (fItem.Equals("PORT_NAME"))
                    {
                        monthModel.PORT_NAME = words[cnt].Replace(",", " ");
                        cnt++;

                    }
                    if (fItem.Equals("TARIFF_CODE"))
                    {
                        monthModel.TARIFF_CODE = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_DESCRIPTION"))
                    {
                        monthModel.TARIFF_LEGEND = words[cnt].Replace(",", " ");
                        cnt++;

                    }

                    if (fItem.Equals("TWO_DIGIT"))
                    {
                        monthModel.TWO_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("SIX_DIGIT"))
                    {
                        monthModel.SIX_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_CATEGORY"))
                    {
                        monthModel.TARIFF_CATEGORY = words[cnt];
                        cnt++;

                    }
                }

                if (keyData.Search.GroupByMonth)
                {
                    try
                    {
                       // if(words.Length <= cnt)
                            monthModel.YEAR = int.Parse(words[cnt]);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                double number;

                if (double.TryParse(Item.JAN, out number))
                    monthModel.JAN_WEIGHT = number;
                if (double.TryParse(Item.FEB, out number))
                    monthModel.FEB_WEIGHT = number;
                if (double.TryParse(Item.MAR, out number))
                    monthModel.MAR_WEIGHT = number;
                if (double.TryParse(Item.APR, out number))
                    monthModel.APR_WEIGHT = number;
                if (double.TryParse(Item.MAY, out number))
                    monthModel.MAY_WEIGHT = number;
                if (double.TryParse(Item.JUN, out number))
                    monthModel.JUN_WEIGHT = number;
                if (double.TryParse(Item.JLY, out number))
                    monthModel.JLY_WEIGHT = number;
                if (double.TryParse(Item.AUG, out number))
                    monthModel.AUG_WEIGHT = number;
                if (double.TryParse(Item.SEP, out number))
                    monthModel.SEP_WEIGHT = number;
                if (double.TryParse(Item.OCT, out number))
                    monthModel.OCT_WEIGHT = number;
                if (double.TryParse(Item.NOV, out number))
                    monthModel.NOV_WEIGHT = number;
                if (double.TryParse(Item.DEC, out number))
                    monthModel.DEC_WEIGHT = number;

                monthModel.ID = Id;
                Id++;
                outList.Add(monthModel);
            };


            if (keyData.Search.Values)
            {
                foreach (var Item in reportByValue)
                {
                    string[] words = Item.KEY.Split('|');

                    var monthModel = outList.Find(x => x.KEY.Equals(Item.KEY));

                    if (monthModel == null)
                    {
                        var cnt = 0;

                        foreach (var fItem in fieldList)
                        {
                            if (fItem.Equals("SC_GEO"))
                            {
                                monthModel.SC_GEO = words[cnt];
                                cnt++;
                                monthModel.SC_NAME = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("SC_GEO_REGION"))
                            {
                                monthModel.SC_GEO_REGION = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("MC_GEO"))
                            {
                                monthModel.MC_GEO = words[cnt];
                                cnt++;
                                monthModel.MC_NAME = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("PORT"))
                            {
                                monthModel.PORT_ID = int.Parse(words[cnt]);
                                cnt++;

                            }

                            if (fItem.Equals("PORT_NAME"))
                            {
                                monthModel.PORT_NAME = words[cnt];
                                cnt++;

                            }

                            if (fItem.Equals("SIDE_OF_TRADE"))
                            {
                                monthModel.SIDE_OF_TRADE = words[cnt];
                                cnt++;

                            }
                        }

                        if (keyData.Search.GroupByMonth)
                            monthModel.YEAR = int.Parse(words[cnt]);
                    }

                    double number;
                    if (double.TryParse(Item.JAN, out number))
                    {
                        monthModel.JAN_VALUE = Math.Round(number,0);
                        monthModel.JAN_VPT = Math.Round(monthModel.JAN_VALUE / monthModel.JAN_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.FEB, out number))
                    {
                        monthModel.FEB_VALUE = Math.Round(number, 0);
                        monthModel.FEB_VPT = Math.Round(monthModel.FEB_VALUE / monthModel.FEB_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.MAR, out number))
                    {
                        monthModel.MAR_VALUE = Math.Round(number, 0);
                        monthModel.MAR_VPT = Math.Round(monthModel.MAR_VALUE / monthModel.MAR_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.APR, out number))
                    {
                        monthModel.APR_VALUE = Math.Round(number, 0);
                        monthModel.APR_VPT = Math.Round(monthModel.APR_VALUE / monthModel.APR_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.MAY, out number))
                    {
                        monthModel.MAY_VALUE = Math.Round(number, 0); 
                        monthModel.MAY_VPT = Math.Round(monthModel.MAY_VALUE / monthModel.MAY_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.JUN, out number))
                    {
                        monthModel.JUN_VALUE = Math.Round(number, 0);
                        monthModel.JUN_VPT = Math.Round(monthModel.JUN_VALUE / monthModel.JUN_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.JLY, out number))
                    {
                        monthModel.JLY_VALUE = Math.Round(number, 0);
                        monthModel.JLY_VPT = Math.Round(monthModel.JLY_VALUE / monthModel.JLY_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.AUG, out number))
                    {
                        monthModel.AUG_VALUE = Math.Round(number, 0);
                        monthModel.AUG_VPT = Math.Round(monthModel.AUG_VALUE / monthModel.AUG_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.SEP, out number))
                    {
                        monthModel.SEP_VALUE = Math.Round(number, 0);
                        monthModel.SEP_VPT = Math.Round(monthModel.SEP_VALUE / monthModel.SEP_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.OCT, out number))
                    {
                        monthModel.OCT_VALUE = Math.Round(number, 0);
                        monthModel.OCT_VPT = Math.Round(monthModel.OCT_VALUE / monthModel.OCT_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.NOV, out number))
                    {
                        monthModel.NOV_VALUE = Math.Round(number, 0);
                        monthModel.NOV_VPT = Math.Round(monthModel.NOV_VALUE / monthModel.NOV_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.DEC, out number))
                    {
                        monthModel.DEC_VALUE = Math.Round(number, 0);
                        monthModel.DEC_VPT = Math.Round(monthModel.DEC_VALUE / monthModel.DEC_WEIGHT, 0);
                    }


                    // monthModel.ID = Id;
                    // Id++;
                    // outList = outList.OrderBy(x => x.YEAR).OrderBy(x => x.SIDE_OF_TRADE).ToList();
                };
            }
            return outList;
        }

        public List<ReportWriterByFlowOutModel> GroupByFlow(List<string> fieldList, List<ReportPivotFlowModel> reportByWeight, List<ReportPivotFlowModel> reportByValue, ReportEngineModel keyData)
        {
            var outList = new List<ReportWriterByFlowOutModel>();
            int Id = 0;


            foreach (var Item in reportByWeight)
            {
                string[] words = Item.KEY.Split('|');

                var flowModel = new ReportWriterByFlowOutModel();
                var cnt = 0;

                for (int i = 0; i < words.Length-1; i++)
                    flowModel.KEY += words[i] + "|";
               

                foreach (var fItem in fieldList)
                {
                    if (fItem.Equals("SC_GEO"))
                    {
                        flowModel.SC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_NAME"))
                    {
                        flowModel.SC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_GEO_REGION"))
                    {
                        flowModel.SC_GEO_REGION = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_GEO"))
                    {
                        flowModel.MC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_NAME"))
                    {
                        flowModel.MC_NAME = words[cnt];
                        cnt++;
                    }

                    //if (fItem.Equals("SIDE_OF_TRADE"))
                    //{
                    //    flowModel.SIDE_OF_TRADE = words[cnt];
                    //    cnt++;

                    //}

                    if (fItem.Equals("PORT"))
                    {
                        flowModel.PORT_ID = int.Parse(words[cnt]);
                        cnt++;

                    }

                    if (fItem.Equals("PORT_NAME"))
                    {
                        flowModel.PORT_NAME = words[cnt].Replace(",", " ");
                        cnt++;

                    }
                    if (fItem.Equals("TARIFF_CODE"))
                    {
                        flowModel.TARIFF_CODE = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_DESCRIPTION"))
                    {
                        flowModel.TARIFF_LEGEND = words[cnt].Replace(",", " ");
                        cnt++;

                    }

                    if (fItem.Equals("TWO_DIGIT"))
                    {
                        flowModel.TWO_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("SIX_DIGIT"))
                    {
                        flowModel.SIX_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_CATEGORY"))
                    {
                        flowModel.TARIFF_CATEGORY = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("YEAR"))
                    {
                        flowModel.YEAR = int.Parse(words[cnt]);
                        cnt++;

                    }

                    if (fItem.Equals("MONTH"))
                    {
                        flowModel.MONTH = int.Parse(words[cnt]);
                        cnt++;

                    }
                }

                //if (keyData.Search.GroupByQuarter)
                //    flowModel.YEAR = int.Parse(words[cnt]);

                double number;


                if (outList.Count == 0)
                {
                    if (double.TryParse(Item.Export, out number))
                        flowModel.EXPORT_WEIGHT = number;

                    if (double.TryParse(Item.Import, out number))
                        flowModel.IMPORT_WEIGHT = number;

                    flowModel.ID = Id;
                    Id++;
                    outList.Add(flowModel);
                }
                else
                {
                    var mModel = outList.Find(x => x.KEY.Equals(flowModel.KEY));
                    if(mModel == null)
                    {
                        if (double.TryParse(Item.Export, out number))
                            flowModel.EXPORT_WEIGHT = number;

                        if (double.TryParse(Item.Import, out number))
                            flowModel.IMPORT_WEIGHT = number;

                        flowModel.ID = Id;
                        Id++;
                        outList.Add(flowModel);
                    }
                    else
                    {
                        if (double.TryParse(Item.Export, out number))
                        {
                            if(mModel.EXPORT_WEIGHT == 0)
                                mModel.EXPORT_WEIGHT = number;
                        }

                        if (double.TryParse(Item.Import, out number))
                        {
                            if (mModel.IMPORT_WEIGHT == 0)
                                mModel.IMPORT_WEIGHT = number;
                        }
                    
                    }
                }
                
            };


            if (keyData.Search.Values)
            {
                foreach (var Item in reportByValue)
                {
                    string[] words = Item.KEY.Split('|');

                    var ModelKey = string.Empty;
                    for (int i = 0; i < words.Length - 1; i++)
                        ModelKey += words[i] + "|";

                    var flowModel = outList.Find(x => x.KEY.Equals(ModelKey));

                    //if (flowModel == null)
                    //{
                    //    var cnt = 0;

                    //    foreach (var fItem in fieldList)
                    //    {
                    //        if (fItem.Equals("SC_GEO"))
                    //        {
                    //            flowModel.SC_GEO = words[cnt];
                    //            cnt++;
                    //        }

                    //        if (fItem.Equals("SC_NAME"))
                    //        {
                    //            flowModel.SC_NAME = words[cnt];
                    //            cnt++;
                    //        }

                    //        if (fItem.Equals("SC_GEO_REGION"))
                    //        {
                    //            flowModel.SC_GEO_REGION = words[cnt];
                    //            cnt++;
                    //        }

                    //        if (fItem.Equals("MC_GEO"))
                    //        {
                    //            flowModel.MC_GEO = words[cnt];
                    //            cnt++;
                    //        }

                    //        if (fItem.Equals("MC_NAME"))
                    //        {
                    //            flowModel.MC_NAME = words[cnt];
                    //            cnt++;
                    //        }

                    //        //if (fItem.Equals("SIDE_OF_TRADE"))
                    //        //{
                    //        //    flowModel.SIDE_OF_TRADE = words[cnt];
                    //        //    cnt++;

                    //        //}

                    //    }

                        //  if (keyData.Search.GroupByMonth)
                      //  flowModel.YEAR = int.Parse(words[cnt]);
                   // }

                    double number;
                    if (double.TryParse(Item.Export, out number))
                    {
                        if (number != 0)
                        {
                            flowModel.EXPORT_VALUE = Math.Round(number, 0);
                            flowModel.EXPORT_VPT = Math.Round(flowModel.EXPORT_VALUE / flowModel.EXPORT_WEIGHT, 0);
                        }
                    }

                    if (double.TryParse(Item.Import, out number))
                    {
                        if (number != 0)
                        {
                            flowModel.IMPORT_VALUE = Math.Round(number, 0);
                            flowModel.IMPORT_VPT = Math.Round(flowModel.IMPORT_VALUE / flowModel.IMPORT_WEIGHT, 0);
                        }
                    }

                    // monthModel.ID = Id;
                    // Id++;
                    // outList = outList.OrderBy(x => x.YEAR).OrderBy(x => x.SIDE_OF_TRADE).ToList();
                };
            }
            return outList;
        }

        public List<ReportWriterByQuarterOutModel> GroupByQuarter(List<string> fieldList, List<ReportPivotQuarterModel> reportByWeight, List<ReportPivotQuarterModel> reportByValue, ReportEngineModel keyData)
        {
            var outList = new List<ReportWriterByQuarterOutModel>();
            int Id = 0;


            foreach (var Item in reportByWeight)
            {
                string[] words = Item.KEY.Split('|');

                var quarterModel = new ReportWriterByQuarterOutModel();
                var cnt = 0;
                quarterModel.KEY = Item.KEY;

                foreach (var fItem in fieldList)
                {
                    if (fItem.Equals("SC_GEO"))
                    {
                        quarterModel.SC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_NAME"))
                    {
                        quarterModel.SC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_GEO_REGION"))
                    {
                        quarterModel.SC_GEO_REGION = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_GEO"))
                    {
                        quarterModel.MC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_NAME"))
                    {
                        quarterModel.MC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SIDE_OF_TRADE"))
                    {
                        quarterModel.SIDE_OF_TRADE = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("PORT"))
                    {
                        quarterModel.PORT_ID = int.Parse(words[cnt]);
                        cnt++;

                    }

                    if (fItem.Equals("PORT_NAME"))
                    {
                        quarterModel.PORT_NAME = words[cnt].Replace(",", " ");
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_CODE"))
                    {
                        quarterModel.TARIFF_CODE = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_DESCRIPTION"))
                    {
                        quarterModel.TARIFF_LEGEND = words[cnt].Replace(",", " ");
                        cnt++;

                    }

                    if (fItem.Equals("TWO_DIGIT"))
                    {
                        quarterModel.TWO_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("SIX_DIGIT"))
                    {
                        quarterModel.SIX_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_CATEGORY"))
                    {
                        quarterModel.TARIFF_CATEGORY = words[cnt];
                        cnt++;

                    }
                }

                if (keyData.Search.GroupByQuarter)
                    quarterModel.YEAR = int.Parse(words[cnt]);

                double number;

                if (double.TryParse(Item.Q1, out number))
                    quarterModel.Q1_WEIGHT = number;
                if (double.TryParse(Item.Q2, out number))
                    quarterModel.Q2_WEIGHT = number;
                if (double.TryParse(Item.Q3, out number))
                    quarterModel.Q3_WEIGHT = number;
                if (double.TryParse(Item.Q4, out number))
                    quarterModel.Q4_WEIGHT = number;

                quarterModel.ID = Id;
                Id++;
                outList.Add(quarterModel);
            };


            if (keyData.Search.Values)
            {
                foreach (var Item in reportByValue)
                {
                    string[] words = Item.KEY.Split('|');

                    var quarterModel = outList.Find(x => x.KEY.Equals(Item.KEY));

                    if (quarterModel == null)
                    {
                        var cnt = 0;

                        foreach (var fItem in fieldList)
                        {
                            if (fItem.Equals("SC_GEO"))
                            {
                                quarterModel.SC_GEO = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("SC_NAME"))
                            {
                                quarterModel.SC_NAME = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("SC_GEO_REGION"))
                            {
                                quarterModel.SC_GEO_REGION = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("MC_GEO"))
                            {
                                quarterModel.MC_GEO = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("MC_NAME"))
                            {
                                quarterModel.MC_NAME = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("SIDE_OF_TRADE"))
                            {
                                quarterModel.SIDE_OF_TRADE = words[cnt];
                                cnt++;

                            }

                        }

                        if (keyData.Search.GroupByMonth)
                            quarterModel.YEAR = int.Parse(words[cnt]);
                    }

                    double number;
                    if (double.TryParse(Item.Q1, out number))
                    {
                        quarterModel.Q1_VALUE = number;
                        quarterModel.Q1_VPT = Math.Round(quarterModel.Q1_VALUE / quarterModel.Q1_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Q2, out number))
                    {
                        quarterModel.Q2_VALUE = number;
                        quarterModel.Q2_VPT = Math.Round(quarterModel.Q2_VALUE / quarterModel.Q2_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Q3, out number))
                    {
                        quarterModel.Q3_VALUE = number;
                        quarterModel.Q3_VPT = Math.Round(quarterModel.Q3_VALUE / quarterModel.Q3_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Q4, out number))
                    {
                        quarterModel.Q4_VALUE = number;
                        quarterModel.Q4_VPT = Math.Round(quarterModel.Q4_VALUE / quarterModel.Q4_WEIGHT, 0);
                    }


                    // monthModel.ID = Id;
                    // Id++;
                    // outList = outList.OrderBy(x => x.YEAR).OrderBy(x => x.SIDE_OF_TRADE).ToList();
                };
            }
            return outList;
        }

        public List<ReportWriterByYearOutModel> GroupByYear(List<string> fieldList, List<ReportPivotYearModel> reportByWeight, List<ReportPivotYearModel> reportByValue, ReportEngineModel keyData)
        {
            var outList = new List<ReportWriterByYearOutModel>();
            int Id = 0;


            foreach (var Item in reportByWeight)
            {
                string[] words = Item.KEY.Split('|');

                var yearModel = new ReportWriterByYearOutModel();
                var cnt = 0;
                yearModel.KEY = Item.KEY;

                foreach (var fItem in fieldList)
                {
                    if (fItem.Equals("SC_GEO"))
                    {
                        yearModel.SC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_NAME"))
                    {
                        yearModel.SC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SC_GEO_REGION"))
                    {
                        yearModel.SC_GEO_REGION = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_GEO"))
                    {
                        yearModel.MC_GEO = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("MC_NAME"))
                    {
                        yearModel.MC_NAME = words[cnt];
                        cnt++;
                    }

                    if (fItem.Equals("SIDE_OF_TRADE"))
                    {
                        yearModel.SIDE_OF_TRADE = words[cnt];
                        cnt++;

                    }
                    if (fItem.Equals("PORT"))
                    {
                        yearModel.PORT_ID = int.Parse(words[cnt]);
                        cnt++;

                    }

                    if (fItem.Equals("PORT_NAME"))
                    {
                        yearModel.PORT_NAME = words[cnt].Replace(",", " ");
                        cnt++;

                    }
                    if (fItem.Equals("TARIFF_CODE"))
                    {
                        yearModel.TARIFF_CODE = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_DESCRIPTION"))
                    {
                        yearModel.TARIFF_LEGEND = words[cnt].Replace(",", " "); ;
                        cnt++;

                    }

                    if (fItem.Equals("TWO_DIGIT"))
                    {
                        yearModel.TWO_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("SIX_DIGIT"))
                    {
                        yearModel.SIX_DIGIT = words[cnt];
                        cnt++;

                    }

                    if (fItem.Equals("TARIFF_CATEGORY"))
                    {
                        yearModel.TARIFF_CATEGORY = words[cnt];
                        cnt++;

                    }

                }

                // if (keyData.Search.GroupByQuarter)
                //    yearModel.YEAR = int.Parse(words[cnt]);
                double number;

                if (double.TryParse(Item.Y1990, out number))
                    yearModel.Y1990_WEIGHT = number;
                if (double.TryParse(Item.Y1991, out number))
                    yearModel.Y1991_WEIGHT = number;
                if (double.TryParse(Item.Y1992, out number))
                    yearModel.Y1992_WEIGHT = number;
                if (double.TryParse(Item.Y1993, out number))
                    yearModel.Y1993_WEIGHT = number;
                if (double.TryParse(Item.Y1994, out number))
                    yearModel.Y1994_WEIGHT = number;
                if (double.TryParse(Item.Y1995, out number))
                    yearModel.Y1995_WEIGHT = number;
                if (double.TryParse(Item.Y1996, out number))
                    yearModel.Y1996_WEIGHT = number;
                if (double.TryParse(Item.Y1997, out number))
                    yearModel.Y1997_WEIGHT = number;
                if (double.TryParse(Item.Y1998, out number))
                    yearModel.Y1998_WEIGHT = number;
                if (double.TryParse(Item.Y1999, out number))
                    yearModel.Y1999_WEIGHT = number;

                if (double.TryParse(Item.Y2000, out number))
                    yearModel.Y2000_WEIGHT = number;
                if (double.TryParse(Item.Y2001, out number))
                    yearModel.Y2001_WEIGHT = number;
                if (double.TryParse(Item.Y2002, out number))
                    yearModel.Y2002_WEIGHT = number;
                if (double.TryParse(Item.Y2003, out number))
                    yearModel.Y2003_WEIGHT = number;
                if (double.TryParse(Item.Y2004, out number))
                    yearModel.Y2004_WEIGHT = number;
                if (double.TryParse(Item.Y2005, out number))
                    yearModel.Y2005_WEIGHT = number;
                if (double.TryParse(Item.Y2006, out number))
                    yearModel.Y2006_WEIGHT = number;
                if (double.TryParse(Item.Y2007, out number))
                    yearModel.Y2007_WEIGHT = number;
                if (double.TryParse(Item.Y2008, out number))
                    yearModel.Y2008_WEIGHT = number;
                if (double.TryParse(Item.Y2009, out number))
                    yearModel.Y2009_WEIGHT = number;

                if (double.TryParse(Item.Y2010, out number))
                    yearModel.Y2010_WEIGHT = number;
                if (double.TryParse(Item.Y2011, out number))
                    yearModel.Y2011_WEIGHT = number;
                if (double.TryParse(Item.Y2012, out number))
                    yearModel.Y2012_WEIGHT = number;
                if (double.TryParse(Item.Y2013, out number))
                    yearModel.Y2013_WEIGHT = number;
                if (double.TryParse(Item.Y2014, out number))
                    yearModel.Y2014_WEIGHT = number;
                if (double.TryParse(Item.Y2015, out number))
                    yearModel.Y2015_WEIGHT = number;
                if (double.TryParse(Item.Y2016, out number))
                    yearModel.Y2016_WEIGHT = number;
                if (double.TryParse(Item.Y2017, out number))
                    yearModel.Y2017_WEIGHT = number;
                if (double.TryParse(Item.Y2018, out number))
                    yearModel.Y2018_WEIGHT = number;
                if (double.TryParse(Item.Y2019, out number))
                    yearModel.Y2019_WEIGHT = number;

                if (double.TryParse(Item.Y2020, out number))
                    yearModel.Y2020_WEIGHT = number;
                if (double.TryParse(Item.Y2021, out number))
                    yearModel.Y2021_WEIGHT = number;
                if (double.TryParse(Item.Y2022, out number))
                    yearModel.Y2022_WEIGHT = number;
                if (double.TryParse(Item.Y2023, out number))
                    yearModel.Y2023_WEIGHT = number;
                if (double.TryParse(Item.Y2024, out number))
                    yearModel.Y2024_WEIGHT = number;
                if (double.TryParse(Item.Y2025, out number))
                    yearModel.Y2025_WEIGHT = number;
                if (double.TryParse(Item.Y2026, out number))
                    yearModel.Y2026_WEIGHT = number;
                if (double.TryParse(Item.Y2027, out number))
                    yearModel.Y2027_WEIGHT = number;
                if (double.TryParse(Item.Y2028, out number))
                    yearModel.Y2028_WEIGHT = number;
                if (double.TryParse(Item.Y2029, out number))
                    yearModel.Y2029_WEIGHT = number;
                if (double.TryParse(Item.Y2030, out number))
                    yearModel.Y2030_WEIGHT = number;

                yearModel.ID = Id;
                Id++;
                outList.Add(yearModel);
            };


            if (keyData.Search.Values)
            {
                foreach (var Item in reportByValue)
                {
                    string[] words = Item.KEY.Split('|');

                    var yearModel = outList.Find(x => x.KEY.Equals(Item.KEY));

                    if (yearModel == null)
                    {
                        var cnt = 0;

                        foreach (var fItem in fieldList)
                        {
                            if (fItem.Equals("SC_GEO"))
                            {
                                yearModel.SC_GEO = words[cnt];
                                cnt++;
                                yearModel.SC_NAME = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("MC_GEO"))
                            {
                                yearModel.MC_GEO = words[cnt];
                                cnt++;
                                yearModel.MC_NAME = words[cnt];
                                cnt++;
                            }

                            if (fItem.Equals("SIDE_OF_TRADE"))
                            {
                                yearModel.SIDE_OF_TRADE = words[cnt];
                                cnt++;

                            }
                        }

                        if (keyData.Search.GroupByMonth)
                            yearModel.YEAR = int.Parse(words[cnt]);
                    }


                    double number;

                    if (double.TryParse(Item.Y1990, out number))
                    {
                        yearModel.Y1990_VALUE = number;
                        yearModel.Y1990_VPT = Math.Round(yearModel.Y1990_VALUE / yearModel.Y1990_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1991, out number))
                    {
                        yearModel.Y1991_VALUE = number;
                        yearModel.Y1991_VPT = Math.Round(yearModel.Y1991_VALUE / yearModel.Y1991_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1992, out number))
                    {
                        yearModel.Y1992_VALUE = number;
                        yearModel.Y1992_VPT = Math.Round(yearModel.Y1992_VALUE / yearModel.Y1992_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1993, out number))
                    {
                        yearModel.Y1993_VALUE = number;
                        yearModel.Y1993_VPT = Math.Round(yearModel.Y1993_VALUE / yearModel.Y1993_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1994, out number))
                    {
                        yearModel.Y1994_VALUE = number;
                        yearModel.Y1994_VPT = Math.Round(yearModel.Y1994_VALUE / yearModel.Y1994_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1995, out number))
                    {
                        yearModel.Y1995_VALUE = number;
                        yearModel.Y1995_VPT = Math.Round(yearModel.Y1995_VALUE / yearModel.Y1995_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1996, out number))
                    {
                        yearModel.Y1996_VALUE = number;
                        yearModel.Y1996_VPT = Math.Round(yearModel.Y1996_VALUE / yearModel.Y1996_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1997, out number))
                    {
                        yearModel.Y1997_VALUE = number;
                        yearModel.Y1997_VPT = Math.Round(yearModel.Y1997_VALUE / yearModel.Y1997_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1998, out number))
                    {
                        yearModel.Y1998_VALUE = number;
                        yearModel.Y1998_VPT = Math.Round(yearModel.Y1998_VALUE / yearModel.Y1998_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y1999, out number))
                    {
                        yearModel.Y1999_VALUE = number;
                        yearModel.Y1999_VPT = Math.Round(yearModel.Y1999_VALUE / yearModel.Y1999_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2000, out number))
                    {
                        yearModel.Y2000_VALUE = number;
                        yearModel.Y2000_VPT = Math.Round(yearModel.Y2000_VALUE / yearModel.Y2000_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2001, out number))
                    {
                        yearModel.Y2001_VALUE = number;
                        yearModel.Y2001_VPT = Math.Round(yearModel.Y2001_VALUE / yearModel.Y2001_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2002, out number))
                    {
                        yearModel.Y2002_VALUE = number;
                        yearModel.Y2002_VPT = Math.Round(yearModel.Y2002_VALUE / yearModel.Y2002_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2003, out number))
                    {
                        yearModel.Y2003_VALUE = number;
                        yearModel.Y2003_VPT = Math.Round(yearModel.Y2003_VALUE / yearModel.Y2003_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2004, out number))
                    {
                        yearModel.Y2004_VALUE = number;
                        yearModel.Y2004_VPT = Math.Round(yearModel.Y2004_VALUE / yearModel.Y2004_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2005, out number))
                    {
                        yearModel.Y2005_VALUE = number;
                        yearModel.Y2005_VPT = Math.Round(yearModel.Y2005_VALUE / yearModel.Y2005_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2006, out number))
                    {
                        yearModel.Y2006_VALUE = number;
                        yearModel.Y2006_VPT = Math.Round(yearModel.Y2006_VALUE / yearModel.Y2006_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2007, out number))
                    {
                        yearModel.Y2007_VALUE = number;
                        yearModel.Y2007_VPT = Math.Round(yearModel.Y2007_VALUE / yearModel.Y2007_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2008, out number))
                    {
                        yearModel.Y2008_VALUE = number;
                        yearModel.Y2008_VPT = Math.Round(yearModel.Y2008_VALUE / yearModel.Y2008_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2009, out number))
                    {
                        yearModel.Y2009_VALUE = number;
                        yearModel.Y2009_VPT = Math.Round(yearModel.Y2009_VALUE / yearModel.Y2009_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2010, out number))
                    {
                        yearModel.Y2010_VALUE = number;
                        yearModel.Y2010_VPT = Math.Round(yearModel.Y2010_VALUE / yearModel.Y2010_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2011, out number))
                    {
                        yearModel.Y2011_VALUE = number;
                        yearModel.Y2011_VPT = Math.Round(yearModel.Y2011_VALUE / yearModel.Y2011_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2012, out number))
                    {
                        yearModel.Y2012_VALUE = number;
                        yearModel.Y2012_VPT = Math.Round(yearModel.Y2012_VALUE / yearModel.Y2012_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2013, out number))
                    {
                        yearModel.Y2013_VALUE = number;
                        yearModel.Y2013_VPT = Math.Round(yearModel.Y2013_VALUE / yearModel.Y2013_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2014, out number))
                    {
                        yearModel.Y2014_VALUE = number;
                        yearModel.Y2014_VPT = Math.Round(yearModel.Y2014_VALUE / yearModel.Y2014_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2015, out number))
                    {
                        yearModel.Y2015_VALUE = number;
                        yearModel.Y2015_VPT = Math.Round(yearModel.Y2015_VALUE / yearModel.Y2015_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2016, out number))
                    {
                        yearModel.Y2016_VALUE = number;
                        yearModel.Y2016_VPT = Math.Round(yearModel.Y2016_VALUE / yearModel.Y2016_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2017, out number))
                    {
                        yearModel.Y2017_VALUE = number;
                        yearModel.Y2017_VPT = Math.Round(yearModel.Y2017_VALUE / yearModel.Y2017_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2018, out number))
                    {
                        yearModel.Y2018_VALUE = number;
                        yearModel.Y2018_VPT = Math.Round(yearModel.Y2018_VALUE / yearModel.Y2018_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2019, out number))
                    {
                        yearModel.Y2019_VALUE = number;
                        yearModel.Y2019_VPT = Math.Round(yearModel.Y2019_VALUE / yearModel.Y2019_WEIGHT,0);
                    }
                    if (double.TryParse(Item.Y2020, out number))
                    {
                        yearModel.Y2020_VALUE = number;
                        yearModel.Y2020_VPT = Math.Round(yearModel.Y2020_VALUE / yearModel.Y2020_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2021, out number))
                    {
                        yearModel.Y2021_VALUE = number;
                        yearModel.Y2021_VPT = Math.Round(yearModel.Y2021_VALUE / yearModel.Y2021_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2022, out number))
                    {
                        yearModel.Y2022_VALUE = number;
                        yearModel.Y2022_VPT = Math.Round(yearModel.Y2022_VALUE / yearModel.Y2022_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2023, out number))
                    {
                        yearModel.Y2023_VALUE = number;
                        yearModel.Y2023_VPT = Math.Round(yearModel.Y2023_VALUE / yearModel.Y2023_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2024, out number))
                    {
                        yearModel.Y2024_VALUE = number;
                        yearModel.Y2024_VPT = Math.Round(yearModel.Y2024_VALUE / yearModel.Y2024_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2025, out number))
                    {
                        yearModel.Y2025_VALUE = number;
                        yearModel.Y2025_VPT = Math.Round(yearModel.Y2025_VALUE / yearModel.Y2025_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2026, out number))
                    {
                        yearModel.Y2026_VALUE = number;
                        yearModel.Y2026_VPT = Math.Round(yearModel.Y2026_VALUE / yearModel.Y2026_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2027, out number))
                    {
                        yearModel.Y2027_VALUE = number;
                        yearModel.Y2027_VPT = Math.Round(yearModel.Y2027_VALUE / yearModel.Y2027_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2028, out number))
                    {
                        yearModel.Y2028_VALUE = number;
                        yearModel.Y2028_VPT = Math.Round(yearModel.Y2028_VALUE / yearModel.Y2028_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2029, out number))
                    {
                        yearModel.Y2029_VALUE = number;
                        yearModel.Y2029_VPT = Math.Round(yearModel.Y2029_VALUE / yearModel.Y2029_WEIGHT, 0);
                    }
                    if (double.TryParse(Item.Y2030, out number))
                    {
                        yearModel.Y2030_VALUE = number;
                        yearModel.Y2030_VPT = Math.Round(yearModel.Y2030_VALUE / yearModel.Y2030_WEIGHT, 0);
                    }



                    // monthModel.ID = Id;
                    // Id++;
                    // outList = outList.OrderBy(x => x.YEAR).OrderBy(x => x.SIDE_OF_TRADE).ToList();
                };
            }
            return outList;
        }
    }
}
