using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Rook : Piece
    {

        public Rook() { }
        public Rook(Game game, User user, int x, int y) : base(game, user, x, y)
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
            }
            return (false, "A Rook must move in straight lines along the x or y axis");
        }
    }
}
