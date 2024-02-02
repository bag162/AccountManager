import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from "@angular/common/http";
import { AddSchedulerTask, UpdateSchedulerTaskDTO } from '../ControlPanel/TaskManagerModule/TaskSchedulerComponent/AddOrViewTaskSchedulerComponent/addorviewtaskscheduler.component';

@Injectable({providedIn: 'root'})
export class SchedulerTaskService {
    static address: string = environment.apiUrl + "/api/SchedulerTask/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) { 
        SchedulerTaskService.httpClient = httpClient;
    }
    
    public static async Delete(ids: number[])
    {
        return SchedulerTaskService.httpClient.delete(SchedulerTaskService.address + 'delete', {
            body: ids
        })
    }

    public async Add(data: AddSchedulerTask)
    {
        return SchedulerTaskService.httpClient.post(SchedulerTaskService.address + 'post', data);
    }

    public static async StartSchedulerTask(data: number[])
    {
        return SchedulerTaskService.httpClient.post(SchedulerTaskService.address + "Start", data);
    }

    public static async StopSchedulerTask(data: number[])
    {
        return SchedulerTaskService.httpClient.post(SchedulerTaskService.address + "Stop", data);
    }

    public async GetSchedulerTaskById(id: number)
    {
        return SchedulerTaskService.httpClient.get(SchedulerTaskService.address + "get/" + id);
    }

    public async UpdateSchedulerTask(data: UpdateSchedulerTaskDTO)
    {
        return SchedulerTaskService.httpClient.put(SchedulerTaskService.address + "put", data)
    }
}