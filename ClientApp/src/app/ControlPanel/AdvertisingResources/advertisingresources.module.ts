import { NgModule } from '@angular/core';
import { AdvertisingResoursesComponent } from './advertisingresourses.component';
import { RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ControlPanelComponent } from '../controlpanel.component';
import { AdvertAccountGroupComponent } from './AdvertAccount/AdvertAccountGroup/advertaccountgroup.component';
import { AdvertAccountComponent } from './AdvertAccount/advertaccount.component';
import { AdvertPostGroupComponent } from './AdvertPost/AdvertPostGroup/advertpostgroup.component';
import { AdvertPostComponent } from './AdvertPost/advertpost.component';
import { GuardService } from 'src/app/Services/GuardService';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'advertresourses', component: AdvertisingResoursesComponent, canActivate: [GuardService.CanAccess],  children: [
                            { path: 'account/group', component: AdvertAccountGroupComponent, canActivate: [GuardService.CanAccess]  },
                            { path: 'account/manager', component: AdvertAccountComponent, canActivate: [GuardService.CanAccess]  },
                            { path: 'account/manager/:groupId', component: AdvertAccountComponent, canActivate: [GuardService.CanAccess]  },
                            { path: 'post/group', component: AdvertPostGroupComponent, canActivate: [GuardService.CanAccess]  },
                            { path: 'post/manager', component: AdvertPostComponent, canActivate: [GuardService.CanAccess]  },
                            { path: 'post/manager/:groupId', component: AdvertPostComponent, canActivate: [GuardService.CanAccess]  },
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
        AdvertisingResoursesComponent,
        AdvertAccountGroupComponent,
        AdvertAccountComponent,
        AdvertPostGroupComponent,
        AdvertPostComponent
    ],
    providers: [],
    bootstrap: [AdvertisingResoursesComponent]
})
export class AdvertisingResourcesModule { }