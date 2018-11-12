import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';
import Mensaje from '../clases/Mensaje';

@Component({
  selector: 'app-login',
  templateUrl: './login.html'
})

export class Login implements OnInit {
  private hubConnection: HubConnection;
  txtDominio = '';
  txtUsuario = '';
  txtClave = '';

  ngOnInit() {
   
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chat')
      .build();

    this.hubConnection.on('loginResult', (estado: number, receivedMessage: string) => {
      alert(estado + ' ' + receivedMessage);
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

  public login(): void {
    this.hubConnection
      .invoke('Login', this.txtDominio, this.txtUsuario, this.txtClave)
      .catch(err => console.error(err));
  }
}
