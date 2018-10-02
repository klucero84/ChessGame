import { User } from './user';
import { Game } from './game';

export class Piece {
    static delimiter = '-';

    id: number;
    x: number;
    y: number;
    discriminator: string;
    ownedBy: User;
    game: Game;
    // move coordinates and the enemy piece discriminator it would capture
    possibleMoves?: Map<string, string>;

    static getMapKey(x: number, y: number) {
        return x.toString() + Piece.delimiter + y.toString();
    }
}
