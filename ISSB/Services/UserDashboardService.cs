using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class UserDashboardService
    {
        public async Task<bool> InsertDashBoard(UserDashboardModel model)
        {
            bool bReturn = false;
            var db = new DBContext();
            var modelList = new List<UserDashboardItemsDB>();

            foreach (var Item in model.Items)
            {
                var mItem = new UserDashboardItemsDB
                {
                    ID = Item.ID,
                    HTML = Item.HTML,
                    Type = Item.Type,
                    SubTitle1 = Item.SubTitle1,
                    SubTitle2 = Item.SubTitle2,
                    SubTitle3 = Item.SubTitle3,
                    SubTitle4 = Item.SubTitle4,
                    Title1 = Item.Title1,
                    Title2 = Item.Title2,
                    Title3 = Item.Title3,
                    Title4 = Item.Title4,
                    SelectedIndex1 = Item.SelectedIndex1,
                    SelectedIndex2 = Item.SelectedIndex2,
                    SelectedIndex3 = Item.SelectedIndex3,
                    SelectedIndex4 = Item.SelectedIndex4,
                    Query1 = new DashBoardQueryDB(),
                    Query2 = new DashBoardQueryDB(),
                    Query3 = new DashBoardQueryDB(),
                    Query4 = new DashBoardQueryDB()
                };
                modelList.Add(mItem);
            }

            var modelDB = new UserDashboardDB
            {
                ID = model.ID,
                Description = model.Description,
                DashBoardName = model.DashBoardName,
                Date = DateTime.Now,
                UserIdentityName = model.UserIdentityName,
                UserName = model.UserName,
                bDesignMode = model.bDesignMode,
                Items = modelList
            };

            await db.UserDashboardDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> UpdateDashBoard(UserDashboardModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            bool bReturn = false;
            var db = new DBContext();
            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x._id, RecordId);

            var cursor = await db.UserDashboardDB.FindAsync(filter);
            IList<UserDashboardDB> results = cursor.ToList();

            var modelList = new List<UserDashboardItemsDB>();

            foreach (var modelItem in model.Items)
            {
                var modelQuery1 = new DashBoardQueryDB();
                if (modelItem.Query1 != null)
                {
                    if (modelItem.Query1.SourceCountryGEOs != null)
                    {
                        modelQuery1.Id = modelItem.Query1.Id;
                        modelQuery1.IncludePorts = modelItem.Query1.IncludePorts;
                        modelQuery1.Name = modelItem.Query1.Name;
                        modelQuery1.PortGroup = modelItem.Query1.PortGroup;
                        modelQuery1.Comments = modelItem.Query1.Comments;
                        modelQuery1.DashItem = modelItem.Query1.DashItem;
                        modelQuery1.DashItemType = modelItem.Query1.DashItemType;
                        modelQuery1.Date = modelItem.Query1.Date;
                        modelQuery1.Description = modelItem.Query1.Description;
                        modelQuery1.GroupByMonth = modelItem.Query1.GroupByMonth;
                        modelQuery1.GroupByQuarter = modelItem.Query1.GroupByQuarter;
                        modelQuery1.GroupByYear = modelItem.Query1.GroupByYear;
                        modelQuery1.ProductGroup = modelItem.Query1.ProductGroup;
                        modelQuery1.User = modelItem.Query1.User;
                        modelQuery1.SourceCountryGroup = modelItem.Query1.SourceCountryGroup;
                        modelQuery1.TradeFlowType = modelItem.Query1.TradeFlowType;
                        modelQuery1.MarketCountryGroup = modelItem.Query1.MarketCountryGroup;
                        modelQuery1.ProductGroupType = modelItem.Query1.ProductGroupType;
                        modelQuery1.TonnesValuesGroup = modelItem.Query1.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query1.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery1.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query1.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery1.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query1.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery1.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query1.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB {  TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery1.TimeIDs = monthList;


                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query1.Ports == null)
                        {
                            modelItem.Query1.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query1.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery1.Ports = portList;
                    }
                }
                var modelQuery2 = new DashBoardQueryDB();
                if (modelItem.Query2 != null)
                {
                    if (modelItem.Query2.SourceCountryGEOs != null)
                    {
                        modelQuery2.Id = modelItem.Query2.Id;
                        modelQuery2.IncludePorts = modelItem.Query2.IncludePorts;
                        modelQuery2.Name = modelItem.Query2.Name;
                        modelQuery2.PortGroup = modelItem.Query2.PortGroup;
                        modelQuery2.Comments = modelItem.Query2.Comments;
                        modelQuery2.DashItem = modelItem.Query2.DashItem;
                        modelQuery2.DashItemType = modelItem.Query2.DashItemType;
                        modelQuery2.Date = modelItem.Query2.Date;
                        modelQuery2.Description = modelItem.Query2.Description;
                        modelQuery2.GroupByMonth = modelItem.Query2.GroupByMonth;
                        modelQuery2.GroupByQuarter = modelItem.Query2.GroupByQuarter;
                        modelQuery2.GroupByYear = modelItem.Query2.GroupByYear;
                        modelQuery2.ProductGroup = modelItem.Query2.ProductGroup;
                        modelQuery2.User = modelItem.Query2.User;
                        modelQuery2.SourceCountryGroup = modelItem.Query2.SourceCountryGroup;
                        modelQuery2.TradeFlowType = modelItem.Query2.TradeFlowType;
                        modelQuery2.MarketCountryGroup = modelItem.Query2.MarketCountryGroup;
                        modelQuery2.ProductGroupType = modelItem.Query2.ProductGroupType;
                        modelQuery2.TonnesValuesGroup = modelItem.Query2.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query2.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery2.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query2.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery2.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query2.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery2.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query2.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery2.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query2.Ports == null)
                        {
                            modelItem.Query2.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query2.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery2.Ports = portList;
                    }

                }
                var modelQuery3 = new DashBoardQueryDB();
                if (modelItem.Query3 != null)
                {
                    if (modelItem.Query3.SourceCountryGEOs != null)
                    {
                        modelQuery3.Id = modelItem.Query3.Id;
                        modelQuery3.IncludePorts = modelItem.Query3.IncludePorts;
                        modelQuery3.Name = modelItem.Query3.Name;
                        modelQuery3.PortGroup = modelItem.Query3.PortGroup;
                        modelQuery3.Comments = modelItem.Query3.Comments;
                        modelQuery3.DashItem = modelItem.Query3.DashItem;
                        modelQuery3.DashItemType = modelItem.Query3.DashItemType;
                        modelQuery3.Date = modelItem.Query3.Date;
                        modelQuery3.Description = modelItem.Query3.Description;
                        modelQuery3.GroupByMonth = modelItem.Query3.GroupByMonth;
                        modelQuery3.GroupByQuarter = modelItem.Query3.GroupByQuarter;
                        modelQuery3.GroupByYear = modelItem.Query3.GroupByYear;
                        modelQuery3.ProductGroup = modelItem.Query3.ProductGroup;
                        modelQuery3.User = modelItem.Query3.User;
                        modelQuery3.SourceCountryGroup = modelItem.Query3.SourceCountryGroup;
                        modelQuery3.TradeFlowType = modelItem.Query3.TradeFlowType;
                        modelQuery3.MarketCountryGroup = modelItem.Query3.MarketCountryGroup;
                        modelQuery3.ProductGroupType = modelItem.Query3.ProductGroupType;
                        modelQuery3.TonnesValuesGroup = modelItem.Query3.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query3.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery3.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query3.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery3.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query3.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery3.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query3.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery3.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query3.Ports == null)
                        {
                            modelItem.Query3.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query3.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery3.Ports = portList;
                    }
                }
                var modelQuery4 = new DashBoardQueryDB();
                if (modelItem.Query4 != null)
                {
                    if (modelItem.Query4.SourceCountryGEOs != null)
                    {
                        modelQuery4.Id = modelItem.Query4.Id;
                        modelQuery4.IncludePorts = modelItem.Query4.IncludePorts;
                        modelQuery4.Name = modelItem.Query4.Name;
                        modelQuery4.PortGroup = modelItem.Query4.PortGroup;
                        modelQuery4.Comments = modelItem.Query4.Comments;
                        modelQuery4.DashItem = modelItem.Query4.DashItem;
                        modelQuery4.DashItemType = modelItem.Query4.DashItemType;
                        modelQuery4.Date = modelItem.Query4.Date;
                        modelQuery4.Description = modelItem.Query4.Description;
                        modelQuery4.GroupByMonth = modelItem.Query4.GroupByMonth;
                        modelQuery4.GroupByQuarter = modelItem.Query4.GroupByQuarter;
                        modelQuery4.GroupByYear = modelItem.Query4.GroupByYear;
                        modelQuery4.ProductGroup = modelItem.Query4.ProductGroup;
                        modelQuery4.User = modelItem.Query4.User;
                        modelQuery4.SourceCountryGroup = modelItem.Query4.SourceCountryGroup;
                        modelQuery4.TradeFlowType = modelItem.Query4.TradeFlowType;
                        modelQuery4.MarketCountryGroup = modelItem.Query4.MarketCountryGroup;
                        modelQuery4.ProductGroupType = modelItem.Query4.ProductGroupType;
                        modelQuery4.TonnesValuesGroup = modelItem.Query4.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query4.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery4.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query4.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery4.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query4.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery4.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query4.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery4.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query4.Ports == null)
                        {
                            modelItem.Query4.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query4.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery4.Ports = portList;
                    }
                }

                var mItem = new UserDashboardItemsDB
                {
                    ID = modelItem.ID,
                    HTML = modelItem.HTML,
                    Type = modelItem.Type,
                    SubTitle1 = modelItem.SubTitle1,
                    SubTitle2 = modelItem.SubTitle2,
                    SubTitle3 = modelItem.SubTitle3,
                    SubTitle4 = modelItem.SubTitle4,
                    Title1 = modelItem.Title1,
                    Title2 = modelItem.Title2,
                    Title3 = modelItem.Title3,
                    Title4 = modelItem.Title4,
                    SelectedIndex1 = modelItem.SelectedIndex1,
                    SelectedIndex2 = modelItem.SelectedIndex2,
                    SelectedIndex3 = modelItem.SelectedIndex3,
                    SelectedIndex4 = modelItem.SelectedIndex4,
                    Query1 = modelQuery1,
                    Query2 = modelQuery2,
                    Query3 = modelQuery3,
                    Query4 = modelQuery4
                };
                modelList.Add(mItem);
            }

            var modelDB = new UserDashboardDB
            {
                _id = results[0]._id,
                ID = model.ID,
                Description = model.Description,
                Date = DateTime.Now,
                DashBoardName = model.DashBoardName,
                UserIdentityName = model.UserIdentityName,
                UserName = model.UserName,
                bDesignMode = model.bDesignMode,
                Items = modelList
            };

            await db.UserDashboardDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;

            return bReturn;

        }

        public async Task<bool> UpdateDashBoardToComplete(UserDashboardModel model)
        {
            bool bReturn = false;
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();
            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x._id, RecordId);

            var cursor = await db.UserDashboardDB.FindAsync(filter);
            IList<UserDashboardDB> results = cursor.ToList();

            var modelList = new List<UserDashboardItemsDB>();

            foreach (var modelItem in model.Items)
            {
                var modelQuery1 = new DashBoardQueryDB();
                if (modelItem.Query1 != null)
                {
                    if (modelItem.Query1.SourceCountryGEOs != null)
                    {
                        modelQuery1.Id = modelItem.Query1.Id;
                        modelQuery1.IncludePorts = modelItem.Query1.IncludePorts;
                        modelQuery1.Name = modelItem.Query1.Name;
                        modelQuery1.PortGroup = modelItem.Query1.PortGroup;
                        modelQuery1.Comments = modelItem.Query1.Comments;
                        modelQuery1.DashItem = modelItem.Query1.DashItem;
                        modelQuery1.DashItemType = modelItem.Query1.DashItemType;
                        modelQuery1.Date = modelItem.Query1.Date;
                        modelQuery1.Description = modelItem.Query1.Description;
                        modelQuery1.GroupByMonth = modelItem.Query1.GroupByMonth;
                        modelQuery1.GroupByQuarter = modelItem.Query1.GroupByQuarter;
                        modelQuery1.GroupByYear = modelItem.Query1.GroupByYear;
                        modelQuery1.ProductGroup = modelItem.Query1.ProductGroup;
                        modelQuery1.User = modelItem.Query1.User;
                        modelQuery1.SourceCountryGroup = modelItem.Query1.SourceCountryGroup;
                        modelQuery1.TradeFlowType = modelItem.Query1.TradeFlowType;
                        modelQuery1.MarketCountryGroup = modelItem.Query1.MarketCountryGroup;
                        modelQuery1.ProductGroupType = modelItem.Query1.ProductGroupType;
                        modelQuery1.TonnesValuesGroup = modelItem.Query1.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query1.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery1.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query1.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery1.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query1.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery1.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query1.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery1.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query1.Ports == null)
                        {
                            modelItem.Query1.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query1.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery1.Ports = portList;
                    }
                }
                var modelQuery2 = new DashBoardQueryDB();
                if (modelItem.Query2 != null)
                {
                    if (modelItem.Query2.SourceCountryGEOs != null)
                    {
                        modelQuery2.Id = modelItem.Query2.Id;
                        modelQuery2.IncludePorts = modelItem.Query2.IncludePorts;
                        modelQuery2.Name = modelItem.Query2.Name;
                        modelQuery2.PortGroup = modelItem.Query2.PortGroup;
                        modelQuery2.Comments = modelItem.Query2.Comments;
                        modelQuery2.DashItem = modelItem.Query2.DashItem;
                        modelQuery2.DashItemType = modelItem.Query2.DashItemType;
                        modelQuery2.Date = modelItem.Query2.Date;
                        modelQuery2.Description = modelItem.Query2.Description;
                        modelQuery2.GroupByMonth = modelItem.Query2.GroupByMonth;
                        modelQuery2.GroupByQuarter = modelItem.Query2.GroupByQuarter;
                        modelQuery2.GroupByYear = modelItem.Query2.GroupByYear;
                        modelQuery2.ProductGroup = modelItem.Query2.ProductGroup;
                        modelQuery2.User = modelItem.Query2.User;
                        modelQuery2.SourceCountryGroup = modelItem.Query2.SourceCountryGroup;
                        modelQuery2.TradeFlowType = modelItem.Query2.TradeFlowType;
                        modelQuery2.MarketCountryGroup = modelItem.Query2.MarketCountryGroup;
                        modelQuery2.ProductGroupType = modelItem.Query2.ProductGroupType;
                        modelQuery2.TonnesValuesGroup = modelItem.Query2.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query2.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery2.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query2.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery2.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query2.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery2.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query2.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery2.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query2.Ports == null)
                        {
                            modelItem.Query2.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query2.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery2.Ports = portList;
                    }

                }
                var modelQuery3 = new DashBoardQueryDB();
                if (modelItem.Query3 != null)
                {
                    if (modelItem.Query3.SourceCountryGEOs != null)
                    {
                        modelQuery3.Id = modelItem.Query3.Id;
                        modelQuery3.IncludePorts = modelItem.Query3.IncludePorts;
                        modelQuery3.Name = modelItem.Query3.Name;
                        modelQuery3.PortGroup = modelItem.Query3.PortGroup;
                        modelQuery3.Comments = modelItem.Query3.Comments;
                        modelQuery3.DashItem = modelItem.Query3.DashItem;
                        modelQuery3.DashItemType = modelItem.Query3.DashItemType;
                        modelQuery3.Date = modelItem.Query3.Date;
                        modelQuery3.Description = modelItem.Query3.Description;
                        modelQuery3.GroupByMonth = modelItem.Query3.GroupByMonth;
                        modelQuery3.GroupByQuarter = modelItem.Query3.GroupByQuarter;
                        modelQuery3.GroupByYear = modelItem.Query3.GroupByYear;
                        modelQuery3.ProductGroup = modelItem.Query3.ProductGroup;
                        modelQuery3.User = modelItem.Query3.User;
                        modelQuery3.SourceCountryGroup = modelItem.Query3.SourceCountryGroup;
                        modelQuery3.TradeFlowType = modelItem.Query3.TradeFlowType;
                        modelQuery3.MarketCountryGroup = modelItem.Query3.MarketCountryGroup;
                        modelQuery3.ProductGroupType = modelItem.Query3.ProductGroupType;
                        modelQuery3.TonnesValuesGroup = modelItem.Query3.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query3.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery3.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query3.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery3.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query3.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery3.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query3.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery3.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query3.Ports == null)
                        {
                            modelItem.Query3.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query3.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery3.Ports = portList;
                    }
                }
                var modelQuery4 = new DashBoardQueryDB();
                if (modelItem.Query4 != null)
                {
                    if (modelItem.Query4.SourceCountryGEOs != null)
                    {
                        modelQuery4.Id = modelItem.Query4.Id;
                        modelQuery4.IncludePorts = modelItem.Query4.IncludePorts;
                        modelQuery4.Name = modelItem.Query4.Name;
                        modelQuery4.PortGroup = modelItem.Query4.PortGroup;
                        modelQuery4.Comments = modelItem.Query4.Comments;
                        modelQuery4.DashItem = modelItem.Query4.DashItem;
                        modelQuery4.DashItemType = modelItem.Query4.DashItemType;
                        modelQuery4.Date = modelItem.Query4.Date;
                        modelQuery4.Description = modelItem.Query4.Description;
                        modelQuery4.GroupByMonth = modelItem.Query4.GroupByMonth;
                        modelQuery4.GroupByQuarter = modelItem.Query4.GroupByQuarter;
                        modelQuery4.GroupByYear = modelItem.Query4.GroupByYear;
                        modelQuery4.ProductGroup = modelItem.Query4.ProductGroup;
                        modelQuery4.User = modelItem.Query4.User;
                        modelQuery4.SourceCountryGroup = modelItem.Query4.SourceCountryGroup;
                        modelQuery4.TradeFlowType = modelItem.Query4.TradeFlowType;
                        modelQuery4.MarketCountryGroup = modelItem.Query4.MarketCountryGroup;
                        modelQuery4.ProductGroupType = modelItem.Query4.ProductGroupType;
                        modelQuery4.TonnesValuesGroup = modelItem.Query4.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesDB>();

                        foreach (var Item in modelItem.Query4.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery4.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesDB>();

                        foreach (var Item in modelItem.Query4.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery4.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsDB>();
                        foreach (var Item in modelItem.Query4.Products)
                        {
                            var pROD = new DashBoardQueryProductsDB { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery4.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateDB>();
                        foreach (var Item in modelItem.Query4.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateDB { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery4.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortDB>();
                        if (modelItem.Query4.Ports == null)
                        {
                            modelItem.Query4.Ports = new List<DashBoardQueryPortModel>();
                        }
                        foreach (var Item in modelItem.Query4.Ports)
                        {
                            var pORT = new DashBoardQueryPortDB { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery4.Ports = portList;
                    }
                }

                var mItem = new UserDashboardItemsDB
                {
                    ID = modelItem.ID,
                    HTML = modelItem.HTML,
                    Type = modelItem.Type,
                    SubTitle1 = modelItem.SubTitle1,
                    SubTitle2 = modelItem.SubTitle2,
                    SubTitle3 = modelItem.SubTitle3,
                    SubTitle4 = modelItem.SubTitle4,
                    Title1 = modelItem.Title1,
                    Title2 = modelItem.Title2,
                    Title3 = modelItem.Title3,
                    Title4 = modelItem.Title4,
                    SelectedIndex1 = modelItem.SelectedIndex1,
                    SelectedIndex2 = modelItem.SelectedIndex2,
                    SelectedIndex3 = modelItem.SelectedIndex3,
                    SelectedIndex4 = modelItem.SelectedIndex4,
                    Query1 = modelQuery1,
                    Query2 = modelQuery2,
                    Query3 = modelQuery3,
                    Query4 = modelQuery4
                };
                modelList.Add(mItem);
            }


            var modelDB = new UserDashboardDB
            {
                _id = results[0]._id,
                ID = model.ID,
                Description = model.Description,
                Date = DateTime.Now,
                DashBoardName = model.DashBoardName,
                UserIdentityName = model.UserIdentityName,
                UserName = model.UserName,
                bDesignMode = model.bDesignMode,
                Items = modelList
            };

            await db.UserDashboardDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;

            return bReturn;

        }

        public async Task<UserDashboardModel> GetCurrentDesignDashboard(string UserIdentityName, bool bDesignMode)
        {
            var builder = Builders<UserDashboardDB>.Filter;

            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x.UserIdentityName, UserIdentityName) & Builders<UserDashboardDB>.Filter.Eq(z => z.bDesignMode, bDesignMode);

            var db = new DBContext();
            var cursor = await db.UserDashboardDB.FindAsync(filter);

            IList<UserDashboardDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                return null;

            }

            var resultItem = results[0];

            var Items = new List<UserDashboardItemsModel>();
            foreach (var lItem in resultItem.Items)
            {

                var modelQuery1 = new DashBoardQueryModel();
                if (lItem.Query1 != null)
                {
                    if (lItem.Query1.SourceCountryGEOs != null)
                    {
                        
                        modelQuery1.Id = lItem.Query1.Id;
                        modelQuery1.IncludePorts = lItem.Query1.IncludePorts;
                        modelQuery1.Name = lItem.Query1.Name;
                        modelQuery1.PortGroup = lItem.Query1.PortGroup;
                        modelQuery1.Comments = lItem.Query1.Comments;
                        modelQuery1.DashItem = lItem.Query1.DashItem;
                        modelQuery1.DashItemType = lItem.Query1.DashItemType;
                        modelQuery1.Date = lItem.Query1.Date;
                        modelQuery1.Description = lItem.Query1.Description;
                        modelQuery1.GroupByMonth = lItem.Query1.GroupByMonth;
                        modelQuery1.GroupByQuarter = lItem.Query1.GroupByQuarter;
                        modelQuery1.GroupByYear = lItem.Query1.GroupByYear;
                        modelQuery1.ProductGroup = lItem.Query1.ProductGroup;
                        modelQuery1.User = lItem.Query1.User;
                        modelQuery1.SourceCountryGroup = lItem.Query1.SourceCountryGroup;
                        modelQuery1.TradeFlowType = lItem.Query1.TradeFlowType;
                        modelQuery1.MarketCountryGroup = lItem.Query1.MarketCountryGroup;
                        modelQuery1.ProductGroupType = lItem.Query1.ProductGroupType;
                        modelQuery1.TonnesValuesGroup = lItem.Query1.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query1.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery1.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query1.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery1.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query1.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery1.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query1.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery1.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query1.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery1.Ports = portList;
                    }
                }

                var modelQuery2 = new DashBoardQueryModel();
                if (lItem.Query2 != null)
                {
                    if (lItem.Query2.SourceCountryGEOs != null)
                    {
                        modelQuery2.Id = lItem.Query2.Id;
                        modelQuery2.IncludePorts = lItem.Query2.IncludePorts;
                        modelQuery2.Name = lItem.Query2.Name;
                        modelQuery2.PortGroup = lItem.Query2.PortGroup;
                        modelQuery2.Comments = lItem.Query2.Comments;
                        modelQuery2.DashItem = lItem.Query2.DashItem;
                        modelQuery2.DashItemType = lItem.Query2.DashItemType;
                        modelQuery2.Date = lItem.Query2.Date;
                        modelQuery2.Description = lItem.Query2.Description;
                        modelQuery2.GroupByMonth = lItem.Query2.GroupByMonth;
                        modelQuery2.GroupByQuarter = lItem.Query2.GroupByQuarter;
                        modelQuery2.GroupByYear = lItem.Query2.GroupByYear;
                        modelQuery2.ProductGroup = lItem.Query2.ProductGroup;
                        modelQuery2.User = lItem.Query2.User;
                        modelQuery2.SourceCountryGroup = lItem.Query2.SourceCountryGroup;
                        modelQuery2.TradeFlowType = lItem.Query2.TradeFlowType;
                        modelQuery2.MarketCountryGroup = lItem.Query2.MarketCountryGroup;
                        modelQuery2.ProductGroupType = lItem.Query2.ProductGroupType;
                        modelQuery2.TonnesValuesGroup = lItem.Query2.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query2.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery2.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query2.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery2.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query2.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery2.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query2.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery2.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query2.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery2.Ports = portList;
                    }
                }
                var modelQuery3 = new DashBoardQueryModel();
                if (lItem.Query3 != null)
                {
                    if (lItem.Query3.SourceCountryGEOs != null)
                    {
                        modelQuery3.Id = lItem.Query3.Id;
                        modelQuery3.IncludePorts = lItem.Query3.IncludePorts;
                        modelQuery3.Name = lItem.Query3.Name;
                        modelQuery3.PortGroup = lItem.Query3.PortGroup;
                        modelQuery3.Comments = lItem.Query3.Comments;
                        modelQuery3.DashItem = lItem.Query3.DashItem;
                        modelQuery3.DashItemType = lItem.Query3.DashItemType;
                        modelQuery3.Date = lItem.Query3.Date;
                        modelQuery3.Description = lItem.Query3.Description;
                        modelQuery3.GroupByMonth = lItem.Query3.GroupByMonth;
                        modelQuery3.GroupByQuarter = lItem.Query3.GroupByQuarter;
                        modelQuery3.GroupByYear = lItem.Query3.GroupByYear;
                        modelQuery3.ProductGroup = lItem.Query3.ProductGroup;
                        modelQuery3.User = lItem.Query3.User;
                        modelQuery3.SourceCountryGroup = lItem.Query3.SourceCountryGroup;
                        modelQuery3.TradeFlowType = lItem.Query3.TradeFlowType;
                        modelQuery3.MarketCountryGroup = lItem.Query3.MarketCountryGroup;
                        modelQuery3.ProductGroupType = lItem.Query3.ProductGroupType;
                        modelQuery3.TonnesValuesGroup = lItem.Query3.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query3.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery3.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query3.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery3.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query3.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery3.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query3.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery3.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query3.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery3.Ports = portList;
                    }
                }
                var modelQuery4 = new DashBoardQueryModel();
                if (lItem.Query4 != null)
                {
                    if (lItem.Query4.SourceCountryGEOs != null)
                    {
                        modelQuery4.Id = lItem.Query4.Id;
                        modelQuery4.IncludePorts = lItem.Query4.IncludePorts;
                        modelQuery4.Name = lItem.Query4.Name;
                        modelQuery4.PortGroup = lItem.Query4.PortGroup;
                        modelQuery4.Comments = lItem.Query4.Comments;
                        modelQuery4.DashItem = lItem.Query4.DashItem;
                        modelQuery4.DashItemType = lItem.Query4.DashItemType;
                        modelQuery4.Date = lItem.Query4.Date;
                        modelQuery4.Description = lItem.Query4.Description;
                        modelQuery4.GroupByMonth = lItem.Query4.GroupByMonth;
                        modelQuery4.GroupByQuarter = lItem.Query4.GroupByQuarter;
                        modelQuery4.GroupByYear = lItem.Query4.GroupByYear;
                        modelQuery4.ProductGroup = lItem.Query4.ProductGroup;
                        modelQuery4.User = lItem.Query4.User;
                        modelQuery4.SourceCountryGroup = lItem.Query4.SourceCountryGroup;
                        modelQuery4.TradeFlowType = lItem.Query4.TradeFlowType;
                        modelQuery4.MarketCountryGroup = lItem.Query4.MarketCountryGroup;
                        modelQuery4.ProductGroupType = lItem.Query4.ProductGroupType;
                        modelQuery4.TonnesValuesGroup = lItem.Query4.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query4.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery4.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query4.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery4.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query4.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery4.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query4.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery4.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query4.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery4.Ports = portList;
                    }
                }

                var lModel = new UserDashboardItemsModel
                {
                    ID = lItem.ID,
                    Type = lItem.Type,
                    SubTitle1 = lItem.SubTitle1,
                    SubTitle2 = lItem.SubTitle2,
                    SubTitle3 = lItem.SubTitle3,
                    SubTitle4 = lItem.SubTitle4,
                    Title1 = lItem.Title1,
                    Title2 = lItem.Title2,
                    Title3 = lItem.Title3,
                    Title4 = lItem.Title4,
                    SelectedIndex1 = lItem.SelectedIndex1,
                    SelectedIndex2 = lItem.SelectedIndex2,
                    SelectedIndex3 = lItem.SelectedIndex3,
                    SelectedIndex4 = lItem.SelectedIndex4,
                    HTML = lItem.HTML,
                    Query1 = modelQuery1,
                    Query2 = modelQuery2,
                    Query3 = modelQuery3,
                    Query4 = modelQuery4
                };

                Items.Add(lModel);
            }

            var model = new UserDashboardModel
            {
                _id = resultItem._id.ToString(),
                ID = resultItem.ID,
                bDesignMode = resultItem.bDesignMode,
                Date = resultItem.Date,
                DashBoardName = resultItem.DashBoardName,
                Description = resultItem.Description,
                UserIdentityName = resultItem.UserIdentityName,
                UserName = resultItem.UserName,
                Items = Items,
            };

            return model;
        }

        public async Task<UserDashboardModel> GetCurrentDashboardByGUID(string Id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));
            var builder = Builders<UserDashboardDB>.Filter;

            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x._id, RecordId);

            var db = new DBContext();
            var cursor = await db.UserDashboardDB.FindAsync(filter);

            IList<UserDashboardDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                return null;

            }

            var resultItem = results[0];

            var Items = new List<UserDashboardItemsModel>();
            foreach (var lItem in resultItem.Items)
            {

                var modelQuery1 = new DashBoardQueryModel();
                if (lItem.Query1 != null)
                {
                    if (lItem.Query1.SourceCountryGEOs != null)
                    {


                        modelQuery1.Id = lItem.Query1.Id;
                        modelQuery1.IncludePorts = lItem.Query1.IncludePorts;
                        modelQuery1.Name = lItem.Query1.Name;
                        modelQuery1.PortGroup = lItem.Query1.PortGroup;
                        modelQuery1.Comments = lItem.Query1.Comments;
                        modelQuery1.DashItem = lItem.Query1.DashItem;
                        modelQuery1.DashItemType = lItem.Query1.DashItemType;
                        modelQuery1.Date = lItem.Query1.Date;
                        modelQuery1.Description = lItem.Query1.Description;
                        modelQuery1.GroupByMonth = lItem.Query1.GroupByMonth;
                        modelQuery1.GroupByQuarter = lItem.Query1.GroupByQuarter;
                        modelQuery1.GroupByYear = lItem.Query1.GroupByYear;
                        modelQuery1.ProductGroup = lItem.Query1.ProductGroup;
                        modelQuery1.User = lItem.Query1.User;
                        modelQuery1.SourceCountryGroup = lItem.Query1.SourceCountryGroup;
                        modelQuery1.TradeFlowType = lItem.Query1.TradeFlowType;
                        modelQuery1.MarketCountryGroup = lItem.Query1.MarketCountryGroup;
                        modelQuery1.ProductGroupType = lItem.Query1.ProductGroupType;
                        modelQuery1.TonnesValuesGroup = lItem.Query1.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query1.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery1.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query1.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery1.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query1.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery1.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query1.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery1.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query1.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery1.Ports = portList;
                    }
                }

                var modelQuery2 = new DashBoardQueryModel();
                if (lItem.Query2 != null)
                {
                    if (lItem.Query2.SourceCountryGEOs != null)
                    {
                        modelQuery2.Id = lItem.Query2.Id;
                        modelQuery2.IncludePorts = lItem.Query2.IncludePorts;
                        modelQuery2.Name = lItem.Query2.Name;
                        modelQuery2.PortGroup = lItem.Query2.PortGroup;
                        modelQuery2.Comments = lItem.Query2.Comments;
                        modelQuery2.DashItem = lItem.Query2.DashItem;
                        modelQuery2.DashItemType = lItem.Query2.DashItemType;
                        modelQuery2.Date = lItem.Query2.Date;
                        modelQuery2.Description = lItem.Query2.Description;
                        modelQuery2.GroupByMonth = lItem.Query2.GroupByMonth;
                        modelQuery2.GroupByQuarter = lItem.Query2.GroupByQuarter;
                        modelQuery2.GroupByYear = lItem.Query2.GroupByYear;
                        modelQuery2.ProductGroup = lItem.Query2.ProductGroup;
                        modelQuery2.User = lItem.Query2.User;
                        modelQuery2.SourceCountryGroup = lItem.Query2.SourceCountryGroup;
                        modelQuery2.TradeFlowType = lItem.Query2.TradeFlowType;
                        modelQuery2.MarketCountryGroup = lItem.Query2.MarketCountryGroup;
                        modelQuery2.ProductGroupType = lItem.Query2.ProductGroupType;
                        modelQuery2.TonnesValuesGroup = lItem.Query2.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query2.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery2.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query2.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery2.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query2.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery2.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query2.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery2.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query2.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery2.Ports = portList;
                    }
                }
                var modelQuery3 = new DashBoardQueryModel();
                if (lItem.Query3 != null)
                {
                    if (lItem.Query3.SourceCountryGEOs != null)
                    {
                        modelQuery3.Id = lItem.Query3.Id;
                        modelQuery3.IncludePorts = lItem.Query3.IncludePorts;
                        modelQuery3.Name = lItem.Query3.Name;
                        modelQuery3.PortGroup = lItem.Query3.PortGroup;
                        modelQuery3.Comments = lItem.Query3.Comments;
                        modelQuery3.DashItem = lItem.Query3.DashItem;
                        modelQuery3.DashItemType = lItem.Query3.DashItemType;
                        modelQuery3.Date = lItem.Query3.Date;
                        modelQuery3.Description = lItem.Query3.Description;
                        modelQuery3.GroupByMonth = lItem.Query3.GroupByMonth;
                        modelQuery3.GroupByQuarter = lItem.Query3.GroupByQuarter;
                        modelQuery3.GroupByYear = lItem.Query3.GroupByYear;
                        modelQuery3.ProductGroup = lItem.Query3.ProductGroup;
                        modelQuery3.User = lItem.Query3.User;
                        modelQuery3.SourceCountryGroup = lItem.Query3.SourceCountryGroup;
                        modelQuery3.TradeFlowType = lItem.Query3.TradeFlowType;
                        modelQuery3.MarketCountryGroup = lItem.Query3.MarketCountryGroup;
                        modelQuery3.ProductGroupType = lItem.Query3.ProductGroupType;
                        modelQuery3.TonnesValuesGroup = lItem.Query3.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query3.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery3.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query3.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery3.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query3.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery3.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query3.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery3.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query3.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery3.Ports = portList;
                    }
                }
                var modelQuery4 = new DashBoardQueryModel();
                if (lItem.Query4 != null)
                {
                    if (lItem.Query4.SourceCountryGEOs != null)
                    {
                        modelQuery4.Id = lItem.Query4.Id;
                        modelQuery4.IncludePorts = lItem.Query4.IncludePorts;
                        modelQuery4.Name = lItem.Query4.Name;
                        modelQuery4.PortGroup = lItem.Query4.PortGroup;
                        modelQuery4.Comments = lItem.Query4.Comments;
                        modelQuery4.DashItem = lItem.Query4.DashItem;
                        modelQuery4.DashItemType = lItem.Query4.DashItemType;
                        modelQuery4.Date = lItem.Query4.Date;
                        modelQuery4.Description = lItem.Query4.Description;
                        modelQuery4.GroupByMonth = lItem.Query4.GroupByMonth;
                        modelQuery4.GroupByQuarter = lItem.Query4.GroupByQuarter;
                        modelQuery4.GroupByYear = lItem.Query4.GroupByYear;
                        modelQuery4.ProductGroup = lItem.Query4.ProductGroup;
                        modelQuery4.User = lItem.Query4.User;
                        modelQuery4.SourceCountryGroup = lItem.Query4.SourceCountryGroup;
                        modelQuery4.TradeFlowType = lItem.Query4.TradeFlowType;
                        modelQuery4.MarketCountryGroup = lItem.Query4.MarketCountryGroup;
                        modelQuery4.ProductGroupType = lItem.Query4.ProductGroupType;
                        modelQuery4.TonnesValuesGroup = lItem.Query4.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query4.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery4.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query4.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery4.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query4.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery4.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query4.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery4.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query4.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery4.Ports = portList;
                    }
                }

                var lModel = new UserDashboardItemsModel
                {
                    ID = lItem.ID,
                    Type = lItem.Type,
                    SubTitle1 = lItem.SubTitle1,
                    SubTitle2 = lItem.SubTitle2,
                    SubTitle3 = lItem.SubTitle3,
                    SubTitle4 = lItem.SubTitle4,
                    Title1 = lItem.Title1,
                    Title2 = lItem.Title2,
                    Title3 = lItem.Title3,
                    Title4 = lItem.Title4,
                    SelectedIndex1 = lItem.SelectedIndex1,
                    SelectedIndex2 = lItem.SelectedIndex2,
                    SelectedIndex3 = lItem.SelectedIndex3,
                    SelectedIndex4 = lItem.SelectedIndex4,
                    HTML = lItem.HTML,
                    Query1 = modelQuery1,
                    Query2 = modelQuery2,
                    Query3 = modelQuery3,
                    Query4 = modelQuery4
                };

                Items.Add(lModel);
            }

            var model = new UserDashboardModel
            {
                _id = resultItem._id.ToString(),
                ID = resultItem.ID,
                Date = resultItem.Date,
                bDesignMode = resultItem.bDesignMode,
                DashBoardName = resultItem.DashBoardName,
                Description = resultItem.Description,
                UserIdentityName = resultItem.UserIdentityName,
                UserName = resultItem.UserName,
                Items = Items,
            };

            return model;
        }

        public async Task<UserDashboardModel> GetCurrentDashboardByID(string Id,bool bStatus)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));
            var builder = Builders<UserDashboardDB>.Filter;

            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x._id, RecordId) & Builders<UserDashboardDB>.Filter.Eq(z => z.bDesignMode, bStatus);

            var db = new DBContext();
            var cursor = await db.UserDashboardDB.FindAsync(filter);

            IList<UserDashboardDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                return null;

            }

            var resultItem = results[0];

            var Items = new List<UserDashboardItemsModel>();
            foreach (var lItem in resultItem.Items)
            {

                var modelQuery1 = new DashBoardQueryModel();
                if (lItem.Query1 != null)
                {
                    if (lItem.Query1.SourceCountryGEOs != null)
                    {


                        modelQuery1.Id = lItem.Query1.Id;
                        modelQuery1.IncludePorts = lItem.Query1.IncludePorts;
                        modelQuery1.Name = lItem.Query1.Name;
                        modelQuery1.PortGroup = lItem.Query1.PortGroup;
                        modelQuery1.Comments = lItem.Query1.Comments;
                        modelQuery1.DashItem = lItem.Query1.DashItem;
                        modelQuery1.DashItemType = lItem.Query1.DashItemType;
                        modelQuery1.Date = lItem.Query1.Date;
                        modelQuery1.Description = lItem.Query1.Description;
                        modelQuery1.GroupByMonth = lItem.Query1.GroupByMonth;
                        modelQuery1.GroupByQuarter = lItem.Query1.GroupByQuarter;
                        modelQuery1.GroupByYear = lItem.Query1.GroupByYear;
                        modelQuery1.ProductGroup = lItem.Query1.ProductGroup;
                        modelQuery1.User = lItem.Query1.User;
                        modelQuery1.SourceCountryGroup = lItem.Query1.SourceCountryGroup;
                        modelQuery1.TradeFlowType = lItem.Query1.TradeFlowType;
                        modelQuery1.MarketCountryGroup = lItem.Query1.MarketCountryGroup;
                        modelQuery1.ProductGroupType = lItem.Query1.ProductGroupType;
                        modelQuery1.TonnesValuesGroup = lItem.Query1.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query1.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery1.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query1.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery1.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query1.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery1.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query1.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery1.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query1.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery1.Ports = portList;
                    }
                }

                var modelQuery2 = new DashBoardQueryModel();
                if (lItem.Query2 != null)
                {
                    if (lItem.Query2.SourceCountryGEOs != null)
                    {
                        modelQuery2.Id = lItem.Query2.Id;
                        modelQuery2.IncludePorts = lItem.Query2.IncludePorts;
                        modelQuery2.Name = lItem.Query2.Name;
                        modelQuery2.PortGroup = lItem.Query2.PortGroup;
                        modelQuery2.Comments = lItem.Query2.Comments;
                        modelQuery2.DashItem = lItem.Query2.DashItem;
                        modelQuery2.DashItemType = lItem.Query2.DashItemType;
                        modelQuery2.Date = lItem.Query2.Date;
                        modelQuery2.Description = lItem.Query2.Description;
                        modelQuery2.GroupByMonth = lItem.Query2.GroupByMonth;
                        modelQuery2.GroupByQuarter = lItem.Query2.GroupByQuarter;
                        modelQuery2.GroupByYear = lItem.Query2.GroupByYear;
                        modelQuery2.ProductGroup = lItem.Query2.ProductGroup;
                        modelQuery2.User = lItem.Query2.User;
                        modelQuery2.SourceCountryGroup = lItem.Query2.SourceCountryGroup;
                        modelQuery2.TradeFlowType = lItem.Query2.TradeFlowType;
                        modelQuery2.MarketCountryGroup = lItem.Query2.MarketCountryGroup;
                        modelQuery2.ProductGroupType = lItem.Query2.ProductGroupType;
                        modelQuery2.TonnesValuesGroup = lItem.Query2.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query2.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery2.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query2.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery2.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query2.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery2.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query2.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery2.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query2.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery2.Ports = portList;
                    }
                }
                var modelQuery3 = new DashBoardQueryModel();
                if (lItem.Query3 != null)
                {
                    if (lItem.Query3.SourceCountryGEOs != null)
                    {
                        modelQuery3.Id = lItem.Query3.Id;
                        modelQuery3.IncludePorts = lItem.Query3.IncludePorts;
                        modelQuery3.Name = lItem.Query3.Name;
                        modelQuery3.PortGroup = lItem.Query3.PortGroup;
                        modelQuery3.Comments = lItem.Query3.Comments;
                        modelQuery3.DashItem = lItem.Query3.DashItem;
                        modelQuery3.DashItemType = lItem.Query3.DashItemType;
                        modelQuery3.Date = lItem.Query3.Date;
                        modelQuery3.Description = lItem.Query3.Description;
                        modelQuery3.GroupByMonth = lItem.Query3.GroupByMonth;
                        modelQuery3.GroupByQuarter = lItem.Query3.GroupByQuarter;
                        modelQuery3.GroupByYear = lItem.Query3.GroupByYear;
                        modelQuery3.ProductGroup = lItem.Query3.ProductGroup;
                        modelQuery3.User = lItem.Query3.User;
                        modelQuery3.SourceCountryGroup = lItem.Query3.SourceCountryGroup;
                        modelQuery3.TradeFlowType = lItem.Query3.TradeFlowType;
                        modelQuery3.MarketCountryGroup = lItem.Query3.MarketCountryGroup;
                        modelQuery3.ProductGroupType = lItem.Query3.ProductGroupType;
                        modelQuery3.TonnesValuesGroup = lItem.Query3.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query3.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery3.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query3.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery3.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query3.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery3.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query3.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery3.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query3.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery3.Ports = portList;
                    }
                }
                var modelQuery4 = new DashBoardQueryModel();
                if (lItem.Query4 != null)
                {
                    if (lItem.Query4.SourceCountryGEOs != null)
                    {
                        modelQuery4.Id = lItem.Query4.Id;
                        modelQuery4.IncludePorts = lItem.Query4.IncludePorts;
                        modelQuery4.Name = lItem.Query4.Name;
                        modelQuery4.PortGroup = lItem.Query4.PortGroup;
                        modelQuery4.Comments = lItem.Query4.Comments;
                        modelQuery4.DashItem = lItem.Query4.DashItem;
                        modelQuery4.DashItemType = lItem.Query4.DashItemType;
                        modelQuery4.Date = lItem.Query4.Date;
                        modelQuery4.Description = lItem.Query4.Description;
                        modelQuery4.GroupByMonth = lItem.Query4.GroupByMonth;
                        modelQuery4.GroupByQuarter = lItem.Query4.GroupByQuarter;
                        modelQuery4.GroupByYear = lItem.Query4.GroupByYear;
                        modelQuery4.ProductGroup = lItem.Query4.ProductGroup;
                        modelQuery4.User = lItem.Query4.User;
                        modelQuery4.SourceCountryGroup = lItem.Query4.SourceCountryGroup;
                        modelQuery4.TradeFlowType = lItem.Query4.TradeFlowType;
                        modelQuery4.MarketCountryGroup = lItem.Query4.MarketCountryGroup;
                        modelQuery4.ProductGroupType = lItem.Query4.ProductGroupType;
                        modelQuery4.TonnesValuesGroup = lItem.Query4.TonnesValuesGroup;

                        var sGeos = new List<DashBoardQuerySourceCountriesModel>();

                        foreach (var Item in lItem.Query4.SourceCountryGEOs)
                        {
                            var dGeo = new DashBoardQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                            sGeos.Add(dGeo);
                        }
                        modelQuery4.SourceCountryGEOs = sGeos;

                        var mGeos = new List<DashBoardQueryMarketCountriesModel>();

                        foreach (var Item in lItem.Query4.MarketCountryGEOs)
                        {
                            var dGeo = new DashBoardQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                            mGeos.Add(dGeo);
                        }
                        modelQuery4.MarketCountryGEOs = mGeos;

                        var prodList = new List<DashBoardQueryProductsModel>();
                        foreach (var Item in lItem.Query4.Products)
                        {
                            var pROD = new DashBoardQueryProductsModel { Product = Item.Product };
                            prodList.Add(pROD);
                        }
                        modelQuery4.Products = prodList;

                        var monthList = new List<DashBoardSaveQueryDateModel>();
                        foreach (var Item in lItem.Query4.TimeIDs)
                        {
                            var mONTH = new DashBoardSaveQueryDateModel { TimeID = Item.TimeID };
                            monthList.Add(mONTH);
                        }
                        modelQuery4.TimeIDs = monthList;

                        var portList = new List<DashBoardQueryPortModel>();
                        foreach (var Item in lItem.Query4.Ports)
                        {
                            var pORT = new DashBoardQueryPortModel { PortID = Item.PortID };
                            portList.Add(pORT);
                        }
                        modelQuery4.Ports = portList;
                    }
                }

                var lModel = new UserDashboardItemsModel
                {
                    ID = lItem.ID,
                    Type = lItem.Type,
                    SubTitle1 = lItem.SubTitle1,
                    SubTitle2 = lItem.SubTitle2,
                    SubTitle3 = lItem.SubTitle3,
                    SubTitle4 = lItem.SubTitle4,
                    Title1 = lItem.Title1,
                    Title2 = lItem.Title2,
                    Title3 = lItem.Title3,
                    Title4 = lItem.Title4,
                    SelectedIndex1 = lItem.SelectedIndex1,
                    SelectedIndex2 = lItem.SelectedIndex2,
                    SelectedIndex3 = lItem.SelectedIndex3,
                    SelectedIndex4 = lItem.SelectedIndex4,
                    HTML = lItem.HTML,
                    Query1 = modelQuery1,
                    Query2 = modelQuery2,
                    Query3 = modelQuery3,
                    Query4 = modelQuery4
                };

                Items.Add(lModel);
            }

            var model = new UserDashboardModel
            {
                _id = resultItem._id.ToString(),
                ID = resultItem.ID,
                Date = resultItem.Date,
                bDesignMode = resultItem.bDesignMode,
                DashBoardName = resultItem.DashBoardName,
                Description = resultItem.Description,
                UserIdentityName = resultItem.UserIdentityName,
                UserName = resultItem.UserName,
                Items = Items,
            };

            return model;
        }

        public async Task<List<UserDashboardModel>> GetUserDashboards(string User)
        {
            var builder = Builders<UserDashboardDB>.Filter;
         
            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x.UserIdentityName, User) & Builders<UserDashboardDB>.Filter.Eq(z => z.bDesignMode, false);

            var db = new DBContext();
            var cursor = await db.UserDashboardDB.FindAsync(filter);

            IList<UserDashboardDB> results = cursor.ToList();
            var modelList = new List<UserDashboardModel>();

            foreach (var Record in results)
            {

                var model = new UserDashboardModel
                {
                    _id = Record._id.ToString(),
                    bDesignMode = Record.bDesignMode,
                    ID = Record.ID,
                    Date = Record.Date,
                    UserName = Record.UserName,
                    DashBoardName = Record.DashBoardName,
                    Description = Record.Description,
                    UserIdentityName = Record.UserIdentityName

                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x._id).ToList();
        }

        public async Task<bool> Delete(string Id)
        {
            var builder = Builders<UserDashboardDB>.Filter;
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));

            var filter = Builders<UserDashboardDB>.Filter.Eq(x => x._id, RecordId);
            var db = new DBContext();

            await db.UserDashboardDB.DeleteOneAsync(filter);
            return true;
        }
    }
}
