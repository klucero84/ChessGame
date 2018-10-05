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
using ChessGameAPI.Helpers;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for Games.
    /// </summary>
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController: ControllerBase
    {
        private readonly IGameRepository _gameRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller responsible for Games.
        /// </summary>
        /// <param name="gameRepo">Data repository for games</param>
        /// <param name="mapper">automapper utility</param>
        /// <param name="userRepo">Data repository for users</param>
        public GameController(IGameRepository gameRepo, IMapper mapper, IUserRepository userRepo)
        {
            _gameRepo = gameRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Gets a list of games for the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/api/games")]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameRepo.GetGamesForUser(this.GetCurrentUserId());
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDto);
        }

        /// <summary>
        /// Creates new game for current user.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewGame()
        {
            // todo: match making queue/service
            var oppenentId = 2;

            User whiteUser = await _userRepo.GetUser(this.GetCurrentUserId());
            User blackUser = await _userRepo.GetUser(oppenentId);

            Game newGame = new Game(whiteUser, blackUser);
            newGame.StatusCode = GameStatus.Inprogress;
            newGame.Initialize();
            _gameRepo.Add(newGame);
            foreach(Piece piece in newGame.Pieces)
            {
                _gameRepo.Add(piece);
            }
            var result = await _gameRepo.SaveAll();
        
            GameDto gameDto = _mapper.Map<GameDto>(newGame);
            return Created("/game/" +gameDto.Id + "/play", gameDto);
            //  return CreatedAtAction("GetGame", gameDto);
            // return CreatedAtRoute( );
            //  Ok(gameDto);
        }

        /// <summary>
        /// Gets a game by its unique identifier.
        /// </summary>
        /// <param name="id">Id of the game to get from data repo</param>
        /// <returns>Piece and Move list that represents a game in progress</returns>
        [HttpGet("{id}", Name="GetGame")]
        public async Task<IActionResult> GetGameForPlay(int id)
        {
            
            var game =  await _gameRepo.GetGameForPlay(id);
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        [HttpPost("~/api/game/{gameId:int}/draw")]
        public async Task<IActionResult> RequestDraw(int gameId)
        {
            Game game = await _gameRepo.GetGameMin(gameId);
            if (this.GetCurrentUserId() == game.WhiteUserId) {
                game.StatusCode = GameStatus.WinBlack;
            } else if (this.GetCurrentUserId() == game.BlackUserId) {
                game.StatusCode = GameStatus.WinWhite;
            }
            await _gameRepo.SaveAll();
            return Ok();
        }

        [HttpPost("~/api/game/{gameId:int}/concede")]
        public async Task<IActionResult> Concede(int gameId)
        {
            Game game = await _gameRepo.GetGameMin(gameId);
            if (this.GetCurrentUserId() == game.WhiteUserId) {
                game.StatusCode = GameStatus.WinBlack;
            } else if (this.GetCurrentUserId() == game.BlackUserId) {
                game.StatusCode = GameStatus.WinWhite;
            }
            await _gameRepo.SaveAll();
            return Ok();
        }
    }
}