using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGameAPI.Models
{
    public class Move
    {
        public Move() { }

        public Move(Game game, User user, Piece piece, int endX, int endY)
        {
            Game = game;
            Piece = piece;
            User = user;
            StartX = piece.X;
            StartY = piece.Y;
            EndX = endX;
            EndY = endY;
        }

        public int Id { get; set; }

        [ForeignKey("Games")]
        public int GameId { get; set; }

        [Required]
        public Game Game { get; set; }

        [ForeignKey("Piece")]
        public int? PieceId { get; set; }
        public Piece Piece { get; set; }
        
        private string _discriminator;

        public string PieceDiscriminator
        {
            get { return _discriminator; }
            set { _discriminator = value; }
        }

        [ForeignKey("Users")]
        public int? UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public int StartX { get; set; }

        [Required]
        public int StartY { get; set; }

        [Required]
        public int EndX { get; set; }

        [Required]
        public int EndY { get; set; }

        public string AlgebraicNotation { get; set; }
        
        [Required]
        [Timestamp]
        public byte[] TimeStamp { get; set; }
        
    }
}
