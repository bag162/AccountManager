import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { SMSServiceDTO } from '../ControlPanel/SMSServiceComponent/smsservice.component'
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class SMSActivationService {
    static address: string = environment.apiUrl + "smsservice/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        SMSActivationService.httpClient = httpClient;
    }

    public async AddSMSService(smsservice: SMSServiceDTO[]) {
        return SMSActivationService.httpClient.post(SMSActivationService.address + "post", smsservice);
    }

    static UpdateSMSService(smsservice: SMSServiceDTO[]) {
        return SMSActivationService.httpClient.put(SMSActivationService.address + "put", smsservice);
    }

    static DeleteSMSService(smsservice: SMSServiceDTO[]) {
        return SMSActivationService.httpClient.delete(this.address + "delete", {
            body: smsservice
        });
    }
}