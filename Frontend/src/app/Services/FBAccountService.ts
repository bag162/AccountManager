import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { FBAccountDTO } from '../ControlPanel/FBAccpuntsComponent/fbaccounts.component';
import {environment} from 'src/environments/environment'
import { FBAccGroupDTO } from '../ControlPanel/FBAccpuntsComponent/GroupFbAccComponent/groupfbaccount.component';
@Injectable({ providedIn: 'root' })
export class FbAccountService {
    static address: string = environment.apiUrl + "fbaccount/";
    static accountGroupAdress: string = environment.apiUrl + "fbgroup/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        FbAccountService.httpClient = httpClient;
    }

    public async AddAccounts(account: FBAccountDTO[])
    {
        return FbAccountService.httpClient.post(FbAccountService.address + "post", account);
    }

    static UpdateAccounts(account: FBAccountDTO[])
    {
        return FbAccountService.httpClient.put(FbAccountService.address + "put", account);
    }
    
    static DeleteAccounts(account: FBAccountDTO[]) {
        return FbAccountService.httpClient.delete(this.address + "delete", { 
            body: account
        });
    }

    static GetFBAccGroups()
    {
        return FbAccountService.httpClient.get(this.accountGroupAdress + "get");
    }

    static AddFBAccGroup(newGroup: FBAccGroupDTO[])
    {
        return FbAccountService.httpClient.post(this.accountGroupAdress + "post", newGroup);
    }
}