using Data.Models;
using Infragistics.Web.Mvc;
using Newtonsoft.Json;
using Services;

namespace API.Models;

/// <summary>reporting engine model</summary>
public class ReportEngineModel
{
    /// <summary>report data</summary>
    public IQueryable<ReportOutModel> Data;

    /// <summary>search criteria</summary>
    public SearchModel Search;

    public string SelectedCurrency { get; set; }

    /// <summary>width of the grid</summary>

    public string GridWidth { get; set; }

    /// <summary>serialised report data</summary>

    public string Serialised => JsonConvert.SerializeObject(Data);

    /// <summary>which groupable selection is not grouped</summary>
    /// <remarks>
    ///     possible values are as follows:
    ///     0. none
    ///     1. source countries
    ///     2. market countries
    ///     4. ports
    ///     8. products
    ///     16. by month
    ///     32. by quarter
    ///     64. by year
    ///     -1. more than one
    /// </remarks>

    public int Separately => GetSeparately();

    /// <summary>which groupable selections are not grouped.</summary>
    /// <remarks>
    ///     possible values are an or'd combination of the following:
    ///     1. source countries
    ///     2. market countries
    ///     4. ports
    ///     8. products
    ///     16. by month
    ///     32. by quarter
    ///     64. by year
    /// </remarks>

    public int AllSeparately => GetAllSeparately();

    /// <summary>label member path</summary>

    public string LabelMemberPath => GetLabelMemberPath();

    /// <summary>Define grid columns.</summary>
    /// <param name="column">column builder</param>
    public GridColumnBuilder<ReportOutModel> Columns(GridColumnBuilder<ReportOutModel> column)
    {
        var iWidth = 0;
        column.For(r => r.ID).HeaderText("").Width("5%");

        // source country

        if (Search.SourceCountryGrouping == GroupingOption.Separately)
        {
            column.For(r => r.SC_GEO).HeaderText("Source Geo").Width("10%");
            iWidth += 10;
            column.For(r => r.SC_NAME).HeaderText("Source Country").Width("15%");
            iWidth += 15;
        }

        // side of trade

        column.For(x => x.SIDE_OF_TRADE).HeaderText("Side Of Trade").Width("10%");
        iWidth += 10;

        // date

        if (Search.GroupByYear)
        {
            column.For(r => r.YEAR).HeaderText("Year").Width("10%");
            iWidth += 10;
        }

        if (Search.GroupByQuarter)
        {
            column.For(r => r.QUARTER).HeaderText("Quarter").Width("7%");
            iWidth += 7;
        }

        if (Search.GroupByMonth)
        {
            column.For(r => r.MONTH).HeaderText("Month").Width("7%");
            iWidth += 7;
        }

        // product

        if (Search.ProductGrouping == GroupingOption.Separately)
        {
            column.For(r => r.TARIFF_CODE).HeaderText("Tariff").Width("10%");
            iWidth += 10;
            column.For(r => r.TARIFF_LEGEND).HeaderText("Tariff Description").Width("30%");
            iWidth += 30;
        }

        // market country

        if (Search.MarketCountryGrouping == GroupingOption.Separately)
        {
            column.For(r => r.MC_GEO).HeaderText("Market Geo").Width("10%");
            iWidth += 10;
            column.For(r => r.MC_NAME).HeaderText("Market Country").Width("15%");
            iWidth += 15;
        }

        // port

        if (Search.PortGrouping == GroupingOption.Separately)
        {
            column.For(r => r.PORT_ID).HeaderText("Port").Width("10%");
            iWidth += 10;
            column.For(r => r.PORT_NAME).HeaderText("Port Name").Width("20%");
            iWidth += 20;
        }

        // weight and value

        column.For(x => x.WEIGHT).HeaderText("Tonnes")
            .Width("12%")
            .Format("#,##0")
            .HeaderCssClass("numberStyle")
            .ColumnCssClass("numberStyle");
        iWidth += 12;

        if (Search.Values)
        {
            column.For(x => x.MONETARY_VALUE).HeaderText(SelectedCurrency)
                .Width("12%")
                .Format("#,##0")
                .HeaderCssClass("numberStyle")
                .ColumnCssClass("numberStyle");
            iWidth += 12;
        }
        //By Request the YTD cols are removed
        //column.For(x => x.YTD_WEIGHT).HeaderText("Tonnes: YTD")
        //                               .Width("12%")
        //                               .Format("#,##0")
        //                               .HeaderCssClass("numberStyle")
        //                               .ColumnCssClass("numberStyle");
        //iWidth += 12;

        if (Search.Values)
        {
            //column.For(x => x.YTD_MONETARY_VALUE).HeaderText(SelectedCurrency + " : YTD")
            //                                       .Width("12%")
            //                                       .Format("#,##0")
            //                                       .HeaderCssClass("numberStyle")
            //                                       .ColumnCssClass("numberStyle");
            //iWidth += 12;
        }

        if (iWidth <= 100)
            iWidth = 100;

        GridWidth = iWidth + "%";

        return column;
    }

    internal object Columns()
    {
        throw new NotImplementedException();
    }

    #region utilities for use internally

    /// <summary>Return which groupable selection is not grouped.</summary>
    /// <remarks>
    ///     Return values are as follows:
    ///     0. none
    ///     1. source countries
    ///     2. market countries
    ///     4. ports
    ///     8. products
    ///     16. by month
    ///     32. by quarter
    ///     64. by year
    ///     -1. more than one
    /// </remarks>
    /// <returns>which groupable selection is not grouped</returns>
    private int GetSeparately()
    {
        var separately = 0; // selection viewed separately

        if (Search.SourceCountryGrouping == GroupingOption.Separately) separately = 1;
        if (Search.MarketCountryGrouping == GroupingOption.Separately) separately = separately == 0 ? 2 : -1;
        if (Search.IncludePorts && Search.PortGrouping == GroupingOption.Separately)
            separately = separately == 0 ? 4 : -1;
        if (Search.ProductGrouping == GroupingOption.Separately) separately = separately == 0 ? 8 : -1;
        if (Search.GroupByMonth) separately = separately == 0 ? 16 : -1;
        if (Search.GroupByQuarter && !Search.GroupByMonth) separately = separately == 0 ? 32 : -1;
        if (Search.GroupByYear && !Search.GroupByQuarter && !Search.GroupByMonth)
            separately = separately == 0 ? 64 : -1;

        return separately;
    }

    /// <summary>Return which groupable selections are not grouped.</summary>
    /// <remarks>
    ///     Return values are an or'd combination of the following:
    ///     1. source countries
    ///     2. market countries
    ///     4. ports
    ///     8. products
    ///     16. by month
    ///     32. by quarter
    ///     64. by year
    /// </remarks>
    /// <returns>which groupable selections are not grouped</returns>
    private int GetAllSeparately()
    {
        var separately = 0; // selection viewed separately

        if (Search.SourceCountryGrouping == GroupingOption.Separately) separately = 1;
        if (Search.MarketCountryGrouping == GroupingOption.Separately) separately += 2;
        if (Search.IncludePorts && Search.PortGrouping == GroupingOption.Separately) separately += 4;
        if (Search.ProductGrouping == GroupingOption.Separately) separately += 8;
        if (Search.GroupByMonth) separately += 16;
        if (Search.GroupByQuarter && !Search.GroupByMonth) separately += 32;
        if (Search.GroupByYear && !Search.GroupByQuarter && !Search.GroupByMonth) separately += 64;

        return separately;
    }

    /// <summary>Return label member path.</summary>
    /// <returns>label member path</returns>
    private string GetLabelMemberPath()
    {
        var separately = GetSeparately(); // selection viewed separately

        switch (separately)
        {
            case 1: return "SC_NAME"; // source countries viewed separately
            case 2: return "MC_NAME"; // market countries viewed separately
            case 4: return "PORT_NAME"; // ports viewed separately
            case 8: return "TARIFF_LEGEND"; // products viewed separately
            case 16: return "DateMonth"; // by month
            case 32: return "DateQuarter"; // by quarter
            case 64: return "YEAR"; // by year

            default: return string.Empty; // not exactly one selection viewed separately
        }
    }

    #endregion

    #region chart utilities

    /// <summary>Set data chart axes.</summary>
    /// <param name="chart">chart to set axes in</param>
    public void SetAxes(DataChart<ReportOutModel> chart)
    {
        var title = Search.Values ? "Value" : "Tonnes"; // y-axis title

        switch (Separately)
        {
            case 1:
                // source countries viewed separately

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.SC_NAME);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;

            case 2:
                // market countries viewed separately

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.MC_NAME);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;

            case 4:
                // ports viewed separately

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.PORT_NAME);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;

            case 8:
                // products viewed separately

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.TARIFF_LEGEND);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;

            case 16:
                // by month

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.DateMonth);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;

            case 32:
                // by quarter

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.DateQuarter);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;

            case 64:
                // by year

                chart.Axes(
                    axes =>
                    {
                        axes.CategoryX("xAxis").Label(item => item.YEAR);
                        axes.NumericY("yAxis").MinimumValue(0).Title(title);
                    }
                );
                break;
        }
    }

    /// <summary>Set pie chart member paths.</summary>
    /// <param name="chart">chart to set paths in</param>
    /// <param name="member">value menber path (monetary value or weight)</param>
    public void SetPath(PieChart<ReportOutModel> chart, string member)
    {
        chart.ValueMemberPath(member);
        chart.LabelMemberPath(LabelMemberPath);
    }

    /// <summary>Set funnel chart member paths.</summary>
    /// <param name="chart">chart to set paths in</param>
    /// <param name="member">value member path (monetary value or weight)</param>
    public void SetPath(FunnelChart<ReportEngineModel> chart, string member)
    {
        chart.ValueMemberPath(member);
        chart.InnerLabelMemberPath(member);
        chart.OuterLabelMemberPath(LabelMemberPath);
    }

    /// <summary>Set doughnut chart series</summary>
    /// <param name="chart">chart to set series in</param>
    public void SetSeries(DoughnutChart<object> chart)
    {
        chart.Series(s => { s.Ring("defaultSeries", Data.AsQueryable()); });

        /*
            This code creates the new series and defines the value and label member paths in a single statement:

                chart = search.Values
                            ? chart.Series( s => { s.Ring( "defaultSeries" , data.AsQueryable() ).ValueMemberPath( o => o.MONETARY_VALUE ).LabelMemberPath( o => o.SC_NAME ) ; } )
                            : chart.Series( s => { s.Ring( "defaultSeries" , data.AsQueryable() ).ValueMemberPath( o => o.WEIGHT ).LabelMemberPath( o => o.SC_NAME ) ; } ) ;
        */

        chart.Model.Series[0].ValueMemberPath = Search.Values ? "MONETARY_VALUE" : "WEIGHT";
        chart.Model.Series[0].LabelMemberPath = LabelMemberPath;
    }

    public async Task ConvertToCurrency(string currency)
    {
        SelectedCurrency = currency;
        if (!SelectedCurrency.Equals("GBP"))
        {
            var OldReportDate = string.Empty;
            double OldRate = 0;
            var exRateService = OpenExchangeRateService.Instance;

            foreach (var Item in Data)
            {
                var ReportDate = Item.YEAR + "-" + Item.MONTH.ToString().PadLeft(2, '0') + "-01";

                if (ReportDate.Contains("-00-"))
                {
                    if (!OldReportDate.Equals(ReportDate))
                    {
                        OldReportDate = ReportDate;
                        var rate = await exRateService.GetRateAverage(ReportDate, SelectedCurrency);
                        OldRate = rate;
                        Item.MONETARY_VALUE = Item.MONETARY_VALUE * rate;
                        Item.YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE * rate;
                    }
                    else
                    {
                        Item.MONETARY_VALUE = Item.MONETARY_VALUE * OldRate;
                        Item.YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE * OldRate;
                    }
                }
                else
                {
                    if (!OldReportDate.Equals(ReportDate))
                    {
                        OldReportDate = ReportDate;
                        var rate = await exRateService.GetRate(ReportDate, SelectedCurrency);
                        OldRate = rate;
                        Item.MONETARY_VALUE = Item.MONETARY_VALUE * rate;
                        Item.YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE * rate;
                    }
                    else
                    {
                        Item.MONETARY_VALUE = Item.MONETARY_VALUE * OldRate;
                        Item.YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE * OldRate;
                    }
                }
            }
        }
    }

    #endregion
}