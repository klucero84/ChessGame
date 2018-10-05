
using ChessGameAPI.Models.Pieces;
using ChessGameAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessGameAPI.Models
{
    public class Game
    {
        public const int BoardSize = 8;

        public int Id { get; set; }

        protected List<Move> _moves;

        protected List<Piece> _pieces;

        [ForeignKey("WhiteUser")]
        public int? WhiteUserId { get; set; }

        public User WhiteUser { get; set; }

        [ForeignKey("BlackUser")]
        public int? BlackUserId { get; set; }

        public User BlackUser { get; set; }

        public DateTime DateCreated { get; set; }
        
        public DateTime? DateCompleted { get; set;}

        /// Decision to use flags on game instead of 
        /// query move table for performance
        public bool CanWhiteKingSideCastle { get; set; }
        public bool CanWhiteQueenSideCastle { get; set; }
        public bool CanBlackKingSideCastle { get; set; }
        public bool CanBlackQueenSideCastle { get; set; }

        public GameStatus StatusCode { get; set; }

        [NotMapped]
        protected IDictionary<(int, int), Piece> pieceDict;

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Move> Moves
        {
            get
            {
                return _moves;
            }
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
        
        public Game() {
            CanWhiteKingSideCastle = true;
            CanWhiteQueenSideCastle = true;
            CanBlackKingSideCastle = true;
            CanBlackQueenSideCastle = true;
        }

        public Game(User user1, User user2)
        {
            WhiteUser = user1;
            BlackUser = user2;
            CanWhiteKingSideCastle = true;
            CanWhiteQueenSideCastle = true;
            CanBlackKingSideCastle = true;
            CanBlackQueenSideCastle = true;
        }

        /// <summary>
        /// Gets Pieces Dictionary
        /// </summary>
        /// <returns>A dictionary with (x,y) tuple keys for pieces.</returns>
        public IDictionary<(int, int), Piece> GetPieces()
        {
            // cache the dict and use that when modifying data
            if (pieceDict == null && _pieces != null) {
                pieceDict = new Dictionary<(int, int), Piece>();
                foreach(Piece piece in _pieces)
                {
                    pieceDict[(piece.X, piece.Y)] = piece;
                }
            }
            return pieceDict;
        }

        /// <summary>
        /// Gets a piece based on it's x, y coordinates.
        /// </summary>
        /// <param name="x">Abscissas</param>
        /// <param name="y">Ordinate</param>
        /// <returns>the piece at given x, y or null if no piece found.</returns>
        public Piece GetPieceForXY(int x, int y)
        {
            IDictionary<(int, int), Piece> pieces = GetPieces();
            if (pieces != null)
            {
                return pieces[(x, y)];
            }
            return null;
        }

        // add move to game
        public MoveResult TryAddMoveToGame(Move move)
        {
            // GetAllPossibleMovesForAllPieces();
            MoveResult result = IsMoveLegal(move);
            if (result != MoveResult.CanMove)
            {
                return result;
            }
            

            Piece pieceBeingMoved = GetPieceForXY(move.StartX, move.StartY);
            if (pieceBeingMoved != null) {
                Piece pieceAtEndLocation = GetPieceForXY(move.EndX, move.EndY);
                if (pieceAtEndLocation != null) {
                    // determine if the piece already there is our piece
                    // this should never be false, enforcing rule
                    if (pieceAtEndLocation.OwnedBy.Id != move.UserId) {
                    // if it's not the same user's piece they are capturing
                        RemovePiece(pieceAtEndLocation);
                    } else {
                        // throw new Error('Illegal move added');
                    }
                }
                if (move.isCastle) {
                    ApplyCastle(move);
                }
                UpdateCastleAbility(move);
                // Moves.Add(move);
                // game.moves.push(move);
                MovePiece(move.StartX, move.StartY, move.EndX, move.EndY);
                ResetGameStatus();
                GetAllPossibleMovesForAllPieces();
                if (StatusCode == GameStatus.CheckWhite || StatusCode == GameStatus.CheckBlack) {
                    IsCheckMate();
                }
            } else {
                return MoveResult.NoPieceAtStart;
            }
            return result;
        }

        /// <summary
        /// Removes a piece from this game.
        /// </summary>
        /// <param name="pieceToRemove"></param>
        protected void RemovePiece(Piece pieceToRemove)
        {
            GetPieces().Remove((pieceToRemove.X, pieceToRemove.Y));
            _pieces.Remove(pieceToRemove);
        }

        /// <summary>
        /// Lowest level piece movement/ managaes piece x, y info and piece dictionary
        /// Ignores piece movement rules.
        /// </summary>
        /// <param name="StartX">Origin Abscissas</param>
        /// <param name="StartY">Origin Ordinate</param>
        /// <param name="EndX">Destination Abscissas</param>
        /// <param name="EndY">Destination Ordinate</param>
        protected void MovePiece(int StartX, int StartY, int EndX, int EndY)
        {
            Piece piece = this.GetPieceForXY(StartX, StartY);
            if (piece == null)
            {
                return;
            }
            piece.X = EndX;
            piece.Y = EndY;
            IDictionary<(int, int), Piece> pieces = GetPieces();
            pieces.Remove((StartX, StartY));
            // item[key] syntax overwrites on duplicate key
            pieces[(EndX, EndY)] = piece;
        }

        /// <summary>
        /// Determes if the current state of the game is checkmate.
        /// </summary>
        /// <returns>true / false if the game is in checkmate</returns>
        protected bool IsCheckMate()
        {   
            // must alreay be in check
            if (StatusCode == GameStatus.CheckWhite || StatusCode == GameStatus.CheckBlack) {
                bool isCheckMate = true;
                IDictionary<(int, int), Piece> allPieces = GetPieces();
                // look at each piece's possible moves see if moving there would stop check
                foreach(KeyValuePair<(int, int), Piece> locationAndPiece in allPieces)
                {
                    (int, int) originalLocation = locationAndPiece.Key;
                    Piece piece = locationAndPiece.Value;
                    if ((piece.OwnedBy.Id == WhiteUserId && StatusCode == GameStatus.CheckWhite ) ||
                        (piece.OwnedBy.Id == BlackUserId && StatusCode == GameStatus.CheckBlack )) {
                        foreach(KeyValuePair<(int, int), Piece> possibleMove in piece.possibleMoves)
                        {
                            isCheckMate = DoesMoveCauseCheck(piece.X, piece.Y, 
                                            possibleMove.Key.Item1, possibleMove.Key.Item2);
                            if (!isCheckMate) {
                                break ;
                            }
                        }// if a move is found that if applied would create a state where the king is not threatened
                        // then we've determined that this is not check mate.
                    }
                    if (!isCheckMate) {
                        break ;
                    }
                }
                if (isCheckMate) {
                    if (StatusCode == GameStatus.CheckWhite ) {
                        StatusCode = GameStatus.WinWhite;
                    } else if(StatusCode == GameStatus.CheckBlack ) {
                        StatusCode = GameStatus.WinBlack;
                    }
                } else {
                    // if we are not in checkmate game keeps going
                    // we scrambled all the possible moves with testing checkmate
                    // so rebuild for all pieces
                    // GetAllPossibleMovesForAllPieces();
                    // do we need to do this? we aren't maintaining state just determining state
                }
                return isCheckMate;
            }
            return false;
        }
        
        /// <summary>
        /// Determine if moving a piece at (StartX, StartY) to (EndX, EndY) would result in check.
        /// </summary>
        /// <param name="StartX">Origin Abscissas</param>
        /// <param name="StartY">Origin Ordinate</param>
        /// <param name="EndX">Destination Abscissas</param>
        /// <param name="EndY">Destination Ordinate</param>
        /// <returns></returns>
        protected bool DoesMoveCauseCheck(int StartX, int StartY, int EndX, int EndY)
        {
            bool causesCheck = true;
            GameStatus originStatusCode = StatusCode;
            Piece pieceAttemptingMove =  GetPieceForXY(StartX, StartY);
            if (pieceAttemptingMove == null) {
                return false;
            } 
            if (pieceAttemptingMove.OwnedBy == null) {
                return false;
            }
            // spoof piece movement to see possibilities
            pieceAttemptingMove.Y = EndX;
            pieceAttemptingMove.Y = EndY;
            // if this is a capture, remove from map
            Piece pieceAtNewLoction = GetPieceForXY(EndX, EndY);
            IDictionary<(int, int), Piece> allPieces = GetPieces();
            allPieces.Remove((StartX, StartY));
            allPieces[(EndX, EndY)] = pieceAttemptingMove;

            ResetGameStatus();
            GetAllPossibleMovesForAllPieces();
            // if all possible moves generate one that doesn't create a check white status
            // see if we are still in check if we are not then this possible move takes us out of check aka no checkmate.
            if ((pieceAttemptingMove.OwnedBy.Id == WhiteUserId && StatusCode != GameStatus.CheckWhite ) ||
            (pieceAttemptingMove.OwnedBy.Id == BlackUserId && StatusCode != GameStatus.CheckBlack )) {
                causesCheck = false;
            }
            // reset
            // put piece back
            pieceAttemptingMove.X = StartX;
            pieceAttemptingMove.Y = StartY;
            allPieces.Remove((EndX, EndY));
            allPieces[(StartX, StartY)] =pieceAttemptingMove;
            // if there was a piece here put it back
            if (pieceAtNewLoction != null) {
                allPieces[(pieceAtNewLoction.X, pieceAtNewLoction.Y)] = pieceAtNewLoction;
            }

            StatusCode = originStatusCode;
            return causesCheck;
        }
    
        /// <summary>
        /// Reset the game status for testing check / checkmate.
        /// </summary>
        protected void ResetGameStatus()
        {
            StatusCode = GameStatus.Inprogress;
        }

        /// <summary>
        /// Updates Castling Status flags on game based on starting x, y of move.
        /// </summary>
        /// <param name="move">a move being added to the game</param>
        protected void UpdateCastleAbility(Move move)
        {
            // white queen side rook starting location 
            if (CanWhiteQueenSideCastle && move.StartY == 0 && move.StartX == 0) {
                CanWhiteQueenSideCastle = false;
            // white king side rook starting location
            } else if (CanWhiteKingSideCastle && move.StartY == 0 && move.StartX == 7) {
                CanWhiteKingSideCastle = false;
            // white king starting location;
            } else if ((CanWhiteKingSideCastle || CanWhiteQueenSideCastle) && move.StartY == 0 && move.StartX == 4) {
                CanWhiteKingSideCastle = false;
                CanWhiteQueenSideCastle = false;
            // black queen side rook starting location
            } else if (CanBlackQueenSideCastle && move.StartY == 7 && move.StartX == 0) {
                CanBlackQueenSideCastle = false;
            // black king side rook starting location
            } else if (CanBlackKingSideCastle && move.StartY == 7 && move.StartX == 7) {
                CanBlackKingSideCastle = false;
            // black king starting location
            } else if ((CanBlackKingSideCastle || CanBlackQueenSideCastle) && move.StartY == 7 && move.StartX == 4) {
                CanBlackKingSideCastle = false;
                CanBlackQueenSideCastle = false;
            }
        }

        /// <summary>
        /// Low level castling implementation.
        /// Ignores piece movement rules.
        /// </summary>
        /// <param name="move"></param>
        protected void ApplyCastle(Move move)
        {
            Piece piece;
            if (move.StartX == 4 && move.StartY == 0) {
                if (move.EndX == 6 && move.EndY == 0) {
                    piece = GetPieceForXY(7, 0);
                    MovePiece(piece.X, piece.Y, 5, piece.Y);
                } else if (move.EndX == 2 && move.EndY == 0) {
                    piece = GetPieceForXY(0, 0);
                    MovePiece(piece.X, piece.Y, 3, piece.Y);
                }
            } else if (move.StartX == 4 && move.StartY == 7) {
                if (move.EndX == 6 && move.EndY == 7) {
                    piece = GetPieceForXY(7, 7);
                    MovePiece(piece.X, piece.Y, 5, piece.Y);
                } else if (move.EndX == 2 && move.EndY == 7) {
                    piece = GetPieceForXY(0, 7);
                    MovePiece(piece.X, piece.Y, 3, piece.Y);
                }
            }
        }
    
        /// <summary>
        /// Builds possible move collections for all pieces in this game.
        /// </summary>
        protected void GetAllPossibleMovesForAllPieces()
        {
            IDictionary<(int, int), Piece> pieces = GetPieces();
            foreach(Piece piece in pieces.Values)
            {
                piece.Game = this;
                piece.GetAllLegalMoves();
            }
        }
        
        /// <summary>
        /// Attempt moving a piece from (StartX, StartY) to (EndX, EndY) 
        /// Ignores piece movement rules.
        /// Adds to possible moves collection.
        /// </summary>
        /// <param name="StartX">Origin Abscissas</param>
        /// <param name="StartY">Origin Ordinate</param>
        /// <param name="EndX">Destination Abscissas</param>
        /// <param name="EndY">Destination Ordinate</param>
        /// <param name="isPawnCapture">If the attempted move is a pawn capture</param>
        /// <returns>true if new square is empty; false if occupied.</returns>
        internal bool tryMove(int StartX, int StartY, int endX, int endY, bool isPawnCapture = false)
        {
            Piece pieceToMove =  GetPieceForXY(StartX, StartY);
            if (pieceToMove == null) {
                return false;
            } 
            if (pieceToMove.OwnedBy == null) {
                return false;
            }
            Piece pieceAtEndLocation = GetPieceForXY(endX, endY);
            if (pieceAtEndLocation?.OwnedBy?.Id != null && pieceToMove?.OwnedBy?.Id != null)
            {
                // if piece is not ours we can move there but no further
                if (pieceAtEndLocation.OwnedBy.Id != pieceToMove.OwnedBy.Id)
                {
                    // piece threatened by new move, not really needed for check but might be nice ui feature
                    if (pieceAtEndLocation.Discriminator == "King")
                    {
                        // the owner of pieceAtNewLocation is in check
                        StatusCode = pieceAtEndLocation.OwnedBy.Id == WhiteUserId ? 
                                            GameStatus.CheckWhite : GameStatus.CheckBlack;
                    }
                    // if attempt to automate this would be where one could store the next tree level possible moves
                    pieceToMove.possibleMoves[(endX, endY)] = pieceAtEndLocation;
                }
                // we can't keep moving if piece is there
                return false;
            }
            if (isPawnCapture) {
                // pawns must be able to capture to make a capture move
                return false;
            }
            // add key with null 
            pieceToMove.possibleMoves[(endX, endY)] = null;
            return true ;
        }
        // is legal move
        // is castle legal

        /// <summary>
        /// Tests to see if move is legal.
        /// </summary>
        /// <param name="move">Move to test.</param>
        /// <returns>blank string if legal if not error message</returns>
        protected MoveResult IsMoveLegal(Move move)
        {
           if (move != null) {
                Piece piece = GetPieceForXY(move.StartX, move.StartY);
                if (piece == null) {
                    return MoveResult.NoPieceAtStart;
                }
                bool causesCheck = DoesMoveCauseCheck(move.StartX, move.StartY, move.EndX, move.EndY);
                if (causesCheck) {
                    return MoveResult.CannotMoveIntoCheck;
                }
                bool canMove = piece.possibleMoves.ContainsKey((move.EndX, move.EndY));
                if (canMove) {
                    if (move.PieceDiscriminator == "King") {
                        return IsCastleLegal(move);
                    // gonna need clause here for en passant
                    }
                    return MoveResult.CanMove;
                }
                switch (move.PieceDiscriminator) {
                    case "Pawn":
                        return MoveResult.Pawn;

                    case "Rook":
                        return MoveResult.Rook;

                    case "Knight":
                        return MoveResult.Knight;

                    case "Bishop":
                        return MoveResult.Bishop;

                    case "Queen":
                        return MoveResult.Queen;

                    case "King":
                        return MoveResult.King;
                    default:
                        return MoveResult.DiscriminatorNotFound;
                }
            } else {
                return MoveResult.NullMove;
            }
        }

        /// <summary>
        /// Tests to see if castle move is legal.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        protected MoveResult IsCastleLegal(Move move)
        {
            bool inCheck = false;
            if ((StatusCode == GameStatus.CheckBlack && move.UserId == BlackUserId) ||
                (StatusCode == GameStatus.CheckWhite && move.UserId == WhiteUserId) ) {
                    inCheck = true;
                }
            if (move.UserId == WhiteUserId && move.StartX == 4 && move.StartY == 0) {
                if (move.EndX == 6 && move.EndY == 0 && CanWhiteKingSideCastle) {
                    // bool canKingMove = GetPieceForXY(4, 0).possibleMoves.Keys.Contains((6, 0));
                    bool canRookMove = GetPieceForXY(7, 0).possibleMoves.Keys.Contains((5, 0));
                    if (DoesMoveCauseCheck(4, 0, 5, 0)) {
                        return MoveResult.CannotCastleThroughCheck;
                    }
                    if (inCheck) {
                        return MoveResult.CannotCastleInCheck;
                    }
                    if (!canRookMove) {
                        return MoveResult.CannotCastleThroughPieces;
                    }
                    move.isCastle = true;
                } else if (move.EndX == 2 && move.EndY == 0 && CanWhiteQueenSideCastle) {
                    // bool canKingMove = GetPieceForXY(4, 0).possibleMoves.Keys.Contains((2, 0));
                    bool canRookMove = GetPieceForXY(0, 0).possibleMoves.Keys.Contains((3, 0));
                    if (DoesMoveCauseCheck(4, 0, 3, 0)) {
                        return MoveResult.CannotCastleThroughCheck;
                    }
                    if (inCheck) {
                        return MoveResult.CannotCastleInCheck;
                    }
                    if (!canRookMove) {
                        return MoveResult.CannotCastleThroughPieces;
                    }
                    move.isCastle = true;
                }
            } else if (move.UserId == BlackUserId && move.StartX == 4 && move.StartY == 7) {
                if (move.EndX == 6 && move.EndY == 7 && CanBlackKingSideCastle) {
                    bool canKingMove = GetPieceForXY(4, 7).possibleMoves.Keys.Contains((6, 7));
                    bool canRookMove = GetPieceForXY(7, 7).possibleMoves.Keys.Contains((5, 7));
                    if (DoesMoveCauseCheck(4, 7, 5, 7)) {
                        return MoveResult.CannotCastleThroughCheck;
                    }
                    if (inCheck) {
                        return MoveResult.CannotCastleInCheck;
                    }
                    if (!canRookMove) {
                        return MoveResult.CannotCastleThroughPieces;
                    }
                    move.isCastle = true;
                } else if (move.EndX == 2 && move.EndY == 7 && CanBlackQueenSideCastle) {
                    bool canKingMove = GetPieceForXY(4, 7).possibleMoves.Keys.Contains((2, 7));
                    bool canRookMove = GetPieceForXY(0, 7).possibleMoves.Keys.Contains((3, 7));
                    if (DoesMoveCauseCheck(4, 7, 3, 7)) {
                        return MoveResult.CannotCastleThroughCheck;
                    }
                    if (inCheck) {
                        return MoveResult.CannotCastleInCheck;
                    }
                    if (!canRookMove) {
                        return MoveResult.CannotCastleThroughPieces;
                    }
                    move.isCastle = true;
                }
            }
            // only concerned with castling legality.
            return MoveResult.CanMove;
        }

        /// <summary>
        /// Sets the game to new status, new pieces, new moves
        /// </summary>
        public void Initialize()
        {
            pieceDict = new Dictionary<(int, int), Piece>();
            _pieces = new List<Piece>(32);
            _moves = new List<Move>();
            DateCreated = DateTime.Now;
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
    }
}
