import { Component, OnInit } from '@angular/core';
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { FbAccountService } from '../../Services/FBAccountService';
import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/Services/DataService'
@Component({
    selector: 'fbaccounts',
    templateUrl: 'fbaccounts.component.html'
})

export class FbAccountComponent implements OnInit {
    fbAccGroups: string[];
    dtOptions: any;
    fbAccData: string;
    fbAccountService: FbAccountService;
    DataService: DataService

    constructor(fbAccountService: FbAccountService, DataService: DataService) {
        this.fbAccountService = fbAccountService;
        this.DataService = DataService;
    }

    async ngOnInit() {
        $('#accountForm').hide();
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + 'fbaccount/get',
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
            var deletedAccounts = new Array<FBAccountDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new FBAccountDTO(element["Id"], element["Login"], element["Password"], element["Group"]);
                deletedAccounts.push(newItem);
            }
            await FbAccountService.DeleteAccounts(deletedAccounts).subscribe({
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
        FbAccountService.GetFBAccGroups().subscribe((data: any) => {
            this.fbAccGroups = data;
        })
    }
    async AddAccounts() {
        var accountArray = this.fbAccData.split('\n');
        var addedAccount = new Array<FBAccountDTO>();
        accountArray.forEach(element => {
            var elements = element.split(":");
            var newItem = new FBAccountDTO('0', elements[0], elements[1], $("#groupAddModal").val().toString());
            addedAccount.push(newItem);
        });

        (await this.fbAccountService.AddAccounts(addedAccount)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(500);
                    $("#accountForm").hide(500);
                    $("#successNot").delay(1500).hide(500);
                }
                else {
                    $("#errorNot").show(500);
                    $("#accountForm").hide(500);
                    $("#errorNot").delay(1500).hide(500);
                }
            },
            error: error => {
                $("#errorNot").show(500);
                $("#accountForm").hide(500);
                $("#errorNot").delay(1500).hide(500);
            }
        });

    }

    async UpdateAccount() {
        var updatedArray = new Array<FBAccountDTO>;
        var updatedAccount = new FBAccountDTO($("#idModal").val().toString(), $("#loginModal").val().toString(), $("#passwordModal").val().toString(), $("#groupUpdateModal").val().toString());
        updatedArray.push(updatedAccount);
        await (await FbAccountService.UpdateAccounts(updatedArray)).subscribe({
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


export class FBAccountDTO {
    constructor(id: string, login: string, password: string, group: string) {
        this.id = id;
        this.Login = login;
        this.Password = password;
        this.Group = group;
    }
    public id: string;
    public Login: string;
    public Password: string;
    public Group: string;
}
