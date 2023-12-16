import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { SMSActivationService } from '../../Services/SMSActivationService';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'smsservice',
    templateUrl: 'smsservice.component.html'
})

export class SMSServiceComponent implements OnInit {
    dtOptions: any;
    smsserviceData: string;
    SMSActivationService: SMSActivationService;

    constructor(SMSActivationService: SMSActivationService) {
        this.SMSActivationService = SMSActivationService;
    }

    ngOnInit() {
        $('#smsserviceForm').hide();
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + 'smsservice/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            // autoFill: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Service Name',
                data: 'ServiceName'
            }, {
                title: 'API URI',
                data: 'APIURI'
            }, {
                title: 'API Key',
                data: 'APIKey'
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
                    text: 'Add SMS Service',
                    key: '1',
                    action: function (e, dt, node, config) {
                        if ($('#smsserviceForm').is(':visible')) {
                            $('#smsserviceForm').hide(500);
                        }
                        else {
                            $('#smsserviceForm').show(500);
                        }
                    }
                },
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
            var deletedsmsservice = new Array<SMSServiceDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new SMSServiceDTO(element["Id"], element["ServiceName"], element["APIURI"], element["APIKey"]);
                deletedsmsservice.push(newItem);
            }
            await SMSActivationService.DeleteSMSService(deletedsmsservice).subscribe({
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
                error: error => {
                    $("#errorNot").val(error);
                    $("#errorNot").show(200);
                    $("#errorNot").delay(2000).hide(200);
                    $("#errorNot").val("Error");
                }
            })
        }

        async function ShowUpdateModal(data: string[], dt: any) {

            var element = data[0];
            $("#idModal").val(element["Id"]);
            $("#servicenameModal").val(element["ServiceName"]);
            $("#apiuriModal").val(element["APIURI"]);
            $("#apikeyModal").val(element["APIKey"]);
            new bootstrap.Modal("#smsserviceModal").show();
        }
    }

    async AddSMSService() {
        var smsserviceArray = this.smsserviceData.split('\n');
        var addedsmsservice = new Array<SMSServiceDTO>();
        smsserviceArray.forEach(element => {
            var elements = element.split(":");
            var newItem = new SMSServiceDTO('0', elements[0], elements[1], elements[2]);
            addedsmsservice.push(newItem);
        });

        (await this.SMSActivationService.AddSMSService(addedsmsservice)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(500);
                    $("#smsserviceForm").hide(500);
                    $("#successNot").delay(1500).hide(500);
                }
                else {
                    $("#errorNot").show(500);
                    $("#smsserviceForm").hide(500);
                    $("#errorNot").delay(1500).hide(500);
                }
            },
            error: error => {
                $("#errorNot").val(error);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
            }
        });

    }

    async UpdateSMSService() {
        var updatedArray = new Array<SMSServiceDTO>;
        var updatedsmsservice = new SMSServiceDTO($("#idModal").val().toString(), $("#servicenameModal").val().toString(), $("#apiuriModal").val().toString(), $("#apikeyModal").val().toString());
        updatedArray.push(updatedsmsservice);
        await (await SMSActivationService.UpdateSMSService(updatedArray)).subscribe({
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
            error: (error: any) => {
                $("#errorNot").val(error);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
            }
        })
    }
}


export class SMSServiceDTO {
    constructor(id: string, ServiceName: string, APIURI: string, APIKey: string) {
        this.id = id;
        this.ServiceName = ServiceName;
        this.APIURI = APIURI;
        this.APIKey = APIKey;
    }
    public id: string;
    public ServiceName: string;
    public APIURI: string;
    public APIKey: string;
}
