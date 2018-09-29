import { Game } from './game';
import { User } from './user';
import { Piece } from './piece';

export class Move {
    id: number;
    game: Game;
    gameId: number;
    piece: Piece;
    pieceId: number;
    user: User;
    userId: number;
    startX: number;
    startY: number;
    endX: number;
    endY: number;
    pieceDiscriminator: string;
    isWhite: boolean;
    isCastle: boolean;
    isCapture: boolean;
    concede: boolean;
    offerDraw: boolean;
    connId: string;


    isLegalMove(isCapturing: boolean) {
        const diffX = Math.abs(this.startX - this.endX);
        const diffY = Math.abs(this.startY - this.endY);

        switch (this.pieceDiscriminator) {
            case 'Pawn':
            if (diffX > 1) { return 'A Pawn cannot move more than one square to the side.'; }
            if (this.isWhite) {
                // white pawns start at row index 1 and move up one at a time
                if (this.startY + 1 === this.endY) {
                    if (diffX === 0) {
                        return true;
                    }  else if (isCapturing && diffX === 1) {
                        // pawns may move one space to the side when capturing
                        return true;
                    }
                } else if (this.startY + 2 === this.endY && this.startY === 1 && diffX === 0) {
                    // if they are at their starting position they may move two spaces
                    return true;
                }
            } else {
                // black pawns start at row index 6 and only move down
                if (this.startY - 1 === this.endY ) {
                    if (diffX === 0) {
                        return true;
                    } else if (isCapturing && diffX === 1) {
                        // pawns may move one pace to either side when capturing
                        return true;
                    }
                } else if (this.startY - 2 === this.endY && this.startY === 6 && diffX === 0) {
                    // if they are at their starting position they may move two spaces
                    return true;
                }
            }
            return 'A Pawn may only move forward one space at a time, ' +
            'capture diagonally, and may move two spaces forward if it is the first move of the pawn.';

            case 'Rook':
            if (diffX === 0 || diffY === 0) {
                return true;
            }
            return 'A Rook must move in straight lines along the x or y axis';

            case 'Knight':
            if (diffX === 1 && diffY === 2) {
                return true;
            } else if (diffX === 2 && diffY === 1) {
                return true;
            }
            return 'A Knight must move two spaces on one axis and one space on the other axis.';

            case 'Bishop':
            if (diffX === diffY) {
                return true;
            }
            return 'A Bishop must move in a diagonal line';

            case 'Queen':
            if (diffX === 0 || diffY === 0) {
                return true;
            } else if (diffX === diffY) {
                return true;
            }
            return 'A Queen must move in a in straight lines along the x or y axis, or in a diagonal line.';

            case 'King':
            // const canCastle = ;
            // console.log(canCastle);
            if (this.isCastleLegal() && diffY === 0 && diffX === 2) {
                return true;
            }
            if (diffX > 1 || diffY > 1) {
                return 'The King can only move one space in any direction.';
            }
            return true;
        }
    }

    isCastleLegal(): boolean {
        if (this.isWhite && this.startX === 4 && this.startY === 0) {
            if (this.endX === 6 && this.endY === 0) {
                this.isCastle = this.game.canWhiteKingSideCastle;
            } else if (this.endX === 2 && this.endY === 0) {
                this.isCastle = this.game.canWhiteQueenSideCastle;
            }
        } else if (!this.isWhite && this.startX === 4 && this.startY === 7) {
            if (this.endX === 6 && this.endY === 7) {
                this.isCastle = this.game.canBlackKingSideCastle;
            } else if (this.endX === 2 && this.endY === 7) {
                this.isCastle = this.game.canBlackQueenSideCastle;
            }
        }
        return this.isCastle;
        // let moves;
        // if (this.isWhite) {
        //     if (this.startX - this.endX === 2) {
        //         moves = this.game.moves.filter(move => (move.startX === 4 && move.startY === 0) ||
        //                                                 move.startX === 7 && move.startY === 0 );
        //     } else if (this.startX - this.endX === -2) {
        //         moves = this.game.moves.filter(move => (move.startX === 4 && move.startY === 0) ||
        //                                                 move.startX === 7 && move.startY === 0 );
        //     }
        // } else {
        //     if (this.startX - this.endX === 2) {
        //         moves = this.game.moves.filter(move => (move.startX === 4 && move.startY === 7) ||
        //                                                 move.startX === 7 && move.startY === 7 );
        //     } else if (this.startX - this.endX === -2) {
        //         moves = this.game.moves.filter(move => (move.startX === 4 && move.startY === 7) ||
        //                                                 move.startX === 7 && move.startY === 7 );
        //     }
        // }
        // console.log(moves);
        // return moves.length === 0;
    }
}
