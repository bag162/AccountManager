import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AddAdvertAccountGroupDTO, DeleteAdvertAccountGroupDTO } from '../ControlPanel/AdvertisingResources/AdvertAccount/AdvertAccountGroup/advertaccountgroup.component'
import { AddAdvertAccountDTO, DeleteAdvertAccountDTO } from '../ControlPanel/AdvertisingResources/AdvertAccount/advertaccount.component';

@Injectable({ providedIn: 'root' })
export class AdvertAccountService {
    private static httpClient: HttpClient;
    private static advertAccountGroupURI: string = environment.apiUrl + "/api/AdvertAccountGroup/";
    private static advertAccountURI: string = environment.apiUrl + "/api/AdvertAccount/";
    constructor(httpClient: HttpClient) {
        AdvertAccountService.httpClient = httpClient;
    }

    public static async DeleteAccounts(accounts: DeleteAdvertAccountDTO[])
    {
        return AdvertAccountService.httpClient.delete(AdvertAccountService.advertAccountURI + "delete", {
            body: accounts
        });
    }

    public async AddAccounts(accounts: AddAdvertAccountDTO[])
    {
        return AdvertAccountService.httpClient.post(AdvertAccountService.advertAccountURI + "post", accounts);
    }

    public static async DeleteGroups(groups: DeleteAdvertAccountGroupDTO[]) {
        return AdvertAccountService.httpClient.delete(AdvertAccountService.advertAccountGroupURI + "delete", {
            body: groups
        });
    }

    public async AddGroups(groups: AddAdvertAccountGroupDTO)
    {
        return AdvertAccountService.httpClient.post(AdvertAccountService.advertAccountGroupURI + "post", groups);
    }

    public async GetGroupNames()
    {
        return AdvertAccountService.httpClient.get(AdvertAccountService.advertAccountGroupURI + "getlist");
    }
}