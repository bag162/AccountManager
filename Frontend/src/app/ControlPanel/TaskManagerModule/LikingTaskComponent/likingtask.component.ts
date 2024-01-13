import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { PostGroupService } from 'src/app/Services/PostGroupService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'liking-task',
    templateUrl: 'likingtask.component.html'
})

export class LikingTaskComponent implements OnInit {
    accountGroup: string;
    proxyGroup: string;
    postGroups: string[];
    likesPerAccount: number;
    taskName: string;
    dataService: DataService;
    taskManagerService: TaskManagerService;
    postGroupService: PostGroupService;

    constructor(dataService: DataService,
        taskManagerService: TaskManagerService,
        postGroupService: PostGroupService) {
            this.dataService = dataService
            this.taskManagerService = taskManagerService;
            this.postGroupService = postGroupService;
    }

    async ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();

        (await this.postGroupService.GetList()).subscribe({
            next: (data: string[]) => {
                this.postGroups = data;
                this.postGroups.unshift("All groups");
            }
        })

        this.dataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        })
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = <string>data;
        })
    }

    async AddTask() {
        var newTask = new AddLikingTaskDTO();
        newTask.ClientTaskName = this.taskName;
        newTask.AccountGroup = this.accountGroup;
        newTask.ProxyGroup = this.proxyGroup;
        newTask.LikesPerAccount = this.likesPerAccount;
        newTask.PostGroup = <string>$('#selectPostGroup option:selected').val();

        (await this.taskManagerService.AddLikingTask(newTask)).subscribe(
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

export class AddLikingTaskDTO {
    ClientTaskName: string;
    ProxyGroup: string;
    AccountGroup: string;
    PostGroup: string;
    LikesPerAccount: number;
}