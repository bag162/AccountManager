import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { delay } from 'rxjs';
import { SchedulerTaskService } from 'src/app/Services/SchedulerTaskService';
import { environment } from 'src/environments/environment'

@Component({
    selector: 'addorview-task-scheduler',
    templateUrl: 'addorviewtaskscheduler.component.html'
})

export class AddOrViewTaskSchedulerComponent implements OnInit {
    StartupType: string;
    TimeBetweenLaunchesMinutes: number = 0;
    SchedulerTaskName: string;
    SchedulerTaskId: number;
    dtOptionsTaskData: any;
    dtOptionsSchedulerTask: any;

    schedulerTaskId: number = undefined;
    schedulerTaskService: SchedulerTaskService;
    constructor(schedulerTaskService: SchedulerTaskService, activateRoute: ActivatedRoute) {
        this.schedulerTaskService = schedulerTaskService;
        this.schedulerTaskId = activateRoute.snapshot.params["id"];
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptionsTaskData = {
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
                {
                    text: 'Add to scheduler',
                    action: function (e, dt, node, config) {
                        AddToScheduler(dt.rows({ selected: true }).data());
                    }
                },
            ]
        };

        this.dtOptionsSchedulerTask = {
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Task Name',
                data: 'ClientTaskName'
            }, {
                title: 'Task Type',
                data: 'TaskType'
            }
            ],
            order: [],
            select: true,
            dom: 'lBfrtip',
            buttons: [
                {
                    text: "Delete",
                    action: function (e, dt, node, config) {
                        dt.rows({ selected: true }).remove().draw();
                    }
                },
                {
                    text: "Up",
                    action: function (e, dt, node, config) {
                        var index = dt.row({ selected: true }).index();
                        if (index == 0) {
                            return;
                        }
                        var dataUp = dt.row({ selected: true }).data();
                        var dataDown = dt.row(index - 1).data();

                        dt.row(index).data(dataDown)
                        dt.row(index - 1).data(dataUp)

                        dt.row(index - 1).select();
                        dt.row(index).deselect();

                        dt.draw();
                    }
                },
                {
                    text: "Down",
                    action: function (e, dt, node, config) {
                        var index = dt.row({ selected: true }).index();
                        if (index == dt.data().length - 1) {
                            return;
                        }
                        var dataDown = dt.row({ selected: true }).data();
                        var dataUp = dt.row(index + 1).data();

                        dt.row(index).data(dataUp);
                        dt.row(index + 1).data(dataDown);

                        dt.row(index + 1).select();
                        dt.row(index).deselect();

                        dt.draw();
                    }
                }
            ]
        };

        if (this.schedulerTaskId == undefined) {
            $('#updateTaskBtn').hide();
        }
        else
        {
            $('#addTaskBtn').hide();
            (await this.schedulerTaskService.GetSchedulerTaskById(this.schedulerTaskId)).subscribe({
                next: async (data: AddSchedulerTask) => {
                    this.SchedulerTaskName = data.SchedulerTaskName;
                    this.StartupType = data.StartupType;
                    this.TimeBetweenLaunchesMinutes = data.TimeBetweenLaunchesMinutes;
                    this.schedulerTaskId = data.Id;

                    var taskSchedulerTable = $('#SchedulerTable').DataTable();
                    // Ожидание загрузки данных в таблицу (server side)
                    while($('#TaskTable').DataTable().data().length == 0){
                        await new Promise(f => setTimeout(f, 100));
                    }

                    var taskTable = $('#TaskTable').DataTable();
                    var taskTableData = taskTable.data();
                    data.TaskIds.forEach(taskId => {
                        for (let index = 0; index < taskTableData.length; index++) {
                            const element = taskTableData[index];
                            if (element['Id'] == taskId) {
                                var row = {
                                    Id: element['Id'],
                                    ClientTaskName: element['ClientTaskName'],
                                    TaskType: element['TaskType'],
                                }
                                taskSchedulerTable.row.add(row);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    });
                    taskSchedulerTable.draw();
                }
            })
        }
        function AddToScheduler(data: string[]) {
            var table = $('#SchedulerTable').DataTable();
            var rowsToAdd = Array<Object>();

            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var row = {
                    Id: element['Id'],
                    ClientTaskName: element['ClientTaskName'],
                    TaskType: element['TaskType'],
                }
                rowsToAdd.push(row);
            }

            table.rows.add(rowsToAdd).draw();
        }
    }

    public async AddSchedulerTask() {
        var schedulerTask = new AddSchedulerTask();
        schedulerTask.TimeBetweenLaunchesMinutes = this.TimeBetweenLaunchesMinutes;
        schedulerTask.StartupType = this.StartupType;
        schedulerTask.SchedulerTaskName = this.SchedulerTaskName;

        var tableData = $('#SchedulerTable').DataTable().data();
        for (let index = 0; index < tableData.length; index++) {
            const element = tableData[index];
            schedulerTask.TaskIds.push(element['Id'])
        }
        (await this.schedulerTaskService.Add(schedulerTask)).subscribe({
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
            error: (data: any) => {
                $("#errorNot").show(200);
                $("#errorNot").delay(400).hide(200);
            }
        })
    }

    public async UpdateSchedulerTask()
    {
        var updatedSchedulerTask = new UpdateSchedulerTaskDTO();
        updatedSchedulerTask.Id = this.schedulerTaskId;
        updatedSchedulerTask.TimeBetweenLaunchesMinutes = this.TimeBetweenLaunchesMinutes;
        var tableData = $('#SchedulerTable').DataTable().data();
        for (let index = 0; index < tableData.length; index++) {
            const element = tableData[index];
            updatedSchedulerTask.TaskIds.push(element['Id'])
        }

        (await this.schedulerTaskService.UpdateSchedulerTask(updatedSchedulerTask)).subscribe({
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
            error: (data: any) => {
                $("#errorNot").show(200);
                $("#errorNot").delay(400).hide(200);
            }
        })
    }
}

export class AddSchedulerTask {
    Id: number;
    StartupType: string;
    TimeBetweenLaunchesMinutes: number;
    SchedulerTaskName: string;
    TaskIds: number[] = new Array<number>;
}

export class UpdateSchedulerTaskDTO
{
    Id: number;
    TimeBetweenLaunchesMinutes: number;
    TaskIds: number[] = new Array<number>;
}