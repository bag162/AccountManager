import { Component, OnInit } from '@angular/core';
import { data } from 'jquery';
import { DataService } from 'src/app/Services/DataService';
import { InstAccountService } from 'src/app/Services/InstAccountService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'following-task',
    templateUrl: 'followingtask.component.html'
})

export class FollowingTaskComponent implements OnInit {
    instAccountService: InstAccountService;
    dataService: DataService;
    taskManagerService: TaskManagerService;

    accountGroups: string[];
    followsPerAccount: number;
    requiredFollowerPerAccount: number;
    taskName: string;
    accountGroup: string;
    proxyGroup: string;

    constructor(instAccountService: InstAccountService,
        dataService: DataService,
        taskManagerService: TaskManagerService) { 
            this.instAccountService = instAccountService;
            this.dataService = dataService;
            this.taskManagerService = taskManagerService;
        }

    async ngOnInit() { 
        $("#successNot").hide();
        $("#errorNot").hide();

        this.dataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        })
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = <string>data;
        })

        await InstAccountService.GetInstAccGroups().subscribe({
            next: (data: string[]) => {
                this.accountGroups = data;
                this.accountGroups.unshift("All groups")
            }
        })
    }

    async AddTask()
    {
        var newTask = new AddFollowingTaskDTO();
        newTask.ClientTaskName = this.taskName;
        newTask.AccountGroup = this.accountGroup;
        newTask.ProxyGroup = this.proxyGroup;
        newTask.FollowsPerAccount = this.followsPerAccount;
        newTask.RequiredFollowersPerAccount = this.requiredFollowerPerAccount;
        newTask.AccountGroupForSubscription = <string>$('#selectAccountGroupForSubscription option:selected').val();

        (await this.taskManagerService.AddFollowingTask(newTask)).subscribe(
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


export class AddFollowingTaskDTO
{
    ClientTaskName: string;
    ProxyGroup: string;
    AccountGroupForSubscription: string;
    AccountGroup: string;
    FollowsPerAccount: number;
    RequiredFollowersPerAccount: number;
}