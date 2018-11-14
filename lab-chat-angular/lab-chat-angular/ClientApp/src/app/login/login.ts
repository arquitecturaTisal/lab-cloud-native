import { Component, OnInit, Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';
import Mensaje from '../clases/Mensaje';
import swal from 'sweetalert2'
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.html'
})

@Injectable()
export class Login implements OnInit {
  private hubConnection: HubConnection;
  txtDominio = 'TISAL';
  txtUsuario = '';
  txtClave = '';

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService) {

  }

  ngOnInit() {
   
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chat')
      .build();

    this.hubConnection.on('loginResult', (estado: number, receivedMessage: string, nick : string, token : string) => {
     
      if (estado == 1) {
        this.storage.set('token', token);
        this.storage.set('user', receivedMessage);
        this.storage.set('nick', nick);
        let timerInterval
        swal({
          title: '¡Bienvenido ' + receivedMessage + '!',
          html: 'Ingresando en <strong></strong> segundos.',
          timer: 5000,
          onOpen: () => {
            swal.showLoading()
            timerInterval = setInterval(() => {
              swal.getContent().querySelector('strong')
                .textContent = Math.round((swal.getTimerLeft() / 1000)).toString()
            }, 300)
          },
          onClose: () => {
            clearInterval(timerInterval)
          }
        }).then((result) => {
        
            location.href = '/home';
        })
      } else {

        swal({
          title: '¡Oops!',
          text: 'Las credenciales son incorrectas',
          type: 'error',
          confirmButtonText: 'Lo intentaré de nuevo...'
        })

      }
     
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
