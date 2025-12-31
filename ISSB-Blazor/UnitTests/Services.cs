using Services;
using Xunit.Abstractions;

namespace UnitTests;

public class Services
{
    private readonly ITestOutputHelper _output;

    public Services(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task GetCustomerRecordsTest()
    {
        _output.WriteLine("Test GetCustomerRecords Started ...");
        var dataSrv = new SalesProfileService();
        var data = await dataSrv.GetSalesProfiles();
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetCustomerRecords Finished ...[" + data.Count + "]");
    }

    [Fact]
    public async Task GetUsersTest()
    {
        _output.WriteLine("Test GetUsers Started ...");
        var dataSrv = new UserServices();
        var data = await dataSrv.GetUsersAsync();
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetUsers Finished ...[" + data.Count + "]");
    }
    
    [Fact]
    public async Task GetUsersTheadingTest()
    {
        _output.WriteLine("Test GetUsers Threading Started ...");
        // var dataSrv = new UserServices();
        // var data = await dataSrv.GetUsers();
        
        var usrService = new UserServices();
        var data = (await usrService.GetUsersAsync())
            .AsParallel()
            .AsOrdered()
            .ToList();
        
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetUsers Finished ...[" + data.Count + "]");
    }
   
    [Fact]
    public async Task SourceCountryTest()
    {
        _output.WriteLine("Test GetSourceCountries Started ...");
        var dataSrv = new SourceCountryService();
        var data = await dataSrv.GetSourceCountries();
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetSourceCountries Finished ...");
    }

    [Fact]
    public async Task MarketCountryTest()
    {
        _output.WriteLine("Test GetMarketCountries Started ...");
        var dataSrv = new MarketCountryService();
        var data = await dataSrv.GetMarketAllCountries();
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetMarketCountries Finished ...");
    }

    [Fact]
    public async Task GetPortsTest()
    {
        _output.WriteLine("Test GetPorts Started ...");
        var dataSrv = new PortsService();
        var data = await dataSrv.GetAllPorts();
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetPorts Finished ...[" + data.Count + "]");
    }

    [Fact]
    public async Task GetTariffTest()
    {
        _output.WriteLine("Test GetTariff Started ...");
        var dataSrv = new TariffService();
        var data = await dataSrv.GetTariffsByRegion("028");
        Assert.True(data.Count > 0);
        _output.WriteLine("Test GetTariff Finished ...[" + data.Count + "]");
    }
}