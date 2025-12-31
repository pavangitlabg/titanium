using Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace ISSB_Prod_Blazor.Hubs;

public class MessageService(IHubContext<MessageHub> hubContext)
{
    public event Action<HubMessageModel> OnMessage;

    public async Task Send(HubMessageModel message)
    {
        OnMessage?.Invoke(message);
        await hubContext.Clients.All.SendAsync("MessageReceived", message);
    }
}