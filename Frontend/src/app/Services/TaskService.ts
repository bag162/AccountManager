import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { TaskDTO } from '../ControlPanel/TaskManagerModule/TaskDataComponent/TaskData.component';

@Injectable({ providedIn: 'root' })
export class TaskService {
    private static address: string = environment.apiUrl + "task/";
    private static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        TaskService.httpClient = httpClient;
    }

    public async AddTask(Task: TaskDTO[]) {
        return TaskService.httpClient.post(TaskService.address + "post", Task);
    }

    public static async DeleteTask(Task: TaskDTO) {
        return TaskService.httpClient.delete(TaskService.address + "delete", { body: Task });
    }

    public static async StopTask(Task: TaskDTO) {
        return TaskService.httpClient.put(TaskService.address + "stop", Task);
    }

    public static async StartTask(Task: TaskDTO) {
        return TaskService.httpClient.put(TaskService.address + "start", Task);
    }
}