using System;
using System.Collections.Generic;

namespace ChessGame.Server.Models.Pieces
{
    public class King : Piece
    {
        public King(User user, int x, int y) : base(user, x, y)
        {
        }

        public override List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove)
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
