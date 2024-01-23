import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { ControlPanelModule } from './ControlPanel/controlpanel.module'
import { DataTablesModule } from 'angular-datatables';
import { TaskManagerModule } from './ControlPanel/TaskManagerModule/taskmanager.module';
import { LoginComponent } from './Authorization/Login/login.component'
import { RegistrationComponent } from './Authorization/Registration/registration.component'
import { AuthorizationComponent } from './Authorization/authorization.component'
import { GuardService } from './Services/GuardService';
import { NoAccessComponent } from './NoAccess/noaccess.component'

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistrationComponent,
    AuthorizationComponent,
    NoAccessComponent
  ],
  imports: [
    RouterModule.forRoot([
      { path: 'noaccess', component: NoAccessComponent },
      { path: 'auth', component: AuthorizationComponent, canActivate: [GuardService.CanAccessAuthComponent], children: [
          { path: 'login', component: LoginComponent },
          { path: 'registration', component: RegistrationComponent }
        ]},
      { path: '', component: ControlPanelModule }

    ]),
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ControlPanelModule,
    TaskManagerModule,
    DataTablesModule
  ],
  providers: [

  ],
  bootstrap: [AppComponent]
})

export class AppModule { }