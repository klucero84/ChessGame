import { User } from './user';
import { Game } from './game';

export interface Piece {
    id: number;
    x: number;
    y: number;
    discriminator: string;
    owner: User;
    game: Game;
}
