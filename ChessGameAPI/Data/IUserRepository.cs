using System.Collections.Generic;
using System.Threading.Tasks;
using ChessGameAPI.Models;

namespace ChessGameAPI.Data
{
    public interface IUserRepository
    {
         /// <summary>
        /// Gets a list of all users asynchronously
        /// </summary>
        /// <returns>an asychronous operation returning an enumerable collection of User models</returns>
         Task<IEnumerable<User>> GetUsers();

        /// <summary>
        /// Gets a user asynchronously
        /// </summary>
        /// <param name="id">Unique identifier of the user to get</param>
        /// <returns>an asynchronous operation return a user model</returns>
         Task<User> GetUser(int id);

        /// <summary>
        /// Gets a photo asynchronously
        /// </summary>
        /// <param name="id">Unique identifer of the photo to get</param>
        /// <returns>an asynchronous operation returning a photo model</returns>
         Task<Photo> GetPhotoAsync(int id);
    }
}