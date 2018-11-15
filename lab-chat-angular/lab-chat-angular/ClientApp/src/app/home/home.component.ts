import { Component, Inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';
import Mensaje from '../clases/Mensaje';
import swal from 'sweetalert2'
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
   templateUrl: './home.component.html'
})

@Injectable()
export class HomeComponent {
  private hubConnection: HubConnection;
  nick = '';
  nombre = '';
  message = '';
  messages: Mensaje[] = [];
  mensajeParse = '';
  mio: boolean;
  private token: string;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService) {

  }


  ngOnInit() {
    
    this.token = this.storage.get('token');
    this.nick = this.storage.get('nick');
    this.nombre = this.storage.get('user');
    console.log('Token: ' + this.token);
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chat', { accessTokenFactory: () => { return this.token } })
      .build();



    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string, fecha: string) => {
      this.mio = false;

      this.mensajeParse = '';

      if (nick == this.nick) {
        nick = "yo";
        this.mio = true;
      }

      var men = new Mensaje(nick, receivedMessage, fecha, this.mio);

      this.messages.push(men);
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
        this.hubConnection.invoke('sendToAll', this.nick, this.nombre + " ha entrado a el chat.")
      })
      .catch(err => {
        console.log('Error while establishing connection...');
      });
    
  }

  public sendMessage(): void {
    this.hubConnection
      .invoke('sendToAll', this.nick, this.message)
      .catch(
       
      function () {
          swal({
            type: 'error',
            title: 'Acceso no autorizado',
            text: 'Inicia sesion para poder chatear',
            footer: "<a href='/login'>Iniciar sesion</a>"
          });
      });
  }
}
