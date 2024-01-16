import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { PostGroupDTO } from '../ControlPanel/PostModule/PostGroupComponent/postgroup.component';
import { environment } from '../../environments/environment'

@Injectable({ providedIn: 'root' })
export class PostGroupService {
    static address: string = environment.apiUrl + "/api/postgroup/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        PostGroupService.httpClient = httpClient;
    }

    public async AddPostGroup(group: PostGroupDTO[]) {
        return PostGroupService.httpClient.post(PostGroupService.address + "post", group);
    }

    static async DeletePostGroup(group: PostGroupDTO[]) {
        return PostGroupService.httpClient.delete(this.address + "delete", {
            body: group
        });
    }

    public async GetList()
    {
        return PostGroupService.httpClient.get(PostGroupService.address + "GetList");
    }
}