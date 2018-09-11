using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(User user, int x, int y) : base(user, x, y)
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
            if(diffX == diffY)
            {
                return (true, null);
            }

            return (false, "A Bishop must move in a diagonal line");
            
        }
    }
}
