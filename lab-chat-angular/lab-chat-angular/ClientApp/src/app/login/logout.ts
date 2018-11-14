import { Component, Inject, Injectable } from '@angular/core';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  template: ''
})

@Injectable()
export class Logout {

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService) {
    
  }

  ngOnInit() {
    this.storage.set('user', null);
    this.storage.set('token', null);

    location.href = '/login'
  } 
}
