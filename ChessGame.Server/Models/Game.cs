using ChessGame.Server.Models.Pieces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public User WhiteUser { get; set; }

        [Required]
        public User BlackUser { get; set; }

        public Game() { }

        public Game(User user1, User user2)
        {
            WhiteUser = user1;
            BlackUser = user2;
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
            //ToggleTurn();
            //save move to db
            return true;
        }

        public void Reset()
        {
            _pieces = new List<Piece>(32);
            //User whiteUser = WhiteUser.Color == UserColor.WHITE ? WhiteUser : User2;
            //User blackUser = User1.Color == UserColor.BLACK ? WhiteUser : User2;

            //i is the x
            for(int i = 0; i < BoardSize; i++)
            {
                //add white pawns to the second row from bottom
                _pieces.Add(new Pawn(WhiteUser, i, 1));
                //add black pawns to the second row from top
                _pieces.Add(new Pawn(BlackUser, i, 6));
                switch (i)
                {
                    case 0:
                    case 7:
                    {//rooks
                        _pieces.Add(new Rook(WhiteUser, i, 0));
                        _pieces.Add(new Rook(BlackUser, i, 7));
                        continue;
                    }
                    case 1:
                    case 6:
                    {//knights
                        _pieces.Add(new Knight(WhiteUser, i, 0));
                        _pieces.Add(new Knight(BlackUser, i, 7));
                        continue;
                    }
                    case 2:
                    case 5:
                    {//bishops
                        _pieces.Add(new Bishop(WhiteUser, i, 0));
                        _pieces.Add(new Bishop(BlackUser, i, 7));
                        continue;
                    }
                    case 3:
                    {//queens
                        _pieces.Add(new Queen(WhiteUser, i, 0));
                        _pieces.Add(new Queen(BlackUser, i, 7));
                        continue;
                    }
                    case 4:
                    {//kings
                        _pieces.Add(new King(WhiteUser, i, 0));
                        _pieces.Add(new King(BlackUser, i, 7));
                        continue;
                    }
                }
            }
        }

        public List<Move> GetMoves()
        {
            return _moves;
        }

        //private void ToggleTurn()
        //{
        //    turn = turn == UserColor.WHITE ? UserColor.BLACK : UserColor.WHITE;
        //}

        public void LoadGame(List<Move> moves)
        {
            _moves = moves;
        }
    }
}
