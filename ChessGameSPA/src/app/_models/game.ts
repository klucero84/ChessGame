import { User } from './user';
import { Piece } from './piece';
import { Move } from './move';

export interface Game {
    id: number;
    users?: User[];
    pieces?: Piece[];
    moves?: Move[];
    dateCreated: Date;
    dateCompleted?: Date;
}
