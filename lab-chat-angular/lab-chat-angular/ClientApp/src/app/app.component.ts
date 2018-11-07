import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private hubConnection: HubConnection;
  nick = '';
  message = '';
  messages: string[] = [];

  ngOnInit() {
    this.nick = window.prompt('Tu Nombre:', 'Lucas P');

    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chat')
      .build();
       
    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string) => {
      const text = `${nick}: ${receivedMessage}`;
      this.messages.push(text);
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(err => {
        console.log('Error while establishing connection...');
      });
  }

  public sendMessage(): void {
    this.hubConnection
      .invoke('sendToAll', this.nick, this.message)
      .catch(err => console.error(err));
  }
}
