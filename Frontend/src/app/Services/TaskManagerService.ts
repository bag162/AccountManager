import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { AddRegistrationTaskDTO } from '../ControlPanel/TaskManagerModule/RegistrationTaskComponent/registrationtask.component'
import { environment } from 'src/environments/environment';
import { AuthorizationTaskDTO } from '../ControlPanel/TaskManagerModule/AuthorizationTaskComponent/authorizationtask.component';
import { AddPostingTaskDTO } from '../ControlPanel/TaskManagerModule/PostingTaskComponent/postingtask.component';

@Injectable({ providedIn: 'root' })
export class TaskManagerService {
    address: string = environment.apiUrl + "/api/taskmanager/";
    httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
    }

    public async AddRegistrationTask(data: AddRegistrationTaskDTO) {
        return this.httpClient.post(this.address + "registrationtask", data);
    }

    public async AddAuthorizationTask(data: AuthorizationTaskDTO)
    {
        return this.httpClient.post(this.address + "AuthorizationTask", data);
    }

    public async AddPostingTask(data: AddPostingTaskDTO)
    {
        return this.httpClient.post(this.address + "PostingTask", data);
    }
}