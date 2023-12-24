import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { EmailDTO } from '../ControlPanel/EmailComponent/email.component';

@Injectable({ providedIn: 'root' })
export class EmailService {
    static address: string = environment.apiUrl + "email/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        EmailService.httpClient = httpClient;
    }

    public async GetEmail()
    {
        return EmailService.httpClient.get(EmailService.address + "getlist");
    }

    public async AddEmail(smsservice: EmailDTO[]) {
        return EmailService.httpClient.post(EmailService.address + "post", smsservice);
    }

    static UpdateEmail(smsservice: EmailDTO[]) {
        return EmailService.httpClient.put(EmailService.address + "put", smsservice);
    }

    static DeleteEmail(smsservice: EmailDTO[]) {
        return EmailService.httpClient.delete(this.address + "delete", {
            body: smsservice
        });
    }
}