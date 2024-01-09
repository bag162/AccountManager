import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment'
import { CRUDCommentGroupDTO } from '../ControlPanel/PostModule/PostCommentComponent/postcomment.component'

@Injectable({ providedIn: 'root' })
export class PostCommentService {
    static address: string = environment.apiUrl + "/api/postcomment/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        PostCommentService.httpClient = httpClient;
    }

    public async Add(comentsDTO: CRUDCommentGroupDTO[]) {
        return PostCommentService.httpClient.post(PostCommentService.address + "post", comentsDTO);
    }

    static async Delete(deletedComment: CRUDCommentGroupDTO[]) {
        return PostCommentService.httpClient.delete(PostCommentService.address + "delete", {
            body: deletedComment
        });
    }
}