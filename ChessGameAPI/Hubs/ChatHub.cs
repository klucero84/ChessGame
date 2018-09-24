using Microsoft.AspNetCore.SignalR;

namespace ChessGameAPI.Hubs
{
    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            // Clients.All.SendAsync()
            Clients.All.SendAsync("sendToAll", name, message);
        }
    }
}