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
    public class OpenExchangeService : IHostedService
    {
        private readonly IMessage _doStuff;
        private readonly SemaphoreSlim semaphoreExchange = new SemaphoreSlim(1, 1);

        public OpenExchangeService(IMessage doStuff)
        {
            _doStuff = doStuff;
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
            DateTime nextStop = DateTime.Now.AddDays(1);
            var timeToWait = nextStop - DateTime.Now;
            var millisToWait = timeToWait.TotalMilliseconds;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = millisToWait;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                if (semaphoreExchange.CurrentCount == 1)
                {
                    await semaphoreExchange.WaitAsync();
                    try
                    {
                        await UpdateRates();
                    }
                    finally
                    {
                        semaphoreExchange.Release();
                    }
                }
            });

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
}
