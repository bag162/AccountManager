import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from './controlpanel.component';
import { ProxyComponent } from './ProxyComponent/proxy.component'
import { SMSServiceComponent } from './SMSServiceComponent/smsservice.component'
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { InstAccountComponent } from './InstAccountComponent/Instaccounts.component'
import { TaskManagerModule } from './TaskManagerModule/taskmanager.module'
import { AddGroupProxyComponent } from './ProxyComponent/GroupProxyComponent/addgroupproxy.component'
import { EmailComponent } from './EmailComponent/email.component'
import { AddGroupInstComponent } from './InstAccountComponent/GroupInstAccComponent/groupInstaccount.component';
import { PostModule } from './PostModule/post.module'
import { DashboardController } from './DashboardController/dashboard.controller'
import { GuardService } from '../Services/GuardService'
import { ProfileFillingModule } from './ProfileFillingModule/profilefilling.module'

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    { path: 'dashboard', component: DashboardController, canActivate: [GuardService.CanAccess] },
                    { path: 'proxy', component: ProxyComponent, canActivate: [GuardService.CanAccess] },
                    { path: 'smsservice', component: SMSServiceComponent, canActivate: [GuardService.CanAccess] },
                    { path: 'Instaccount', component: InstAccountComponent, canActivate: [GuardService.CanAccess] },
                    { path: 'taskmanager', component: TaskManagerModule, canActivate: [GuardService.CanAccess] },
                    { path: 'postgroup', component: PostModule, canActivate: [GuardService.CanAccess] },
                    { path: 'email', component: EmailComponent, canActivate: [GuardService.CanAccess] },
                    { path: 'profilefilling', component: ProfileFillingModule }
                ]
            },
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule,
        TaskManagerModule,
        PostModule,
        ProfileFillingModule
    ],
    exports: [RouterModule],
    declarations: [
        ControlPanelComponent,
        ProxyComponent,
        EmailComponent,
        SMSServiceComponent,
        InstAccountComponent,
        AddGroupProxyComponent,
        AddGroupInstComponent
    ],
    providers: [],
    bootstrap: [ControlPanelComponent]
})

export class ControlPanelModule { }
