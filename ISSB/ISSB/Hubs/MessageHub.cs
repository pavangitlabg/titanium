using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Data.Models;

namespace ISSB.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessage _message;

        public MessageHub(IMessage doStuff)
        {
            _message = doStuff;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("----Connected--- " + Context.ConnectionId);
            // Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task Send(string message)
        {
            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
            string toClient = routeOb.To;
            Console.WriteLine("Message Received on: " + Context.ConnectionId);

            if (toClient == string.Empty)
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            else
            {
                await Clients.Client(toClient).SendAsync("ReceiveMessage", message);
            }
        }
    }
}
