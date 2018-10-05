using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen() { }
        public Queen(Game game, User user, int x, int y) : base(game, user, x, y)
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
            int tempX = X;
            int tempY = Y;
            bool keepGoing = true;
            // up and right
            while (tempX < 7 && tempY < 7 && keepGoing) {
                tempX++;
                tempY++;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }

            // up and left
            tempX = X;
            tempY = Y;
            keepGoing = true;
            while (tempX > 0 && tempY < 7 && keepGoing) {
                tempX--;
                tempY++;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }

            // down and right
            tempX = X;
            tempY = Y;
            keepGoing = true;
            while (tempX < 7 && tempY > 0 && keepGoing) {
                tempX++;
                tempY--;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }

            // down and left
            tempX = X;
            tempY = Y;
            keepGoing = true;
            while (tempX > 0 && tempY > 0 && keepGoing) {
                tempX--;
                tempY--;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }
            tempX = X;
            tempY = Y;
            keepGoing = true;
            // right
            while (tempX < 7 && keepGoing) {
                tempX++;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }

            // left
            tempX = X;
            tempY = Y;
            keepGoing = true;
            while (tempX > 0 && keepGoing) {
                tempX--;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }

            // up
            tempX = X;
            tempY = Y;
            keepGoing = true;
            while (tempY < 7 && keepGoing) {
                tempY++;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }

            // down
            tempX = X;
            tempY = Y;
            keepGoing = true;
            while (tempY > 0 && keepGoing) {
                tempY--;
                keepGoing = Game.tryMove(X, Y, tempX, tempY);
            }
        }
    }
}
