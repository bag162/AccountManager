import { NgModule } from '@angular/core';
import { TaskManagerComponent } from './taskmanager.component';
import { RegistrationTaskComponent } from './RegistrationTaskComponent/registrationtask.component';
import { RouterModule } from '@angular/router';
import { TaskDataComponent } from './TaskDataComponent/TaskData.component'
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { SelectProxyGroupComponent } from './MComponents/SelectProxyGroupComponent/selectproxygroup.component'
import { SelectSMSServiceComponent } from './MComponents/SelectSMSServiceGroupComponent/selectsmsservice.component'
import { SelectAccountGroupComponent } from './MComponents/SelectAccountGroupComponent/selectaccountgroup.component'
@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'taskmanager', component: TaskManagerComponent, children: [
                    { path: "taskdata", component: TaskDataComponent },
                    { path: "registration", component: RegistrationTaskComponent }
                ]
            }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule
    ],
    exports: [RouterModule],
    declarations: [
        TaskManagerComponent,
        RegistrationTaskComponent,
        TaskDataComponent,
        SelectProxyGroupComponent,
        SelectSMSServiceComponent,
        SelectAccountGroupComponent
    ],
    providers: [],
    bootstrap: [TaskManagerComponent]
})

export class TaskManagerModule { }
