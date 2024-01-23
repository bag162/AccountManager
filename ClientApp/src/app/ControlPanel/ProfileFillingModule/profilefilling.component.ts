import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { ProfileFillingService } from 'src/app/Services/ProfileFillingService';

@Component({
    selector: 'profile-filling',
    templateUrl: 'profilefilling.component.html'
})

export class ProfileFillingComponent implements OnInit {
    dtOptions: any;
    profileFillingService: ProfileFillingService;
    static router: Router;

    constructor(router: Router, profileFillingService: ProfileFillingService) { 
        ProfileFillingComponent.router = router;
        this.profileFillingService = profileFillingService;
    }

    ngOnInit() { 
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + '/api/ProfileFilling/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Filling name',
                data: 'Name'
            }, {
                title: 'Count linked accounts',
                data: 'CountLinkedAccounts'
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
                    text: 'Add filling data',
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
                    text: 'View selected filling data',
                    action: function (e, dt, node, config) {
                        View(dt.rows({selected: true}).data()[0]["Id"]);
                    }
                }
            ]
        };
        function Add()
        {
            ProfileFillingComponent.router.navigate(['profilefilling/add']);
        }
        function View(id: number)
        {
            ProfileFillingComponent.router.navigate(['profilefilling/view/' + id]);
        }

        async function Delete(data: string[], dt: any)
        {
            var deletedFillingData = new Array<ProfileFillingTableDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new ProfileFillingTableDTO();
                newItem.Id = element["Id"]
                newItem.CountLinkedAccounts = element["CountLinkedAccounts"]
                newItem.Name = element["Name"]


                deletedFillingData.push(newItem);
            }
            (await ProfileFillingService.DeleteProfileFillingData(deletedFillingData)).subscribe({
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
}

export class ProfileFillingTableDTO
{
    Id: number;
    Name: string;
    CountLinkedAccounts: number;
}