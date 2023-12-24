import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { InstAccountDTO } from '../ControlPanel/InstAccountComponent/Instaccounts.component';
import {environment} from 'src/environments/environment'
import { InstAccGroupDTO } from '../ControlPanel/InstAccountComponent/GroupInstAccComponent/groupInstaccount.component';

@Injectable({
     providedIn: 'root' })
export class InstAccountService {
    static address: string = environment.apiUrl + "Instaccount/";
    static accountGroupAdress: string = environment.apiUrl + "Instgroup/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        InstAccountService.httpClient = httpClient;
    }

    public async AddAccounts(account: InstAccountDTO[])
    {
        return InstAccountService.httpClient.post(InstAccountService.address + "post", account);
    }

    static UpdateAccounts(account: InstAccountDTO[])
    {
        return InstAccountService.httpClient.put(InstAccountService.address + "put", account);
    }
    
    static DeleteAccounts(account: InstAccountDTO[]) {
        return InstAccountService.httpClient.delete(this.address + "delete", { 
            body: account
        });
    }

    static GetInstAccGroups()
    {
        return InstAccountService.httpClient.get(this.accountGroupAdress + "get");
    }

    static AddInstAccGroup(newGroup: InstAccGroupDTO[])
    {
        return InstAccountService.httpClient.post(this.accountGroupAdress + "post", newGroup);
    }
}