using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Helpers;
using ChessGameAPI.Hubs;
using ChessGameAPI.Models;
using ChessGameAPI.Models.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for moves.
    /// </summary>
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly IGameRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<MoveHub> _hub;

        /// <summary>
        /// Controller responsible for moves.
        /// </summary>
        /// <param name="repo">Data repository for moves</param>
        /// <param name="mapper">automapper utility</param>
        public MoveController (IGameRepository repo, IMapper mapper, IHubContext<MoveHub> hub)
        {
            _repo = repo;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("~/api/game/{gameId:int}/moves")]
        public async Task<IActionResult> GetMovesForGame(int gameId)
        {
            await _repo.SaveAll();
            return Ok();
        }


        /// <summary>
        /// Main action driver. updates peices and moves list that represent the game.
        /// </summary>
        /// <param name="dto">Move information to add to the data repo</param>
        /// <returns>an asynchronous operation returning an action result</returns>
        [HttpPost]
        public async Task<IActionResult> AddMove(MoveForAddMoveDto dto)
        {
            if (dto.UserId != this.GetCurrentUserId()) {
                return Unauthorized();
            }

            Game game = await _repo.GetGameForAddMove(dto.GameId);
            if (dto.UserId != game.WhiteUserId && dto.UserId != game.BlackUserId) {
                return Unauthorized();
            }

            dto.Notation = game.GetNotation(dto);
            Move newMove = _mapper.Map<Move>(dto);
            MoveResult result = game.TryAddMoveToGame(newMove);
            if (result != MoveResult.CanMove) {
                return BadRequest(result.GetDescription());
            }

            int code = await _repo.SaveAll();
            await _hub.Clients.GroupExcept(dto.GameId.ToString(), dto.ConnId).SendAsync("addMoveToGame", dto);
            return Ok(dto); 
        }

        /// <summary>
        /// Two Player mode. Mostly for development. Not sure if this will be a feature.
        /// Maybe make it a practice or not ranked game.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("~/api/move/two-player")]
        public async Task<IActionResult> AddMoveTwoPlayer(MoveForAddMoveDto dto)
        {
            Game game = await _repo.GetGameForAddMove(dto.GameId);
            if (dto.UserId != game.WhiteUserId && dto.UserId != game.BlackUserId) {
                return Unauthorized();
            }

            dto.Notation = game.GetNotation(dto);
            Move newMove = _mapper.Map<Move>(dto);
            MoveResult result = game.TryAddMoveToGame(newMove);
            if (result != MoveResult.CanMove) {
                BadRequest(result.GetDescription());
            }

            int code = await _repo.SaveAll();
            await _hub.Clients.GroupExcept(dto.GameId.ToString(), dto.ConnId).SendAsync("addMoveToGame", dto);
            return Ok(dto); 
        }
    }
}