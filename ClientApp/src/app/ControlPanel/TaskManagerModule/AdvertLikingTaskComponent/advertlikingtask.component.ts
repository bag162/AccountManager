import { Component, OnInit } from '@angular/core';
import { AdvertPostService } from 'src/app/Services/AdvertPostService';
import { DataService } from 'src/app/Services/DataService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'advert-liking-task',
    templateUrl: 'advertlikingtask.component.html'
})

export class AdvertLikingTaskComponent implements OnInit {
    taskName: string;
    likesPerAccount: number;
    advertPostGroupNames: string[];
    likeIfPostCommentedPreviously: boolean = false;
    accountGroup: string;
    proxyGroup: string;

    advertPostService: AdvertPostService;
    taskManagerService: TaskManagerService;
    dataService: DataService;
    
    constructor(advertPostService: AdvertPostService, 
        taskManagerService: TaskManagerService,
        dataService: DataService) { 
        this.advertPostService = advertPostService;
        this.taskManagerService = taskManagerService;
        this.dataService = dataService;
    }

    async ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();

        (await this.advertPostService.GetGroupNames()).subscribe({
            next: (data: string[]) => {
                this.advertPostGroupNames = data;
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
        var newTask = new AddAdvertLikingTaskDTO();
        newTask.AccountGroup = this.accountGroup;
        newTask.ProxyGroup = this.proxyGroup;
        newTask.TaskName = this.taskName;
        newTask.LikesPerAccount = this.likesPerAccount;
        newTask.LikeIfPostCommentedPreviously = this.likeIfPostCommentedPreviously;
        newTask.AdvertPostGroup = <string>$('#selectAdvertPostGroup option:selected').val();

        (await this.taskManagerService.AddAdvertLikingTask(newTask)).subscribe(
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

export class AddAdvertLikingTaskDTO
{
    TaskName: string;
    AccountGroup: string;
    ProxyGroup: string;
    AdvertPostGroup: string;
    LikesPerAccount: number;
    LikeIfPostCommentedPreviously: boolean;
}