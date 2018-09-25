using ChessGameAPI.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace ChessGameAPI.Hubs
{
    public class ChatHub : Hub
    {
        public void SendToAll(MoveDto move)
        {
            // Clients.All.SendAsync()
            Clients.All.SendAsync("sendToAll", move);
        }
    }
}