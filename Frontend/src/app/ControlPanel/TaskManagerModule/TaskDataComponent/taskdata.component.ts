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
        $('#TaskForm').hide();
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
                title: 'Status',
                data: 'Status'
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
                    text: 'Delete selected',
                    action: function (e, dt, node, config) {
                        Delete(dt.rows({ selected: true }).data(), dt);
                    }
                },
                {
                    text: 'Update selected',
                    action: function (e, dt, node, config) {
                        ShowUpdateModal(dt.rows({ selected: true }).data(), dt);
                    }
                },
                {
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                }
            ]
        };
        async function Delete(data: string[], dt: any) {
            // TODO logic deleting
        }

        async function ShowUpdateModal(data: string[], dt: any) {

            var element = data[0];
            $("#idModal").val(element["Id"]);
            $("#servicenameModal").val(element["ServiceName"]);
            $("#apiuriModal").val(element["APIURI"]);
            $("#apikeyModal").val(element["APIKey"]);
            new bootstrap.Modal("#TaskModal").show();
        }
    }

    async UpdateTask() {
         // TODO logic deleting
    }
}

export class TaskDTO {
    constructor(id: string, Status: string, UsefulData: string, AccountGroup: string, ProxyGroup: string) {
        this.id = id;
        this.Status = Status;
        this.UsefulData = UsefulData;
        this.AccountGroup = AccountGroup;
        this.ProxyGroup = ProxyGroup;
    }
    id: string;
    Status: string;
    UsefulData: string;
    AccountGroup: string;
    ProxyGroup: string;
}