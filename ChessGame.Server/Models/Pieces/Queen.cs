using System;
using System.Collections.Generic;

namespace ChessGame.Server.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(User user, int x, int y) : base(user, x, y)
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
            if (diffX == 0 || diffY == 0)
            {
                return (true, null);
            } else if (diffX == diffY)
            {
                return (true, null);
            }
            return (false, "A Queen must move in a in straight lines along the x or y axis, or in a diagonal line.");
        }
    }
}
