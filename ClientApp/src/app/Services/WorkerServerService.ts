import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../environments/environment'
import { AddWorkerServerDTO } from '../ControlPanel/ServerAccessModule/serveraccess.component'


@Injectable({ providedIn: 'root' })
export class WorkerServerService {
    private static address: string = environment.apiUrl + "/api/WorkerServer/";
    private static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        WorkerServerService.httpClient = httpClient;
    }

    public async AddWorkerServer(server: AddWorkerServerDTO) {
        return WorkerServerService.httpClient.post(WorkerServerService.address + "post", server);
    }

    public static async DeleteWorkerServers(ids: number[]) {
        return WorkerServerService.httpClient.delete(WorkerServerService.address + "delete", {
            body: ids
        });
    }

    public static async ChangeStatus(ids: number[]) {
        return WorkerServerService.httpClient.post(WorkerServerService.address + "ChangeStatus", ids);
    }
}