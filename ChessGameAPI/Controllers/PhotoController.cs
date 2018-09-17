using ChessGameAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller reponsible for photos
    /// </summary>
    [Authorize]
    [Route("api/users{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        public PhotoController(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
    }
}