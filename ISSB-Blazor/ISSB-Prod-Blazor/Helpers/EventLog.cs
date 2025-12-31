using Data.Models;
using Services;

namespace ISSB_Prod_Blazor.Helpers;

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