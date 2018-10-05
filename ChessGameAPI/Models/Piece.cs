using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGameAPI.Models
{
    public class Piece
    {
        /// <summary>
        /// Unique db identifier
        /// </summary>
        public int Id { get; set; }

       
        /// <summary>
        /// Abscissas value (x)
        /// </summary>
        [Required]
        [Range(0, 7)]
        public int X { get; set; }

        /// <summary>
        /// Ordinate value (y)
        /// </summary>
        [Required]
        [Range(0, 7)]
        public int Y { get; set; }

        /// <summary>
        /// Owner of this piece
        /// </summary>
        /// <returns></returns>
        public User OwnedBy { get; set; }

        /// <summary>
        /// Game to which this piece belongs
        /// </summary>
        /// <value></value>
        [Required]
        public Game Game { get; set; }

        /// <summary>
        /// subclass discriminator
        /// </summary>
        /// <value></value>
        public string Discriminator { get; set; }

        public IEnumerable<Move> Moves { get; set;}

        [NotMapped]
        public IDictionary<(int, int), Piece> possibleMoves;

        /// <summary>
        /// Blank Constructor 
        /// </summary>
        public Piece() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Game to which the piece belongs</param>
        /// <param name="user">User to which the piece belongs</param>
        /// <param name="x">Abscissas value of piece</param>
        /// <param name="y">Ordinate value of the piece</param>
        public Piece(Game game, User user, int x, int y)
        {
            Game = game;
            OwnedBy = user;
            X = x;
            Y = y;
        }

        public virtual void GetAllLegalMoves()
        {
            throw new NotImplementedException("GetAllLegalMoves is not imeplemented.");
        }
    }

    
}
