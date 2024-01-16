import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import {EmailService} from 'src/app/Services/EmailService'
@Component({
    selector: 'select-email',
    templateUrl: 'selectemail.component.html'
})

export class SelectEmailComponent implements OnInit {
    EmailService: EmailService;
    smsServices: string[];
    DataService: DataService;
    data;
    constructor(EmailService: EmailService, DataService: DataService) {
        this.EmailService = EmailService;
        this.DataService = DataService;
    }

    async ngOnInit() {
        (await this.EmailService.GetEmail()).subscribe({
            next: (data: string[]) => {
                this.smsServices = data;
            }
        })
    }

    UpdateData() {
        this.DataService.UpdateEmail(<string>$('#selectemail option:selected').val());
    }
}