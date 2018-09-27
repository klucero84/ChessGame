
using ChessGameAPI.Models;
using ChessGameAPI.Models.Pieces;
using Microsoft.EntityFrameworkCore;

namespace ChessGameAPI.Data
{
    /// <summary>
    /// default data context
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


        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(game => game.BlackUserId)
                .IsRequired(false);
            modelBuilder.Entity<Game>()
                .Property(game => game.WhiteUserId)
                .IsRequired(false);
            // modelBuilder.Entity<User>().HasData( 
            //     for(int i = 0; i < 100; i++) {
            //         new {}
            //     }
            // )
            // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // modelBuilder.Entity<Game>()y
            //     .HasOne(typeof(User))
            //     .WithOne()
            //     .HasForeignKey(nameof(User))
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.SetNull);
            // modelBuilder.Entity<User>()
            //     .HasMany(user =
            //     .OnDelete(DeleteBehavior.)
                
                // this bullshit doesn't have an option for no action which is 
                // needed to prevent cascade loops on piece deletion
            // modelBuilder.Entity<Piece>()
            //     .HasMany(piece => piece.Moves)
            //     .WithOne(move => move.Piece)
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.SetNull);
            // modelBuilder.Entity<User>()
            //     .HasMany(user => user.Pieces)
            //     .WithOne(piece => piece.OwnedBy)
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.SetNull);
            //  modelBuilder.Entity<User>()
            //     .HasMany(user => user.Moves)
            //     .WithOne(move => move.User)
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.SetNull);
    

            
        }
        
        
    }
}
