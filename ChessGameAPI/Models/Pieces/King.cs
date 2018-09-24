using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class King : Piece
    {
        public King() { }
        public King(Game game, User user, int x, int y) : base(game, user, x, y)
        {
        }

        public override List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove, bool isWhite)
        {
            int diffX = Math.Abs(X - attemptedMove.EndX);
            int diffY = Math.Abs(Y - attemptedMove.EndY);
            if (diffX > 1 || diffY > 1) {
                return (false, "The King can only move one space in any direction.");
            }
            return (true, null);
        }
    }
}
