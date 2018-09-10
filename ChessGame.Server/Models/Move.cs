using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
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

        [Required]
        public Game Game { get; set; }

        [Required]
        public Piece Piece { get; set; }

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
        
        [Required]
        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
