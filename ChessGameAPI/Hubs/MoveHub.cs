using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ChessGameAPI.Hubs
{
    public class MoveHub : Hub
    {
        private readonly IGameRepository _gameRepo;
        public MoveHub (IGameRepository gameRepo)
        {
            _gameRepo = gameRepo;
        }
        public async Task<string> JoinGame(int gameId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            return Context.ConnectionId;
        }

        public Task LeaveGame(int gameId) {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        // public override Task OnDisconnectedAsync(System.Exception exception)
        // {
            //  if (Users.Any(x => x.ConnectionID == Context.ConnectionId))
            // {
            //     User user = Users.First(x => x.ConnectionID == Context.ConnectionId);
            //     Clients.Others.userLeft(user.Name);   
            //     Users.Remove(user);
            // }
            // return base.OnDisconnectedAsync(exception);
           
        // }
        // public override OnDisconnectedAsync(){

        //     return base.OnDisconnectedAsync();
        // }

        public async void AddMoveToGame(MoveForAddMoveDto newMoveDto) {

            Game game = await _gameRepo.GetGameForAddMove(newMoveDto.GameId);

            

            await Clients.OthersInGroup(newMoveDto.GameId.ToString()).SendAsync("addMoveToGame", newMoveDto);
            // Clients.All.SendAsync("sendToAll", move);
        }

        // public void SendToAll(){
        // }
    }
}