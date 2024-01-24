import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { AdvertAccountService } from '../../../../Services/AdvertAccountService'
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'advert-account-group',
    templateUrl: 'advertaccountgroup.component.html'
})

export class AdvertAccountGroupComponent implements OnInit {
    dtOptions: any;
    newGroup: string;
    private static router: Router;
    private static AdvertAccountService: AdvertAccountService;
    constructor(router: Router, advertAccountService: AdvertAccountService) {
        AdvertAccountGroupComponent.router = router;
        AdvertAccountGroupComponent.AdvertAccountService = advertAccountService;
    }

    ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#accountGroupForm').hide();

        this.dtOptions = {
            ajax: environment.apiUrl + '/api/AdvertAccountGroup/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Name',
                data: 'Name'
            }, {
                title: 'Count pinned accounts',
                data: 'CountPinnedAccounts'
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
                    text: 'Add group',
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
                },
                {
                    text: 'View accounts by Group',
                    action: function (e, dt, node, config) {
                        ViewAccounts(dt.rows({ selected: true }).data()[0]["Id"]);
                    }
                }
            ]
        };
        function Add() {
            $('#accountGroupForm').show(500);
        }

        function ViewAccounts(id: number) {
            AdvertAccountGroupComponent.router.navigate(['advertresourses/account/manager/' + id]);
        }

        async function Delete(data: string[], dt: any) {
            var deletedGroups = new Array<DeleteAdvertAccountGroupDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new DeleteAdvertAccountGroupDTO();
                newItem.Id = element["Id"]
                deletedGroups.push(newItem);
            }
            (await AdvertAccountService.DeleteGroups(deletedGroups)).subscribe({
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

    public async AddGroup() {
        var addedGroup = new AddAdvertAccountGroupDTO();
        addedGroup.Name = this.newGroup;

        (await AdvertAccountGroupComponent.AdvertAccountService.AddGroups(addedGroup)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#successNot").delay(400).hide(200);
                    $("#accountGroupForm").hide(500);
                }
                else {
                    $("#errorNot").show(200);
                    $("#errorNot").delay(400).hide(200);
                    $("#accountGroupForm").hide(500);
                }
            },
            error: (error: HttpErrorResponse) => {
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
            }
        })
    }
}

export class AddAdvertAccountGroupDTO {
    Name: string;
}

export class DeleteAdvertAccountGroupDTO {
    Id: number;
}