import { NgModule } from '@angular/core';
import { TaskManagerComponent } from './taskmanager.component';
import { RegistrationTaskComponent } from './RegistrationTaskComponent/registrationtask.component';
import { RouterModule } from '@angular/router';
import { TaskDataComponent } from './TaskDataComponent/TaskData.component'
@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'taskmanager', component: TaskManagerComponent, children: [
                    { path: "taskdata", component: TaskDataComponent },
                    { path: "registration", component: RegistrationTaskComponent }
                ]
            }
        ]),],
    exports: [RouterModule],
    declarations: [
        TaskManagerComponent,
        RegistrationTaskComponent
    ],
    providers: [],
    bootstrap: [TaskManagerComponent]
})

export class TaskManagerModule { }
