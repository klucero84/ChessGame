using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    public class Piece
    {
        /// <summary>
        /// Unique db identifier
        /// </summary>
        public int Id { get; set; }

        [Required]
        /// <summary>
        /// Abscissas value (x)
        /// </summary>
        public int X { get; set; }

        [Required]
        /// <summary>
        /// Ordinate value (y)
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Owner of this piece
        /// </summary>
        /// <returns></returns>
        [Required]
        public User OwnedBy { get; set; }

        [Required]
        public Game Game { get; set; }

        /// <summary>
        /// Blank Constructor 
        /// </summary>
        public Piece() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">User to which the piece belongs</param>
        /// <param name="x">Abscissas value of piece</param>
        /// <param name="y">Ordinate value of the piece</param>
        public Piece(User user, int x, int y)
        {
            OwnedBy = user;
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attemptedMove"></param>
        /// <returns></returns>
        public (bool, string) IsLegalMove(Move attemptedMove)
        {
            //shared logic for determining legality of a move is here
            if (!IsMe(attemptedMove?.Piece))
            {
                return (false, "Piece doesn't match.");
            }
            if (!IsMyUser(attemptedMove?.User))
            {
                return (false, "User doesn't match.");
            }
            if (IsOutOfBounds(attemptedMove))
            {
                return (false, "Attempted move is out of bounds.");
            }
            if (!IsActuallyMoving(attemptedMove))
            {
                return (false, "Attempted move doesn't change board state.");
            }
            //determine if move will put user in check

            //determine if user is in check and this will not take them out of check


            //each subclass is responsible for it's own class (piece) specific movement logic
            return IsLegalMoveForPiece(attemptedMove);
        }

        protected virtual (bool, string) IsLegalMoveForPiece(Move attemptedMove)
        {
            throw new NotImplementedException("IsLegalMoveForPiece is not implemented.");
        }

        public virtual List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException("GetAllLegalMoves is not imeplemented.");
        }
        
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
            if (user?.Id == OwnedBy.Id)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the move put a piece out bounds.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected bool IsOutOfBounds(Move move)
        {
            if (move?.EndY > Game.BoardSize - 1 || move?.EndY < 0) {
                return true;
            }
            if (move?.EndX > Game.BoardSize - 1 || move?.EndX < 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the move has different X and Y coordinates aka the move moves a piece.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected bool IsActuallyMoving(Move move)
        {
            return !(X == move.EndX && Y == move.EndY);
        }
    }
}
