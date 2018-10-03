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

    static getXYFromKey(mapKey: string) {
        if (mapKey) {
            const strArray = mapKey.split(Piece.delimiter);
            if (strArray && strArray.length === 2) {
                const xInt = Number.parseInt(strArray[0]);
                const yInt = Number.parseInt(strArray[1]);
                return {x: xInt, y: yInt};
            }
        }
    }
}
