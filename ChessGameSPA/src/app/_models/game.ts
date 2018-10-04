import { User } from './user';
import { Piece } from './piece';
import { Move } from './move';
import { GameStatus } from './game-status';

export class Game {
    id: number;
    connId: string;
    whiteUser: User;
    blackUser: User;
    private pieces?: Piece[];
    moves?: Move[];
    // <x-y, piece>
    private piecesMap?: Map<string, Piece>;
    dateCreated: Date;
    dateCompleted?: Date;
    statusCode: GameStatus;
    canWhiteKingSideCastle: boolean;
    canWhiteQueenSideCastle: boolean;
    canBlackKingSideCastle: boolean;
    canBlackQueenSideCastle: boolean;


    // a Map<string, Piece> that has each piece's xy location as key
    static getPiecesMap(game: Game) {
        // cache the map and use that when modifying data
        if (game && !game.piecesMap && game.pieces) {
            game.piecesMap = new Map<string, Piece>();
            for (const piece of game.pieces) {
                game.piecesMap.set(Piece.getMapKey(piece.x, piece.y), piece);
            }
        }
        return game.piecesMap;
    }

    // returns a piece from the pieces map based on x, y
    static getPieceForXY(game: Game, x: number, y: number) {
        const map = this.getPiecesMap(game);
        if (map) {
            return map.get(Piece.getMapKey(x, y));
        }
    }

    // process for adding a move to a game
    // entry point for all move logic
    static addMoveToGame(move: Move, game: Game) {

        const pieceBeingMoved = this.getPieceForXY(game, move.startX, move.startY);
        if (pieceBeingMoved) {
          const pieceAtEndLocation = this.getPieceForXY(game, move.endX, move.endY);
          if (pieceAtEndLocation) {
            // determine if the piece already there is our piece
            // this should never be false, enforcing rule
            if (pieceAtEndLocation.ownedBy.id !== move.userId) {
              // if it's not the same user's piece they are capturing
              this.removePiece(game, move.endX, move.endY);
            } else {
                throw new Error('Illegal move added');
            }
          }
          if (move.isCastle) {
              this.applyCastle(game, move);
          }
          this.updateCastleAbility(game, move);
          game.moves.push(move);
          this.movePiece(game, pieceBeingMoved, move.startX, move.startY, move.endX, move.endY);
        } else {
            throw new Error('No piece being moved.');
        }
    }

    private static removePiece(game: Game, x: number, y: number) {
        const map = this.getPiecesMap(game);
        map.delete(Piece.getMapKey(x, y));
    }

    private static movePiece(game: Game, piece: Piece, startX: number, startY: number, endX: number, endY: number, isCastle = false) {
        piece.x = endX;
        piece.y = endY;
        const map = this.getPiecesMap(game);
        map.delete(Piece.getMapKey(startX, startY));
        map.set(Piece.getMapKey(endX, endY), piece);
        if (!isCastle) {
            this.resetGameStatus(game);
            this.getPossibleMovesforAllPieces(game);
        }
        if (game.statusCode === GameStatus.CheckWhite || game.statusCode === GameStatus.CheckBlack) {
            this.isCheckMate(game);
        }
    }

    private static isCheckMate(game: Game) {
        if (game.statusCode === GameStatus.CheckWhite || game.statusCode === GameStatus.CheckBlack) {
            const allPieces = this.getPiecesMap(game);
            let isCheckMate = true;
            const pieceArray = Array.from(allPieces.entries());
            let keyCounter = pieceArray.length;
            // look at each piece's possible moves see if moving there would stop check
            while (isCheckMate &&  keyCounter > 0) {
                keyCounter--;
                // <string (xy), Piece> = <string (xy), Piece>[i]
                const pieceEntry = pieceArray[keyCounter];
                const piece = pieceEntry[1];
                if ((piece.ownedBy.id === game.whiteUser.id && game.statusCode === GameStatus.CheckWhite ) ||
                    (piece.ownedBy.id !== game.whiteUser.id && game.statusCode === GameStatus.CheckBlack )) {

                    const moveArray = Array.from(piece.possibleMoves);
                    let possibleMoveCounter =  moveArray.length;
                    while (isCheckMate && possibleMoveCounter > 0) {

                        possibleMoveCounter--;
                        const possibleMoveXY = moveArray[possibleMoveCounter];
                        // xy of possible move
                        const xyTuple = Piece.getXYFromKey(possibleMoveXY[0]);
                        isCheckMate = this.doesMoveCauseCheck(game, piece.x, piece.y, xyTuple.x, xyTuple.y);
                    }
                }
            }
            if (isCheckMate) {
                game.statusCode =
                game.statusCode === GameStatus.CheckWhite ? GameStatus.WinBlack : GameStatus.WinWhite;
            } else {
                // if we are not in checkmate game keeps going
                // we scrambled all the possible moves with testing checkmate
                // so rebuild for all pieces
                this.getPossibleMovesforAllPieces(game);
            }
        }
    }

    /// always call getPossibleMovesforAllPieces after because scrambles all the possible moves
    // not called inside so this can be called in loop
    private static doesMoveCauseCheck(game: Game, startX: number, startY: number, endX: number, endY: number) {
        let causesCheck = true;
        const originStatusCode = game.statusCode;
        const piece =  this.getPieceForXY(game, startX, startY);
        if (!piece) {
            return null;
        }
        // spoof piece movement to see possibilities
        piece.x = endX;
        piece.y = endY;
        // if this is a capture, remove from map
        const pieceAtLoction = this.getPieceForXY(game, endX, endY);
        const allPieces = Game.getPiecesMap(game);
        allPieces.delete(Piece.getMapKey(startX, startY));
        allPieces.set(Piece.getMapKey(endX, endY), piece);

        this.resetGameStatus(game);
        this.getPossibleMovesforAllPieces(game);
        // if all possible moves generate one that doesn't create a check white status
        // see if we are still in check if we are not then this possible move takes us out of check aka no checkmate.
        if ((piece.ownedBy.id === game.whiteUser.id && game.statusCode !== GameStatus.CheckWhite ) ||
        (piece.ownedBy.id !== game.whiteUser.id && game.statusCode !== GameStatus.CheckBlack )) {
            causesCheck = false;
        }
        // reset
        // put piece back
        piece.x = startX;
        piece.y = startY;
        allPieces.delete(Piece.getMapKey(endX, endY));
        allPieces.set(Piece.getMapKey(startX, startY), piece);
        // if there was a piece here put it back
        if (pieceAtLoction) {
            allPieces.set(Piece.getMapKey(pieceAtLoction.x, pieceAtLoction.y), pieceAtLoction);
        }

        game.statusCode = originStatusCode;
        return causesCheck;
    }
    private static resetGameStatus(game: Game) {
        game.statusCode = GameStatus.Inprogress;
    }

    private static updateCastleAbility(game: Game, move: Move) {
        if (game.canWhiteQueenSideCastle && move.startY === 0 && move.startX === 0) {
            game.canWhiteQueenSideCastle = false;
          } else if (game.canWhiteKingSideCastle && move.startY === 0 && move.startX === 7) {
            game.canWhiteKingSideCastle = false;
          } else if ((game.canWhiteKingSideCastle || game.canWhiteQueenSideCastle) &&
              move.startY === 0 && move.startX === 4) {
            game.canWhiteKingSideCastle = false;
            game.canWhiteQueenSideCastle = false;
          } else if (game.canBlackQueenSideCastle && move.startY === 0 && move.startX === 0) {
            game.canBlackQueenSideCastle = false;
          } else if (game.canBlackKingSideCastle && move.startY === 0 && move.startX === 7) {
            game.canBlackKingSideCastle = false;
          } else if ((game.canBlackKingSideCastle || game.canBlackQueenSideCastle) &&
              move.startY === 0 && move.startX === 4) {
            game.canBlackKingSideCastle = false;
            game.canBlackQueenSideCastle = false;
          }
    }

    private static applyCastle(game: Game, move: Move) {
        let piece: Piece;
        if (move.startX === 4 && move.startY === 0) {
            if (move.endX === 6 && move.endY === 0) {
                piece = this.getPieceForXY(game, 7, 0);
                this.movePiece(game, piece, piece.x, piece.y, 5, piece.y, true);
            } else if (move.endX === 2 && move.endY === 0) {
                piece = this.getPieceForXY(game, 0, 0);
                this.movePiece(game, piece, piece.x, piece.y, 3, piece.y, true);
            }
        } else if (move.startX === 4 && move.startY === 7) {
            if (move.endX === 6 && move.endY === 7) {
                piece = this.getPieceForXY(game, 7, 7);
                this.movePiece(game, piece, piece.x, piece.y, 5, piece.y, true);
            } else if (move.endX === 2 && move.endY === 7) {
                piece = this.getPieceForXY(game, 0, 7);
                this.movePiece(game, piece, piece.x, piece.y, 3, piece.y, true);
            }
        }
    }

    static getPossibleMovesforAllPieces(game: Game) {
        const map = this.getPiecesMap(game);
        map.forEach( (piece) => {
            this.getPossibleMovesForPiece(game, piece);
        });
    }


    private static getPossibleMovesForPiece(game: Game, piece: Piece) {
        if (!piece || !piece.discriminator || !game || !piece.ownedBy) {
            return null;
        }
        if (!piece.possibleMoves) {
            piece.possibleMoves = new Map<string, string>();
        }
        piece.possibleMoves.clear();
        switch (piece.discriminator) {
            case 'Pawn':
                return this.getPossibleMovesForPawn(game, piece);
            case 'Knight':
                return this.getPossibleMovesForKnight(game, piece);
            case 'Rook':
                return this.getPossibleMovesForRook(game, piece);
            case 'Bishop':
                return this.getPossibleMovesForBishop(game, piece);
            case 'Queen':
                return this.getPossibleMovesForQueen(game, piece);
            case 'King':
                return this.getPossibleMovesForKing(game, piece);
            default:
                return;
        }
    }

    private static getPossibleMovesForPawn(game: Game, piece: Piece) {
        // white side pawn
        if (piece.ownedBy.id === game.whiteUser.id) {
            if (piece.y === 1) {
                this.tryMove(game, piece, piece.x, piece.y + 2, false);
            }
            if (piece.y < 7) {
                this.tryMove(game, piece, piece.x, piece.y + 1, false);
                if (piece.x < 7) {
                    this.tryMove(game, piece, piece.x + 1, piece.y + 1, true);
                }
                if (piece.x > 0) {
                    this.tryMove(game, piece, piece.x - 1, piece.y + 1, true);
                }
            }
        } else { // black side pawn
            if (piece.y === 6) {
                this.tryMove(game, piece, piece.x, piece.y - 2, false);
            }
            if (piece.y > 0) {
                this.tryMove(game, piece, piece.x, piece.y - 1, false);
                if (piece.x < 7) {
                    this.tryMove(game, piece, piece.x + 1, piece.y - 1, true);
                }
                if (piece.x > 0) {
                    this.tryMove(game, piece, piece.x - 1, piece.y - 1, true);
                }
            }
        }
        return piece.possibleMoves;
    }

    private static getPossibleMovesForRook(game: Game, piece: Piece) {
        let x = piece.x;
        let y = piece.y;
        let keepGoing = true;
        // x axis right
        while (x < 7 && keepGoing) {
            x++;
            keepGoing = this.tryMove(game, piece, x, y);
        }

        // x axis left
        x = piece.x;
        keepGoing = true;
        while (x > 0 && keepGoing) {
            x--;
            keepGoing = this.tryMove(game, piece, x, y);
        }

        // y axis up
        x = piece.x;
        keepGoing = true;
        while (y < 7 && keepGoing) {
            y++;
            keepGoing = this.tryMove(game, piece, x, y);
        }

        // y axis down
        y = piece.y;
        keepGoing = true;
        while (y > 0 && keepGoing) {
            y--;
            keepGoing = this.tryMove(game, piece, x, y);
        }
        return piece.possibleMoves;
    }

    private static getPossibleMovesForKnight(game: Game, piece: Piece) {
        // All possible offset combinations of a knight move
        const xOptions = [2, 1, -1, -2, -2, -1, 1, 2];
        const yOptions = [1, 2, 2, 1, -1, -2, -2, -1];

        // Check if each possible move is valid or not
        for (let i = 0; i < 8; i++) {
            // if we don't go off the board
            if (piece.x + xOptions[i] >= 0 && piece.x + xOptions[i] <= 7
                && piece.y + yOptions[i] >= 0 && piece.y + yOptions[i] <= 7) {
                this.tryMove(game, piece, piece.x + xOptions[i], piece.y + yOptions[i]);
            }
        }
        return piece.possibleMoves;
    }

    private static getPossibleMovesForBishop(game: Game, piece: Piece) {
        let x = piece.x;
        let y = piece.y;
        let keepGoing = true;
        // up and right
        while (x < 7 && y < 7 && keepGoing) {
            x++;
            y++;
            keepGoing = this.tryMove(game, piece, x, y);
        }

        // up and left
        x = piece.x;
        y = piece.y;
        keepGoing = true;
        while (x > 0 && y < 7 && keepGoing) {
            x--;
            y++;
            keepGoing = this.tryMove(game, piece, x, y);
        }

        // down and right
        x = piece.x;
        y = piece.y;
        keepGoing = true;
        while (x < 7 && y > 0 && keepGoing) {
            y--;
            x++;
            keepGoing = this.tryMove(game, piece, x, y);
        }

        // down and left
        x = piece.x;
        y = piece.y;
        keepGoing = true;
        while (x > 0 && y > 0 && keepGoing) {
            y--;
            x--;
            keepGoing = this.tryMove(game, piece, x, y);
        }
        return piece.possibleMoves;
    }

    private static getPossibleMovesForQueen(game: Game, piece: Piece) {
        // diagonals
        this.getPossibleMovesForBishop(game, piece);
        // lines
        this.getPossibleMovesForRook(game, piece);
        return piece.possibleMoves;
    }

    private static getPossibleMovesForKing(game: Game, piece: Piece) {
        const canMoveRight = piece.x > 0;
        const canMoveLeft = piece.x < 7;
        const canMoveUp = piece.y < 7;
        const canMoveDown = piece.y > 0;

        if (canMoveRight) {
            this.tryMove(game, piece, piece.x + 1, piece.y);
            if (canMoveUp) {
                this.tryMove(game, piece, piece.x + 1, piece.y + 1);
            }
            if (canMoveDown) {
                this.tryMove(game, piece, piece.x + 1, piece.y - 1);
            }
        }
        if (canMoveLeft) {
            this.tryMove(game, piece, piece.x - 1, piece.y);
            if (canMoveUp) {
                this.tryMove(game, piece, piece.x - 1, piece.y + 1);
            }
            if (canMoveDown) {
                this.tryMove(game, piece, piece.x - 1, piece.y - 1);
            }
        }
        if (canMoveUp) {
            this.tryMove(game, piece, piece.x, piece.y + 1);
        }
        if (canMoveDown) {
            this.tryMove(game, piece, piece.x, piece.y - 1);
        }
        if (piece.ownedBy.id === game.whiteUser.id) {
            if (game.canWhiteKingSideCastle) {
                this.tryMove(game, piece, 6, 0);
            }
            if (game.canWhiteQueenSideCastle) {
                this.tryMove(game, piece, 2, 0);
            }
        } else {
            if (game.canBlackKingSideCastle) {
                this.tryMove(game, piece, 6, 7);
            }
            if (game.canBlackQueenSideCastle) {
                this.tryMove(game, piece, 2, 7);
            }
        }
        return piece.possibleMoves;
    }

    private static tryMove(game: Game, piece: Piece, x: number, y: number, isPawnCapture = false) {
        const pieceAtNewLocation = this.getPieceForXY(game, x, y);
        if (pieceAtNewLocation && pieceAtNewLocation.ownedBy) {
            // if piece is not ours we can move there but no further
            if (pieceAtNewLocation.ownedBy.id !== piece.ownedBy.id) {
                // piece threatened by new move, not really needed for check but might be nice ui feature
                if (pieceAtNewLocation.discriminator === 'King') {
                    // the owner of pieceAtNewLocation is in check
                    if (pieceAtNewLocation.ownedBy.id === game.whiteUser.id) {
                        game.statusCode = GameStatus.CheckWhite;
                    } else {
                        game.statusCode = GameStatus.CheckBlack;
                    }
                }
                // if attempt to automate this would be where one could store the next tree level possible moves
                piece.possibleMoves.set(Piece.getMapKey(x, y), pieceAtNewLocation.discriminator);
            }
            return false;
        }
        if (isPawnCapture) {
            // pawns must be able to capture to make a capture move
            return false;
        }
        piece.possibleMoves.set(Piece.getMapKey(x, y), null);
        return true ;
    }


    static isLegalMove(game: Game, move: Move) {
        if (move) {
            const piece = this.getPieceForXY(game, move.startX, move.startY);
            if (!piece) {
                return 'No piece at starting location';
            }
            const capturing = this.getPieceForXY(game, move.startX, move.startY)
                                        .possibleMoves
                                        .get(Piece.getMapKey(move.endX, move.endY));
            move.isCapture = (!capturing);

            const canMove = this.getPieceForXY(game, move.startX, move.startY)
                                        .possibleMoves
                                        .has(Piece.getMapKey(move.endX, move.endY));
            if (canMove) {
                const causesCheck = this.doesMoveCauseCheck(game, move.startX, move.startY, move.endX, move.endY);
                if (causesCheck) {
                    return 'A player may not make any move that places or leaves their king in check.';
                }

                if (move.pieceDiscriminator === 'King') {
                    return this.isCastleLegal(game, move);
                // gonna need clause here for en passant
                }
                return canMove;
            }
            switch (move.pieceDiscriminator) {
                case 'Pawn':
                    return 'A Pawn may only move forward one space at a time, ' +
                    'capture diagonally, and may move two spaces forward if it is the first move of the pawn.';

                case 'Rook':
                    return 'A Rook must move in straight lines along the x or y axis, and cannot jump over other pieces.';

                case 'Knight':
                    return 'A Knight must move two spaces on one axis and one space on the other axis.';

                case 'Bishop':
                    return 'A Bishop must move in a diagonal line, and cannot jump over other pieces.';

                case 'Queen':
                    return 'A Queen must move in a in straight lines along the x or y axis, or in a diagonal line,' +
                            ' and cannot jump over other pieces.';

                case 'King':
                    return 'The King can only move one space in any direction.';
            }
        }
    }

    private static isCastleLegal(game: Game, move: Move) {
        move.isCastle = false;
        if ((game.statusCode === GameStatus.CheckBlack && !move.isWhite) ||
            (game.statusCode === GameStatus.CheckWhite && move.isWhite) ) {
            return 'Cannot Castle while in check';
        }
        if (move.isWhite && move.startX === 4 && move.startY === 0) {
            if (move.endX === 6 && move.endY === 0) {
                const canKingMove = this.getPieceForXY(game, 4, 0).possibleMoves.has(Piece.getMapKey(6, 0));
                const canRookMove = this.getPieceForXY(game, 7, 0).possibleMoves.has(Piece.getMapKey(5, 0));
                const isKingMovingThroughCheck = this.doesMoveCauseCheck(game, 4, 0, 5, 0);
                move.isCastle = game.canWhiteKingSideCastle && canKingMove && canRookMove && isKingMovingThroughCheck;
            } else if (move.endX === 2 && move.endY === 0) {
                const canKingMove = this.getPieceForXY(game, 4, 0).possibleMoves.has(Piece.getMapKey(2, 0));
                const canRookMove = this.getPieceForXY(game, 0, 0).possibleMoves.has(Piece.getMapKey(3, 0));
                const isKingMovingThroughCheck = this.doesMoveCauseCheck(game, 4, 0, 3, 0);
                move.isCastle = game.canWhiteQueenSideCastle && canKingMove && canRookMove && isKingMovingThroughCheck;
            }
        } else if (!move.isWhite && move.startX === 4 && move.startY === 7) {
            if (move.endX === 6 && move.endY === 7) {
                const canKingMove = this.getPieceForXY(game, 4, 7).possibleMoves.has(Piece.getMapKey(6, 7));
                const canRookMove = this.getPieceForXY(game, 7, 7).possibleMoves.has(Piece.getMapKey(5, 7));
                const isKingMovingThroughCheck = this.doesMoveCauseCheck(game, 4, 7, 5, 7);
                move.isCastle = game.canBlackKingSideCastle && canKingMove && canRookMove && isKingMovingThroughCheck;
            } else if (move.endX === 2 && move.endY === 7) {
                const canKingMove = this.getPieceForXY(game, 4, 7).possibleMoves.has(Piece.getMapKey(2, 7));
                const canRookMove = this.getPieceForXY(game, 0, 7).possibleMoves.has(Piece.getMapKey(3, 7));
                const isKingMovingThroughCheck = this.doesMoveCauseCheck(game, 4, 7, 3, 7);
                move.isCastle = game.canBlackQueenSideCastle && canKingMove && canRookMove && isKingMovingThroughCheck;
            }
        }
        return move.isCastle;
    }
}


