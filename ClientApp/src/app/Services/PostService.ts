import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../environments/environment'
import { PostDTO } from '../ControlPanel/PostModule/PostManagerComponent/postmanager.component'
import { CRUDPostDTO, UpdatePostDTO } from '../ControlPanel/PostModule/PostManagerComponent/OverviewPostComponent/overviewpost.component';
@Injectable({ providedIn: 'root' })
export class PostService {
    static address: string = environment.apiUrl + "/api/post/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        PostService.httpClient = httpClient;
    }

    public async AddPost(post: CRUDPostDTO) {
        return PostService.httpClient.post(PostService.address + "post", post);
    }

    static async DeletePost(post: PostDTO[]) {
        return PostService.httpClient.delete(this.address + "delete", {
            body: post
        });
    }

    public async GetPostById(postId: number) {
        return PostService.httpClient.get(PostService.address + "get" + "/" + postId);
    }

    public async UpdatePost(post: UpdatePostDTO) {
        return PostService.httpClient.put(PostService.address + "put", post);
    }
}