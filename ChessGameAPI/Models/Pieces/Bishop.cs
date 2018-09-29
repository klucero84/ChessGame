using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop() { }
        
        public Bishop(Game game, User user, int x, int y) : base(game, user, x, y)
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
            if(diffX == diffY)
            {
                return (true, null);
            }

            return (false, MoveErrors.Bishop);
            
        }
    }
}
