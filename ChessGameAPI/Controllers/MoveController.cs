using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using ChessGameAPI.Hubs;
using ChessGameAPI.Models;
using ChessGameAPI.Models.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChessGameAPI.Controllers
{
    /// <summary>
    /// Controller responsible for moves.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly IGameRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<MoveHub> _hub;

        /// <summary>
        /// Controller responsible for moves.
        /// </summary>
        /// <param name="repo">Data repository for moves</param>
        /// <param name="mapper">automapper utility</param>
        public MoveController (IGameRepository repo, IMapper mapper, IHubContext<MoveHub> hub)
        {
            _repo = repo;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("~/api/game/{gameId:int}/moves")]
        public async Task<IActionResult> GetMovesForGame(int gameId)
        {
            await _repo.SaveAll();
            return Ok();
        }


        /// <summary>
        /// Main action driver. updates peices and moves list that represent the game.
        /// </summary>
        /// <param name="dto">Move information to add to the data repo</param>
        /// <returns>an asynchronous operation returning an action result</returns>
        [HttpPost]
        public async Task<IActionResult> AddMove(MoveForAddMoveDto dto)
        {
            
            Move newMove = _mapper.Map<Move>(dto);
            _repo.Add(newMove);
            Piece movedPiece = await _repo.GetPiece(dto.PieceId);
            
            var moveAttempt =  movedPiece.IsLegalMove(newMove, dto.IsWhite);
            if (moveAttempt.Item1) {

                movedPiece.X = newMove.EndX;
                movedPiece.Y = newMove.EndY;
                int code = await _repo.SaveAll();
                return Ok(code); 
            } else {
                return BadRequest(moveAttempt.Item2);
            }
           
        }

        [HttpPost("~/api/move/two-player")]
        public async Task<IActionResult> AddMoveTwoPlayer(MoveForAddMoveDto dto)
        {
            Move newMove = _mapper.Map<Move>(dto);
            _repo.Add(newMove);
            Piece movedPiece = await _repo.GetPiece(dto.PieceId);
            if (movedPiece == null){
                return BadRequest("Piece not found");
            }
            var moveAttempt =  movedPiece.IsLegalMoveTwoPlayer(newMove, dto.IsWhite);
            if (moveAttempt.Item1) {
                Piece pieceAtNewLocation = await _repo.GetPieceByXY(dto.GameId, dto.EndX, dto.EndY);
                if (pieceAtNewLocation != null) {
                    if (pieceAtNewLocation.OwnedBy.Id != dto.UserId){
                        _repo.Delete(pieceAtNewLocation);
                        dto.IsCapture = true;
                    } else {
                        return BadRequest(MoveErrors.SelfOccupiedSquare);
                    }
                }
                // It might be better to keep this as a flag on game that gets triggered instead of querying moves table
                // if we are castling we need to look at all our previous moves
                if (moveAttempt.Item2 == MoveErrors.KingCastling){
                    if ((dto.IsWhite && dto.StartX == 4 && dto.StartY == 0) || 
                        (!dto.IsWhite && dto.StartX == 4 && dto.StartY == 7)) {
                            // check status flag on game model
                        Game currentGame = await _repo.GetGameMin(dto.GameId);
                        Piece rookToMove = null;
                        int rookX = 0, rookY = 0, newRookX = 0;
                        if (dto.IsWhite && dto.StartX == 4 && dto.StartY == 0) {
                            if (dto.EndX == 2 && dto.EndY == 0 && currentGame.CanWhiteKingSideCastle) {
                                rookX = 0;
                                rookY = 0;
                                newRookX = 3;
                            } else if (dto.EndX == 6 && dto.EndY == 0 && currentGame.CanWhiteQueenSideCastle) {
                                rookX = 7;
                                rookY = 0;
                                newRookX = 5;
                            }
                        } else if (!dto.IsWhite && dto.StartX == 4 && dto.StartY == 7) {
                            if (dto.EndX == 2 && dto.EndY == 7 && currentGame.CanBlackKingSideCastle) {
                                rookX = 7;
                                rookY = 7;
                                newRookX = 5;
                            } else if (dto.EndX == 6 && dto.EndY == 7 && currentGame.CanBlackQueenSideCastle) {
                                rookX = 0;
                                rookY = 7;
                                newRookX = 3;
                            }
                        }
                        // at this point we've determined that we are allowed to according to the game
                        // and that the king is moving from the correct position
                        

                        
                        // IList<Move> previousMoves = await _repo.GetMovesForGameForUser(dto.GameId, dto.UserId);
                        // bool kingSide = (dto.StartX - dto.EndX) < 0; 
                        // foreach(Move previousMove in previousMoves){
                        //     // white side king start location
                        //     if ((dto.isWhite && (previousMove.StartX == 4 && previousMove.StartY == 0) && 
                        //     // white side king side's rook start location
                        //     (kingSide && (previousMove.StartX == 7 && previousMove.StartY == 0)) || 
                        //     // white side queen side's rook start location
                        //     !kingSide && (previousMove.StartX == 0 && previousMove.StartY ==0))
                        //     // black side king start location
                        //     ||(!dto.isWhite && (previousMove.StartX == 4 && previousMove.StartY == 7) &&
                        //     // black side king side's rook start location
                        //     (kingSide && (previousMove.StartX == 7 && previousMove.StartY == 7 ) ||
                        //     // black side queen side's rook start location
                        //     (!kingSide && (previousMove.StartX == 0 && previousMove.StartY == 7))))){
                        //         return BadRequest(MoveErrors.King);
                        //     }
                        // }

                        rookToMove = await _repo.GetPieceByXY(dto.GameId, rookX, rookY);
                        var rookCasted = rookToMove as Rook;
                        if (rookCasted != null) {
                            rookCasted.X = newRookX;
                            dto.IsCastle = true;
                        }
                        // return dto.IsCastle;
                    } else {
                        return BadRequest(MoveErrors.King);
                    }
                }
                
                movedPiece.X = newMove.EndX;
                movedPiece.Y = newMove.EndY;
                if (dto.StartY  == 0 && dto.StartX == 0) {
                    Game currentGame = await _repo.GetGameMin(dto.GameId);
                    currentGame.CanWhiteQueenSideCastle = false;
                } else if (dto.StartY == 0 && dto.StartX == 7) {
                    Game currentGame = await _repo.GetGameMin(dto.GameId);
                    currentGame.CanWhiteKingSideCastle = false;
                } else if (dto.StartY == 0 && dto.StartX == 4) {
                    Game currentGame = await _repo.GetGameMin(dto.GameId);
                    currentGame.CanWhiteKingSideCastle = false;
                    currentGame.CanWhiteQueenSideCastle = false;
                } else if (dto.StartY == 0 && dto.StartX == 0) {
                    Game currentGame = await _repo.GetGameMin(dto.GameId);
                    currentGame.CanBlackQueenSideCastle = false;
                } else if (dto.StartY == 0 && dto.StartX == 7) {
                    Game currentGame = await _repo.GetGameMin(dto.GameId);
                    currentGame.CanBlackKingSideCastle = false;
                } else if (dto.StartY == 0 && dto.StartX == 4) {
                    Game currentGame = await _repo.GetGameMin(dto.GameId);
                    currentGame.CanBlackKingSideCastle = false;
                    currentGame.CanBlackQueenSideCastle = false;
                }
                // this.UpdateCastleStatus(dto);
                int code = await _repo.SaveAll();
                await _hub.Clients.GroupExcept(dto.GameId.ToString(), dto.connId).SendAsync("addMoveToGame", dto);
                return Ok(dto); 
            } else {
                return BadRequest(moveAttempt.Item2);
            }
        }

        // private async Task<bool> AttemptCastle(MoveForAddMoveDto dto) 
        // {
        //     if ((dto.IsWhite && dto.StartX == 4 && dto.StartY == 0) || 
        //         (!dto.IsWhite && dto.StartX == 4 && dto.StartY == 7)) {
        //             // check status flag on game model
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         Piece rookToMove = null;
        //         int rookX = 0, rookY = 0, newRookX = 0;
        //         if (dto.IsWhite && dto.StartX == 4 && dto.StartY == 0) {
        //             if (dto.EndX == 2 && dto.EndY == 0 && currentGame.CanWhiteKingSideCastle) {
        //                 rookX = 7;
        //                 rookY = 0;
        //                 newRookX = 5;
        //             } else if (dto.EndX == 6 && dto.EndY == 0 && currentGame.CanWhiteQueenSideCastle) {
        //                 rookX = 0;
        //                 rookY = 0;
        //                 newRookX = 2;
        //             }
        //         } else if (!dto.IsWhite && dto.StartX == 4 && dto.StartY == 7) {
        //             if (dto.EndX == 2 && dto.EndY == 7 && currentGame.CanBlackKingSideCastle) {
        //                 rookX = 7;
        //                 rookY = 7;
        //                 newRookX = 5;
        //             } else if (dto.EndX == 6 && dto.EndY == 7 && currentGame.CanBlackQueenSideCastle) {
        //                 rookX = 0;
        //                 rookY = 7;
        //                 newRookX = 2;
        //             }
        //         }
        //         // at this point we've determined that we are allowed to according to the game
        //         // and that the king is moving from the correct position
                

                
        //         // IList<Move> previousMoves = await _repo.GetMovesForGameForUser(dto.GameId, dto.UserId);
        //         // bool kingSide = (dto.StartX - dto.EndX) < 0; 
        //         // foreach(Move previousMove in previousMoves){
        //         //     // white side king start location
        //         //     if ((dto.isWhite && (previousMove.StartX == 4 && previousMove.StartY == 0) && 
        //         //     // white side king side's rook start location
        //         //     (kingSide && (previousMove.StartX == 7 && previousMove.StartY == 0)) || 
        //         //     // white side queen side's rook start location
        //         //     !kingSide && (previousMove.StartX == 0 && previousMove.StartY ==0))
        //         //     // black side king start location
        //         //     ||(!dto.isWhite && (previousMove.StartX == 4 && previousMove.StartY == 7) &&
        //         //     // black side king side's rook start location
        //         //     (kingSide && (previousMove.StartX == 7 && previousMove.StartY == 7 ) ||
        //         //     // black side queen side's rook start location
        //         //     (!kingSide && (previousMove.StartX == 0 && previousMove.StartY == 7))))){
        //         //         return BadRequest(MoveErrors.King);
        //         //     }
        //         // }

        //         rookToMove = await _repo.GetPieceByXY(dto.GameId, rookX, rookY);
        //         var rookCasted = rookToMove as Rook;
        //         if (rookCasted != null) {
        //             rookCasted.X = newRookX;
        //             dto.IsCastle = true;
        //         }
        //         return dto.IsCastle;
        //     } else {
        //         return false;
        //     }
        // }

        // private async void UpdateCastleStatus(MoveForAddMoveDto dto)
        // {
        //     // udpate status flag on game model
        //     // instead of looking at move list
        //     if (dto.StartY  == 0 && dto.StartX == 0) {
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         currentGame.CanWhiteQueenSideCastle = false;
        //     } else if (dto.StartY == 0 && dto.StartX == 7) {
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         currentGame.CanWhiteKingSideCastle = false;
        //     } else if (dto.StartY == 0 && dto.StartX == 4) {
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         currentGame.CanWhiteKingSideCastle = false;
        //         currentGame.CanWhiteQueenSideCastle = false;
        //     } else if (dto.StartY == 0 && dto.StartX == 0) {
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         currentGame.CanBlackQueenSideCastle = false;
        //     } else if (dto.StartY == 0 && dto.StartX == 7) {
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         currentGame.CanBlackKingSideCastle = false;
        //     } else if (dto.StartY == 0 && dto.StartX == 4) {
        //         Game currentGame = await _repo.GetGameMin(dto.GameId);
        //         currentGame.CanBlackKingSideCastle = false;
        //         currentGame.CanBlackQueenSideCastle = false;
        //     }
        // }
    }
}