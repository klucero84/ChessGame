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

        /// <summary>
        /// Populates possible moves collection with all legal options for Knight.
        /// </summary>
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
            // All possible offset combinations of a knight move
            int[] xOptions = new int[] {2, 1, -1, -2, -2, -1, 1, 2};
            int[] yOptions = new int[] {1, 2, 2, 1, -1, -2, -2, -1};

            // Check if each possible move is valid or not
            for (int i = 0; i < 8; i++) {
                // if we don't go off the board
                if (X + xOptions[i] >= 0 && X + xOptions[i] <= 7
                    && Y + yOptions[i] >= 0 && Y + yOptions[i] <= 7) {
                    Game.tryMove(X, Y, X + xOptions[i], Y + yOptions[i]);
                }
            }
        }
    }
}
