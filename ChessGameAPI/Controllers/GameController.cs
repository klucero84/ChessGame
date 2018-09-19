using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for the game
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GameController: AuthorizedControllerBase
    {
        private readonly IGameRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public GameController(IGameRepository repo, IMapper mapper, IUserRepository userRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            return Ok("Hello Api");
        }

        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            var result = await _repo.GetGamesForUser(GetCurrentUserId());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> NewGame()
        {
            // todo: match making queue/service
            var oppenentId = 2;

            User whiteUser = await _userRepo.GetUser(GetCurrentUserId());
            User blackUser = await _userRepo.GetUser(oppenentId);

            Game newGame = new Game(whiteUser, blackUser);
            newGame.Reset();
            _repo.Add(newGame);
            foreach(Piece piece in newGame.GetPieces())
            {
                _repo.Add(piece);
            }
            _repo.Add(newGame.GetPieces());
            var result = await _repo.SaveAll();

            GameDto gameDto = _mapper.Map<GameDto>(newGame);
            return Ok(gameDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id of the game to get from data repo</param>
        /// <returns>Piece and Move list that represents a game in progress</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            
            var game =  await _repo.GetGame(id);
            var gameDto = _mapper.Map<GameDto>(game);
            var moves = _mapper.Map<IEnumerable<MoveDto>>(game.GetMoves());
            var peices = _mapper.Map<IEnumerable<PieceDto>>(game.GetPieces());
            return Ok(gameDto);
        }

        /// <summary>
        /// Main action driver. updates peices and moves list that represents the game.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AddMove(MoveForAddMoveDto dto)
        {
            await _repo.SaveAll();
            return Ok();
        }

        
    }
}