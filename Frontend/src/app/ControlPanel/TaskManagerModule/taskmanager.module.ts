import { NgModule } from '@angular/core';
import { TaskManagerComponent } from './taskmanager.component';
import { RegistrationTaskComponent } from './RegistrationTaskComponent/registrationtask.component';
import { RouterModule } from '@angular/router';
import { TaskDataComponent } from './TaskDataComponent/TaskData.component'
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { WorkerTaskDataComponent } from './TaskDataComponent/WorkerTaskDataComponent/workertaskdata.component'
import { SelectProxyGroupComponent } from './MComponents/SelectProxyGroupComponent/selectproxygroup.component'
import { SelectSMSServiceComponent } from './MComponents/SelectSMSServiceGroupComponent/selectsmsservice.component'
import { SelectAccountGroupComponent } from './MComponents/SelectAccountGroupComponent/selectaccountgroup.component'
import { SelectEmailComponent } from './MComponents/SelectEmailGroupComponent/selectemail.component'
import { AuthorizatioonTaskComponent } from './AuthorizationTaskComponent/authorizationtask.component'
import { PostingTaskComponent } from './PostingTaskComponent/postingtask.component'
import { CommentingTaskComponent } from './CommentingTaskComponent/commentingtask.component'


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'taskmanager', component: TaskManagerComponent, children: [
                    { path: "taskdata", component: TaskDataComponent },
                    { path: "registration", component: RegistrationTaskComponent },
                    { path: "authorization", component: AuthorizatioonTaskComponent },
                    { path: "posting", component: PostingTaskComponent },
                    { path: "commenting", component: CommentingTaskComponent },
                    { path: "workertaskdata/:id", component: WorkerTaskDataComponent }
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
        AuthorizatioonTaskComponent,
        TaskDataComponent,
        SelectProxyGroupComponent,
        SelectSMSServiceComponent,
        SelectAccountGroupComponent,
        SelectEmailComponent,
        WorkerTaskDataComponent,
        PostingTaskComponent,
        CommentingTaskComponent
    ],
    providers: [],
    bootstrap: [TaskManagerComponent]
})

export class TaskManagerModule { }
