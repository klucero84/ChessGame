using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Pawn : Piece
    {
        public bool IsCapturing;

        public Pawn() { }

        public Pawn(Game game, User user, int x, int y) : base(game, user, x, y)
        {
        }

        public override List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) IsLegalMoveForPiece(Move attemptedMove, bool isWhite)
        {
            int diffX = Math.Abs(X - attemptedMove.EndX);
            if (diffX > 1)
            {
                return (false, "A Pawn cannot move more than one square to the side.");
            }
            if (isWhite)
            {
                //white pawns start at row index 1 and move up one at a time
                if (Y + 1 == attemptedMove.EndY)
                {
                    if (diffX == 0)
                    {
                        return (true, null);
                    } //pawns may move one space to eother side when capturing
                    else if (diffX == 1)
                    {
                        return (true, null);
                    }
                }
                else if (Y + 2 == attemptedMove.EndY && Y == 1 && diffX == 0)
                {//if they are at their starting position they may move two spaces
                    return (true, null);
                }
            }
            else
            {
                //black pawns start at row index 6 and only move down
                if (Y - 1 == attemptedMove.EndY)
                {
                    if (diffX == 0)
                    {
                        return (true, null);
                    } //pawns may move one pace to either side when capturing
                    else if (diffX == 1)
                    {
                        return (true, null);
                    }
                }
                else if (Y - 2 == attemptedMove.EndY && Y == 6 && diffX == 0)
                {//if they are at their starting position they may move two spaces
                    return (true, null);
                }
            }
            return (false, "A Pawn may only move forward one space at a time, " +
                "capture diagonally, and may move two spaces forward if it is the first move of the pawn.");
        }
    }
}
