import { Component, OnInit } from '@angular/core';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';
import { DataService } from 'src/app/Services/DataService';
import { PostGroupService } from 'src/app/Services/PostGroupService';

@Component({
    selector: 'commenting-task',
    templateUrl: 'commentingtask.component.html'
})

export class CommentingTaskComponent implements OnInit {
    accountGroup: string;
    proxyGroup: string;
    postGroups: string[];
    commentsPerAccount: number;
    taskName: string;
    dataService: DataService;
    taskManagerService: TaskManagerService;
    postGroupService: PostGroupService;

    constructor(DataService: DataService, taskManagerService: TaskManagerService, postGroupService: PostGroupService) {
        this.dataService = DataService;
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
            this.proxyGroup = data;
        })
    }

    async AddTask() {
        var newTask = new AddCommentingTaskDTO();
        newTask.ClientTaskName = this.taskName;
        newTask.AccountGroup = this.accountGroup;
        newTask.ProxyGroup = this.proxyGroup;
        newTask.CommentsPerAccount = this.commentsPerAccount;
        newTask.PostGroup = <string>$('#selectGroupPost option:selected').val();

        (await this.taskManagerService.AddCommentingTask(newTask)).subscribe(
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

export class AddCommentingTaskDTO {
    ClientTaskName: string;
    ProxyGroup: string;
    AccountGroup: string;
    PostGroup: string;
    CommentsPerAccount: number;
}