import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from './ControlPanelComponent/controlpanel.conponent';
import { ProxyComponent } from './ProxyComponent/proxy.component'
import { SMSServiceComponent } from './SMSServiceComponent/smsservice.component'
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { FbAccountComponent } from './FBAccpuntsComponent/fbaccounts.component'
import { TaskManagerModule } from './TaskManagerModule/taskmanager.module'
import { AddGroupProxyComponent } from './ProxyComponent/GroupProxyComponent/addgroupproxy.component'
import { AddGroupFBACCComponent } from './FBAccpuntsComponent/GroupFbAccComponent/groupfbaccount.component'
@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', component: ControlPanelComponent },
            { path: 'proxy', component: ProxyComponent },
            { path: 'smsservice', component: SMSServiceComponent },
            { path: 'fbaccount', component: FbAccountComponent },
            { path: 'taskmanager', component: TaskManagerModule }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule,
        TaskManagerModule
    ],
    exports: [RouterModule],
    declarations: [
        ControlPanelComponent,
        ProxyComponent,
        SMSServiceComponent,
        FbAccountComponent,
        AddGroupProxyComponent,
        AddGroupFBACCComponent
    ],
    providers: [],
    bootstrap: [ControlPanelComponent]
})

export class ControlPanelModule { }
