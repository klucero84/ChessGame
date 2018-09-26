import { User } from './user';
import { Piece } from './piece';
import { Move } from './move';

export interface Game {
    id: number;
    connId: string;
    whiteUser: User;
    blackUser: User;
    pieces?: Piece[];
    moves?: Move[];
    dateCreated: Date;
    dateCompleted?: Date;
}
