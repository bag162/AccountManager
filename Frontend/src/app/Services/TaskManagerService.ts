import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { AddRegistrationTaskDTO } from '../ControlPanel/TaskManagerModule/RegistrationTaskComponent/registrationtask.component'
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class TaskManagerService {
    address: string = environment.apiUrl + "taskmanager/";
    httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
    }

    public async AddRegistrationTask(data: AddRegistrationTaskDTO) {
        return this.httpClient.post(this.address + "registrationtask", data);
    }
}