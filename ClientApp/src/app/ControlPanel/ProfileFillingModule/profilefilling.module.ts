import { NgModule } from '@angular/core';

import { ProfileFillingComponent } from './profilefilling.component';
import { OverfiewProfileFillingComponent } from './OverviewProfileComponent/overfiewprofile.component'
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from '../controlpanel.component';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { GuardService } from 'src/app/Services/GuardService';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    { path: 'profilefilling', component: ProfileFillingComponent, canActivate: [GuardService.CanAccess]  },
                    { path: "profilefilling/add", component: OverfiewProfileFillingComponent, canActivate: [GuardService.CanAccess]  },
                    { path: "profilefilling/view/:fillingDataId", component: OverfiewProfileFillingComponent, canActivate: [GuardService.CanAccess]  }
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
