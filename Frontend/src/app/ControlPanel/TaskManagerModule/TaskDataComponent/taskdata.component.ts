import { Component, OnInit } from '@angular/core';
import { TaskService } from 'src/app/Services/TaskService'
import { environment } from 'src/environments/environment'
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';

@Component({
    selector: 'taskdata',
    templateUrl: 'taskdata.component.html'
})

export class TaskDataComponent implements OnInit {
    dtOptions: any;
    TaskData: string;
    taskService: TaskService;

    constructor(taskService: TaskService) {
        this.taskService = taskService;
    }

    ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + 'task/get',
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
                        StopTask(dt.rows({ selected: true }).data()[0], dt);
                    }
                }, {
                    text: 'Start selected',
                    action: function (e, dt, node, config) {
                        StartTask(dt.rows({ selected: true }).data()[0], dt);
                    }
                }, {
                    text: 'Delete selected',
                    action: function (e, dt, node, config) {
                        Delete(dt.rows({ selected: true }).data()[0], dt);
                    }
                }, {
                    text: 'Info selected',
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
        async function Delete(data: string, dt: any) {
            let deletedData: TaskDTO = JSON.parse(JSON.stringify(data));
            if (deletedData.Status == "Completed" || deletedData.Status == "Canceled") {
                (await TaskService.DeleteTask(deletedData)).subscribe(
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
            else {
                $("#errorNot").show(200);
                $("#errorNot").delay(400).hide(200);
            }
        }

        async function StartTask(data: string, dt: any) {
            let startTask: TaskDTO = JSON.parse(JSON.stringify(data));
            if (startTask.Status == "Completed" || startTask.Status == "Canceled") {
                (await TaskService.StartTask(startTask)).subscribe(
                    {
                        next: (data: boolean) => {
                            console.log(data)
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
            else {
                $("#errorNot").show(200);
                $("#errorNot").delay(400).hide(200);
            }
        }
        async function StopTask(data: string, dt: any) {
            let stopTask: TaskDTO = JSON.parse(JSON.stringify(data));
            if (stopTask.Status != "Completed" && stopTask.Status != "Canceled") {
                (await TaskService.StopTask(stopTask)).subscribe(
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
            else {
                $("#errorNot").show(200);
                $("#errorNot").delay(400).hide(200);
            }
        }

        async function RouteToWorkerData(data: string, dt: any) {
            $(location).attr('href', window.location.origin.toString() + "/taskmanager/workertaskdata/" + data["Id"]);
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