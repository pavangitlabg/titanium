using System;
using System.Net.Http;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Services;

namespace ISSB.Helpers
{
    public class EventLog
    {

        public EventLog(SystemEventLogsModel model)
        {
            Task.Run(async () =>
            {
                var sysSrv = new SystemEventService();
                await sysSrv.UpdateLog(model);
            });
        }

    }
}
