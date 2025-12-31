using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;

namespace ISSB.Controllers
{
    public class MigrationController : Controller
    {
        [Authorize]
        private async Task<double> ConvertCurrency(string ReportDate, string code, double value)
        {
            var srv = new OpenExchangeRateService();
            var rate = await srv.GetRate(ReportDate, code);

            if (rate.Equals(0))
            {
                return 0;
            }

            // double cRate = 1 / rate;
            double total = value / rate;
            return total;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            await Task.Delay(1000);
            // var srv = new UpsertService();
            // srv.PostAmendments(3633,"006");

            //var PortsSrv = new PortsService();
            //var PortsList = await PortsSrv.GetPortsByGeoCode("8049");

            //var text = string.Empty;

            //var tariffService = new TariffService();
            //var Region = await tariffService.GetTariffsByRegion("701");

            //foreach (var Item in Region)
            //{
            //    Item.SIDE_OF_TRADE = "B";
            //    await tariffService.Save(Item);
            //}

            try
            {

                var importService = new MigrationService();
                var tariffService = new TariffService();
                var importList = await importService.GetTariffImports();
                //This is the last TariffID in the TariffDB Collection
                int TariffID = 1552987; 
                int noCnt = 0;
                int loopCnt = 1;

                foreach (var Item in importList)
                {

                    var tModel = new TariffModel
                    {
                        DISCONTINUED_DATE = DateTime.Now,
                        HARMONISED_TARIFF_CODE = Item.HS ?? string.Empty,
                        HARMONISED_TARIFF_LONG_LEGEND = Item.DESCRIPTION.ToUpper() ?? string.Empty,
                        HARMONISED_TARIFF_SHORT_LEGEND = Item.DESCRIPTION.ToUpper() ?? string.Empty,
                        HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = Item.DESCRIPTION.ToUpper() ?? string.Empty,
                        ISSB_STORED_CODE = "",
                        ISSB_STORED_LEGEND = Item.DESCRIPTION.ToUpper() ?? string.Empty,
                        LOWER_VPT = "",
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE ?? string.Empty,
                        SOURCE_COUNTRY_TARIFF_CODE = Item.TARIFF ?? string.Empty,
                        SOURCE_COUNTRY_TARIFF_LONG_LEGEND = Item.DESCRIPTION.ToUpper() ?? string.Empty,
                        SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = Item.DESCRIPTION.ToUpper() ?? string.Empty,
                        START_DATE = DateTime.Now,
                        TARIFF_CODE_TABLE_CODE = Item.REGION_CODE ?? string.Empty,
                        TARIFF_ID = TariffID,
                        UPPER_VPT = "",
                        WTO_ALLOY_CODE = "",
                        WTO_ALLOY_LEGEND = "",
                        WTO_CODE = "X9",
                        WTO_CODE_LEGEND = Item.DESCRIPTION.ToUpper() ?? string.Empty
                    };

                    var result = await tariffService.DoesExist(Item.TARIFF, Item.REGION_CODE);
                    if (result == 0)
                    {
                        await tariffService.Add(tModel);
                        TariffID++;
                    }
                    else
                    {
                        noCnt++;
                    }
                    loopCnt++;
                    var message = "Records: " + loopCnt;
                    Console.WriteLine(message);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return View();
        }

        [Authorize]
        public ViewResult Start(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
                {
                    await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                    try
                    {
                        //Pass the file path and file name to the StreamReader constructor
                        StreamReader sr = new StreamReader(model.FilePath);
                        StreamWriter sw = new StreamWriter(model.FilePath + ".updated");

                        //Read the first line of text
                        string json = sr.ReadLine();
                        if (json != null)
                        {
                            TradeFactModel data = JsonConvert.DeserializeObject<TradeFactModel>(json);
                            if (data != null)
                            {
                                //await srv.UpdateTradeFact(data);
                                data.year = int.Parse(data.time_id.ToString().Substring(0, 4));
                                data.month = int.Parse(data.time_id.ToString().Substring(5, 2));
                                data.quarter = int.Parse(data.time_id.ToString().Substring(4, 1));
                                data.batch = 1;
                                var outModel = new TradeFactDBImport
                                {
                                    COO_GEO_CODE = data.coo_geo_code,
                                    CWC_GEO_CODE = data.cwc_geo_code,
                                    ESTIMATED = data.estimated,
                                    MARKET_COUNTRY_ID = data.market_country_id,
                                    MONETARY_VALUE = data.monetary_value,
                                    MONTH = data.month,
                                    PORT_ID = data.port_id,
                                    SIDE_OF_TRADE = data.side_of_trade,
                                    SOURCE_COUNTRY_ID = data.source_country_id,
                                    TARIFF_ID = data.tariff_id,
                                    TIME_ID = data.time_id,
                                    WEIGHT = data.weight,
                                    YEAR = data.year,
                                    YTD_MONETARY_VALUE = data.ytd_monetary_value,
                                    YTD_WEIGHT = data.ytd_weight,
                                    BATCH = data.batch,
                                    QUARTER = data.quarter

                                };
                                sw.WriteLine(JsonConvert.SerializeObject(outModel));
                            }
                        }


                        //Continue to read until you reach end of file
                        while (json != null)
                        {
                            json = sr.ReadLine();

                            if (json != null)
                            {
                                TradeFactModel data = JsonConvert.DeserializeObject<TradeFactModel>(json);
                                if (data != null)
                                {
                                    //await srv.UpdateTradeFact(data);
                                    data.year = int.Parse(data.time_id.ToString().Substring(0, 4));
                                    data.month = int.Parse(data.time_id.ToString().Substring(5, 2));
                                    data.quarter = int.Parse(data.time_id.ToString().Substring(4, 1));
                                    data.batch = 1;
                                    var outModel = new TradeFactDBImport
                                    {
                                        COO_GEO_CODE = data.coo_geo_code,
                                        CWC_GEO_CODE = data.cwc_geo_code,
                                        ESTIMATED = data.estimated,
                                        MARKET_COUNTRY_ID = data.market_country_id,
                                        MONETARY_VALUE = data.monetary_value,
                                        MONTH = data.month,
                                        PORT_ID = data.port_id,
                                        SIDE_OF_TRADE = data.side_of_trade,
                                        SOURCE_COUNTRY_ID = data.source_country_id,
                                        TARIFF_ID = data.tariff_id,
                                        TIME_ID = data.time_id,
                                        WEIGHT = data.weight,
                                        YEAR = data.year,
                                        YTD_MONETARY_VALUE = data.ytd_monetary_value,
                                        YTD_WEIGHT = data.ytd_weight,
                                        BATCH = data.batch,
                                        QUARTER = data.quarter
                                    };
                                    sw.WriteLine(JsonConvert.SerializeObject(outModel));
                                }
                            }
                        }
                        sw.Close();
                        sr.Close();



                    }
                    catch (Exception e)
                    {
                        var eModel = new ErrorModel
                        {
                            Code = "MigrationController",
                            ErrorMessage = e.Message
                        };

                        await errorSrv.UpdateError(eModel);
                        Console.WriteLine("Exception: " + e.Message);
                    }
                    finally
                    {
                        Console.WriteLine("Executing finally block.");
                        await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                    }

                    ///Volumes/Extreme SSD/trade_fact_1985_1989.json
                });
            return View();
        }

        [Authorize]
        public ViewResult Tariff(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        TariffModel data = JsonConvert.DeserializeObject<TariffModel>(json);
                        if (data != null)
                            await srv.UpdateTariffCodes(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            TariffModel data = JsonConvert.DeserializeObject<TariffModel>(json);
                            if (data != null)
                                await srv.UpdateTariffCodes(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult SourceCountry(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        SourceCountryModel data = JsonConvert.DeserializeObject<SourceCountryModel>(json);
                        if (data != null)
                            await srv.UpdateSourceCountry(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            SourceCountryModel data = JsonConvert.DeserializeObject<SourceCountryModel>(json);
                            if (data != null)
                                await srv.UpdateSourceCountry(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult MarketCountry(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        MarketCountryModel data = JsonConvert.DeserializeObject<MarketCountryModel>(json);
                        if (data != null)
                            await srv.UpdateMarketCountry(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            MarketCountryModel data = JsonConvert.DeserializeObject<MarketCountryModel>(json);
                            if (data != null)
                                await srv.UpdateMarketCountry(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult WorldCity(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();
            var countrySrv = new MarketCountryService();

            List<WorldCityModel> cityList = new List<WorldCityModel>();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string csv = sr.ReadLine();
                    string[] lines = csv.Split(",");

                    foreach (string line in lines)
                    {
                        //Reading the Header
                        Console.WriteLine(line);
                    }

                    if (csv != null)
                    {
                        // MarketCountryModel data = JsonConvert.DeserializeObject<MarketCountryModel>(json);
                        // if (data != null)
                        //     await srv.UpdateMarketCountry(data);
                    }

                    //Continue to read until you reach end of file
                    int Idx = 0;
                    while (csv != null)
                    {
                        csv = sr.ReadLine();

                        if (csv != null)
                        {
                            Idx++;
                            string[] data = csv.Split(",");

                            if (Idx > 32)
                            {

                            }

                            var mCountry = await countrySrv.GetMarketCountryByName(data[5]);

                            string sGeo_Code = string.Empty;

                            if (mCountry != null)
                            {
                                sGeo_Code = mCountry.GEO_CODE;
                            }
                            else
                            {
                                sGeo_Code = string.Empty;
                            }

                            if (mCountry.GEO_CODE == null)
                                sGeo_Code = string.Empty;

                            var CityModel = new WorldCityModel
                            {
                                Idx = Idx,
                                GEO = sGeo_Code,
                                City = data[0],
                                CityAscii = data[1],
                                LAT = float.Parse(data[2]),
                                LNG = float.Parse(data[3]),
                                Population = float.Parse(data[4]),
                                Country = data[5],
                                ISO2 = data[6],
                                ISO3 = data[7],
                                Province = data[8]

                            };

                            if (csv != null)
                                await srv.InsertWorldCity(CityModel);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult ReadExcel(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {

                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {

                    var ExcelSrv = new ExcelHelper();
                    var result = ExcelSrv.ReadFromExcel(model.FilePath, true);

                    var db = new DBContext();

                    // string doc = JsonConvert.SerializeObject(result);

                    var data = new ImportDataModel();

                    data.Id = 1;
                    data.Name = "Test Doc";
                    data.Date = DateTime.Now;

                    dynamic dData = data;
                    dData.Entered = DateTime.Now;
                    dData.Accesses = 1;


                    // await db.ImportFile(result);

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult UpdatePorts(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        PortsModel data = JsonConvert.DeserializeObject<PortsModel>(json);
                        if (data != null)
                            await srv.UpdatePort(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            PortsModel data = JsonConvert.DeserializeObject<PortsModel>(json);
                            if (data != null)
                                await srv.UpdatePort(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult UpdateTime(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        TimeDimensionModel data = JsonConvert.DeserializeObject<TimeDimensionModel>(json);
                        if (data != null)
                            await srv.UpdateTimeDimension(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            TimeDimensionModel data = JsonConvert.DeserializeObject<TimeDimensionModel>(json);
                            if (data != null)
                                await srv.UpdateTimeDimension(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult UpdateBroadTariff(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        ProductBroadFilterModel data = JsonConvert.DeserializeObject<ProductBroadFilterModel>(json);
                        if (data != null)
                            await srv.InsertBroadTrariffFilter(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            ProductBroadFilterModel data = JsonConvert.DeserializeObject<ProductBroadFilterModel>(json);
                            if (data != null)
                                await srv.InsertBroadTrariffFilter(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult UpdateMedumTariff(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        ProductMedumFilterModel data = JsonConvert.DeserializeObject<ProductMedumFilterModel>(json);
                        if (data != null)
                            await srv.InsertMedumTrariffFilter(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            ProductMedumFilterModel data = JsonConvert.DeserializeObject<ProductMedumFilterModel>(json);
                            if (data != null)
                                await srv.InsertMedumTrariffFilter(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }
        [Authorize]
        public ViewResult UpdateAllTariff(MigrationModel model)
        {
            var srv = new MigrationService();
            var errorSrv = new ErrorService();
            var mailSrv = new EmailService();

            Task.Run(async () =>
            {
                await mailSrv.SendEmail("Prossessing file start ", model.FilePath);
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(model.FilePath);

                    //Read the first line of text
                    string json = sr.ReadLine();
                    if (json != null)
                    {
                        ProductAllFilterModel data = JsonConvert.DeserializeObject<ProductAllFilterModel>(json);
                        if (data != null)
                            await srv.InsertAllTrariffFilter(data);
                    }

                    //Continue to read until you reach end of file
                    while (json != null)
                    {
                        json = sr.ReadLine();

                        if (json != null)
                        {
                            ProductAllFilterModel data = JsonConvert.DeserializeObject<ProductAllFilterModel>(json);
                            if (data != null)
                                await srv.InsertAllTrariffFilter(data);
                        }
                    }

                    sr.Close();

                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "MigrationController",
                        ErrorMessage = e.Message
                    };

                    await errorSrv.UpdateError(eModel);
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                    await mailSrv.SendEmail("Prossessing file Finished", model.FilePath);
                }

                ///Volumes/Extreme SSD/trade_fact_1985_1989.json
            });
            return View();
        }

    }
}
