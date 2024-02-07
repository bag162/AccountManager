import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'
import { AddClonGroupDTO } from '../ControlPanel/CloneModule/ClonGroupComponent/clongroup.component'

@Injectable({ providedIn: 'root' })
export class ClonGroupService {

    private httpClient: HttpClient;
    private address: string = environment.apiUrl + "/api/ClonGroup/";
    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
    }

    public async GetClonNames()
    {
        return this.httpClient.get(this.address + "getlist");
    }

    public async AddClonGroup(data: AddClonGroupDTO) {
        return this.httpClient.post(this.address + "post", data);
    }
}