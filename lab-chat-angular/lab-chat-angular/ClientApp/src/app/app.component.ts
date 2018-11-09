import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';
import  Mensaje from './clases/Mensaje';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  private hubConnection: HubConnection;
  nick = '';
  message = '';
  messages: Mensaje[] = [];
  mensajeParse = '';
  mio : boolean;

  ngOnInit() {
    this.nick = window.prompt('Ingresa tu Nick:', '');

    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chat')
      .build();
       
    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string,fecha : string) => {
      this.mio = false;

      this.mensajeParse = '';

      if (nick == this.nick) {
        nick = "yo";
        this.mio = true;
      }

      var men = new Mensaje(nick, receivedMessage, fecha,this.mio);
      
      this.messages.push(men);
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
