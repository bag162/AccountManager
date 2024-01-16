import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment'
import { CRUDCommentGroupDTO } from '../ControlPanel/PostModule/PostCommentComponent/PostCommentGroupComponent/PostCommentGroup.component';

@Injectable({ providedIn: 'root' })
export class PostCommentGroupService {
    static httpClient: HttpClient;
    static address: string = environment.apiUrl + "/api/PostCommentGroup/";
    constructor(httpClient: HttpClient) {
        PostCommentGroupService.httpClient = httpClient;
    }

    public async AddGroup(group: CRUDCommentGroupDTO) {
        return PostCommentGroupService.httpClient.post(PostCommentGroupService.address + "post", group);
    }

    static async DeleteGroup(group: CRUDCommentGroupDTO[]) {
        return PostCommentGroupService.httpClient.delete(PostCommentGroupService.address + "delete", {
            body: group
        });
    }

    public async GetGroupNames()
    {
        return PostCommentGroupService.httpClient.get(PostCommentGroupService.address + "GetGroupNames");
    }
}