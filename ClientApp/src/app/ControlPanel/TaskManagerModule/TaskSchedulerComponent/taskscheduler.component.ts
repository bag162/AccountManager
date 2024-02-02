import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment'
import { SchedulerTaskService } from '../../../Services/SchedulerTaskService'
@Component({
    selector: 'task-scheduler',
    templateUrl: 'taskscheduler.component.html'
})

export class TaskSchedulerComponent implements OnInit {
    public dtOptionsSchedulerTaskData: any;
    static router: Router;
    schedulerTaskService: SchedulerTaskService;
    constructor(router: Router, schedulerTaskService: SchedulerTaskService) {
        TaskSchedulerComponent.router = router;
        this.schedulerTaskService = schedulerTaskService;
    }

    ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();

        this.dtOptionsSchedulerTaskData = {
            ajax: environment.apiUrl + '/api/SchedulerTask/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Scheduler task name',
                data: 'Name'
            }, {
                title: 'Count pinned tasks',
                data: "CountPinnedTasks"
            }, {
                title: 'Status',
                data: 'SchedulerTaskStatus'
            }
            ],
            select: true,
            dom: 'lBfrtip',
            buttons: [
                {
                    text: "View&Update selected",
                    action: function (e, dt, node, config) {
                        TaskSchedulerComponent.router.navigate(['taskmanager/taskscheduler/view/' + dt.rows({ selected: true }).data()[0]["Id"]])
                    }
                },
                {
                    text: "Add scheduler task",
                    action: function (e, dt, node, config) {
                        TaskSchedulerComponent.router.navigate(['taskmanager/taskscheduler/add'])
                    }
                },
                {
                    text: "Deleted selected",
                    action: async function (e, dt, node, config) {
                        var data = dt.rows({ selected: true }).data();
                        var ids = new Array<number>();
                        for (let index = 0; index < data.length; index++) {
                            const element = data[index];
                            ids.push(element['Id']);
                        }
                        (await SchedulerTaskService.Delete(ids)).subscribe({
                            next: (data: boolean) => {
                                if (data) {
                                    $("#successNot").show(200);
                                    $("#successNot").delay(400).hide(200);
                                    dt.ajax.reload();
                                }
                                else {
                                    $("#errorNot").show(200);
                                    $("#errorNot").delay(400).hide(200);
                                }
                            },
                            error: (data: any) => {
                                $("#errorNot").show(200);
                                $("#errorNot").delay(400).hide(200);
                            }
                        })
                    }
                }, {
                    text: 'Start selected',
                    action: async function (e, dt, node, config) {
                        var data = dt.rows({ selected: true }).data();
                        var ids = new Array<number>();
                        for (let index = 0; index < data.length; index++) {
                            const element = data[index];
                            ids.push(element['Id']);
                        }
                        (await SchedulerTaskService.StartSchedulerTask(ids)).subscribe({
                            next: (data: boolean) => {
                                if (data) {
                                    $("#successNot").show(200);
                                    $("#successNot").delay(400).hide(200);
                                    dt.ajax.reload();
                                }
                                else {
                                    $("#errorNot").show(200);
                                    $("#errorNot").delay(400).hide(200);
                                }
                            },
                            error: (data: any) => {
                                $("#errorNot").show(200);
                                $("#errorNot").delay(400).hide(200);
                            }
                        })
                    }
                }, {
                    text: 'Stop selected',
                    action: async function (e, dt, node, config) {
                        var data = dt.rows({ selected: true }).data();
                        var ids = new Array<number>();
                        for (let index = 0; index < data.length; index++) {
                            const element = data[index];
                            ids.push(element['Id']);
                        }
                        (await SchedulerTaskService.StopSchedulerTask(ids)).subscribe({
                            next: (data: boolean) => {
                                if (data) {
                                    $("#successNot").show(200);
                                    $("#successNot").delay(400).hide(200);
                                    dt.ajax.reload();
                                }
                                else {
                                    $("#errorNot").show(200);
                                    $("#errorNot").delay(400).hide(200);
                                }
                            },
                            error: (data: any) => {
                                $("#errorNot").show(200);
                                $("#errorNot").delay(400).hide(200);
                            }
                        })
                    }
                }, {
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                }
            ]
        };
    }
}