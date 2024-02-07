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
import { ProfileFillingTaskComponent } from './ProfileFillingTaskComponent/profilefillingtask.component'
import { AdvertFollowingTaskComponent } from './AdvertFollowingTaskComponent/advertfollowingtask.component'
import { AdvertLikingTaskComponent } from './AdvertLikingTaskComponent/advertlikingtask.component'
import { AdvertCommentingTaskComponent } from './AdvertCommentingTaskComponent//advertcommentingtask.component'
import { TaskSchedulerComponent } from './TaskSchedulerComponent/taskscheduler.component'
import { AddOrViewTaskSchedulerComponent } from './TaskSchedulerComponent/AddOrViewTaskSchedulerComponent/addorviewtaskscheduler.component'
import { ParsingCloningInformationComponent } from './ParsingCloningInformationComponent/parsingcloninginformation.component'

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'taskmanager', component: TaskManagerComponent, canActivate: [GuardService.CanAccess], children: [
                            { path: "taskdata", component: TaskDataComponent, canActivate: [GuardService.CanAccess] },
                            { path: "registration", component: RegistrationTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "authorization", component: AuthorizatioonTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "posting", component: PostingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "commenting", component: CommentingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "liking", component: LikingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "folowing", component: FollowingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "profilefilling", component: ProfileFillingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "advertliking", component: AdvertLikingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "advertfollowing", component: AdvertFollowingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "advertcommenting", component: AdvertCommentingTaskComponent, canActivate: [GuardService.CanAccess] },
                            { path: "workertaskdata/:id", component: WorkerTaskDataComponent, canActivate: [GuardService.CanAccess] },
                            { path: "taskscheduler", component: TaskSchedulerComponent, canActivate: [GuardService.CanAccess] },
                            { path: "taskscheduler/add", component: AddOrViewTaskSchedulerComponent, canActivate: [GuardService.CanAccess] },
                            { path: "taskscheduler/view/:id", component: AddOrViewTaskSchedulerComponent, canActivate: [GuardService.CanAccess] },
                            { path: "parsingcloninginformation", component: ParsingCloningInformationComponent }
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
        FollowingTaskComponent,
        ProfileFillingTaskComponent,
        AdvertFollowingTaskComponent,
        AdvertLikingTaskComponent,
        AdvertCommentingTaskComponent,
        TaskSchedulerComponent,
        AddOrViewTaskSchedulerComponent,
        ParsingCloningInformationComponent
    ],
    providers: [],
    bootstrap: [TaskManagerComponent]
})

export class TaskManagerModule { }
