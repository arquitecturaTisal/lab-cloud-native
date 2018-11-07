using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clases
{
    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }
    }
}
