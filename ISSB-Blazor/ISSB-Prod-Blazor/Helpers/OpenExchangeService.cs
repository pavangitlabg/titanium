using System.Timers;
using ISSB_Prod_Blazor.Hubs;
using Services;

namespace ISSB_Prod_Blazor.Helpers;

public sealed class OpenExchangeService(ILogger<OpenExchangeService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("WorkerService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Doing work at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            await UpdateRates();
        }

        logger.LogInformation("WorkerService stopped");
    }
    
    private async Task UpdateRates()
    {
        var srv = OpenExchangeRateService.Instance;
        //Method to rebuild all exchange rates
        //await srv.RebuildRates();
        var ToDay = DateTime.Now.ToString("dd.MM.yyyy");
        string[] dates = ToDay.Split('.');
        //Api update from previous day
        int day = int.Parse(dates[0]) - 1;
        var Start = dates[2] + "-" + dates[1].PadLeft(2, '0') + "-01";
        var End = dates[2] + "-" + dates[1].PadLeft(2, '0') + "-" + day.ToString().PadLeft(2, '0');

        await srv.GetRateByDayRange(Start, End);
        await srv.SetAverageTable(int.Parse(dates[2]), int.Parse(dates[1]));

        // await srv.SetAverageTable(2020, 2);
        //for (int y = 1999; y <= 2019; y++)
        //{
        //    for (int m = 1; m <= 12; m++)
        //    {
        //        await srv.SetAverageTable(y, m);
        //    }
        //}

    }
}