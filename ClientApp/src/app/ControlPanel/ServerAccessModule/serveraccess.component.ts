import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { WorkerServerService } from '../../Services/WorkerServerService'

@Component({
    selector: 'server-accesss',
    templateUrl: 'serveraccess.component.html'
})

export class ServerAccessComponent implements OnInit {
    dtOptions: any;
    WorkerServerService: WorkerServerService;
    addedServer: AddWorkerServerDTO;

    constructor(WorkerServerService: WorkerServerService) {
        this.WorkerServerService = WorkerServerService;
        this.addedServer = new AddWorkerServerDTO();
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#addServerForm').hide();

        this.dtOptions = {
            ajax: environment.apiUrl + '/api/WorkerServer/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Name',
                data: 'Name'
            }, {
                title: 'API Key',
                data: 'APIKey'
            }, {
                title: 'Server status',
                data: 'WorkerServerStatus'
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
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                },
                {
                    text: 'Delete selected',
                    action: async function (e, dt, node, config) {
                        var iDs = new Array<number>();
                        for (let index = 0; index < dt.rows({ selected: true }).data().length; index++) {
                            const element = dt.rows({ selected: true }).data()[index];
                            iDs.push(element['Id']);
                        }
                        (await WorkerServerService.DeleteWorkerServers(iDs)).subscribe({
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
                            error: (error) => {
                                $("#errorNot").val(error);
                                $("#errorNot").show(200);
                                $("#errorNot").delay(2000).hide(200);
                                $("#errorNot").val("Error");
                            }
                        })
                    }
                },
                {
                    text: 'Add',
                    action: function (e, dt, node, config) {
                        if ($('#addServerForm').is(':visible')) {
                            $('#addServerForm').hide(500);
                        }
                        else {
                            $('#addServerForm').show(500);
                        }
                    }
                },
                {
                    text: 'Change status',
                    action: async function (e, dt, node, config) {
                        var Ids = new Array<number>();
                        for (let index = 0; index < dt.rows({ selected: true }).data().length; index++) {
                            const element = dt.rows({ selected: true }).data()[index];
                            Ids.push(element['Id']);
                        }

                        (await WorkerServerService.ChangeStatus(Ids)).subscribe({
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
                            error: (error) => {
                                $("#errorNot").val(error);
                                $("#errorNot").show(200);
                                $("#errorNot").delay(2000).hide(200);
                                $("#errorNot").val("Error");
                            }
                        })
                        
                    }
                }
            ]
        };
    }

    async AddServer() {
        if (this.addedServer.Name != undefined && this.addedServer.APIKey != undefined && this.addedServer.WorkerServerStatus != undefined) {
            (await this.WorkerServerService.AddWorkerServer(this.addedServer)).subscribe({
                next: (data: boolean) => {
                    if (data) {
                        $('#addServerForm').hide(500);
                        $("#successNot").show(200);
                        $("#successNot").delay(400).hide(200);
                    }
                    else {
                        $("#errorNot").show(200);
                        $("#errorNot").delay(400).hide(200);
                    }
                },
                error: (error) => {
                    $("#errorNot").val(error);
                    $("#errorNot").show(200);
                    $("#errorNot").delay(2000).hide(200);
                    $("#errorNot").val("Error");
                }
            })
        }
        else {
            $("#errorNot").show(200);
            $("#errorNot").delay(2000).hide(200);
        }
    }
}

export class AddWorkerServerDTO {
    Name: string;
    APIKey: string;
    WorkerServerStatus: string;
}