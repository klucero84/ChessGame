import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  private hubConnection: HubConnection;
  nick = '';
  message = '';
  messages: string[] = [];

  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = this.authService.getAuthToken();
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }

    this.nick = window.prompt('Your name:', 'John');
    this.hubConnection = new HubConnectionBuilder().withUrl('http://localhost:5000/chat').build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string) => {
      const text = `${nick}: ${receivedMessage}`;
      this.messages.push(text);
    });
  }
  public sendMessage(): void {
    this.hubConnection
      .invoke('sendToAll', this.nick, this.message)
      .catch(err => console.error(err));
  }
}
