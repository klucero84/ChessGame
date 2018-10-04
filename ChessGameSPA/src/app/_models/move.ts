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
    notation: string;
}
