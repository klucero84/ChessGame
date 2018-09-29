using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGameAPI.Models
{
    public class Piece
    {
        /// <summary>
        /// Unique db identifier
        /// </summary>
        public int Id { get; set; }

       
        /// <summary>
        /// Abscissas value (x)
        /// </summary>
        [Required]
        public int X { get; set; }

        /// <summary>
        /// Ordinate value (y)
        /// </summary>
        [Required]
        public int Y { get; set; }

        /// <summary>
        /// Owner of this piece
        /// </summary>
        /// <returns></returns>
        public User OwnedBy { get; set; }

        /// <summary>
        /// Game to which this piece belongs
        /// </summary>
        /// <value></value>
        [Required]
        public Game Game { get; set; }

        /// <summary>
        /// subclass discriminator
        /// </summary>
        /// <value></value>
        public string Discriminator { get; set; }

        public IEnumerable<Move> Moves { get; set;}

        /// <summary>
        /// Blank Constructor 
        /// </summary>
        public Piece() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Game to which the piece belongs</param>
        /// <param name="user">User to which the piece belongs</param>
        /// <param name="x">Abscissas value of piece</param>
        /// <param name="y">Ordinate value of the piece</param>
        public Piece(Game game, User user, int x, int y)
        {
            Game = game;
            OwnedBy = user;
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attemptedMove"></param>
        /// <returns></returns>
        public (bool, string) IsLegalMove(Move attemptedMove, bool isWhite)
        {
            //shared logic for determining legality of a move is here

            if (IsOutOfBounds(attemptedMove))
            {
                return (false, MoveErrors.OutOfBounds);
            }
            if (!IsActuallyMoving(attemptedMove))
            {
                return (false, MoveErrors.NoChange);
            }
            //determine if move will put user in check

            //determine if user is in check and this will not take them out of check


            //each subclass is responsible for it's own class (piece) specific movement logic
            return IsLegalMoveForPiece(attemptedMove, isWhite);
        }


        public (bool isLegal, string message) IsLegalMoveTwoPlayer(Move attemptedMove, bool isWhite) {
            //shared logic for determining legality of a move is here
            if (IsOutOfBounds(attemptedMove))
            {
                return (false, MoveErrors.OutOfBounds);
            }
            if (!IsActuallyMoving(attemptedMove))
            {
                return (false, MoveErrors.NoChange);
            }
            // virtual method sub classes implement to have their exteneded functionality.
            return IsLegalMoveForPiece(attemptedMove, isWhite);
        }
        protected virtual (bool, string) IsLegalMoveForPiece(Move attemptedMove, bool isWhite)
        {
            throw new NotImplementedException("IsLegalMoveForPiece is not implemented.");
        }

        public virtual List<Move> GetAllLegalMoves()
        {
            throw new NotImplementedException("GetAllLegalMoves is not imeplemented.");
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

    public static class MoveErrors {
        public const string OutOfBounds = "Attempted move is out of bounds.";
        public const string NoChange = "Attempted move doesn't change board state.";
        public const string SelfOccupiedSquare = "Pieces must move to either an unoccupied square or one occupied by an opponent's piece.";
        public const string Bishop = "A Bishop must move in a diagonal line";
        public const string King = "The King can only move one space in any direction, unless castling.";
        public const string KingCastling = "Castling";
        public const string Knight =  "A Knight must move two spaces on one axis and one space on the other axis.";
        public const string Pawn = "A Pawn may only move forward one space at a time, " +
                "capture diagonally, and may move two spaces forward if it is the first move of the pawn.";
        public const string Queen ="A Queen must move in a in straight lines along the x or y axis, or in a diagonal line.";
        public const string Rook ="A Rook must move in straight lines along the x or y axis";



    }
}
