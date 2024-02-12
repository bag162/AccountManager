import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from '../controlpanel.component';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CloneComponent } from './clone.component';
import { ClonGroupComponent } from './ClonGroupComponent/clongroup.component'
import { ClonManagerComponent } from './ClonManagerComponent/clonmanager.component'
import { ViewClonComponent } from './ClonManagerComponent/ViewClonComponent/viewclon.component'
import { ProfileFillingModule } from '../ProfileFillingModule/profilefilling.module';
import { PostModule } from '../PostModule/post.module';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'clone', component: CloneComponent, children: [
                            { path: 'group', component: ClonGroupComponent },
                            { path: 'manager', component: ClonManagerComponent },
                            { path: 'manager/:groupId', component: ClonManagerComponent },
                            { path: 'manager/view/:clonId', component: ViewClonComponent }
                        ]
                    }
                ]
            }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule,
        ProfileFillingModule,
        PostModule
    ],
    exports: [],
    declarations: [
        CloneComponent,
        ClonGroupComponent,
        ClonManagerComponent,
        ViewClonComponent
    ],
    providers: [],
})
export class CloneModule { }