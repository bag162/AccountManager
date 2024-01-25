import { Component, OnInit } from '@angular/core';
import { AdvertAccountService } from 'src/app/Services/AdvertAccountService';
import { DataService } from 'src/app/Services/DataService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'advert-following-task',
    templateUrl: 'advertfollowingtask.component.html'
})

export class AdvertFollowingTaskComponent implements OnInit {
    taskName: string;
    followsPerAccount: number;
    advertAccountGroupNames: string[];
    accountGroup: string;
    proxyGroup: string;

    advertAccountService: AdvertAccountService;
    taskManagerService: TaskManagerService;
    dataService: DataService;
    
    constructor(advertAccountService: AdvertAccountService, 
        taskManagerService: TaskManagerService,
        dataService: DataService) { 
        this.advertAccountService = advertAccountService;
        this.taskManagerService = taskManagerService;
        this.dataService = dataService;
    }

    async ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();

        (await this.advertAccountService.GetGroupNames()).subscribe({
            next: (data: string[]) => {
                this.advertAccountGroupNames = data;
            }
        })

        this.dataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        })
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = <string>data;
        })
     }

    public async AddTask()
    {
        var newTask = new AddAdvertFollowingTaskDTO();
        newTask.AccountGroup = this.accountGroup;
        newTask.ProxyGroup = this.proxyGroup;
        newTask.TaskName = this.taskName;
        newTask.FollowsPerAccount = this.followsPerAccount;
        newTask.AdvertAccountGroup = <string>$('#selectAdvertAccountGroup option:selected').val();

        (await this.taskManagerService.AddAdvertFollowingTask(newTask)).subscribe(
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
                    $("#errorNot").show(200);
                    $("#errorNot").delay(2000).hide(200);
                }
            }
        );
    }
}

export class AddAdvertFollowingTaskDTO
{
    TaskName: string;
    AccountGroup: string;
    ProxyGroup: string;
    AdvertAccountGroup: string;
    FollowsPerAccount: number;
}