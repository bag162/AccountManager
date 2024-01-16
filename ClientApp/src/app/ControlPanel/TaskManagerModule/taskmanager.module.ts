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
import { LikingTaskComponent } from './LikingTaskComponent/likingtask.component'
import { FollowingTaskComponent } from './FolowingTaskComponent/followingtask.component'
import { ControlPanelComponent } from '../controlpanel.component';
import { GuardService } from 'src/app/Services/GuardService';



@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'taskmanager', component: TaskManagerComponent, children: [
                            { path: "taskdata", component: TaskDataComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "registration", component: RegistrationTaskComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "authorization", component: AuthorizatioonTaskComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "posting", component: PostingTaskComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "commenting", component: CommentingTaskComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "liking", component: LikingTaskComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "folowing", component: FollowingTaskComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "workertaskdata/:id", component: WorkerTaskDataComponent, canActivate: [GuardService.CanAccess]  }
                        ]
                    }
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
        CommentingTaskComponent,
        LikingTaskComponent,
        FollowingTaskComponent
    ],
    providers: [],
    bootstrap: [TaskManagerComponent]
})

export class TaskManagerModule { }
