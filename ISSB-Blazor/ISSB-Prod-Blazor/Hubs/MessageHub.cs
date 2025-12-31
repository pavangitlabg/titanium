using System;
using Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace ISSB_Prod_Blazor.Hubs;

public class MessageHub : Hub
{
    public async Task SendMessage(HubMessageModel message)
    {
        await Clients.All.SendAsync("MessageReceived", message);
    }
    
}
