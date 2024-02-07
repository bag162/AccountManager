import { Component, OnInit } from '@angular/core';
import { AdvertAccountService } from 'src/app/Services/AdvertAccountService';
import { ClonGroupService } from 'src/app/Services/ClonGroupService';
import { DataService } from 'src/app/Services/DataService';
import { TaskManagerService } from 'src/app/Services/TaskManagerService';

@Component({
    selector: 'parsing-cloning-information',
    templateUrl: 'parsingcloninginformation.component.html'
})

export class ParsingCloningInformationComponent implements OnInit {
    accountGroup: string;
    proxyGroup: string;

    countCommentToCollect: number;
    countPostToCollect: number;
    taskName: string;

    cloneGroupForSave: string[];
    advertAccountGroupsForCloning: string[];

    advertAccountService: AdvertAccountService;
    cloneGroupService: ClonGroupService;
    taskManagerService: TaskManagerService;
    dataService: DataService;

    constructor(advertAccountService: AdvertAccountService,
        cloneGroupService: ClonGroupService,
        taskManagerService: TaskManagerService,
        dataService: DataService) {
        this.cloneGroupService = cloneGroupService;
        this.advertAccountService = advertAccountService;
        this.taskManagerService = taskManagerService;
        this.dataService = dataService;
    }

    async ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();

        (await this.cloneGroupService.GetClonNames()).subscribe({
            next: (data: string[]) => {
                this.cloneGroupForSave = data;
            }
        });

        (await this.advertAccountService.GetGroupNames()).subscribe({
            next: (data: string[]) => {
                this.advertAccountGroupsForCloning = data;
            }
        });

        this.dataService.subscriberAccountGroup$.subscribe((data) => {
            this.accountGroup = <string>data;
        })
        this.dataService.subscriberProxyGroup$.subscribe((data: string) => {
            this.proxyGroup = <string>data;
        })
    }

    async AddTask() {
        var addedtask = new AddParseCloningTaskDTO();
        addedtask.TaskName = this.taskName;
        addedtask.CountPostToCollect = this.countPostToCollect;
        addedtask.CountCommentToCollect = this.countCommentToCollect;
        addedtask.AccountGroup = this.accountGroup;
        addedtask.ProxyGroup = this.proxyGroup;
        addedtask.AdvertAccountGroupsForCloning = <string>$('#selectAdvertAccountGroup option:selected').val();
        addedtask.CloneGroupForSave = <string>$('#selectCloneGroup option:selected').val();

        (await this.taskManagerService.AddParseCloningInformationTask(addedtask)).subscribe(
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

export class AddParseCloningTaskDTO {
    TaskName: string;
    AccountGroup: string;
    ProxyGroup: string;
    CountCommentToCollect: number;
    CountPostToCollect: number;

    CloneGroupForSave: string;
    AdvertAccountGroupsForCloning: string;
}