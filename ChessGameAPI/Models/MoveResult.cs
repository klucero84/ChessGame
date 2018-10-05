using System.ComponentModel;

namespace ChessGameAPI.Models
{
    public enum MoveResult
    {
        [Description("")]
        CanMove,

        [Description("Null move")]
        NullMove,

        [Description("Discriminator Not Implemented")]
        DiscriminatorNotFound,

        [Description("Attempted move is out of bounds.")]
        OutOfBounds,

        [Description("Attempted move doesn't change board state.")]
        NoChange,

        [Description("No piece at starting location")]
        NoPieceAtStart,

        [Description("A player may not make any move that places or leaves their king in check.")]
        CannotMoveIntoCheck,

        [Description("Cannot Castle while in check.")]
        CannotCastleInCheck,

        [Description("'King cannot move through check, while Castling.'")]
        CannotCastleThroughCheck,

        [Description("There must be no pieces between the king and chosen Rook, while Castling.")]
        CannotCastleThroughPieces,

        [Description("Pieces must move to either an unoccupied square or one occupied by an opponent's piece.")]
        SelfOccupiedSquare,

        // = "";
        // public const string 
        [Description("A Bishop must move in a diagonal line, and cannot jump over other pieces.")]
        Bishop,

        [Description("The King can only move one space in any direction, unless castling.")]
        King,

        [Description("A Knight must move two spaces on one axis and one space on the other axis.")]
        Knight,

        [Description("A Pawn may only move forward one space at a time, capture diagonally, " +
                        "and may move two spaces forward if it is the first move of the pawn.")]
        Pawn,

        [Description("A Queen must move in a in straight lines along the x or y axis, or in a diagonal line, and cannot jump over other pieces.")]
        Queen,

        [Description("A Rook must move in straight lines along the x or y axis, and cannot jump over other pieces.")]
        Rook
    }
}