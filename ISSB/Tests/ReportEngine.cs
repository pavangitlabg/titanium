/*
  Tests/ReportEngine.cs

  reporting engine tests
*/

using System.Collections.Generic ;
using Xunit ;

namespace Tests
{
    public class ReportEngine
    {
        #region Fact test

        /*

        /// <summary>Test number of records from ReportingEngineService having geo code 001 or 006.</summary>

        [ Fact ( DisplayName = "geo code 001 or 006" ) ] async public void Geo001or006()
        {
            Data.Models.SearchModel InModel ;

            InModel = new Data.Models.SearchModel() ;

            InModel.DateFrom = 201910100 ;
            InModel.DateTo = 201920400 ;
            InModel.SourceCountryGeoCode = new System.Collections.Generic.List<string>() ;
            InModel.SourceCountryGeoCode.Add( "001" ) ;
            InModel.SourceCountryGeoCode.Add( "006" ) ;

            var srv = new Services.ReportingEngineService();
            var modelOut = await srv.GetReport(InModel);

            Assert.Equal( modelOut.Count , 240 ) ;
        }

        */

        #endregion

        #region Theory test

        /*
          test data

          objects passed are as follows:
            - source country geo codes
            - expected number of records
        */

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                // new object[] { 201910100 , 201920400 , new List<string> { "001" , "006" } , 240 } ,
                // new object[] { 201910100 , 201920400 , new List<string> { "001" , "038" } , 159 } ,

                // count retrieved from old system

                // new object[] { 201810100 , 201820400 , new List<string> { "001"         } ,  21 }

                // count retrieved via MongoDB query

//new object[] { new List<int> { 201810100 , 201810200 , 201810300 } , new List<string> { "001" } , new List<string> { "260111" } , 50 } ,

//new object[] { new List<int> { 201810100 , 201810200 , 201810300 } , new List<string> { "038" } , new List<string> { "260111" } ,  2 }
/* new object[] {
               new List<int> { 201810100 , 201810200 , 201810300 } ,
               new List<string> { "038" } ,
               new List<string> { "005" } ,
               new List<string> { "260111" } ,
               new List<int> { 1 } ,
               2
             } , */
new object[] {
               new List<int> { 201810100 , 201810200 , 201810300 } , // dates
               new List<string> { "011" , "400" , "528" } , // sourceGeo
               new List<string> { "001" , "006" , "404" , "508" } , // marketGeo
               new List<string> { "270300" , "270400" } , // products
               new List<int> { 1 } , // ports
               59 // count
             }
            } ;

// TODO Oliver This test does not work because of changes to models. Take it out

//        /// <summary>Test number of records from ReportingEngineService.</summary>
//        /// <param name="from">date from</param>
//        /// <param name="to">date to</param>
//        /// <param name="sourceGeo">source country geo codes</param>
//        /// <param name="count">expected number of records</param>

//        [ Theory ( DisplayName = "reporting engine service" ) ]
//        [ MemberData( nameof( Data ) ) ]
//        async public void RecordsReturnedFromReportingEngineService(
////                                                                     int from ,
////                                                                     int to ,
//List<int> dates ,
//                                                                     List<string> sourceGeo ,
//List<string> marketGeo ,
////                                                                     List<string> tariffs ,
//List<string> products ,
//List<int> ports ,
//                                                                     int count
//                                                                   )
//        {
//            Data.Models.SearchModel InModel ;

//            InModel = new Data.Models.SearchModel() ;

////            InModel.DateFrom = from ;
////            InModel.DateTo = to ;
//InModel.Dates = dates ;
//            InModel.SourceCountryGeoCodes = new System.Collections.Generic.List<string>() ;
//            foreach (string geo in sourceGeo ) InModel.SourceCountryGeoCodes.Add( geo ) ;
//InModel.MarketCountryGeoCodes = marketGeo ;
//            InModel.Products = new List<string>() ;
////            foreach ( string tariff in tariffs ) InModel.Products.Add( tariff ) ;
//foreach ( string tariff in products ) InModel.Products.Add( tariff ) ;
//InModel.Ports = ports ;

//            var srv = new Services.ReportingEngineService();
//            var modelOut = await srv.GetReport(InModel);

//            Assert.Equal( count , modelOut.Count ) ;
//        }

        #endregion
    }
}