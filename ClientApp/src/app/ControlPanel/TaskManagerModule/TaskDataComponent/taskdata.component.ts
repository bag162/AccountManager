import { Component, OnInit } from '@angular/core';
import { TaskService } from 'src/app/Services/TaskService'
import { environment } from 'src/environments/environment'
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { Router } from '@angular/router';

@Component({
    selector: 'taskdata',
    templateUrl: 'taskdata.component.html'
})

export class TaskDataComponent implements OnInit {
    public static router: Router;
    dtOptions: any;
    TaskData: string;
    taskService: TaskService;

    constructor(taskService: TaskService, router: Router) {
        this.taskService = taskService;
        TaskDataComponent.router = router;
    }

    ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + '/api/task/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Task Name',
                data: 'ClientTaskName'
            }, {
                title: 'Status',
                data: "Status"
            }, {
                title: 'Task Type',
                data: 'TaskType'
            }, {
                title: 'Useful data',
                data: 'UsefulData'
            }, {
                title: 'Account group',
                data: 'AccountGroup'
            }, {
                title: 'Proxy group',
                data: 'ProxyGroup'
            }
            ],
            select: true,
            dom: 'lBfrtip',
            buttons: [
                'colvis',
                {
                    extend: 'copy',
                    text: 'Copy selected'
                },
                'print',
                'selectAll',
                {
                    text: 'Stop selected',
                    action: function (e, dt, node, config) {
                        StopTask(dt.rows({ selected: true }).data(), dt);
                    }
                }, {
                    text: 'Start selected',
                    action: function (e, dt, node, config) {
                        StartTask(dt.rows({ selected: true }).data(), dt);
                    }
                }, {
                    text: 'Delete selected',
                    action: function (e, dt, node, config) {
                        Delete(dt.rows({ selected: true }).data(), dt);
                    }
                }, {
                    text: 'View WorkerTasks selected Task',
                    action: function (e, dt, node, config) {
                        RouteToWorkerData(dt.rows({ selected: true }).data()[0], dt);
                    }
                }, {
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                }
            ]
        };
        async function Delete(datas: string[], dt: any) {
            var tasksToDelete = new Array<TaskDTO>();
            for (let index = 0; index < datas.length; index++) {
                const element = datas[index];
                let deletedData: TaskDTO = JSON.parse(JSON.stringify(element));
                if (deletedData.Status == "Completed" || deletedData.Status == "Canceled"){
                    tasksToDelete.push(deletedData);
                }
            }
            (await TaskService.DeleteTask(tasksToDelete)).subscribe(
                {
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
                    error: (data: string) => {
                        $("#errorNot").show(200);
                        $("#errorNot").delay(400).hide(200);
                    }
                }
            )
        }

        async function StartTask(datas: string[], dt: any) {
            var taskToStart = new Array<TaskDTO>();
            for (let index = 0; index < datas.length; index++) {
                const element = datas[index];
                let startTask: TaskDTO = JSON.parse(JSON.stringify(element));
                if (startTask.Status == "Completed" || startTask.Status == "Canceled"){
                    taskToStart.push(startTask);
                }
            }
            (await TaskService.StartTask(taskToStart)).subscribe(
                {
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
                    error: (data: string) => {
                        $("#errorNot").show(200);
                        $("#errorNot").delay(400).hide(200);
                    }
                }
            )
        }
        async function StopTask(datas: string[], dt: any) {
            var taskToStop = new Array<TaskDTO>();
            for (let index = 0; index < datas.length; index++) {
                const element = datas[index];
                let stopTask: TaskDTO = JSON.parse(JSON.stringify(element));
                if (stopTask.Status != "Completed" && stopTask.Status != "Canceled"){
                    taskToStop.push(stopTask);
                }
            }

            (await TaskService.StopTask(taskToStop)).subscribe(
                {
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
                    error: (data: string) => {
                        $("#errorNot").show(200);
                        $("#errorNot").delay(400).hide(200);
                    }
                }
            )
        }

        async function RouteToWorkerData(data: string, dt: any) {
            TaskDataComponent.router.navigate(["/taskmanager/workertaskdata/" + data["Id"]])
        }
    }
}

export class TaskDTO {
    constructor(id: string, Status: string, UsefulData: string, AccountGroup: string, ProxyGroup: string, ClientTaskName: string) {
        this.id = id;
        this.Status = Status;
        this.UsefulData = UsefulData;
        this.AccountGroup = AccountGroup;
        this.ProxyGroup = ProxyGroup;
        this.ClientTaskName = ClientTaskName;
    }
    id: string;
    ClientTaskName: string;
    Status: string;
    UsefulData: string;
    AccountGroup: string;
    ProxyGroup: string;
}