using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    public class Move
    {
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

        public Game Game { get; }
        public Piece Piece { get; }
        public User User { get; }
        public int StartX { get; }
        public int StartY { get; }
        public int EndX { get; }
        public int EndY { get; }
    }
}
