using System.IO;
using Microsoft.AspNetCore.Mvc;
namespace ChessGameAPI.API.Controllers
{
    /// <summary>
    /// Serves root index page as catch all for urls not matching router definitions
    /// </summary>
    public class FallBack : Controller
    {
        /// <summary>
        /// default catch all action
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() 
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}
