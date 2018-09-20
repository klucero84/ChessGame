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
    /// Controller responsible for Games.
    /// </summary>
    public class GameController: AuthorizedControllerBase
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
        [Route("~/api/games")]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameRepo.GetGamesForUser(GetCurrentUserId());
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDto);
        }

        /// <summary>
        /// Creates new game for current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> NewGame()
        {
            // todo: match making queue/service
            var oppenentId = 2;

            User whiteUser = await _userRepo.GetUser(GetCurrentUserId());
            User blackUser = await _userRepo.GetUser(oppenentId);

            Game newGame = new Game(whiteUser, blackUser);
            newGame.Reset();
            _gameRepo.Add(newGame);
            foreach(Piece piece in newGame.Pieces)
            {
                _gameRepo.Add(piece);
            }
            var result = await _gameRepo.SaveAll();

            GameDto gameDto = _mapper.Map<GameDto>(newGame);
            return Ok(gameDto);
        }

        /// <summary>
        /// Gets a game by its unique identifier.
        /// </summary>
        /// <param name="id">Id of the game to get from data repo</param>
        /// <returns>Piece and Move list that represents a game in progress</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            
            var game =  await _gameRepo.GetGame(id);
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }
    }
}