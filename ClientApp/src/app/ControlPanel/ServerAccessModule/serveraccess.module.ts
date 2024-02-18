import { NgModule } from '@angular/core';
import { ControlPanelComponent } from '../controlpanel.component';
import { RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ServerAccessComponent } from './serveraccess.component'

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    { path: 'serveraccess', component: ServerAccessComponent }
                ]
            }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule
    ],
    exports: [],
    declarations: [
        ServerAccessComponent
    ],
    providers: [],
})

export class ServerAccessModule { }
