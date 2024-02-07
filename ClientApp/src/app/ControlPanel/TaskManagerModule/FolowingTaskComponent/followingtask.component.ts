import { Component, OnInit } from '@angular/core';
import { data } from 'jquery';
import { ClonGroupService } from 'src/app/Services/ClonGroupService';
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
    clonGroupService: ClonGroupService;
    
    clonGroupNames: string[];
    accountGroups: string[];
    followsPerAccount: number;
    requiredFollowerPerAccount: number;
    taskName: string;
    accountGroup: string;
    proxyGroup: string;
    followingPostResourses: string;
    followingAccountResourses: string;

    constructor(instAccountService: InstAccountService,
        dataService: DataService,
        taskManagerService: TaskManagerService,
        clonGroupService: ClonGroupService) { 
            this.instAccountService = instAccountService;
            this.dataService = dataService;
            this.taskManagerService = taskManagerService;
            this.clonGroupService = clonGroupService;
        }

    async ngOnInit() { 
        $("#successNot").hide();
        $("#errorNot").hide();

        this.dataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        });
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = <string>data;
        });

        await InstAccountService.GetInstAccGroups().subscribe({
            next: (data: string[]) => {
                this.accountGroups = data;
                this.accountGroups.unshift("All groups")
            }
        });

        (await this.clonGroupService.GetClonNames()).subscribe({
            next: (data:string[]) => {
                this.clonGroupNames = data;
            }
        });
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

        newTask.accountResourseType = this.followingAccountResourses;
        newTask.followingResourseType = this.followingPostResourses;
        if (newTask.accountResourseType == "By cloning information") {
            newTask.accountClonName = <string>$('#followingSelectClonName option:selected').val();
        }
        if (newTask.followingResourseType == "By cloning information") {
            newTask.followingClonName = <string>$('#postSelectClonName option:selected').val();
        }
        
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

    followingResourseType: string;
    followingClonName: string;

    accountResourseType: string;
    accountClonName: string;
}