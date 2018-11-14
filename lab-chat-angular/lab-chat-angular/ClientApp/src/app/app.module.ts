import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { Login } from './login/login';
import { Logout } from './login/logout';
import { StorageServiceModule } from 'angular-webstorage-service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    Login,
    Logout
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    StorageServiceModule,
    RouterModule.forRoot([
      { path: '', component: Login },
      { path: 'home', component: HomeComponent},
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'login', component: Login },
      { path: 'logout', component: Logout }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
