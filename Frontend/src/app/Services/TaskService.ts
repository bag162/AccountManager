import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { TaskDTO } from '../ControlPanel/TaskManagerModule/TaskDataComponent/TaskData.component';

@Injectable({ providedIn: 'root' })
export class TaskService {
    static address: string = environment.apiUrl + "task/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        TaskService.httpClient = httpClient;
    }

    public async AddTask(Task: TaskDTO[]) {
        return TaskService.httpClient.post(TaskService.address + "post", Task);
    }
}