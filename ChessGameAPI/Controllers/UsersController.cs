using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : AuthorizedControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;


        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetUsers()
        {
            var result =  await _repo.GetUsers();
            return Ok(result);
        }

        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _repo.GetUser(id);
            return Ok(result);
        }
        
    }
}