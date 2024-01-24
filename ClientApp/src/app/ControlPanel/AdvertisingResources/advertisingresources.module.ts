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

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'advertresourses', component: AdvertisingResoursesComponent, children: [
                            { path: 'account/group', component: AdvertAccountGroupComponent },
                            { path: 'account/manager', component: AdvertAccountComponent },
                            { path: 'account/manager/:groupId', component: AdvertAccountComponent },
                            { path: 'post/group', component: AdvertPostGroupComponent },
                            { path: 'post/manager', component: AdvertPostComponent },
                            { path: 'post/manager/:groupId', component: AdvertPostComponent },
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