using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    public class Game
    {
        public int Id { get; set; }

        private List<Move> _moves;

        private Board _board;

        public User User1 { get; set; }

        public User User2 { get; set; }

        public UserColor turn;

        public Game() { }

        public Game(User user1, User user2)
        {
            User1 = user1;
            User2 = user2;
        }

        public Game StartNewGame(User user1, User user2)
        {
            Game newGame = new Game(user1, user2);
            newGame.GetBoard().ResetBoard();
            return newGame;
        }

        /// <summary>
        /// Gets the current board or a new board for this game
        /// </summary>
        /// <returns></returns>
        public Board GetBoard()
        {
            if (_board == null)
            {
                _board = new Board(this);
            }
            return _board;
        }

        public bool AddMove(Move attemptedMove)
        {
            Piece pieceToBeMoved = attemptedMove.Piece;
            
            //determine if this move is legal
            (bool, string) result = pieceToBeMoved.IsLegalMove(attemptedMove);
            if (!result.Item1)
            {
                //illegal move tell
                return false;
            }
            _moves.Add(attemptedMove);
            ToggleTurn();
            //save move to db
            return true;
        }

        public List<Move> GetMoves()
        {
            return _moves;
        }

        private void ToggleTurn()
        {
            turn = turn == UserColor.WHITE ? UserColor.BLACK : UserColor.WHITE;
        }
    }
}
