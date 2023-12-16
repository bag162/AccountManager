import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { ControlPanelModule } from './ControlPanel/controlpanel.module'
import { DataTablesModule } from 'angular-datatables';
import { TaskManagerModule } from './ControlPanel/TaskManagerModule/taskmanager.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    RouterModule.forRoot([
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