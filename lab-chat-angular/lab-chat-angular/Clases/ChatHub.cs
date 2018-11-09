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
            String fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Clients.All.SendAsync("sendToAll", name, message,fecha);
        }
    }
}
