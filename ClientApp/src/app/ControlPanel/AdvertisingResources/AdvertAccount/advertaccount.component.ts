import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { AdvertAccountService } from '../../../Services/AdvertAccountService'

@Component({
    selector: 'advert-account',
    templateUrl: 'advertaccount.component.html'
})

export class AdvertAccountComponent implements OnInit {
    dtOptions: any;
    newGroup: string;
    URIPath: string;
    accountData: string;
    groupNames: string[];

    private static router: Router;
    private static AdvertAccountService: AdvertAccountService;
    constructor(router: Router, advertAccountService: AdvertAccountService, activateRoute: ActivatedRoute) {
        AdvertAccountComponent.router = router;
        AdvertAccountComponent.AdvertAccountService = advertAccountService;
        var groupId = activateRoute.snapshot.params["groupId"];
        if (groupId == undefined) {
            this.URIPath = environment.apiUrl + '/api/AdvertAccount/get'
        }
        else {
            this.URIPath = environment.apiUrl + '/api/AdvertAccount/get/' + groupId
        }
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#addAdvertAccountForm').hide();

        this.dtOptions = {
            ajax: this.URIPath,
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Account URL',
                data: 'AccountURL'
            }, {
                title: 'Account Status',
                data: 'AdvertAccountStatus'
            }, {
                title: 'Group',
                data: 'AdvertAccountGroupName'
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
                    text: 'Add account',
                    key: '1',
                    action: function (e, dt, node, config) {
                        Add();
                    }
                },
                {
                    text: 'Delete selected',
                    action: async function (e, dt, node, config) {
                        await Delete(dt.rows({ selected: true }).data(), dt);
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

        function Add() {
            $("#addAdvertAccountForm").show(500);
        }

        async function Delete(data: string[], dt: any) {
            var deletedAccounts = new Array<DeleteAdvertAccountDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new DeleteAdvertAccountDTO();
                newItem.Id = element["Id"]
                deletedAccounts.push(newItem);
            }
            (await AdvertAccountService.DeleteAccounts(deletedAccounts)).subscribe({
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

        (await AdvertAccountComponent.AdvertAccountService.GetGroupNames()).subscribe({
            next: (data: string[]) => {
                this.groupNames = data;
            }
        })
    }

    public async AddAccount() {
        var accountArray = this.accountData.split('\n');
        var accountToAdd = new Array<AddAdvertAccountDTO>();
        accountArray.forEach(element => {
            var newAccount = new AddAdvertAccountDTO();
            newAccount.AccountURL = element;
            newAccount.AdvertAccountGroupName = $("#selectAdvertGroup").val().toString()
            accountToAdd.push(newAccount);
        });

        (await AdvertAccountComponent.AdvertAccountService.AddAccounts(accountToAdd)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#successNot").delay(400).hide(200);
                    $("#addAdvertAccountForm").hide(500);
                }
                else {
                    $("#errorNot").show(200);
                    $("#errorNot").delay(400).hide(200);
                    $("#addAdvertAccountForm").hide(500);
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

export class AddAdvertAccountDTO {
    AccountURL: string;
    AdvertAccountGroupName: string;
}

export class DeleteAdvertAccountDTO {
    Id: number;
}