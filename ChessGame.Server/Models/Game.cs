using ChessGame.Server.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    public class Game
    {
        public const int BoardSize = 8;

        public int Id { get; set; }

        private List<Move> _moves;

        private List<Piece> _pieces;

        public User User1 { get; set; }

        public User User2 { get; set; }

        public UserColor turn;

        public Game() { }

        public Game(User user1, User user2)
        {
            User1 = user1;
            User2 = user2;
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

        public void Reset()
        {
            _pieces = new List<Piece>(32);
            User whiteUser = User1.Color == UserColor.WHITE ? User1 : User2;
            User blackUser = User1.Color == UserColor.BLACK ? User1 : User2;
            //i is the x
            for(int i = 0; i < BoardSize; i++)
            {
                //add white pawns to the second row from bottom
                _pieces.Add(new Pawn(whiteUser, i, 1));
                //add black pawns to the second row from top
                _pieces.Add(new Pawn(blackUser, i, 6));
                switch (i)
                {
                    case 0:
                    case 7:
                    {//rooks
                        _pieces.Add(new Rook(whiteUser, i, 0));
                        _pieces.Add(new Rook(blackUser, i, 7));
                        continue;
                    }
                    case 1:
                    case 6:
                    {//knights
                        _pieces.Add(new Knight(whiteUser, i, 0));
                        _pieces.Add(new Knight(blackUser, i, 7));
                        continue;
                    }
                    case 2:
                    case 5:
                    {//bishops
                        _pieces.Add(new Bishop(whiteUser, i, 0));
                        _pieces.Add(new Bishop(blackUser, i, 7));
                        continue;
                    }
                    case 3:
                    {//queens
                        _pieces.Add(new Queen(whiteUser, i, 0));
                        _pieces.Add(new Queen(blackUser, i, 7));
                        continue;
                    }
                    case 4:
                    {//kings
                        _pieces.Add(new King(whiteUser, i, 0));
                        _pieces.Add(new King(blackUser, i, 7));
                        continue;
                    }
                }
            }
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
