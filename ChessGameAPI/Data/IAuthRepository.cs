using System.Threading.Tasks;
using ChessGameAPI.Models;

namespace ChessGameAPI.Data
{
     /// <summary>
    /// Authorization Repository Interface
    /// </summary>
    public interface IAuthRepository
    {
          /// <summary>
        /// Registration process, adds user to the application
        /// </summary>
        /// <param name="user">a user model to create</param>
        /// <param name="password">password for the user</param>
        /// <returns></returns>
         Task<User> Register(User user, string password);

        /// <summary>
        /// Login process
        /// </summary>
        /// <param name="email">email to authorize</param>
        /// <param name="password">password to authenticate</param>
        /// <returns></returns>
         Task<User> Login(string email, string password);

        /// <summary>
        /// Checks to see if a user exists, used in registration to ensure unique username
        /// </summary>
        /// <param name="email">proposed email</param>
        /// <returns>an async operation returning a bool, true = user exisit</returns>
         Task<bool> UserExists(string email);

         Task<bool> LogUserActivity(int userId);
    }
}