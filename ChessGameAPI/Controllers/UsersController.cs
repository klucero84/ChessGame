using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller responsible for users
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result =  await _repo.GetUsers();
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _repo.GetUser(id);
            return Ok(result);
        }

        [HttpGet("/match-history/{id}")]
        public async Task<IActionResult> GetUserMatchHistory(int id) 
        {
            await _repo.SaveAll();
            return Ok();
        }
        
    }
}