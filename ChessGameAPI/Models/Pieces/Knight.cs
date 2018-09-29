using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight() { }
        public Knight(Game game, User user, int x, int y) : base(game, user, x, y)
        {
        }

        public override List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove, bool isWhite)
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
            return (false, MoveErrors.Knight);
        }
    }
}
