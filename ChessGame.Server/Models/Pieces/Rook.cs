using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models.Pieces
{
    public class Rook : Piece
    {
        public Rook(Board board, User user) : base(board, user)
        {
        }

        public override List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove)
        {
            return (true, null);
        }
    }
}
