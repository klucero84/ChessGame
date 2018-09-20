import { Game } from './game';
import { User } from './user';
import { Piece } from './piece';

export interface Move {
    id: number;
    gameId: number;
    game?: Game;
    pieceId: number;
    piece?: Piece;
    userId: number;
    user?: User;
    startX: number;
    startY: number;
    endX: number;
    endY: number;
    discriminator: string;
}
