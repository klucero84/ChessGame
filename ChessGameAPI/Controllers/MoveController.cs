using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Hubs;
using ChessGameAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for moves.
    /// </summary>
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

        [Route("~/api/game/{gameId:int}/moves")]
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
            
            Move newMove = _mapper.Map<Move>(dto);
            _repo.Add(newMove);
            Piece movedPiece = await _repo.GetPiece(dto.PieceId);
            
            var moveAttempt =  movedPiece.IsLegalMove(newMove, dto.isWhite);
            if (moveAttempt.Item1) {
                movedPiece.X = newMove.EndX;
                movedPiece.Y = newMove.EndY;
                int code = await _repo.SaveAll();
                return Ok(code); 
            } else {
                return BadRequest(moveAttempt.Item2);
            }
           
        }

        [HttpPost("~/api/move/two-player")]
        public async Task<IActionResult> AddMoveTwoPlayer(MoveForAddMoveDto dto)
        {
            Move newMove = _mapper.Map<Move>(dto);
            _repo.Add(newMove);
            Piece movedPiece = await _repo.GetPiece(dto.PieceId);
            var moveAttempt =  movedPiece.IsLegalMoveTwoPlayer(newMove, dto.isWhite);
            if (moveAttempt.Item1) {
                movedPiece.X = newMove.EndX;
                movedPiece.Y = newMove.EndY;
                int code = await _repo.SaveAll();
                // this isn't 
                string connId =  dto.connId;
                await _hub.Clients.GroupExcept(dto.GameId.ToString(), connId).SendAsync("addMoveToGame", dto);
                return Ok(code); 
            } else {
                return BadRequest(moveAttempt.Item2);
            }
        }
    }
}