
import { Injectable } from '@angular/core';
import * as socketIo from 'socket.io-client';
import { Observable } from 'rxjs';
import { Move } from '../_models/move';

const SERVER_URL = 'http://localhost:8080';

@Injectable()
export class MoveSocketService {
    private socket;

    public initSocket(): void {
        this.socket = socketIo(SERVER_URL);
    }

    public send(move: Move): void {
        this.socket.emit('move', move);
    }

    public onMove(): Observable<Move> {
        return new Observable<Move>(observer => {
            this.socket.on('move', (data: Move) => observer.next(data));
        });
    }
}

