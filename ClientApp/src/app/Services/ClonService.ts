import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'

@Injectable({ providedIn: 'root' })
export class ClonService {

    private static httpClient: HttpClient;
    private static address: string = environment.apiUrl + "/api/clon/";
    constructor(httpClient: HttpClient) {
        ClonService.httpClient = httpClient;
    }

    public async GetClonInfo(clonId: number)
    {
        return ClonService.httpClient.get(ClonService.address + "getdata/" + clonId);
    }

    public static async DeleteClones(ids: number[])
    {
        return ClonService.httpClient.delete(ClonService.address + "delete", {
            body: ids
        });
    }
}