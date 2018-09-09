using ChessGame.Server.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    /// <summary>
    /// Dictionary<int,Dictionary<int, Piece>> In memory datastructure representation of current board state. should not be persisted.
    /// </summary>
    public class Board
    {
        private Game _game;
        public const int BoardSize = 8;
        public List<Piece> Pieces = new List<Piece>();
        private Dictionary<int,Dictionary<int, Piece>> _surface = new Dictionary<int, Dictionary<int, Piece>>(BoardSize);

        public Board(Game game)
        {
            _game = game;
            InitializeBoard();
            //if the game on this board has any moves
            if (_game?.GetMoves()?.Count() > 0)
            {

            }
        }

        /// <summary>
        /// resets the _surface container to a blank board with no pieces
        /// </summary>
        private void InitializeBoard(Dictionary<int, Dictionary<int, Piece>> pieces = null)
        {
            if (pieces != null)
            {
                _surface = pieces;
                return;
            }

           for(int x = 0; x < BoardSize; x++)
           {
                Dictionary<int, Piece> column = new Dictionary<int, Piece>(BoardSize);
                for(int y = 0; y < BoardSize; y++)
                {
                    column[y] = null;
                }
                _surface[x] = column;
           }
        }

        /// <summary>
        /// places pieces in their starting locations
        /// </summary>
        public void ResetBoard()
        {
            for(int x = 0; x < BoardSize; x++)
            {
                Dictionary<int, Piece> column = new Dictionary<int, Piece>(BoardSize);
                for(int y = 0; y < BoardSize; y++)
                {
                    if (y == 0 || y == 7)
                    {
                        //white at bottom, black at top
                        User currentUser = x == 0 ? _game.User1 : _game.User2;
                        if (x == 0 || x == 7)
                        {
                            //add rooks
                        } else if (x == 1 || x == 6)
                        {
                            //add knights
                        } else if (x == 2 || x == 5)
                        {
                            //add bishops

                        } else if (x == 3)
                        {
                            //add queens
                        } else
                        {
                            //add kings
                        }
                    } else if (y == 1)
                    {
                        //add white pawns
                    } else if (y == 6)
                    {
                        //add black pawns
                    } else { 
                        //set rest to empty squares
                        column[y] = null;
                    }
                }
                _surface[x] = column;
           }
        }
    }
}
