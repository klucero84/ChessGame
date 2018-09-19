using ChessGameAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller reponsible for photos.
    /// </summary>
    public class PhotoController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        /// <summary>
        /// Controller responsible for photos.
        /// </summary>
        /// <param name="userRepo">Data repository for Users</param>
        /// <param name="config">current configuration profile</param>
        public PhotoController(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }
    }
}