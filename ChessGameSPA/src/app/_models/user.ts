import { Photo } from './photo';

export interface User {
    id: number;
    name: string;
    email: string;
    created: Date;
    lastActive: Date;
    photos?: Photo[];
}

