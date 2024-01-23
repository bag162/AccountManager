import { Component, OnInit } from '@angular/core';
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { InstAccountService } from 'src/app/Services/InstAccountService';
import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/Services/DataService'
@Component({
    selector: 'Instaccounts',
    templateUrl: 'instaccounts.component.html'
})

export class InstAccountComponent implements OnInit {
    InstAccGroups: string[];
    dtOptions: any;
    InstAccData: string;
    InstAccountService: InstAccountService;
    DataService: DataService

    constructor(InstAccountService: InstAccountService, DataService: DataService) {
        this.InstAccountService = InstAccountService;
        this.DataService = DataService;
    }

    async ngOnInit() {
        $('#accountForm').hide();
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + '/api/Instaccount/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            // autoFill: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Login',
                data: 'Login'
            }, {
                title: 'Password',
                data: 'Password'
            }, {
                title: 'Group',
                data: 'Group'
            }, {
                title: 'Account status',
                data: 'AccountStatus'
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
                    text: 'Add accounts',
                    key: '1',
                    action: function (e, dt, node, config) {
                        if ($('#accountForm').is(':visible')) {
                            $('#accountForm').hide(500);
                        }
                        else {
                            $('#accountForm').show(500);
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
        this.DataService.subscriber$.subscribe(data => {
            this.LoadGroupData();
          });
        await this.LoadGroupData();

        async function Delete(data: string[], dt: any) {
            var deletedAccounts = new Array<InstAccountDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new InstAccountDTO(element["Id"], element["Login"], element["Password"], element["Group"], element["AccountStatus"]);
                deletedAccounts.push(newItem);
            }
            await InstAccountService.DeleteAccounts(deletedAccounts).subscribe({
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
            $("#loginModal").val(element["Login"]);
            $("#passwordModal").val(element["Password"]);
            new bootstrap.Modal("#accountModal").show();
        }
    }
    async LoadGroupData() {
        InstAccountService.GetInstAccGroups().subscribe((data: any) => {
            this.InstAccGroups = data;
        })
    }
    async AddAccounts() {
        var accountArray = this.InstAccData.split('\n');
        var addedAccount = new Array<InstAccountDTO>();
        accountArray.forEach(element => {
            var elements = element.split(":");
            var newItem = new InstAccountDTO('0', elements[0], elements[1], $("#groupAddModal").val().toString(), "NotAuthorized");
            addedAccount.push(newItem);
        });

        (await this.InstAccountService.AddAccounts(addedAccount)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#accountForm").hide(200);
                    $("#successNot").delay(400).hide(200);
                }
                else {
                    $("#errorNot").show(200);
                    $("#accountForm").hide(200);
                    $("#errorNot").delay(400).hide(200);
                }
            },
            error: error => {
                $("#errorNot").show(200);
                $("#accountForm").hide(200);
                $("#errorNot").delay(400).hide(200);
            }
        });

    }

    async UpdateAccount() {
        var updatedArray = new Array<InstAccountDTO>;
        var updatedAccount = new InstAccountDTO($("#idModal").val().toString(), $("#loginModal").val().toString(), $("#passwordModal").val().toString(), $("#groupUpdateModal").val().toString(), "0");
        updatedArray.push(updatedAccount);
        await (await InstAccountService.UpdateAccounts(updatedArray)).subscribe({
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


export class InstAccountDTO {
    constructor(id: string, login: string, password: string, group: string, AccountStatus: string) {
        this.id = id;
        this.Login = login;
        this.Password = password;
        this.Group = group;
        this.AccountStatus = AccountStatus;
    }
    public id: string;
    public Login: string;
    public Password: string;
    public Group: string;
    public AccountStatus: string;
}
