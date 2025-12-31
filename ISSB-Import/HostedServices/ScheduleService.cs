using System;
using System.Threading;
using Data.Models;
using Services;

namespace ISSB.Import.HostedServices
{
    public class ScheduleService : BackgroundService
    {
        public ScheduleService() { }
      
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var stSrv = SystemLockService.Instance;

            while (!stoppingToken.IsCancellationRequested)
            {
                var sModel = await stSrv.GetLockStatus();

                if (!sModel.IsProcessing & sModel.Period is not null & sModel.SelectedRows.Count > 0)
                {
                    sModel.IsProcessing = true;
                    await stSrv.Update(sModel);

                    if (sModel.Period is not null)
                    {
                        DateTime batchDate = sModel.Period ?? DateTime.Now;
                        List<int> selectedCountries = sModel.SelectedRows;
                        _ = new Services.ScheduleService(batchDate, selectedCountries);
                    }
                }
                await Task.Delay(5000);

            }
        }
    }
}

