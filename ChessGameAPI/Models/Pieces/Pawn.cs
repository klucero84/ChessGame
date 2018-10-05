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

        public override void GetAllLegalMoves()
        {
            if(Game == null)
            {
                throw new NullReferenceException($"Piece with id:{Id} is not part of a game.");
            }
            if (possibleMoves == null) {
                possibleMoves = new Dictionary<(int, int), Piece>();
            } else {
                possibleMoves.Clear();
            }
            // white side pawn
            if (OwnedBy.Id == Game.WhiteUserId) {
                if (Y == 1) {
                    Game.tryMove(X, Y, X, Y + 2, false);
                }
                if (Y < 7) {
                    Game.tryMove(X, Y, X, Y + 1, false);
                    if (X < 7) {
                        Game.tryMove(X, Y, X + 1, Y + 1, true);
                    }
                    if (X > 0) {
                        Game.tryMove(X, Y, X - 1, Y + 1, true);
                    }
                }
            } else { // black side pawn
                if (Y == 6) {
                    Game.tryMove(X, Y, X, Y - 2, false);
                }
                if (Y > 0) {
                    Game.tryMove(X, Y, X, Y - 1, false);
                    if (X < 7) {
                        Game.tryMove(X, Y, X + 1, Y - 1, true);
                    }
                    if (X > 0) {
                        Game.tryMove(X, Y, X - 1, Y - 1, true);
                    }
                }
            }
        }
    }
}
