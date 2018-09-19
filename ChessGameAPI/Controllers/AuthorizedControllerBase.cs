using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Abstract Base Class for authorized controllers
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class AuthorizedControllerBase: ControllerBase
    {
        /// <summary>
        /// Get the user id from the NameIdentifier claimtype.
        /// </summary>
        /// <returns>User id of current authorized user.</returns>
        protected int GetCurrentUserId()
        {
            int userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userIdClaim = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                var userIdValue = userIdClaim?.Value;
                int.TryParse(userIdValue, out userId);
            }   
            return userId;
        }
    }
}