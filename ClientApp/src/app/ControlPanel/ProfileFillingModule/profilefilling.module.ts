import { NgModule } from '@angular/core';

import { ProfileFillingComponent } from './profilefilling.component';
import { OverfiewProfileFillingComponent } from './OverviewProfileComponent/overfiewprofile.component'
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from '../controlpanel.component';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    { path: 'profilefilling', component: ProfileFillingComponent },
                    { path: "profilefilling/add", component: OverfiewProfileFillingComponent },
                    { path: "profilefilling/view/:fillingDataId", component: OverfiewProfileFillingComponent }
                ]
            }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule
    ],
    exports: [],
    declarations: [
        ProfileFillingComponent,
        OverfiewProfileFillingComponent
    ],
    providers: [],
    bootstrap: [OverfiewProfileFillingComponent]
})
export class ProfileFillingModule { }
