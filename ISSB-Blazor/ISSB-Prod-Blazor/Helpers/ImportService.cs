
using ISSB_Prod_Blazor.Hubs;


namespace ISSB_Prod_Blazor.Helpers;

public class ImportService(MessageService messageService,ILogger<ImportService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("WorkerService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Doing work at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            await DataImport();
        }

        logger.LogInformation("WorkerService stopped");
    }
    
    private async Task DataImport()
    {
        var srv = new DataImportService(messageService);
        
        
        await srv.ScanDirectory();
    }
    
    // void Srv_Callback(string message)
    // {
    //     Task.Run(async () =>
    //     {
    //         //var data = _doStuff.Send(message);
    //        // await _hub.Clients.All.SendAsync("ReceiveMessage", data);
    //     });
    // }

    // void Srv_CallbackPercent(string message)
    // {
    //     Task.Run(async () =>
    //     {
    //        // var data = _doStuff.Send(message);
    //        // await _hub.Clients.All.SendAsync("ReceiveProgressMessage", data);
    //     });
    // }
}