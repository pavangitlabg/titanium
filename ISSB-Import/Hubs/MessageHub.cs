using System;
using Microsoft.AspNetCore.SignalR;

namespace ISSB.Import.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string country, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", country, message);
        }
    }
}

