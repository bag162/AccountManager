import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AddAdvertPostDTO, DeleteAdvertPostDTO } from '../ControlPanel/AdvertisingResources/AdvertPost/advertpost.component'
import { AddAdvertPostGroupDTO, DeleteAdvertPostGroupDTO } from '../ControlPanel/AdvertisingResources/AdvertPost/AdvertPostGroup/advertpostgroup.component'

@Injectable({ providedIn: 'root' })
export class AdvertPostService {
    private static httpClient: HttpClient;
    private static advertPostGroupURI: string = environment.apiUrl + "/api/AdvertPostGroup/";
    private static advertPostURI: string = environment.apiUrl + "/api/AdvertPost/";

    constructor(httpClient: HttpClient) {
        AdvertPostService.httpClient = httpClient;
    }

    public static async DeletePosts(Posts: DeleteAdvertPostDTO[]) {
        return AdvertPostService.httpClient.delete(AdvertPostService.advertPostURI + "delete", {
            body: Posts
        });
    }

    public async AddPosts(Posts: AddAdvertPostDTO[]) {
        return AdvertPostService.httpClient.post(AdvertPostService.advertPostURI + "post", Posts);
    }

    public static async DeleteGroups(groups: DeleteAdvertPostGroupDTO[]) {
        return AdvertPostService.httpClient.delete(AdvertPostService.advertPostGroupURI + "delete", {
            body: groups
        });
    }

    public async AddGroups(groups: AddAdvertPostGroupDTO) {
        return AdvertPostService.httpClient.post(AdvertPostService.advertPostGroupURI + "post", groups);
    }

    public async GetGroupNames() {
        return AdvertPostService.httpClient.get(AdvertPostService.advertPostGroupURI + "getlist");
    }
}