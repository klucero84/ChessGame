import { User } from './user';
import { Piece } from './piece';
import { Move } from './move';

export class Game {
    id: number;
    connId: string;
    whiteUser: User;
    blackUser: User;
    pieces?: Piece[];
    moves?: Move[];
    dateCreated: Date;
    dateCompleted?: Date;
    canWhiteKingSideCastle: boolean;
    canWhiteQueenSideCastle: boolean;
    canBlackKingSideCastle: boolean;
    canBlackQueenSideCastle: boolean;

    // addMove(move: Move) {
    //     if (this.canWhiteQueenSideCastle && move.startY === 0 && move.startX === 0) {
    //         this.canWhiteQueenSideCastle = false;
    //     } else if (this.canWhiteKingSideCastle && move.startY === 0 && move.startX === 7) {
    //         this.canWhiteKingSideCastle = false;
    //     } else if ((this.canWhiteKingSideCastle || this.canWhiteQueenSideCastle) &&
    //         move.startY === 0 && move.startX === 4) {
    //         this.canWhiteKingSideCastle = false;
    //         this.canWhiteQueenSideCastle = false;
    //     } else if (this.canBlackQueenSideCastle && move.startY === 0 && move.startX === 0) {
    //         this.canBlackQueenSideCastle = false;
    //     } else if (this.canBlackKingSideCastle && move.startY === 0 && move.startX === 7) {
    //         this.canBlackKingSideCastle = false;
    //     } else if ((this.canBlackKingSideCastle || this.canBlackQueenSideCastle) &&
    //         move.startY === 0 && move.startX === 4) {
    //         this.canBlackKingSideCastle = false;
    //         this.canBlackQueenSideCastle = false;
    //     }

    //     if (move.isCastle) {
    //         this.applyCastle(move);
    //     }
    //     return this.moves.push(move);
    // }
    // getMoves(): Move[] {
    //     return this.moves;
    // }

    // getPiecesForXY(x: number, y: number): Piece[] {
    //     return this.pieces.filter(p => p.x === x && p.y === y);
    // }

    // private applyCastle(move: Move) {
    //     let piece: Piece;
    //     if (move.startX === 4 && move.startY === 0) {
    //         if (move.endX === 6 && move.endY === 0) {
    //             piece = this.getPiecesForXY(7, 0)[0];
    //             piece.x = 5;
    //         } else if (move.endX === 2 && move.endY === 0) {
    //             piece = this.getPiecesForXY(0, 0)[0];
    //             piece.x = 2;
    //         }
    //     } else if (move.startX === 4 && move.startY === 7) {
    //         if (move.endX === 6 && move.endY === 7) {
    //             piece = this.getPiecesForXY(7, 7)[0];
    //             piece.x = 5;
    //         } else if (move.endX === 2 && move.endY === 7) {
    //             piece = this.getPiecesForXY(0, 7)[0];
    //             piece.x = 2;
    //         }
    //     }
    // }
}
