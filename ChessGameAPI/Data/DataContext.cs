
using ChessGameAPI.Models;
using ChessGameAPI.Models.Pieces;
using Microsoft.EntityFrameworkCore;

namespace ChessGameAPI.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Contructor with options injected
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataContext(DbContextOptions<DataContext> options) : base (options) { }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Move> Moves { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Piece> Pieces { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Photo> Photos { get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Pawn> Pawns { get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Rook> Rooks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Knight> Knights { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Bishop> Bishops { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Queen> Queens { get; set; }    

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<King> Kings { get; set; }
        
        
    }
}
