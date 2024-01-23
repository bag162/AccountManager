import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { ProfileFillingService } from 'src/app/Services/ProfileFillingService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'profile-filling',
    templateUrl: 'profilefillingtask.component.html'
})

export class ProfileFillingTaskComponent implements OnInit {
    taskName: string;
    fillingData: string[];
    accountGroup: string;
    proxyGroup: string;
    profileFillingService: ProfileFillingService;
    taskManagerService: TaskManagerService;
    dataService: DataService;

    constructor(profileFillingService: ProfileFillingService, taskManagerService: TaskManagerService, dataService: DataService) { 
        this.profileFillingService = profileFillingService;
        this.taskManagerService = taskManagerService;
        this.dataService = dataService;
    }

    async ngOnInit() { 
        $("#successNot").hide();
        $("#errorNot").hide();

        (await this.profileFillingService.GetProfileFillingNames()).subscribe({
            next: (data:string[]) => {
                this.fillingData = data;
            }
        })

        this.dataService.subscriberAccountGroup$.subscribe((data: string) => {
            this.accountGroup = data;
        })
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = data;
        })
    }

    async AddTask()
    {
        var addedTask = new AddFillingProfileTaskDTO();
        addedTask.ClientTaskName = this.taskName;
        addedTask.AccountGroup = this.accountGroup;
        addedTask.ProxyGroup = this.proxyGroup;
        addedTask.FillingProfileName = <string>$('#selectFillingData option:selected').val();
        
        (await this.taskManagerService.AddFillingProfileTask(addedTask)).subscribe(
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

export class AddFillingProfileTaskDTO
{
    ClientTaskName: string;
    ProxyGroup: string;
    AccountGroup: string;
    FillingProfileName: string;
}