import { Game } from './game';
import { User } from './user';
import { Piece } from './piece';

export class Move {
    id: number;
    gameId: number;
    pieceId: number;
    piece?: Piece;
    userId: number;
    startX: number;
    startY: number;
    endX: number;
    endY: number;
    discriminator: string;
}
