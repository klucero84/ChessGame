using System.Collections.Generic;
using System.Threading.Tasks;
using ChessGameAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChessGameAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context">data context to access</param>>
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a photo asynchronously
        /// </summary>
        /// <param name="id">Unique identifer of the photo to get</param>
        /// <returns>an asynchronous operation returning a photo model</returns>
        public async Task<Photo> GetPhotoAsync(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Returns a User for a given unique id.
        /// </summary>
        /// <param name="id">Integer value of Id of the User</param>
        /// <returns>An async operation returning a User model</returns>
        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Gets all User entities in an IEnumerable Collection.
        /// </summary>
        /// <returns>an async operation return an IEnumerable of type User</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }
    }
}