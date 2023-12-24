import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { SMSActivationService } from '../../Services/SMSActivationService';
import { environment } from 'src/environments/environment';
import { EmailService } from '../../Services/EmailService';

@Component({
    selector: 'email',
    templateUrl: 'email.component.html'
})

export class EmailComponent implements OnInit {
    dtOptions: any;
    emailData: string;
    EmailService: EmailService;

    constructor(EmailService: EmailService) {
        this.EmailService = EmailService;
    }

    ngOnInit() {
        $('#emailForm').hide();
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + 'email/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            // autoFill: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Name',
                data: 'Name'
            }, {
                title: 'Email Type',
                data: 'EmailType'
            }, {
                title: 'API Token',
                data: 'APIToken'
            }, {
                title: 'Mail Domain',
                data: 'MailDomain'
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
                    text: 'Add Email Service',
                    key: '1',
                    action: function (e, dt, node, config) {
                        if ($('#emailForm').is(':visible')) {
                            $('#emailForm').hide(500);
                        }
                        else {
                            $('#emailForm').show(500);
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
            var deletedemail = new Array<EmailDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new EmailDTO(element["Id"], element["Name"], element["EmailType"], element["APIToken"], element["MailDomain"]);
                deletedemail.push(newItem);
            }
            await EmailService.DeleteEmail(deletedemail).subscribe({
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
            $("#nameModal").val(element["Name"]);
            $("#apitokenModal").val(element["APIToken"]);
            $("#maildomainModal").val(element["MailDomain"]);
            $("#emailtypeModal").val("kopeechkaStore");
            
            new bootstrap.Modal("#emailModal").show();
        }
    }

    async AddEmail() {
        var emailArray = this.emailData.split('\n');
        var addedemail = new Array<EmailDTO>();
        emailArray.forEach(element => {
            var elements = element.split(":");
            var newItem = new EmailDTO('0', elements[0], "kopeechkaStore", elements[1], elements[2]);
            console.log(newItem)
            addedemail.push(newItem);
        });

        (await this.EmailService.AddEmail(addedemail)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(500);
                    $("#emailForm").hide(500);
                    $("#successNot").delay(1500).hide(500);
                }
                else {
                    $("#errorNot").show(500);
                    $("#emailForm").hide(500);
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

    async UpdateEmail() {
        var updatedArray = new Array<EmailDTO>;
        var updatedemail = new EmailDTO($("#idModal").val().toString(), $("#nameModal").val().toString(), "kopeechkaStore", $("#apitokenModal").val().toString(), $("#maildomainModal").val().toString());
        updatedArray.push(updatedemail);
        await (await EmailService.UpdateEmail(updatedArray)).subscribe({
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


export class EmailDTO {
    constructor(id: string, Name: string, EmailType: string, APIToken: string, MailDomain: string) {
        this.id = id;
        this.Name = Name;
        this.EmailType = EmailType;
        this.APIToken = APIToken;
        this.MailDomain = MailDomain;
    }
    public id: string;
    public Name: string;
    public EmailType: string;
    public APIToken: string;
    public MailDomain: string;
}
