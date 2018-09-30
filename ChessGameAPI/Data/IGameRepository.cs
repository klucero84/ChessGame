using System.Collections.Generic;
using System.Threading.Tasks;
using ChessGameAPI.Models;

namespace ChessGameAPI.Data
{
    /// <summary>
    /// Game Repository Interface
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Adds an entity of type T
        /// </summary>
        /// <typeparam name="T">a reference type</typeparam>
         void Add<T>(T entity) where T: class;

        /// <summary>
        /// Deletes an entity of type T
        /// </summary>
        /// <typeparam name="T">a reference type</typeparam>
         void Delete<T>(T entity) where T: class;

        /// <summary>
        /// Saves all changes made to data context
        /// </summary>
        /// <returns>an operation returning a int, the number of objects written to the underlying data context.</returns>
         Task<int> SaveAll();

        /// <summary>
        /// Gets a list of games for a user id.
        /// </summary>
        /// <param name="userId">user id to get list owned.</param>
        /// <returns>an enumerable collection of game models with the given user id as a player</returns>
        Task<IEnumerable<Game>> GetGamesForUser(int userId);

        /// <summary>
        /// Gets a game by it's unique id
        /// </summary>
        /// <param name="gameId">unique identifier of the game to get</param>
        /// <returns>the game matching the id, if any.</returns>
        Task<Game> GetGame(int gameId);

        Task<Game> GetGameMin(int gameId);

        /// <summary>
        /// Gets a peice by its unique id
        /// </summary>
        /// <param name="pieceId">unique identifier of the peice to get</param>
        /// <returns>the peice matching the id, if any.</returns>
        Task<Piece> GetPiece(int pieceId);

        Task<Piece> GetPieceByXY(int gameId, int x, int y);

        Task<IList<Move>> GetMovesForGameForUser(int gameId, int userId);
    }
}