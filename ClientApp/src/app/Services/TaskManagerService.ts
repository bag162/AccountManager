import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { AddRegistrationTaskDTO } from '../ControlPanel/TaskManagerModule/RegistrationTaskComponent/registrationtask.component'
import { environment } from 'src/environments/environment';
import { AuthorizationTaskDTO } from '../ControlPanel/TaskManagerModule/AuthorizationTaskComponent/authorizationtask.component';
import { AddPostingTaskDTO } from '../ControlPanel/TaskManagerModule/PostingTaskComponent/postingtask.component';
import { AddCommentingTaskDTO } from '../ControlPanel/TaskManagerModule/CommentingTaskComponent/commentingtask.component';
import { AddLikingTaskDTO } from '../ControlPanel/TaskManagerModule/LikingTaskComponent/likingtask.component';
import { AddFollowingTaskDTO } from '../ControlPanel/TaskManagerModule/FolowingTaskComponent/followingtask.component';
import { AddFillingProfileTaskDTO } from '../ControlPanel/TaskManagerModule/ProfileFillingTaskComponent/profilefillingtask.component';

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

    public async AddCommentingTask(data: AddCommentingTaskDTO)
    {
        return this.httpClient.post(this.address + "CommentingTask", data);
    }

    public async AddLikingTask(data: AddLikingTaskDTO)
    {
        return this.httpClient.post(this.address + "LikingTask", data);
    }

    public async AddFollowingTask(data: AddFollowingTaskDTO)
    {
        return this.httpClient.post(this.address + "FollowingTask", data);
    }

    public async AddFillingProfileTask(data: AddFillingProfileTaskDTO)
    {
        return this.httpClient.post(this.address + "FillingProfileTask", data);
    }
}