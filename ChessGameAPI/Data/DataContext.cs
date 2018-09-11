
using ChessGameAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChessGameAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) { }

        public DbSet<Game> Games { get; set; }

        public DbSet<Move> Moves { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Piece> Pieces { get; set; }
        
    }
}
