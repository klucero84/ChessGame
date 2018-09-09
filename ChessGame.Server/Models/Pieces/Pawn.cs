using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Board board, User user) : base(board, user)
        {

        }

        public override List<Move> GetAllLegalMoves()
        {
            
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove)
        {
            UserColor color = attemptedMove.User.Color;
            if (color == UserColor.WHITE)
            {
                //white pawns start at row index 1 and only move up
                //if they are at their starting position they may move two spaces
                //if a white pawn is at row index 4 they could eligable to capture with en passant

            }
            else if(color == UserColor.BLACK)
            {
                //black pawns start at row index 6 and only move down
                //if they are at their starting position they may move two spaces
                //if a black pawn is at row index 3 they could eligable to capture with en passant
            }
            return (true, null);
        }
    }
}
