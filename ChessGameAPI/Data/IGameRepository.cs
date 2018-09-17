using System.Threading.Tasks;

namespace ChessGameAPI.Data
{
    public interface IGameRepository
    {
          /// <summary>
        /// Adds n entity of type T
        /// </summary>
        /// <typeparam name="T">a reference type</typeparam>
         void Add<T>(T entity) where T: class;

        /// <summary>
        /// Deletes an entity of type T
        /// </summary>
        /// <typeparam name="T">a reference type</typeparam>
         void Delete<T>(T entity) where T: class;

        /// <summary>
        /// Saves all changes asynchronously
        /// </summary>
        /// <returns>an asynchronous operation returning a boolean</returns>
         Task<bool> SaveAll();
    }
}