
using ChessGameAPI.Models.Pieces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessGameAPI.Models
{
    public class Game
    {
        public const int BoardSize = 8;

        public int Id { get; set; }

        private List<Move> _moves;

        private List<Piece> _pieces;

        [ForeignKey("Users")]
        public int WhiteUserId { get; set; }

        public User WhiteUser { get; set; }

        [ForeignKey("Users")]
        public int BlackUserId { get; set; }

        public User BlackUser { get; set; }

        public Game() { }

        public Game(User user1, User user2)
        {
            WhiteUser = user1;
            BlackUser = user2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attemptedMove"></param>
        /// <returns></returns>
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
            _moves = new List<Move>();
            //User whiteUser = WhiteUser.Color == UserColor.WHITE ? WhiteUser : User2;
            //User blackUser = User1.Color == UserColor.BLACK ? WhiteUser : User2;

            //i is the x
            for(int i = 0; i < BoardSize; i++)
            {
                //add white pawns to the second row from bottom
                _pieces.Add(new Pawn(this, WhiteUser, i, 1));
                //add black pawns to the second row from top
                _pieces.Add(new Pawn(this, BlackUser, i, 6));
                switch (i)
                {
                    case 0:
                    case 7:
                    {//rooks
                        _pieces.Add(new Rook(this, WhiteUser, i, 0));
                        _pieces.Add(new Rook(this, BlackUser, i, 7));
                        continue;
                    }
                    case 1:
                    case 6:
                    {//knights
                        _pieces.Add(new Knight(this, WhiteUser, i, 0));
                        _pieces.Add(new Knight(this, BlackUser, i, 7));
                        continue;
                    }
                    case 2:
                    case 5:
                    {//bishops
                        _pieces.Add(new Bishop(this, WhiteUser, i, 0));
                        _pieces.Add(new Bishop(this, BlackUser, i, 7));
                        continue;
                    }
                    case 3:
                    {//queens
                        _pieces.Add(new Queen(this, WhiteUser, i, 0));
                        _pieces.Add(new Queen(this, BlackUser, i, 7));
                        continue;
                    }
                    case 4:
                    {//kings
                        _pieces.Add(new King(this, WhiteUser, i, 0));
                        _pieces.Add(new King(this, BlackUser, i, 7));
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Move> GetMoves()
        {
            return _moves;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Piece> Pieces
        {
            get
            {
                return _pieces;
            }
        }
    }
}
