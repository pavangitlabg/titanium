using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ISSB.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Services;


namespace ISSB.Helpers
{
    public class ImportService : IHostedService
    {
        private readonly IMessage _doStuff;
        private readonly IHubContext<MessageHub> _hub;
        private readonly SemaphoreSlim semaphoreImport = new SemaphoreSlim(1, 1);

        public ImportService(IMessage doStuff, IHubContext<MessageHub> hub)
        {
            _doStuff = doStuff;
            _hub = hub;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(() =>
            {
                TaskRoutine();
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Sync Task stopped");
            return null;
        }

        public void TaskRoutine()
        {
            //Change the timer from 1 to 5
            DateTime nextStop = DateTime.Now.AddSeconds(30);
            var timeToWait = nextStop - DateTime.Now;
            var millisToWait = timeToWait.TotalMilliseconds;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = millisToWait;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                if (semaphoreImport.CurrentCount == 0)
                {
                    await _hub.Clients.All.SendAsync("LockMessage", "The system is currently locked as data files are being processed. Please wait.");
                }
                else
                {
                    await _hub.Clients.All.SendAsync("LockMessage", "Clear for processing.");

                }
                await semaphoreImport.WaitAsync();
                try
                {
                    await DataImport();
                }
                finally
                {
                    semaphoreImport.Release();
                }
            });


        }

        private async Task DataImport()
        {
            var srv = DataImportService.Instance;

            srv.Callback -= Srv_Callback;
            srv.CallbackPercent -= Srv_CallbackPercent;

            srv.Callback += Srv_Callback;
            srv.CallbackPercent += Srv_CallbackPercent;

            await srv.ScanDirectory();
        }


        void Srv_Callback(string message)
        {
            Task.Run(async () =>
            {
                var data = _doStuff.Send(message);
                await _hub.Clients.All.SendAsync("ReceiveMessage", data);
            });
        }

        void Srv_CallbackPercent(string message)
        {
            Task.Run(async () =>
            {
                var data = _doStuff.Send(message);
                await _hub.Clients.All.SendAsync("ReceiveProgressMessage", data);
            });
        }
    }
}
