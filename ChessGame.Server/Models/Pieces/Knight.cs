using System;
using System.Collections.Generic;

namespace ChessGame.Server.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(User user, int x, int y) : base(user, x, y)
        {
        }

        public override List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove)
        {
            //move must be abs two in one axis and abs one in another axis
            int diffX = Math.Abs(X - attemptedMove.EndX);
            int diffY = Math.Abs(Y - attemptedMove.EndY);
            if (diffX == 1 && diffY == 2)
            {
                return (true, null);
            } else if (diffX == 2 && diffY == 1)
            {
                return (true, null);
            }
            return (false, "A Knight must move two spaces on one axis and one space on the other axis.");
        }
    }
}
