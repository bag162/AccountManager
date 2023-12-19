import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService'
@Component({
    selector: 'registrationtask',
    templateUrl: 'registrationtask.component.html'
})

export class RegistrationTaskComponent implements OnInit {
    DataService: DataService;
    TaskManagerService: TaskManagerService;
    // Input Data
    accountGroup: string;
    proxyGroup: string;
    smsService: string;
    countAccount: number;

    constructor(DataService: DataService, TaskManagerService: TaskManagerService) {
        this.DataService = DataService;
        this.TaskManagerService = TaskManagerService;
    }

    ngOnInit() {
        this.DataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        })
        this.DataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = data;
        })
        this.DataService.subscriberSMSService$.subscribe((data: string) => {
            this.smsService = data;
        })
        $("#successNot").hide();
        $("#errorNot").hide();
    }

    public async AddTask() {
        var SMSServiceId = this.smsService.split(":")[0];
        var task = new AddRegistrationTaskDTO(this.proxyGroup, this.accountGroup, Number(SMSServiceId), this.countAccount);
        (await this.TaskManagerService.AddRegistrationTask(task)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#successNot").delay(400).hide(200);
                }
                else {
                    $("#errorNot").show(200);
                    $("#errorNot").delay(400).hide(200);
                }
            },
            error: data => {
                $("#errorNot").val(data);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
            }
        })
    }
}

export class AddRegistrationTaskDTO {
    constructor(ProxyGroup: string, AccountGroup: string, SMSServiceId: number, CountAccount: number) {
        this.ProxyGroup = ProxyGroup;
        this.AccountGroup = AccountGroup;
        this.SMSServiceId = SMSServiceId;
        this.CountAccount = CountAccount;
    }

    ProxyGroup: string;
    AccountGroup: string;
    SMSServiceId: number;
    CountAccount: number;
}