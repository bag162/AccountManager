import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'authoorization-task',
    templateUrl: 'authorizationtask.component.html'
})

export class AuthorizatioonTaskComponent implements OnInit {
    accountGroup: string;
    proxyGroup: string;
    taskName: string;

    dataService: DataService;
    taskManagerService: TaskManagerService;

    constructor(dataService: DataService, taskManagerService: TaskManagerService) {
        this.dataService = dataService;
        this.taskManagerService = taskManagerService;
    }

    ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();
        this.dataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        })
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = data;
        })
    }

    async AddTask() {
        var newTask = new AuthorizationTaskDTO(this.taskName, this.proxyGroup, this.accountGroup);
        (await this.taskManagerService.AddAuthorizationTask(newTask)).subscribe(
            {
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
                error: (error: string) => {
                    $("#errorNot").val(error);
                    $("#errorNot").show(200);
                    $("#errorNot").delay(2000).hide(200);
                    $("#errorNot").val("Error");
                }
            }
        );
    }
}

export class AuthorizationTaskDTO {
    constructor(ClientTaskName: string, ProxyGroup: string, AccountGroup: string) {
        this.ClientTaskName = ClientTaskName;
        this.ProxyGroup = ProxyGroup;
        this.AccountGroup = AccountGroup;
    }
    public ClientTaskName: string;
    public ProxyGroup: string;
    public AccountGroup: string;
}