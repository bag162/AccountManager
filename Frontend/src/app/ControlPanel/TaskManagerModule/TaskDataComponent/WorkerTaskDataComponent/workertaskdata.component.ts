import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment'
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'workertaskdata',
    templateUrl: 'workertaskdata.component.html'
})

export class WorkerTaskDataComponent implements OnInit {
    dtOptions: any;
    TaskData: string;
    taskId: number;
    constructor(private activateRoute: ActivatedRoute) {
        this.taskId = activateRoute.snapshot.params["id"];
    }

    ngOnInit() {
        $('#TaskForm').hide();
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + 'taskmanager/get/' + this.taskId,
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'WorkerId',
                data: 'WorkerId'
            }, {
                title: 'InstanceId',
                data: 'InstanceId'
            }, {
                title: 'Status',
                data: 'Status'
            }, {
                title: 'InstAccountLogin',
                data: 'InstAccountLogin'
            }, {
                title: 'ProxyId',
                data: 'ProxyId'
            }, {
                title: 'UsefulData',
                data: 'UsefulData'
            }, {
                title: 'ErrorMessage',
                data: 'ErrorMessage'
            }],
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
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                }
            ]
        };
    }
}

export class WorkerTaskDTO {
    constructor(id: number, Status: string, UsefulData: string, WorkerId: number, InstanceId: number, InstAccountLogin: string, ProxyId: number, ErrorMessage: string) {
        this.id = id;
        this.Status = Status;
        this.UsefulData = UsefulData;
        this.ErrorMessage = ErrorMessage;
        this.InstAccountLogin = InstAccountLogin;
        this.InstanceId = InstanceId;
        this.ProxyId = ProxyId;
        this.WorkerId = WorkerId;
    }

    id: number;
    WorkerId: number;
    InstanceId: number;
    Status: string;
    InstAccountLogin: string;
    ProxyId: number;
    UsefulData: string;
    ErrorMessage: string;
}