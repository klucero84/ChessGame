using System.Threading.Tasks;
using ChessGameAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for the game
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController: ControllerBase
    {
        private readonly IGameRepository _repo;
        public GameController(IGameRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return Ok("Hello Api");
        }
    }
}