using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    public abstract class Piece
    {
        /// <summary>
        /// Unique db identifier
        /// </summary>
        public int Id { get; set; }

        private int _x;
        private int _y;
        private readonly User _user;

        /// <summary>
        /// Abscissas value (x)
        /// </summary>
        public int X { get; protected set; }


        /// <summary>
        /// Ordinate value (y)
        /// </summary>
        public int Y { get; protected set; }

        /// <summary>
        /// Owner of this piece
        /// </summary>
        /// <returns></returns>
        public User OwnedBy() {
            return _user;
        }

        private Board _board;

        public Piece(Board board, User user)
        {
            _board = board;
            _user = user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attemptedMove"></param>
        /// <returns></returns>
        public (bool, string) IsLegalMove(Move attemptedMove)
        {
            //shared logic for determining legality of a move is here
            if (!IsMe(attemptedMove?.Piece)) {
                return (false, "Piece doesn't match");
            }
            if (!IsMyUser(attemptedMove?.User)){
                return (false, "User doesn't match");
            }
            if (IsOutOfBounds(attemptedMove)) {
                return (false, "attempted move is out of bounds");
            }

            //determine if move will put user in check

            //determine if user is in check and this will not take them out of check


            //each subclass is responsible for it's own class (piece) specific movement logic
            return IsLegalMoveForPiece(attemptedMove);
        }


        //protected Move MovePiece(User user, Int32 newX, Int32 newY)
        //{

        //}

        protected abstract (bool, string) IsLegalMoveForPiece(Move attemptedMove);

        public abstract List<Move> GetAllLegalMoves();
        
        protected bool IsMe (Piece piece)
        {
            if (piece?.Id == Id)
            {
                return true;
            }
            return false;
        }

        protected bool IsMyUser(User user)
        {
            if (user?.Id == _user.Id)
            {
                return true;
            }
            return false;
        }

        protected bool IsOutOfBounds(Move move)
        {
            if (move?.EndY > Board.BoardSize || move?.EndY < 0) {
                return true;
            }
            if (move?.EndX > Board.BoardSize || move?.EndX < 0) {
                return true;
            }
            return false;
        }
        
    }


}
