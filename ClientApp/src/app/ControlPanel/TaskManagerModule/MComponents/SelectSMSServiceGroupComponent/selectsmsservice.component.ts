import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { SMSActivationService } from 'src/app/Services/SMSActivationService'
@Component({
    selector: 'select-sms-service',
    templateUrl: 'selectsmsservice.component.html'
})

export class SelectSMSServiceComponent implements OnInit {
    SMSActivationService: SMSActivationService;
    smsServices: string[];
    DataService: DataService;
    data;
    constructor(SMSActivationService: SMSActivationService, DataService: DataService) {
        this.SMSActivationService = SMSActivationService;
        this.DataService = DataService;
    }

    async ngOnInit() {
        (await this.SMSActivationService.GetSMSServices()).subscribe({
            next: (data: string[]) => {
                this.smsServices = data;
            }
        })
    }

    UpdateData() {
        this.DataService.UpdateSMSService(<string>$('#selectSMSService option:selected').val());
    }
}