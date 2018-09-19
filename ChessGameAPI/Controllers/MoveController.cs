using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for moves.
    /// </summary>
    public class MoveController : AuthorizedControllerBase
    {
        private readonly IGameRepository _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller responsible for moves.
        /// </summary>
        /// <param name="repo">Data repository for moves</param>
        /// <param name="mapper">automapper utility</param>
        public MoveController (IGameRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
            int code = await _repo.SaveAll();
            return Ok();
        }
    }
}