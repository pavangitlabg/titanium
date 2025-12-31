/*
  Data/Models/SearchModel.cs

  model containing search definition
*/

using System ;
using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations ;
using System.Linq ;
using MongoDB.Bson ;
using MongoDB.Driver ;

namespace Data.Models
{
// TODO Oliver SearchModel: Remove enum ReportType if not required.

//    /// <summary>report types</summary>

//    public enum ReportType
//    {
//        /// <summary>no grouping in anything</summary>

//        NoGroup = 0 ,

//        /// <summary>grouping source countries and products, markets shown separately</summary>

//        GroupSourceCountry = 1 ,

//        /// <summary>source countries separate, products and markets grouped</summary>

//        GroupProductMarket = 2 ,

//        /// <summary>source countries and products separate, markets grouped</summary>

//        GroupMarket = 3 ,

//        /// <summary>products separate, source countries and markets grouped</summary>

//        GroupCountryMarket = 4 ,

//        /// <summary>all grouped source market products</summary>

//        AllGrouped = 5 ,

//        /// <summary>source grouped, markets and products separate</summary>

//        GroupSource = 6 ,

//        /// <summary>source and market countries separately, products grouped</summary>

//        GroupProduct = 7
//    }

    /// <summary>country grouping options</summary>

    public enum GroupingOption
    {
        /// <summary>group countries together</summary>

        Together,

        /// <summary>view countries separately</summary>

        Separately
    }

    /// <summary>trade flow selection options</summary>

    public enum TradeFlowOption
    {
        /// <summary>select imports only</summary>

        Imports ,

        /// <summary>select exports only</summary>

        Exports ,

        /// <summary>select both imports and exports</summary>

        Both
    }

    /// <summary>product detail options</summary>

    public enum ProductDetailOption
    {
        /// <summary>broad product</summary>

        Broad ,

        /// <summary>medium level product detail (2 digits)</summary>

        Medium ,

        /// <summary>high level product detail</summary>

        High ,

        /// <summary>full product detail</summary>

        Long ,

        Custom
    }

    /// <summary>model containing search definition</summary>

    public class SearchModel
    {
        public bool ShowWeightZero = true;
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public bool IsPivot { get; set; }

        #region selection criteria

        /*
          The following options are presented to the user when designing a report:
            - source country grouping:  group source countries together or separately
            - source country selection: source country geo codes to include
            - trade flow selection:     whether to select imports, exports or both
            - market country grouping:  group market countries together or separately
            - market country selection: market country geo codes to include
            - ports use:                whether to use ports
            - ports grouping:           group ports together or separately
            - port selection:           ports to include
            - product detail:           broad, medium or high
            - product grouping:         group products together or separately
            - product selection:        products to include
            - month grouping:           whether to display dates by month
            - quarter grouping:         whether to display dates by quarter
            - year grouping:            whether to display dates by year
            - dates to include:         years and months
            - tonnes and values:        whether to select tonnes only or tonnes and values
        */

        /// <summary>source country grouping</summary>

        public GroupingOption SourceCountryGrouping ;

        /// <summary>source country geo codes to include</summary>

        [ Display( Name = "Source Country Geo Codes" ) ] public List<string> SourceCountryGeoCodes { get ; set ; }

        /// <summary>trade flow selection</summary>

        public TradeFlowOption TradeFlow ;

        /// <summary>market country grouping</summary>

        public GroupingOption MarketCountryGrouping ;

        /// <summary>market country geo codes to include</summary>

        [ Display( Name = "Market Country Geo Codes" ) ] public List<string> MarketCountryGeoCodes { get ; set ; }

        /// <summary>whether to use ports</summary>

        public bool IncludePorts { get; set; }

        /// <summary>port grouping</summary>

        public GroupingOption PortGrouping ;

        /// <summary>port IDs to include</summary>

        public List<int> Ports { get ; set ; }

        /// <summary>product detail</summary>

        public ProductDetailOption ProductDetail ;

        /// <summary>product grouping</summary>

        public GroupingOption ProductGrouping ;

        /// <summary>products to include (harmonised tariff codes)</summary>

        [ Display( Name = "Tariff Codes" ) ] public List<string> Products { get ; set ; }

        /// <summary>whether to display dates by month</summary>

        public bool GroupByMonth { get ; set ; }

        /// <summary>whether to display dates by quarter</summary>

        public bool GroupByQuarter { get ; set ; }

        /// <summary>whether to display dates by year</summary>

        public bool GroupByYear { get; set; }
        
        /// <summary>dates to include, in the form yyyyqmm00</summary>

        public List<int> Dates { get ; set ; }

        /// <summary>whether to select tonnes only (false) or tonnes and values (true)</summary>

        public bool Values { get ; set ; }

        /// <summary>source country grouping option</summary>

        public string SourceCountryGroup
        {
            set
            {
                SourceCountryGrouping = value.Equals( "ViewCountriesSeparately" ) ? GroupingOption.Separately : GroupingOption.Together ;
            }
        }
        
        /// <summary>trade flow selection option</summary>

        public string TradeFlowType
        {
            set
            {
                switch ( value )
                {
                    case "SelectImportsOnly" : TradeFlow = TradeFlowOption.Imports ; break ;
                    case "SelectExportsOnly" : TradeFlow = TradeFlowOption.Exports ; break ;
                    case "SelectBothImportsandExports" : TradeFlow = TradeFlowOption.Both ; break ;
                }
            }
        }

        /// <summary>market country grouping option</summary>

        public string MarketCountryGroup
        {
            set
            {
                MarketCountryGrouping = value.Equals( "ViewCountriesSeparately" ) ? GroupingOption.Separately : GroupingOption.Together ;
            }
        }

        /// <summary>port grouping option</summary>

        public string PortGroup
        { set { PortGrouping = value.Equals( "ViewPortsSeparately" ) ? GroupingOption.Separately : GroupingOption.Together ; } }

        /// <summary>product detail option</summary>

        public string ProductGroupType
        {
            set
            {
                switch ( value )
                {
                    case "Broad" : ProductDetail = ProductDetailOption.Broad ; break ;
                    case "Medium" : ProductDetail = ProductDetailOption.Medium ; break ;
                    case "High" : ProductDetail = ProductDetailOption.High ; break ;
                    case "Long" : ProductDetail = ProductDetailOption.Long ; break ;
                    case "Custom": ProductDetail = ProductDetailOption.Custom; break;
                }
            }

            get
            {
                switch ( ProductDetail )
                {
                    case ProductDetailOption.Broad : return "Broad" ;
                    case ProductDetailOption.Medium : return "Medium" ;
                    case ProductDetailOption.High : return "High" ;
                    case ProductDetailOption.Long : return "Long" ;
                    case ProductDetailOption.Custom: return "Custom";
                }

                return string.Empty ;
            }
        }

        /// <summary>product grouping option</summary>

        public string ProductGroup
        { set { ProductGrouping = value.Equals( "ViewProductsSeparately" ) ? GroupingOption.Separately : GroupingOption.Together ; } }

        /// <summary>whether to select values option</summary>

        public string TonnesValuesGroup { set { Values = value.Equals( "SelectBothTonnesandValues" ) ; } }

        #endregion

        /// <summary>when matching, whether to match by dates first</summary>

        public bool SelectDatesFirst { get ; set ; }
        
        /// <summary>Return the year component of a date.</summary>
        /// <param name="date">date, in the form yyyyqmmxx</param>
        /// <returns>year component of date</returns>

        private int ToYear( int date ) { return date / 100000 ; }

        /// <summary>Return the month component of a date.</summary>
        /// <param name="date">date, in the form yyyyqmmxx</param>
        /// <returns>month component of date</returns>

        private int ToMonth( int date ) { return date % 10000 / 100 ; }

        /// <summary>Return a date as a number of months.</summary>
        /// <param name="date">date, in the form yyyyqmmxx</param>
        /// <returns>date as number of months</returns>

        private int Months( int date ) { return ToYear( date ) * 12 + ToMonth( date ) ; }

        /// <summary>Return whether dates in a list are all sequential.</summary>
        /// <param name="dates">list of dates, in the form yyyyqmm00</param>
        /// <returns>whether dates are all sequential</returns>

        private bool Sequential( List<int> dates )
        {
            bool sequential ; // whether dates are sequential
            int last ; // last date as months
            int current ; // current date as months

            sequential = true ;
            last = -1 ; // first date

            foreach ( int d in dates.OrderBy( o => o ) )
            {
                current = Months( d ) ;
                if ( last != -1 && current != last + 1 ) sequential = false ;
                last = current ;
            }

            return sequential ;
        }

        public SearchModel()
        {
            Dates = new List<int>() ;
            SourceCountryGeoCodes = new List<string>() ;
            MarketCountryGeoCodes = new List<string>() ;
            Products = new List<string>() ;
            Ports = new List<int>() ;

            SelectDatesFirst = false ;
        }
        
        /// <summary>Return the search pipeline.</summary>
        /// <param name="source">geo code to source country ID mapping</param>
        /// <param name="market">geo code to market country ID mapping</param>
        /// <param name="tariffs">harmonised tariff code to tariff ID mapping</param>
        /// <returns>search pipeline</returns>

        public PipelineDefinition<BsonDocument,BsonDocument> pipeline()
        {
            BsonArray    aDates  ; // dates array
            BsonValue    mDates  ; // dates match value
            BsonArray    aSource ; // source country codes array
            BsonValue    mSource ; // source country match value
            BsonArray    aMarket ; // market country codes array
            BsonValue    mMarket ; // market country match value
            BsonArray    aTariff ; // tariff codes array
            BsonValue    mTariff ; // tariff match value
            BsonArray    aPort   ; // port IDs array
            BsonValue    mPort   ; // port match value
            BsonArray    aFlow   ; // trade flow array
            BsonValue    mFlow   ; // trade flow match value
            BsonDocument group   ; // grouping
            BsonDocument groupId ; // grouping: _id
            BsonDocument sort    ; // sorting
            BsonDocument facet; // sorting

            int            element   ;
            BsonDocument[] documents ;

            #region dates to include

            if ( Sequential( Dates ) )
                mDates = new BsonDocument().Add( "TIME_ID" , new BsonDocument().Add( "$gte" , Dates.Min() ).Add( "$lte" , Dates.Max() ) ) ;
            else
            {
                aDates = new BsonArray() ;
                foreach ( int date in Dates ) aDates.Add( date ) ;
                mDates = new BsonDocument().Add( "TIME_ID" , new BsonDocument().Add( "$in" , aDates ) ) ;
            }

            #endregion

            #region source country geo codes to include

            aSource = new BsonArray() ;
            foreach ( string country in SourceCountryGeoCodes ) aSource.Add( country ) ;
            mSource = new BsonDocument().Add( "SC_GEO" , new BsonDocument().Add( "$in" , aSource ) ) ;

            #endregion

            #region market country geo codes to include

            aMarket = new BsonArray() ;
            foreach ( string country in MarketCountryGeoCodes ) aMarket.Add( country ) ;
            mMarket = new BsonDocument().Add( "MC_GEO" , new BsonDocument().Add( "$in" , aMarket ) ) ;

            #endregion

            #region tariff codes to include

            aTariff = new BsonArray() ;

            if ( ProductDetail == ProductDetailOption.Long )
            {
                // full product detail

                foreach ( string tariff in Products ) aTariff.Add( int.Parse( tariff ) ) ;
                mTariff = new BsonDocument().Add( "TARIFF_ID" , new BsonDocument().Add( "$in" , aTariff ) ) ;
            }
            else
            {
                // broad product or medium or higl level product detail
                bool IsTID = false;
                foreach (string tariff in Products)
                {
                    if (tariff.Length != 6)
                    {
                        IsTID = true;

                        aTariff.Add(int.Parse(tariff));
                    }
                    else
                    {
                        aTariff.Add(tariff);
                    }
                }

                if(IsTID)
                   mTariff = new BsonDocument().Add( "TARIFF_ID" , new BsonDocument().Add( "$in" , aTariff ) ) ;
                else
                   mTariff = new BsonDocument().Add("H_TARIFF", new BsonDocument().Add("$in", aTariff));
            }

            #endregion

            #region ports to include

            aPort = new BsonArray() ;
            foreach ( int port in Ports ) aPort.Add( port ) ;
            mPort = new BsonDocument().Add( "PORT_ID" , new BsonDocument().Add( "$in" , aPort ) ) ;

            #endregion

            #region trade flow

            aFlow = new BsonArray() ;

            switch ( TradeFlow )
            {
                case TradeFlowOption.Imports : aFlow.Add( "I" ) ; break ;
                case TradeFlowOption.Exports : aFlow.Add( "E" ) ; break ;
                case TradeFlowOption.Both    : aFlow.Add( "I" ) ; aFlow.Add( "E" ) ; break ;
            }

            mFlow = new BsonDocument().Add( "SIDE_OF_TRADE" , new BsonDocument().Add( "$in" , aFlow ) ) ;

            #endregion

            #region grouping and column selection

            groupId = new BsonDocument() ;

            if ( SourceCountryGrouping == GroupingOption.Separately ) groupId.Add( "SC_GEO" , "$SC_GEO" ) ;

            groupId.Add( "SIDE_OF_TRADE" , "$SIDE_OF_TRADE" ) ;

            if ( GroupByYear || GroupByQuarter || GroupByMonth )
            {
                groupId.Add( "YEAR" , "$YEAR" ) ;
                if ( GroupByQuarter || GroupByMonth )
                {
                    groupId.Add( "QUARTER" , "$QUARTER" ) ;
                    if ( GroupByMonth ) groupId.Add( "MONTH" , "$MONTH" ) ;
                }

            }

            if ( ProductDetail == ProductDetailOption.Long )
            {
                if ( ProductGrouping == GroupingOption.Separately ) groupId.Add( "TARIFF_ID" , "$TARIFF_ID" ) ;
            }
            else
            {
                if ( ProductGrouping == GroupingOption.Separately ) groupId.Add( "H_TARIFF" , "$H_TARIFF" ) ;
            }

            if ( MarketCountryGrouping == GroupingOption.Separately ) groupId.Add( "MC_GEO" , "$MC_GEO" ) ;

            if ( PortGrouping == GroupingOption.Separately ) groupId.Add( "PORT_ID" , "$PORT_ID" ) ;

            group = new BsonDocument().Add( "_id" , groupId ).Add( "WEIGHT" , new BsonDocument().Add( "$sum" , "$WEIGHT" ) ) ;
            if ( Values ) group.Add( "MONETARY_VALUE" , new BsonDocument().Add( "$sum" , "$MONETARY_VALUE" ) ) ;

            group.Add( "YTD_WEIGHT" , new BsonDocument().Add( "$sum" , "$YTD_WEIGHT" ) ) ;
            if ( Values ) group.Add( "YTD_MONETARY_VALUE" , new BsonDocument().Add( "$sum" , "$YTD_MONETARY_VALUE" ) ) ;

            #endregion

            #region sorting

            sort = new BsonDocument() ;

            if ( SourceCountryGrouping == GroupingOption.Separately ) sort.Add( "_id.SC_GEO" , 1.0 ) ;

            sort.Add( "_id.SIDE_OF_TRADE" , 1.0 ) ;

            if ( GroupByYear || GroupByQuarter || GroupByMonth )
            {
                sort.Add( "_id.YEAR" , 1.0 ) ;
                if ( GroupByQuarter || GroupByMonth )
                {
                    sort.Add( "_id.QUARTER" , 1.0 ) ;
                    if ( GroupByMonth ) sort.Add( "_id.MONTH" , 1.0 ) ;
                }
            }

            if ( ProductGrouping == GroupingOption.Separately ) sort.Add( "_id.H_TARIFF" , 1.0 ) ;

            if ( MarketCountryGrouping == GroupingOption.Separately ) sort.Add( "_id.MC_GEO" , 1.0 ) ;

            if ( PortGrouping == GroupingOption.Separately ) sort.Add( "_id.PORT_ID" , 1.0 ) ;

            #endregion

            element = 7 ;
            if ( IncludePorts ) element ++ ;

            //Debug
           // IsPivot = true;
            if (IsPivot) element++;
            documents = new BsonDocument[ element ] ;

            if ( SelectDatesFirst )
            {
                documents[ 0 ] = new BsonDocument( "$match" , mDates ) ;
                documents[ 1 ] = new BsonDocument( "$match" , mSource ) ;
            }
            else
            {
                documents[ 0 ] = new BsonDocument( "$match" , mSource ) ;
                documents[ 1 ] = new BsonDocument( "$match" , mDates ) ;
            }
            documents[ 2 ] = new BsonDocument( "$match" , mMarket ) ;
            documents[ 3 ] = new BsonDocument( "$match" , mTariff ) ;
            documents[ 4 ] = new BsonDocument( "$match" , mFlow ) ;

            element = 5 ;// 7

            if ( IncludePorts )
            {
                documents[ element ] = new BsonDocument( "$match" , mPort ) ;
                element ++ ;
            }

            if (!IsPivot)
            {
                documents[element] = new BsonDocument("$group", group);
                element++;
                documents[element] = new BsonDocument("$sort", sort);
            }

            //Add Pivot
           // IsPivot = true;
            if (IsPivot)
            {
                //var facetSort = new BsonDocument();

                //foreach(var Item in sort)
                //{
                //    facetSort.Add(Item.Name.Replace("_id.",""), "$"+Item.Name);

                //}

                //facet = new BsonDocument();

                facet = new BsonDocument("$group", new BsonDocument()
                      .Add("_id", new BsonDocument()
                              .Add("SC_GEO", mSource)
                              .Add("MC_GEO", mMarket)
                              .Add("H_TARIFF", mTariff)
                              .Add("SIDE_OF_TRADE", mFlow)
                              .Add("YEAR", "_id.YEAR")
                              .Add("MONTH", "_id.MONTH")
                      )
                      .Add("WEIGHT", new BsonDocument()
                              .Add("$sum", "$WEIGHT")
                      )
                      .Add("MONETARY_VALUE", new BsonDocument()
                              .Add("$sum", "$MONETARY_VALUE")
                      ));


                documents[element] = new BsonDocument(facet);

                facet = new BsonDocument("$replaceRoot", new BsonDocument()
                      .Add("newRoot", new BsonDocument()
                                .Add("$mergeObjects", new BsonArray()
                                        .Add(new BsonDocument()
                                                .Add("SC_GEO", "$_id.SC_GEO")
                                                .Add("MC_GEO", "$_id.MC_GEO")
                                                .Add("H_TARIFF", "$_id.H_TARIFF")
                                                .Add("SIDE_OF_TRADE", "$_id.SIDE_OF_TRADE")
                                                .Add("YEAR", "$_id.YEAR")
                                                .Add("MONTH", "$_id.MONTH")
                                        )
                                        .Add(new BsonDocument()
                                                .Add("WEIGHT", new BsonDocument()
                                                        .Add("$sum", "$_id.WEIGHT")
                                                )
                                        )
                                        .Add(new BsonDocument()
                                                .Add("MONETARY_VALUE", new BsonDocument()
                                                        .Add("$sum", "$_id.MONETARY_VALUE")
                                                )
                                        )
                                )
                       ));

                element++;
                documents[element] = new BsonDocument(facet);

            }

            PipelineDefinition<BsonDocument,BsonDocument> pipeline = documents ;

            return pipeline ;
        }
    }
}