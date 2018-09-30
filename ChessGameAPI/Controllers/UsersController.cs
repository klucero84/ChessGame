using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
           var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
             var userFromRepo = await _repo.GetUser(currentUserId);
             userParams.UserId = currentUserId;
             var users = await _repo.GetUsers(userParams);
             var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
             Response.AddPagination(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);
            string s = "'";
            return Ok(usersToReturn);
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