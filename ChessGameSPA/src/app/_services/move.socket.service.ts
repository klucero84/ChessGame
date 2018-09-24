
import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { Observable, observable } from 'rxjs';
import { Move } from '../_models/move';


// "ws://" + window.location.host + "/ws";
const SERVER_URL = 'ws://localhost:5000/ws';

@Injectable()
export class MoveSocketService {
    private socket: WebSocket;
    private moveHub: HubConnection;

    public initSocket(): void {
        this.socket = new WebSocket(SERVER_URL);
        this.socket.onopen = function(event) {
            console.log('opened connection to ' + SERVER_URL);
        };
        this.socket.onclose = function(event) {
            console.log('closed connection from ' + SERVER_URL);
        };
        this.socket.onmessage = function(event) {
            console.log('message: ' + event.data);
        };
        this.socket.onerror = function(event) {
            console.log('error: ' + event);
        };
    }

    public send(move: Move): void {
        this.socket.send(move.toString());
    }

    public onMove(callback) {
        if (typeof callback === 'function') {
            return this.socket.onmessage = callback;
        }
        // return new Observable<Move>(observer => {
        //     if (typeof callback === 'function') {
        //         callback();
        //     }
        // });
    }

    // public onMove(): Observable<Move> {
    //     return new Observable<Move>(observer => {
    //         // this.socket.
    //         // this.socket.on('move', (data: Move) => observer.next(data));
    //     });
    // }
}

