using System;
using System.Collections.Generic;

namespace ChessGameAPI.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop() { }
        
        public Bishop(Game game, User user, int x, int y) : base(game, user, x, y)
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
        }
    }
}
