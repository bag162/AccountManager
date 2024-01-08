import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'posting-task',
    templateUrl: 'postingtask.component.html'
})

export class PostingTaskComponent implements OnInit {
    accountGroup: string;
    proxyGroup: string;
    dataService: DataService;
    taskManagerService: TaskManagerService;
    postPerAccount:number;
    taskName: string;

    constructor(DataService: DataService, taskManagerService: TaskManagerService) {
        this.dataService = DataService;
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
        var addedTask: AddPostingTaskDTO = new AddPostingTaskDTO();
        addedTask.AccountGroup = this.accountGroup;
        addedTask.ClientTaskName = this.taskName;
        addedTask.PostPerAccount = this.postPerAccount;
        addedTask.ProxyGroup = this.proxyGroup;

        (await this.taskManagerService.AddPostingTask(addedTask)).subscribe(
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

export class AddPostingTaskDTO {
    public ClientTaskName: string;
    public PostPerAccount: number;
    public ProxyGroup: string;
    public AccountGroup: string;
}