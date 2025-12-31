using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using Services;

namespace ISSB.Helpers
{
    public class MailBoxServices
    {
        public class MailBoxService : IHostedService
        {

            public Task StartAsync(CancellationToken cancellationToken)
            {
                //Task.Run(TaskRoutine, cancellationToken);
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
                DateTime nextStop = DateTime.Now.AddMinutes(1);
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
                    var srv = new EmailService();
                    await srv.ScanMail();
                });
            }
        }
    }
}
