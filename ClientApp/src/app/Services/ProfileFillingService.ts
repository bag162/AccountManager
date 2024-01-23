import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from '../../environments/environment'
import { ProfileFillingDataDTO } from '../ControlPanel/ProfileFillingModule/OverviewProfileComponent/overfiewprofile.component'
import { ProfileFillingTableDTO } from '../ControlPanel/ProfileFillingModule/profilefilling.component';

@Injectable({ providedIn: 'root' })
export class ProfileFillingService {
    private static address: string = environment.apiUrl + "/api/ProfileFilling/";
    private static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        ProfileFillingService.httpClient = httpClient;
    }

    public async AddProfileFillingData(data: ProfileFillingDataDTO) {
        return ProfileFillingService.httpClient.post(ProfileFillingService.address + "post", data);
    }

    public async GetProfileFillingData(id: number)
    {
        return ProfileFillingService.httpClient.get(ProfileFillingService.address + "get/" + id);
    }

    public static async DeleteProfileFillingData(data: ProfileFillingTableDTO[])
    {
        return ProfileFillingService.httpClient.delete(ProfileFillingService.address + "delete", {
            body: data
        });
    }
    
    public async GetProfileFillingNames()
    {
        return ProfileFillingService.httpClient.get(ProfileFillingService.address + "GetList");
    }
}