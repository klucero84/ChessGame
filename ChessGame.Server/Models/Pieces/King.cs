using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models.Pieces
{
    public class King : Piece
    {
        public King(Board board, User user) : base(board, user)
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
