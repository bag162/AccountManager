import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from '../controlpanel.component';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CloneComponent } from './clone.component';
import { ClonGroupComponent } from './ClonGroupComponent/clongroup.component'

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'clone', component: CloneComponent, children: [
                            { path: 'group', component: ClonGroupComponent }
                        ]
                    }
                ]
            }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule
    ],
    exports: [],
    declarations: [
        CloneComponent,
        ClonGroupComponent
    ],
    providers: [],
})
export class CloneModule { }