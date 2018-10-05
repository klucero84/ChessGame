using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class King : Piece
    {
        public King() { }
        public King(Game game, User user, int x, int y) : base(game, user, x, y)
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
            possibleMoves.Clear();
            bool canMoveRight = X > 0;
            bool canMoveLeft = X < 7;
            bool canMoveUp = Y < 7;
            bool canMoveDown = Y > 0;

            if (canMoveRight) {
                Game.tryMove(X, Y, X + 1, Y);
                if (canMoveUp) {
                    Game.tryMove(X, Y, X + 1, Y + 1);
                }
                if (canMoveDown) {
                    Game.tryMove(X, Y, X + 1, Y - 1);
                }
            }
            if (canMoveLeft) {
                Game.tryMove(X, Y, X - 1, Y);
                if (canMoveUp) {
                    Game.tryMove(X, Y, X - 1, Y + 1);
                }
                if (canMoveDown) {
                    Game.tryMove(X, Y, X - 1, Y - 1);
                }
            }
            if (canMoveUp) {
                Game.tryMove(X, Y, X, Y + 1);
            }
            if (canMoveDown) {
                Game.tryMove(X, Y, X, Y - 1);
            }
            if (OwnedBy.Id == Game.WhiteUserId) {
                if (Game.CanWhiteKingSideCastle) {
                    Game.tryMove(X, Y, 6, 0);
                }
                if (Game.CanWhiteQueenSideCastle) {
                    Game.tryMove(X, Y, 2, 0);
                }
            } else {
                if (Game.CanBlackKingSideCastle) {
                    Game.tryMove(X, Y, 6, 7);
                }
                if (Game.CanBlackQueenSideCastle) {
                    Game.tryMove(X, Y, 2, 7);
                }
            }
        }
    }
}
