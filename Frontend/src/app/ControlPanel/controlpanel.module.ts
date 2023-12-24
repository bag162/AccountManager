import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ControlPanelComponent } from './ControlPanelComponent/controlpanel.conponent';
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
@NgModule({
    imports: [
        RouterModule.forChild([
            { path: '', component: ControlPanelComponent },
            { path: 'proxy', component: ProxyComponent },
            { path: 'smsservice', component: SMSServiceComponent },
            { path: 'Instaccount', component: InstAccountComponent },
            { path: 'taskmanager', component: TaskManagerModule },
            { path: 'email', component: EmailComponent }

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
