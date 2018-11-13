import { Component, Inject, Injectable } from '@angular/core';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

@Injectable()
export class AppComponent {
  txtToken = '';
  txtUsuario = '';
  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService) {
    
  }


  ngOnInit() {

    if (this.storage.get('token') != null) {
      this.txtToken = "OK";
    } else {
      this.txtToken = "NOK";
    }

    if (this.storage.get('user') != null) {
      this.txtUsuario = this.storage.get('user');
    } else {
      this.txtUsuario = "NN";
    }
  }

 
}
