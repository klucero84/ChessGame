using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Base Class for all authorized controllers
    /// </summary>
    
    [Authorize]
    public abstract class AuthorizedControllerBase: ControllerBase
    {
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