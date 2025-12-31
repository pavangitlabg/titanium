using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Data.Models;
using ISSB.Models;
using Services;

namespace ISSB.Helpers
{
    public class ReportWriterHelper
    {
        public int TableID = 1;
        public List<ReportWriterByMonthModel> _sortItems;

        public async Task<string> BuildHeader(ReportEngineModel keyData, List<string> FieldSort, ReportDesignerModel model)
        {
            var htmlStr = string.Empty;
            var keyService = new ReportKeysService();
            var KeysList = await keyService.GetReportKeys();
            var keyModel = new ReportKeysModel();

            htmlStr = model.TableStyle;
            htmlStr += "<thead><tr>";

            foreach (var Item in FieldSort)
            {
                if (Item.Equals("SC_GEO"))
                {
                    // if (keyData.search.SourceCountryGrouping == GroupingOption.Separately)
                    // {

                    keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("SC_NAME"))
                {
                    //if (keyData.search.SourceCountryGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("SC_NAME"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("SC_GEO_REGION"))
                {
                    //if (keyData.search.SourceCountryGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO_REGION"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("MC_GEO"))
                {
                    // if (keyData.search.MarketCountryGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("MC_GEO"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("MC_NAME"))
                {
                    //if (keyData.search.MarketCountryGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("MC_NAME"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("SIDE_OF_TRADE"))
                {
                    keyModel = KeysList.Find(c => c.Key.Equals("SIDE_OF_TRADE"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                }

                if (Item.Equals("TARIFF_CODE"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("TWO_DIGIT"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("TWO_DIGIT"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("SIX_DIGIT"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("SIX_DIGIT"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("TARIFF_CATEGORY"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CATEGORY"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("TARIFF_DESCRIPTION"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_DESCRIPTION"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("PORT"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("PORT"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                    //}
                }

                if (Item.Equals("PORT_NAME"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("PORT_NAME"));
                    htmlStr += "<th>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("YEAR"))
                {
                    //if (keyData.search.GroupByYear)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("YEAR"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";

                    // }
                }

                if (Item.Equals("QUARTER"))
                {
                    //if (keyData.search.GroupByQuarter)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("QUARTER"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("MONTH"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("MONTH"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("YTD_WEIGHT"))
                {
                    //if (keyData.search.GroupByMonth)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("YTD_WEIGHT"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    //}
                }

                if (Item.Equals("WEIGHT"))
                {
                    //if (keyData.search.GroupByMonth)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("WEIGHT"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    //}
                }

                if (Item.Equals("YTD_VALUE"))
                {
                    //if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("YTD_VALUE"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("VALUE"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("VALUE"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("VPT"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("VPT"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("YTD_VPT"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("YTD_VPT"));
                    htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    // }
                }

                if (Item.Equals("JAN_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("FEB_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("MAR_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("APR_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("MAY_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("JUN_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }


                if (Item.Equals("JUL_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("AUG_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("SEP_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("OCT_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("NOV_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("DEC_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("JAN_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("FEB_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("MAR_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("APR_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("MAY_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("JUN_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }


                if (Item.Equals("JUL_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("AUG_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("SEP_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("OCT_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("NOV_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("DEC_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("JAN_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("FEB_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("MAR_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("APR_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }


                if (Item.Equals("MAY_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("JUN_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("JUL_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("AUG_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("SEP_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("OCT_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("NOV_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("DEC_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("TOTAL_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_WEIGHT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("TOTAL_VALUE"))
                {
                    if (keyData.Search.Values)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VALUE"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

                if (Item.Equals("TOTAL_VPT"))
                {
                    if (keyData.Search.Values)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VPT"));
                        htmlStr += "<th style='width: 10px; text-align:right'>" + keyModel.Title + "</th>";
                    }
                }

            }
            htmlStr += "</tr></thead>";

            return htmlStr;
        }

        public async Task<string> BuildCSVHeader(ReportEngineModel keyData, List<string> FieldSort, ReportDesignerModel model, string seperator)
        {
            var htmlStr = string.Empty;
            var keyService = new ReportKeysService();
            var KeysList = await keyService.GetReportKeys();
            var keyModel = new ReportKeysModel();

            htmlStr = string.Empty;
            

            foreach (var Item in FieldSort)
            {
                if (Item.Equals("SC_GEO"))
                {
                    // if (keyData.search.SourceCountryGrouping == GroupingOption.Separately)
                    // {

                    keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("SC_NAME"))
                {
                    //if (keyData.search.SourceCountryGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("SC_NAME"));
                    htmlStr += keyModel.Title + seperator;

                    // }
                }

                if (Item.Equals("SC_GEO_REGION"))
                {
                    //if (keyData.search.SourceCountryGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO_REGION"));
                    htmlStr += keyModel.Title + seperator;

                    // }
                }

                if (Item.Equals("MC_GEO"))
                {
                    // if (keyData.search.MarketCountryGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("MC_GEO"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("MC_NAME"))
                {
                    //if (keyData.search.MarketCountryGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("MC_NAME"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("SIDE_OF_TRADE"))
                {
                    keyModel = KeysList.Find(c => c.Key.Equals("SIDE_OF_TRADE"));
                    htmlStr += keyModel.Title + seperator;
                }

                if (Item.Equals("TARIFF_CODE"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                    htmlStr += keyModel.Title + seperator;

                    // }
                }

                if (Item.Equals("TWO_DIGIT"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("TWO_DIGIT"));
                    htmlStr += keyModel.Title + seperator;

                    // }
                }

                if (Item.Equals("SIX_DIGIT"))
                {
                    //if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("SIX_DIGIT"));
                    htmlStr += keyModel.Title + seperator;

                    // }
                }

                if (Item.Equals("TARIFF_DESCRIPTION"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_DESCRIPTION"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("TARIFF_CATEGORY"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CATEGORY"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("PORT"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("PORT"));
                    htmlStr += keyModel.Title + seperator;
                    //}
                }

                if (Item.Equals("PORT_NAME"))
                {
                    // if (keyData.search.ProductGrouping == GroupingOption.Separately)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("PORT_NAME"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("YEAR"))
                {
                    //if (keyData.search.GroupByYear)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("YEAR"));
                    htmlStr += keyModel.Title + seperator;

                    // }
                }

                if (Item.Equals("QUARTER"))
                {
                    //if (keyData.search.GroupByQuarter)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("QUARTER"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("MONTH"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("MONTH"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("YTD_WEIGHT"))
                {
                    //if (keyData.search.GroupByMonth)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("YTD_WEIGHT"));
                    htmlStr += keyModel.Title + seperator;
                    //}
                }

                if (Item.Equals("WEIGHT"))
                {
                    //if (keyData.search.GroupByMonth)
                    //{
                    keyModel = KeysList.Find(c => c.Key.Equals("WEIGHT"));
                    htmlStr += keyModel.Title + seperator;
                    //}
                }

                if (Item.Equals("YTD_VALUE"))
                {
                    //if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("YTD_VALUE"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("VALUE"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("VALUE"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("VPT"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("VPT"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("YTD_VPT"))
                {
                    // if (keyData.search.GroupByMonth)
                    // {
                    keyModel = KeysList.Find(c => c.Key.Equals("YTD_VPT"));
                    htmlStr += keyModel.Title + seperator;
                    // }
                }

                if (Item.Equals("JAN_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("FEB_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("MAR_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("APR_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("MAY_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("JUN_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }


                if (Item.Equals("JUL_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("AUG_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("SEP_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("OCT_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("NOV_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("DEC_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("JAN_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("FEB_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("MAR_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("APR_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("MAY_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("JUN_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }


                if (Item.Equals("JUL_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("AUG_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("SEP_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("OCT_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("NOV_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("DEC_VALUE"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("JAN_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("FEB_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("MAR_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("APR_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }


                if (Item.Equals("MAY_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("JUN_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("JUL_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("AUG_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("SEP_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("OCT_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("NOV_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("DEC_VPT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("TOTAL_WEIGHT"))
                {
                    if (keyData.Search.GroupByMonth)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_WEIGHT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("TOTAL_VALUE"))
                {
                    if (keyData.Search.Values)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VALUE"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

                if (Item.Equals("TOTAL_VPT"))
                {
                    if (keyData.Search.Values)
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VPT"));
                        htmlStr += keyModel.Title + seperator;
                    }
                }

            }
           

            return htmlStr;
        }

        public string BuildGrandTotalsHeader(List<string> FieldSort, ReportDesignerModel model)
        {
            var htmlStr = string.Empty;
            htmlStr = model.TableStyle;
            htmlStr += "<thead><tr>";

            foreach (var Item in FieldSort)
            {
                if (Item.Equals("SC_GEO"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("SC_NAME"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("SC_GEO_REGION"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("MC_GEO"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("MC_NAME"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("SIDE_OF_TRADE"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("TARIFF_CODE"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("TWO_DIGIT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("SIX_DIGIT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("TARIFF_CATEGORY"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("TARIFF_DESCRIPTION"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("PORT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("PORT_NAME"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("YEAR"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("QUARTER"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("MONTH"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("YTD_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("YTD_VALUE"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("VALUE"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("JAN_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("FEB_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("MAR_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("APR_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("MAY_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("JUN_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }


                if (Item.Equals("JUL_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("AUG_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("SEP_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("OCT_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("NOV_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("DEC_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }


                if (Item.Equals("TOTAL_WEIGHT"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

                if (Item.Equals("TOTAL_VALUE"))
                {
                    htmlStr += "<th style='width: 10px; text-align:right'>";
                }

            }
            htmlStr += "</tr></thead>";

            return htmlStr;
        }

        public async Task<List<PivotModel>> PivotTableCSVBuilder(ReportEngineModel keyData, List<ReportWriterByMonthModel> sortItems, List<string> FieldSort, ReportDesignerModel model, string seperator)
        {
            IList<PropertyInfo> properties = typeof(PivotModel).GetProperties().ToList();
            var pivotModelList = new List<PivotModel>();
            int RecCount = 1;
            var keyService = new ReportKeysService();
            var KeysList = await keyService.GetReportKeys();
            var keyModel = new ReportKeysModel();
            var PageBreakList = new List<string>();
            var LastCompare = string.Empty;
            var FilterString = string.Empty;
            var fString = string.Empty;
            var Flow = string.Empty;
            var Year = string.Empty;

            foreach (var mItem in sortItems)
            {
                var pivotModel = new PivotModel();
                pivotModel.Id = RecCount;
                RecCount++;
                var IsFirst = false;
                Year = mItem.YEAR.ToString();
                Flow = mItem.SIDE_OF_TRADE;
                int cnt = 1;
                foreach (var f in FieldSort)
                {
                    fString = string.Empty;
                    if (f.Equals("SC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO"));
                        properties[cnt].SetValue(pivotModel, mItem.SC_GEO + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SC_GEO;
                    }

                    if (f.Equals("SC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_NAME"));
                        properties[cnt].SetValue(pivotModel, mItem.SC_NAME.Replace(seperator, " ") + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SC_NAME;
                    }

                    if (f.Equals("SC_GEO_REGION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO_REGION"));
                        properties[cnt].SetValue(pivotModel,  mItem.SC_GEO_REGION + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SC_GEO_REGION;
                    }

                    if (f.Equals("MC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_GEO"));
                        properties[cnt].SetValue(pivotModel,  mItem.MC_GEO + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.MC_GEO;
                    }

                    if (f.Equals("MC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_NAME"));
                        properties[cnt].SetValue(pivotModel, mItem.MC_NAME.Replace(seperator, " ") + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.MC_NAME;
                    }

                    if (f.Equals("TARIFF_CODE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        properties[cnt].SetValue(pivotModel,  mItem.TARIFF_CODE + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TARIFF_CODE;
                    }

                    if (f.Equals("TWO_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        properties[cnt].SetValue(pivotModel,  mItem.TWO_DIGIT + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TWO_DIGIT;
                    }

                    if (f.Equals("SIX_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        properties[cnt].SetValue(pivotModel,  mItem.SIX_DIGIT + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SIX_DIGIT;
                    }

                    if (f.Equals("TARIFF_CATEGORY"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CATEGORY"));
                        properties[cnt].SetValue(pivotModel, mItem.TARIFF_CATEGORY + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TARIFF_CATEGORY;
                    }

                    if (f.Equals("TARIFF_DESCRIPTION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_DESCRIPTION"));
                        properties[cnt].SetValue(pivotModel,  mItem.TARIFF_LEGEND.Replace(seperator," ") + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TARIFF_LEGEND;
                    }

                    if (f.Equals("SIDE_OF_TRADE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SIDE_OF_TRADE"));
                        properties[cnt].SetValue(pivotModel,  mItem.SIDE_OF_TRADE + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SIDE_OF_TRADE;

                    }

                    if (f.Equals("YEAR"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YEAR"));
                        properties[cnt].SetValue(pivotModel,  mItem.YEAR + seperator);
                        cnt++;
                        IsFirst = true;
                        fString = mItem.YEAR.ToString();

                    }

                    if (f.Equals("QUARTER"))
                    {
                        if (keyData.Search.GroupByQuarter)
                        {
                            keyModel = KeysList.Find(c => c.Key.Equals("QUARTER"));
                            properties[cnt].SetValue(pivotModel, mItem.QUARTER + seperator);
                            cnt++;
                            IsFirst = true;
                            fString = mItem.QUARTER.ToString();
                        }
                    }

                    //if (f.Equals("MONTH"))
                    //{
                    //    if (keyData.search.GroupByMonth)
                    //    {
                    //        keyModel = KeysList.Find(c => c.Key.Equals("MONTH"));
                    //        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + mItem. + "</td>");
                    //        cnt++;
                    //    }
                    //}

                    if (f.Equals("JAN_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_1 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("FEB_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, mItem.WEIGHT_2 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("MAR_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_3 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("APR_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_4 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("MAY_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_5 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("JUN_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_6 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("JUL_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_7 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("AUG_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_8 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("SEP_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_9 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("OCT_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_10 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("NOV_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_11 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("DEC_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.WEIGHT_12 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("JAN_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_1 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("FEB_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_2 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("MAR_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_3 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("APR_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_4 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("MAY_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_5 + seperator);
                        cnt++;
                        IsFirst = true;
                    }

                    if (f.Equals("JUN_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_6 + seperator);
                        cnt++;
                        IsFirst = true;

                    }

                    if (f.Equals("JUL_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_7 + seperator);
                        cnt++;

                    }

                    if (f.Equals("AUG_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VALUE"));
                        properties[cnt].SetValue(pivotModel,mItem.VALUE_8 + seperator);
                        cnt++;

                    }

                    if (f.Equals("SEP_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VALUE"));
                        properties[cnt].SetValue(pivotModel, mItem.VALUE_9 + seperator);
                        cnt++;

                    }

                    if (f.Equals("OCT_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_10 + seperator);
                        cnt++;

                    }

                    if (f.Equals("NOV_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_11 + seperator);
                        cnt++;

                    }

                    if (f.Equals("DEC_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.VALUE_12 + seperator);
                        cnt++;

                    }

                    if (f.Equals("JAN_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_1 + seperator);
                        cnt++;

                    }

                    if (f.Equals("FEB_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_2 + seperator);
                        cnt++;

                    }

                    if (f.Equals("MAR_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_3 + seperator);
                        cnt++;

                    }

                    if (f.Equals("APR_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_4 + seperator);
                        cnt++;

                    }

                    if (f.Equals("MAY_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_5 + seperator);
                        cnt++;

                    }

                    if (f.Equals("JUN_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_6 + seperator);
                        cnt++;

                    }

                    if (f.Equals("JUL_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_7 + seperator);
                        cnt++;

                    }

                    if (f.Equals("AUG_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_8 + seperator);
                        cnt++;

                    }

                    if (f.Equals("SEP_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_9 + seperator);
                        cnt++;

                    }

                    if (f.Equals("OCT_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_10 + seperator);
                        cnt++;

                    }

                    if (f.Equals("NOV_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VPT"));
                        properties[cnt].SetValue(pivotModel, mItem.VPT_11 + seperator);
                        cnt++;

                    }

                    if (f.Equals("DEC_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.VPT_12 + seperator);
                        cnt++;

                    }


                    if (f.Equals("TOTAL_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_WEIGHT"));
                        properties[cnt].SetValue(pivotModel,  mItem.TOTAL_WEIGHT + seperator);
                        cnt++;


                    }


                    if (f.Equals("TOTAL_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VALUE"));
                        properties[cnt].SetValue(pivotModel,  mItem.TOTAL_VALUE + seperator);
                        cnt++;


                    }

                    if (f.Equals("TOTAL_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VPT"));
                        properties[cnt].SetValue(pivotModel,  mItem.TOTAL_VPT + seperator);
                        cnt++;


                    }

                    

                }

                pivotModelList.Add(pivotModel);

            }
            return pivotModelList;
        }

        public async Task<List<PivotModel>> PivotTableBuilder(ReportEngineModel keyData, List<ReportWriterByMonthModel> sortItems, List<string> FieldSort, ReportDesignerModel model)
        {
            IList<PropertyInfo> properties = typeof(PivotModel).GetProperties().ToList();
            var pivotModelList = new List<PivotModel>();
            int RecCount = 1;
            var keyService = new ReportKeysService();
            var KeysList = await keyService.GetReportKeys();
            var keyModel = new ReportKeysModel();
            bool IsFirstRecord = true;
            var bPage = false;
            var PageBreakList = new List<string>();
            var LastCompare = string.Empty;
            var FilterString = string.Empty;
            var fString = string.Empty;
            var Flow = string.Empty;
            var Year = string.Empty;

            foreach (var mItem in sortItems)
            {
                var pivotModel = new PivotModel();
                pivotModel.Id = RecCount;
                RecCount++;
                var IsFirst = false;
                Year = mItem.YEAR.ToString();
                Flow = mItem.SIDE_OF_TRADE;
                int cnt = 1;
                foreach (var f in FieldSort)
                {
                    fString = string.Empty;
                    if (f.Equals("SC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.SC_GEO + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SC_GEO;
                    }

                    if (f.Equals("SC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_NAME"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.SC_NAME + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SC_NAME;
                    }

                    if (f.Equals("SC_GEO_REGION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO_REGION"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.SC_GEO_REGION + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SC_GEO_REGION;
                    }

                    if (f.Equals("MC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_GEO"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.MC_GEO + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.MC_GEO;
                    }

                    if (f.Equals("MC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_NAME"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.MC_NAME + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.MC_NAME;
                    }

                    if (f.Equals("PORT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("PORT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.PORT_ID + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.PORT_ID.ToString();
                    }

                    if (f.Equals("PORT_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("PORT_NAME"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.PORT_NAME + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.PORT_NAME;
                    }

                    if (f.Equals("TARIFF_CODE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.TARIFF_CODE + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TARIFF_CODE;
                    }

                    if (f.Equals("TWO_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.TWO_DIGIT + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TWO_DIGIT;
                    }

                    if (f.Equals("SIX_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.SIX_DIGIT + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SIX_DIGIT;
                    }

                    if (f.Equals("TARIFF_CATEGORY"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CATEGORY"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.TARIFF_CATEGORY + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TARIFF_CATEGORY;
                    }

                    if (f.Equals("TARIFF_DESCRIPTION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_DESCRIPTION"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.TARIFF_LEGEND + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.TARIFF_LEGEND;
                    }

                    if (f.Equals("SIDE_OF_TRADE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SIDE_OF_TRADE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "'>" + mItem.SIDE_OF_TRADE + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.SIDE_OF_TRADE;
                       
                    }

                    if (f.Equals("YEAR"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YEAR"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + mItem.YEAR + "</td>");
                        cnt++;
                        IsFirst = true;
                        fString = mItem.YEAR.ToString();
                        
                    }

                    if (f.Equals("QUARTER"))
                    {
                        if (keyData.Search.GroupByQuarter)
                        {
                            keyModel = KeysList.Find(c => c.Key.Equals("QUARTER"));
                            properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + mItem.QUARTER + "</td>");
                            cnt++;
                            IsFirst = true;
                            fString = mItem.QUARTER.ToString(); 
                        }
                    }

                    //if (f.Equals("MONTH"))
                    //{
                    //    if (keyData.search.GroupByMonth)
                    //    {
                    //        keyModel = KeysList.Find(c => c.Key.Equals("MONTH"));
                    //        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + mItem. + "</td>");
                    //        cnt++;
                    //    }
                    //}

                    if (f.Equals("JAN_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_1) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("FEB_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_2) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("MAR_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_3) + "</td>");
                        cnt++;
                        IsFirst = true;
                        
                    }

                    if (f.Equals("APR_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_4) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("MAY_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_5) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("JUN_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_6) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("JUL_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_7) + "</td>");
                        cnt++;
                        IsFirst = true;
                        
                    }

                    if (f.Equals("AUG_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_8) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("SEP_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_9) + "</td>");
                        cnt++;
                        IsFirst = true;
                        
                    }

                    if (f.Equals("OCT_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_10) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("NOV_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_11) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("DEC_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT_12) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("JAN_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_1) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("FEB_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_2) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("MAR_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_3) + "</td>");
                        cnt++;
                        IsFirst = true;
                       
                    }

                    if (f.Equals("APR_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_4) + "</td>");
                        cnt++;
                        IsFirst = true;
                      
                    }

                    if (f.Equals("MAY_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_5) + "</td>");
                        cnt++;
                        IsFirst = true;
                    }

                    if (f.Equals("JUN_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_6) + "</td>");
                        cnt++;
                        IsFirst = true;
                        
                    }

                    if (f.Equals("JUL_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_7) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("AUG_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_8) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("SEP_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_9) + "</td>");
                        cnt++;
                   
                    }

                    if (f.Equals("OCT_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_10) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("NOV_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_11) + "</td>");
                        cnt++;
                     
                    }

                    if (f.Equals("DEC_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VALUE_12) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("JAN_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JAN_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_1) + "</td>");
                        cnt++;
                     
                    }

                    if (f.Equals("FEB_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("FEB_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_2) + "</td>");
                        cnt++;
                       
                    }

                    if (f.Equals("MAR_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAR_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_3) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("APR_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("APR_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_4) + "</td>");
                        cnt++;
                       
                    }

                    if (f.Equals("MAY_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MAY_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_5) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("JUN_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUN_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_6) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("JUL_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("JUL_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_7) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("AUG_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("AUG_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_8) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("SEP_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SEP_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_9) + "</td>");
                        cnt++;
                    
                    }

                    if (f.Equals("OCT_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("OCT_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_10) + "</td>");
                        cnt++;
                   
                    }

                    if (f.Equals("NOV_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("NOV_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_11) + "</td>");
                        cnt++;
                      
                    }

                    if (f.Equals("DEC_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("DEC_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.VPT_12) + "</td>");
                        cnt++;
                     
                    }


                    if (f.Equals("TOTAL_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_WEIGHT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.TOTAL_WEIGHT) + "</td>");
                        cnt++;
                     
                       
                    }


                    if (f.Equals("TOTAL_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VALUE"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.TOTAL_VALUE) + "</td>");
                        cnt++;
                      
                       
                    }

                    if (f.Equals("TOTAL_VPT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TOTAL_VPT"));
                        properties[cnt].SetValue(pivotModel, "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.TOTAL_VPT) + "</td>");
                        cnt++;
                     
                       
                    }

                    if (IsFirstRecord)
                    {
                        LastCompare = fString;
                        IsFirstRecord = false;
                    }

                    if (IsFirst && cnt == 2)
                    {
                        if (!LastCompare.Equals(fString))
                        {
                            FilterString = LastCompare;
                            bPage = true;
                        }

                        LastCompare = fString;
                    }

                }

                if (!model.FileType.Equals("Excel"))
                {
                    if (bPage && model.PageBreak)
                    {
                        if (model.PageBreakType.Equals("After"))
                        {
                        
                            var pModel = new PivotModel();
                            pModel.PageBreak = "PAGE-AFTER";
                            pModel.Field1 = FilterString;
                            pModel.Field2 = LastCompare;
                            pModel.Year = Year;
                            pModel.Flow = Flow;
                            pivotModelList.Add(pModel);
                        }
                        else
                        {
     
                            var pModel = new PivotModel();
                            pModel.PageBreak = "PAGE-INNER";
                            pModel.Field1 = FilterString;
                            pModel.Field2 = LastCompare;
                            pModel.Year = Year;
                            pModel.Flow = Flow;
                            pivotModelList.Add(pModel);
                        }
                        bPage = false;
                    }
                }

                pivotModelList.Add(pivotModel);

            }
            return pivotModelList;
        }

        public async Task<List<PivotModel>> SourceGrouping(ReportEngineModel keyData, ReportOutModelJsonModel dataOut, List<string> FieldSort, ReportDesignerModel model, string seperator)
        {
            List<ReportWriterModel> mcItems = dataOut.data;
            var dataModel = new ReportWriterModel();

            var sortItems = new List<ReportWriterByMonthModel>();
           
            await Task.Run(() => sortItems = dataModel.GroupByType(mcItems));

            //sortItems = dataModel.GroupByType(mcItems);


            _sortItems = sortItems;
            var pivotModelList = new List<PivotModel>();

            if(model.FileType.Equals("TXT"))
            {
                pivotModelList = await PivotTableCSVBuilder(keyData, sortItems, FieldSort, model, seperator);
            }
            else
            {
                pivotModelList = await PivotTableBuilder(keyData, sortItems, FieldSort, model);
            }
            
            return pivotModelList;
        }

        public async Task<List<string>> TableCSVBuilder(ReportEngineModel keyData, ReportOutModelJsonModel dataOut, List<string> FieldSort, ReportDesignerModel model,string seperator)
        {
            var keyService = new ReportKeysService();
            var KeysList = await keyService.GetReportKeys();
            var keyModel = new ReportKeysModel();
            var PageBreakList = new List<string>();
            var LastCompare = string.Empty;
            var bPage = false;
            bool IsFirstRecord = true;
            var fString = string.Empty;
            var LineStr = string.Empty;
            var dataStrList = new List<string>();

            var sumDataList = new List<ReportWriterModel>();


            foreach (var mItem in dataOut.data)
            {

                var IsFirst = false;
                sumDataList.Add(mItem);

                LineStr = string.Empty;
                int cnt = 0;
                foreach (var f in FieldSort)
                {
                    fString = string.Empty;
                    if (f.Equals("SC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO"));
                        fString = mItem.SC_GEO + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_NAME"));
                        fString =  mItem.SC_NAME.Replace(seperator, " ") + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SC_GEO_REGION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO_REGION"));
                        fString =  mItem.SC_GEO_REGION + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("MC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_GEO"));
                        fString =  mItem.MC_GEO + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SIDE_OF_TRADE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SIDE_OF_TRADE"));
                        fString = mItem.SIDE_OF_TRADE + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("MC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_NAME"));
                        fString =  mItem.MC_NAME.Replace(seperator, " ") + seperator; 

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TARIFF_CODE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        fString =  mItem.TARIFF_CODE + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TWO_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        fString =  mItem.TWO_DIGIT + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SIX_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        fString = mItem.SIX_DIGIT + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TARIFF_CATEGORY"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CATEGORY"));
                        fString = mItem.TARIFF_CATEGORY + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TARIFF_DESCRIPTION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_DESCRIPTION"));
                        fString =  mItem.TARIFF_LEGEND.Replace(seperator," ") + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("YEAR"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YEAR"));
                        fString =  mItem.YEAR + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("QUARTER"))
                    {

                        keyModel = KeysList.Find(c => c.Key.Equals("QUARTER"));
                        fString =  mItem.QUARTER + seperator;

                        IsFirst = true;
                        cnt++;

                    }

                    if (f.Equals("MONTH"))
                    {

                        keyModel = KeysList.Find(c => c.Key.Equals("MONTH"));
                        fString =  mItem.MONTH + seperator;

                        IsFirst = true;
                        cnt++;

                    }

                    if (f.Equals("WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("WEIGHT"));
                        fString = mItem.WEIGHT + seperator;

                        cnt++;
                    }

                    if (f.Equals("YTD_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YTD_WEIGHT"));
                        fString =  mItem.YTD_WEIGHT + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("VPT"))
                    {
                        double dVPT = Math.Round(mItem.MONETARY_VALUE / mItem.WEIGHT, 0);
                        if (double.IsInfinity(dVPT))
                        {
                            dVPT = 0;
                        }

                        if (double.IsNaN(dVPT))
                        {
                            dVPT = 0;
                        }

                        keyModel = KeysList.Find(c => c.Key.Equals("VPT"));
                        fString =  dVPT + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("YTD_VPT"))
                    {
                        var dVPT = Math.Round(mItem.YTD_MONETARY_VALUE / (mItem.YTD_WEIGHT), 0);
                        if (double.IsInfinity(dVPT))
                        {
                            dVPT = 0;
                        }

                        if (double.IsNaN(dVPT))
                        {
                            dVPT = 0;
                        }

                        keyModel = KeysList.Find(c => c.Key.Equals("VPT"));
                        fString =  dVPT + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("YTD_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YTD_VALUE"));
                        fString = Math.Round(mItem.YTD_MONETARY_VALUE) + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("VALUE"));
                        fString = Math.Round(mItem.MONETARY_VALUE) + seperator;

                        IsFirst = true;
                        cnt++;
                    }

                    LineStr += fString;
                    
                }

                LineStr = LineStr.Remove(LineStr.Length - 1);
                dataStrList.Add(LineStr);
                LineStr = string.Empty;
            }

           

            return dataStrList;
        }

        public async Task<List<string>> TableBuilder(ReportEngineModel keyData, ReportOutModelJsonModel dataOut, List<string> FieldSort, ReportDesignerModel model)
        {
            var keyService = new ReportKeysService();
            var KeysList = await keyService.GetReportKeys();
            var keyModel = new ReportKeysModel();
            var PageBreakList = new List<string>();
            var LastCompare = string.Empty;
            var bPage = false;
            bool IsFirstRecord = true;
            var fString = string.Empty;
            var LineStr = string.Empty;
            var dataStr = string.Empty;
            var dataStrList = new List<string>();

            var sumDataList = new List<ReportWriterModel>();


            foreach (var mItem in dataOut.data)
            {
                
                var IsFirst = false;
                sumDataList.Add(mItem);

                LineStr = string.Empty;
                int cnt = 0;
                foreach (var f in FieldSort)
                {
                    fString = string.Empty;
                    if (f.Equals("SC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.SC_GEO + "</td>";
                    
                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_NAME"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.SC_NAME + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SC_GEO_REGION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SC_GEO_REGION"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.SC_GEO_REGION + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("MC_GEO"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_GEO"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.MC_GEO + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SIDE_OF_TRADE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("SIDE_OF_TRADE"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.SIDE_OF_TRADE + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("MC_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("MC_NAME"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.MC_NAME + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("PORT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("PORT"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.PORT_ID + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("PORT_NAME"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("PORT_NAME"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.PORT_NAME + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TARIFF_CODE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.TARIFF_CODE + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TWO_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.TWO_DIGIT + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("SIX_DIGIT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CODE"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.SIX_DIGIT + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TARIFF_CATEGORY"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_CATEGORY"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.TARIFF_CATEGORY + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("TARIFF_DESCRIPTION"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("TARIFF_DESCRIPTION"));
                        fString = "<td style='width:" + keyModel.Width + "' td>" + mItem.TARIFF_LEGEND + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("YEAR"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YEAR"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + mItem.YEAR + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("QUARTER"))
                    {

                        keyModel = KeysList.Find(c => c.Key.Equals("QUARTER"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + mItem.QUARTER + "</td>";

                        IsFirst = true;
                        cnt++;

                    }

                    if (f.Equals("MONTH"))
                    {

                        keyModel = KeysList.Find(c => c.Key.Equals("MONTH"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + mItem.MONTH + "</td>";

                        IsFirst = true;
                        cnt++;

                    }

                    if (f.Equals("WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("WEIGHT"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.WEIGHT) + "</td>";

                        cnt++;
                    }

                    if (f.Equals("YTD_WEIGHT"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YTD_WEIGHT"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.WeightFormat, mItem.YTD_WEIGHT) + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("VPT"))
                    {
                        double dVPT = Math.Round(mItem.MONETARY_VALUE / mItem.WEIGHT, 0);
                        if (double.IsInfinity(dVPT))
                        {
                            dVPT = 0;
                        }

                        if (double.IsNaN(dVPT))
                        {
                            dVPT = 0;
                        }

                        keyModel = KeysList.Find(c => c.Key.Equals("VPT"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, dVPT) + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("YTD_VPT"))
                    {
                        var dVPT = Math.Round(mItem.YTD_MONETARY_VALUE / (mItem.YTD_WEIGHT), 0);
                        if (double.IsInfinity(dVPT))
                        {
                            dVPT = 0;
                        }

                        if (double.IsNaN(dVPT))
                        {
                            dVPT = 0;
                        }

                        keyModel = KeysList.Find(c => c.Key.Equals("VPT"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, dVPT) + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("YTD_VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("YTD_VALUE"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.YTD_MONETARY_VALUE) + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    if (f.Equals("VALUE"))
                    {
                        keyModel = KeysList.Find(c => c.Key.Equals("VALUE"));
                        fString = "<td style='width:" + keyModel.Width + "' align='right'>" + string.Format(model.ValueFormat, mItem.MONETARY_VALUE) + "</td>";

                        IsFirst = true;
                        cnt++;
                    }

                    LineStr += fString;


                    if (IsFirstRecord)
                    {
                        LastCompare = fString;
                        IsFirstRecord = false;
                    }

                    if (IsFirst && cnt == 1)
                    {
                        if (!LastCompare.Equals(fString))
                        {
                            bPage = true;
                        }

                        LastCompare = fString;
                    }
                }

                if (!model.FileType.Equals("Excel"))
                {
                    if (bPage && model.PageBreak)
                    {
                        if (model.ShowTotals)
                        {
                            var lastItem = sumDataList[sumDataList.Count - 1];
                            sumDataList.RemoveAt(sumDataList.Count - 1);
                           // dataStr = "<tfoot class='table striped' style=\'font-size: " + model.FontSize + "'><tfoot>";
                           // dataStrList.Add(dataStr);
                            dataStr = SummaryBuilder(sumDataList, FieldSort, model);
                            dataStrList.Add(dataStr);
                            sumDataList.Clear();
                            sumDataList.Add(lastItem);
                           // dataStr = "</tfoot>";
                           // dataStrList.Add(dataStr);
                        }
                        dataStr = "</table>";
                        dataStrList.Add(dataStr);
                        if (model.PageBreakType.Equals("After"))
                        {
                            dataStr = "<div style='page-break-after: always; '></div>";
                            dataStrList.Add(dataStr);
                        }
                        else
                        {
                            dataStr = "<div style='page-break-inner: always; '></div>";
                            dataStrList.Add(dataStr);
                        }

                        if (model.TableHeader.Equals("<p><br></p>"))
                        {
                            var th = await BuildHeader(keyData, FieldSort, model);
                            int nID = TableID + 1;
                            th = th.Replace("id='1'", "id='" + nID + "'");
                            TableID++;
                            dataStr = th;
                            dataStrList.Add(dataStr);
                        }
                        else
                        {
                            dataStr = model.TableHeader;
                            dataStrList.Add(dataStr);
                        }

                       // dataStr += "<tbody>";
                       // dataStrList.Add(dataStr);

                        dataStr = "<tr>" + LineStr + "</tr>";
                        dataStrList.Add(dataStr);

                        bPage = false;
                    }
                    else
                    {
                        dataStr = "<tr>" + LineStr + "</tr>";
                        dataStrList.Add(dataStr);
                    }
                }
                else
                {
                    dataStr = "<tr>" + LineStr + "</tr>";
                    dataStrList.Add(dataStr);
                }

                //dataStrList.Add(dataStr);
                dataStr = string.Empty;
            }

            if (model.ShowTotals)
            {

               // dataStr = "<tfoot class='table striped' style=\'font-size: " + model.FontSize + "'><tfoot>";
               // dataStrList.Add(dataStr);

                var totStr = SummaryBuilder(sumDataList, FieldSort, model);
               // dataStr += totStr;
                dataStrList.Add(totStr);
                sumDataList.Clear();

                if (model.PageBreak)
                {
                    var summary = SummaryBuilder(dataOut.data, FieldSort, model);
                    dataStr += summary;
                    dataStrList.Add(dataStr);
                }

                //dataStr += "</tfoot></table>";
                //dataStr = "</tfoot></table>";
                //dataStrList.Add(dataStr);

                //if (model.TableHeader.Equals("<p><br></p>"))
                //{
                //    var th = BuildGrandTotalsHeader(FieldSort, model);
                //    int nID = TableID + 1;
                //    th = th.Replace("id='1'", "id='" + nID + "'");
                //    TableID++;
                //   // dataStr += th;
                //    dataStr = th;
                //    dataStrList.Add(dataStr);
                //}
                //else
                //{
                //    //dataStr += model.TableHeader;
                //    dataStr = model.TableHeader;
                //    dataStrList.Add(dataStr);
                //}


                //var summary = SummaryBuilder(dataOut.data, FieldSort, model);
                //dataStr += summary;

                //dataStr += "</tfoot></table>";
            }

            return dataStrList;
        }

        public string SummaryBuilder(List<ReportWriterModel> dataOut, List<string> FieldSort, ReportDesignerModel model)
        {
            var dataStr = string.Empty;
            double TotalLineWeight = 0;
            double TotalLineValue = 0;
            double TotalYTDWeight = 0;
            double TotalYTDValue = 0;
           

            foreach (var mItem in dataOut)
            {

                foreach (var f in FieldSort)
                {
                    if (f.Equals("VALUE"))
                    {
                        TotalLineValue += mItem.MONETARY_VALUE;
                    }



                    if (f.Equals("WEIGHT"))
                    {
                        TotalLineWeight += mItem.WEIGHT;
                    }


                    //if (f.Equals("VPT"))
                    // {

                    //    var dVPT = Math.Round(TotalLineValue / TotalLineWeight, 0);
                    //    TotalLineVPT += dVPT;
                    //}


                    if (f.Equals("YTD_WEIGHT"))
                    {
                        TotalYTDWeight += mItem.YTD_WEIGHT;

                    }

                    if (f.Equals("YTD_VALUE"))
                    {
                        TotalYTDValue += mItem.YTD_MONETARY_VALUE;
                    }
                }

            }

            dataStr += "<tr>";

            foreach (var f in FieldSort)
            {
                if (TotalLineWeight > 0)
                {
                    if (f.Equals("SC_GEO"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("SC_NAME"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("SC_GEO_REGION"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("MC_GEO"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("SIDE_OF_TRADE"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("MC_NAME"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("TARIFF_CODE"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("TWO_DIGIT"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("SIX_DIGIT"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("TARIFF_CATEGORY"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("TARIFF_DESCRIPTION"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("YEAR"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("QUARTER"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("MONTH"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>";
                    }

                    if (f.Equals("YTD_VALUE"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalYTDValue) + "</th>";
                    }

                    if (f.Equals("VALUE"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue) + "</th>";
                    }

                    if (f.Equals("YTD_WEIGHT"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalYTDWeight) + "</th>";
                    }


                    if (f.Equals("WEIGHT"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight) + "</th>";
                    }

                    if (f.Equals("VPT"))
                    {
                        dataStr += "<th style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue / TotalLineWeight, 0)) + "</th>";
                    }

                    if (f.Equals("YTD_VPT"))
                    {
                        double dVPT = Math.Round(TotalYTDValue / TotalYTDWeight, 0);
                        if (double.IsNaN(dVPT))
                            dVPT = 0;

                        dataStr += "<th style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, dVPT) + "</th>";
                    }
                }
            }

            dataStr += "</tr>";
            return dataStr;
        }

        public string SummaryPivotBuilder(List<string> FieldSort, ReportDesignerModel model, string filter, string year, string flow)
        {
            var dataStr = string.Empty;
           
            double TotalLineWeight1 = 0;
            double TotalLineWeight2 = 0;
            double TotalLineWeight3 = 0;
            double TotalLineWeight4 = 0;
            double TotalLineWeight5 = 0;
            double TotalLineWeight6 = 0;
            double TotalLineWeight7 = 0;
            double TotalLineWeight8 = 0;
            double TotalLineWeight9 = 0;
            double TotalLineWeight10 = 0;
            double TotalLineWeight11 = 0;
            double TotalLineWeight12 = 0;

            double TotalLineValue1 = 0;
            double TotalLineValue2 = 0;
            double TotalLineValue3 = 0;
            double TotalLineValue4 = 0;
            double TotalLineValue5 = 0;
            double TotalLineValue6 = 0;
            double TotalLineValue7 = 0;
            double TotalLineValue8 = 0;
            double TotalLineValue9 = 0;
            double TotalLineValue10 = 0;
            double TotalLineValue11 = 0;
            double TotalLineValue12 = 0;
            double TotalLineWeight = 0;
            double TotalLineValue = 0;

            //double TotalLineVPT = 0;

            var filterField = FieldSort[0];
            var FilterList = _sortItems;

            var TradeFlow = flow;
            

            if (!filter.Equals(string.Empty))
            {
               
                switch (filterField)
                {
                    
                    case "SC_GEO":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.SC_GEO.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.SC_GEO.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "SC_NAME":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.SC_NAME.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.SC_NAME.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    
                    case "MC_GEO":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.MC_GEO.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.MC_GEO.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                   
                    case "MC_NAME":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.MC_NAME.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.MC_NAME.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "TARIFF_CODE":
                        //if(filter.Equals("722490") && flow.Equals("Export"))
                        //{

                        //}
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.TARIFF_CODE.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.TARIFF_CODE.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "TARIFF_DESCRIPTION":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.TARIFF_LEGEND.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.TARIFF_LEGEND.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "PORT":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.PORT_ID.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.PORT_ID.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "PORT_NAME":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.PORT_NAME.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.PORT_NAME.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "SIDE_OF_TRADE":
                            FilterList = _sortItems.Where(x => x.SIDE_OF_TRADE.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                          break;
                    case "YEAR":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    
                    case "QUARTER":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.QUARTER.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.QUARTER.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "SC_GEO_REGION":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.SC_GEO_REGION.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.SC_GEO_REGION.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    case "SIX_DIGIT":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.SIX_DIGIT.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.SIX_DIGIT.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    //case "TARIFF_CATEGORY":
                    //    if (!string.IsNullOrEmpty(flow))
                    //        FilterList = _sortItems.Where(x => x.TARIFF_CATEGORY.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                    //    else
                    //        FilterList = _sortItems.Where(x => x.SIX_DIGIT.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                    //    break;
                    case "TWO_DIGIT":
                        if (!string.IsNullOrEmpty(flow))
                            FilterList = _sortItems.Where(x => x.TWO_DIGIT.Equals(filter) && x.SIDE_OF_TRADE.Equals(TradeFlow) && x.YEAR.Equals(int.Parse(year))).ToList();
                        else
                            FilterList = _sortItems.Where(x => x.TWO_DIGIT.Equals(filter) && x.YEAR.Equals(int.Parse(year))).ToList();
                        break;
                    default:
                        FilterList = _sortItems;
                        break;
                }
            }
           
            

            foreach (var mItem in FilterList)
            {
                foreach (var f in FieldSort)
                {
                    if (f.Equals("JAN_WEIGHT"))
                    {
                        TotalLineWeight1 += mItem.WEIGHT_1;
                    }

                    if (f.Equals("FEB_WEIGHT"))
                    {
                        TotalLineWeight2 += mItem.WEIGHT_2;
                    }

                    if (f.Equals("MAR_WEIGHT"))
                    {
                        TotalLineWeight3 += mItem.WEIGHT_3;
                    }

                    if (f.Equals("APR_WEIGHT"))
                    {
                        TotalLineWeight4 += mItem.WEIGHT_4;
                    }

                    if (f.Equals("MAY_WEIGHT"))
                    {
                        TotalLineWeight5 += mItem.WEIGHT_5;
                    }

                    if (f.Equals("JUN_WEIGHT"))
                    {
                        TotalLineWeight6 += mItem.WEIGHT_6;
                    }

                    if (f.Equals("JUL_WEIGHT"))
                    {
                        TotalLineWeight7 += mItem.WEIGHT_7;
                    }

                    if (f.Equals("AUG_WEIGHT"))
                    {
                        TotalLineWeight8 += mItem.WEIGHT_8;
                    }

                    if (f.Equals("SEP_WEIGHT"))
                    {
                        TotalLineWeight9 += mItem.WEIGHT_9;
                    }

                    if (f.Equals("OCT_WEIGHT"))
                    {
                        TotalLineWeight10 += mItem.WEIGHT_10;
                    }

                    if (f.Equals("NOV_WEIGHT"))
                    {
                        TotalLineWeight11 += mItem.WEIGHT_11;
                    }

                    if (f.Equals("DEC_WEIGHT"))
                    {
                        TotalLineWeight12 += mItem.WEIGHT_12;
                    }

                    if (f.Equals("JAN_VALUE"))
                    {
                        TotalLineValue1 += mItem.VALUE_1;
                    }

                    if (!f.Equals("JAN_VALUE") || f.Equals("JAN_VPT"))
                    {
                        TotalLineValue1 += mItem.VALUE_1;
                    }

                    if (f.Equals("FEB_VALUE"))
                    {
                        TotalLineValue2 += mItem.VALUE_2;
                    }

                    if (!f.Equals("FEB_VALUE") || f.Equals("FEB_VPT"))
                    {
                        TotalLineValue2 += mItem.VALUE_2;
                    }

                    if (f.Equals("MAR_VALUE"))
                    {
                        TotalLineValue3 += mItem.VALUE_3;
                    }

                    if (!f.Equals("MAR_VALUE") || f.Equals("MAR_VPT"))
                    {
                        TotalLineValue3 += mItem.VALUE_3;
                    }

                    if (f.Equals("APR_VALUE"))
                    {
                         TotalLineValue4 += mItem.VALUE_4;
                    }

                    if (!f.Equals("APR_VALUE") || f.Equals("APR_VPT"))
                    {
                        TotalLineValue4 += mItem.VALUE_4;
                    }

                    if (f.Equals("MAY_VALUE"))
                    {
                        TotalLineValue5 += mItem.VALUE_5;
                    }

                    if (!f.Equals("MAY_VALUE") || f.Equals("MAY_VPT"))
                    {
                        TotalLineValue5 += mItem.VALUE_5;
                    }

                    if (f.Equals("JUN_VALUE"))
                    {
                        TotalLineValue6 += mItem.VALUE_6;
                    }

                    if (!f.Equals("JUN_VALUE") || f.Equals("JUN_VPT"))
                    {
                        TotalLineValue6 += mItem.VALUE_6;
                    }

                    if (f.Equals("JUL_VALUE"))
                    {
                        TotalLineValue7 += mItem.VALUE_7;
                    }

                    if (!f.Equals("JUL_VALUE") || f.Equals("JUL_VPT"))
                    {
                        TotalLineValue7 += mItem.VALUE_7;
                    }

                    if (f.Equals("AUG_VALUE"))
                    {
                        TotalLineValue8 += mItem.VALUE_8; 
                    }

                    if (!f.Equals("AUG_VALUE") || f.Equals("AUG_VPT"))
                    {
                        TotalLineValue8 += mItem.VALUE_8;
                    }

                    if (f.Equals("SEP_VALUE"))
                    {
                        TotalLineValue9 += mItem.VALUE_9;
                    }

                    if (!f.Equals("SEP_VALUE") || f.Equals("SEP_VPT"))
                    {
                        TotalLineValue9 += mItem.VALUE_9;
                    }

                    if (f.Equals("OCT_VALUE"))
                    {
                        TotalLineValue10 += mItem.VALUE_10;
                    }

                    if (!f.Equals("OCT_VALUE") || f.Equals("OCT_VPT"))
                    {
                        TotalLineValue10 += mItem.VALUE_10;
                    }

                    if (f.Equals("NOV_VALUE"))
                    {
                        TotalLineValue11 += mItem.VALUE_11; 
                    }

                    if (!f.Equals("NOV_VALUE") || f.Equals("NOV_VPT"))
                    {
                        TotalLineValue11 += mItem.VALUE_11;
                    }

                    if (f.Equals("DEC_VALUE"))
                    {
                        TotalLineValue12 += mItem.VALUE_12;
                    }

                    if (!f.Equals("DEC_VALUE") || f.Equals("DEC_VPT"))
                    {
                        TotalLineValue12 += mItem.VALUE_12;
                    }

                    if (f.Equals("TOTAL_WEIGHT"))
                    {
                        TotalLineWeight += mItem.TOTAL_WEIGHT;
                    }

                    if (f.Equals("TOTAL_VALUE"))
                    {
                        TotalLineValue += mItem.TOTAL_VALUE;
                    }

                    if (f.Equals("TOTAL_VPT") || !f.Equals("TOTAL_VALUE"))
                    {
                        TotalLineValue += mItem.TOTAL_VALUE;
                    }
                }
                //double dVPT = Math.Round(TotalLineValue / TotalLineWeight, 0);

                //if (double.IsNaN(dVPT))
                //    TotalLineVPT = 0;
                //else
                //    TotalLineVPT = dVPT;

            }
            dataStr += "<tr>";

            foreach (var f in FieldSort)
            {
                if (f.Equals("SC_GEO"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("SC_NAME"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("SC_GEO_REGION"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("MC_GEO"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("SIDE_OF_TRADE"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("MC_NAME"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("TARIFF_CODE"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("TWO_DIGIT"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("SIX_DIGIT"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("TARIFF_CATEGORY"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("TARIFF_DESCRIPTION"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("YEAR"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("QUARTER"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("MONTH"))
                {
                    dataStr += "<td align='right'></td>";
                }

                if (f.Equals("JAN_WEIGHT"))
                {
                   //CJ
                      dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight1) + "</td>";
                }

                if (f.Equals("FEB_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight2) + "</td>";
                }

                if (f.Equals("MAR_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight3) + "</td>";
                }

                if (f.Equals("APR_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight4) + "</td>";
                }

                if (f.Equals("MAY_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight5) + "</td>";
                }

                if (f.Equals("JUN_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight6) + "</td>";
                }

                if (f.Equals("JUL_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight7) + "</td>";
                }

                if (f.Equals("AUG_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight8) + "</td>";
                }

                if (f.Equals("SEP_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight9) + "</td>";
                }

                if (f.Equals("OCT_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight10) + "</td>";
                }

                if (f.Equals("NOV_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight11) + "</td>";
                }

                if (f.Equals("DEC_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat, TotalLineWeight12) + "</td>";
                }

                if (f.Equals("JAN_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue1) + "</td>";
                }

                if (f.Equals("FEB_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue2) + "</td>";
                }

                if (f.Equals("MAR_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue3) + "</td>";
                }

                if (f.Equals("APR_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue4) + "</td>";
                }

                if (f.Equals("MAY_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue5) + "</td>";
                }

                if (f.Equals("JUN_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue6) + "</td>";
                }

                if (f.Equals("JUL_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue7) + "</td>";
                }

                if (f.Equals("AUG_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue8) + "</td>";
                }

                if (f.Equals("SEP_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue9) + "</td>";
                }

                if (f.Equals("OCT_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue10) + "</td>";
                }

                if (f.Equals("NOV_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue11) + "</td>";
                }

                if (f.Equals("DEC_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue12) + "</td>";
                }

                if (f.Equals("JAN_VPT"))
                {
                   
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue1 / TotalLineWeight1, 0)) + "</td>";
                }

                if (f.Equals("FEB_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue2 / TotalLineWeight2, 0)) + "</td>";
                }

                if (f.Equals("MAR_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue3 / TotalLineWeight3, 0)) + "</td>";
                }

                if (f.Equals("APR_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue4 / TotalLineWeight4, 0)) + "</td>";
                }

                if (f.Equals("MAY_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue5 / TotalLineWeight5, 0)) + "</td>";
                }

                if (f.Equals("JUN_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue6 / TotalLineWeight6, 0)) + "</td>";
                }

                if (f.Equals("JUL_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue7 / TotalLineWeight7, 0)) + "</td>";
                }

                if (f.Equals("AUG_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue8 / TotalLineWeight8, 0)) + "</td>";
                }

                if (f.Equals("SEP_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue9 / TotalLineWeight9, 0)) + "</td>";
                }

                if (f.Equals("OCT_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue10 / TotalLineWeight10, 0)) + "</td>";
                }

                if (f.Equals("NOV_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue11 / TotalLineWeight11, 0)) + "</td>";
                }

                if (f.Equals("DEC_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue12 / TotalLineWeight12, 0)) + "</td>";
                }

                if (f.Equals("TOTAL_WEIGHT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.WeightFormat,  TotalLineWeight) + "</td>";
                }

                if (f.Equals("TOTAL_VALUE"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, TotalLineValue) + "</td>";
                }

                if (f.Equals("TOTAL_VPT"))
                {
                    dataStr += "<td style='width: 10px; text-align:right'>" + string.Format(model.ValueFormat, Math.Round(TotalLineValue / TotalLineWeight, 0)) + "</td>";
                }
            }

            dataStr += "</tr>";
            return dataStr;
        }

       
    }
}
