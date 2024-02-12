import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment'
import { ClonGroupService } from 'src/app/Services/ClonGroupService'
import { Router } from '@angular/router';
@Component({
    selector: 'clon-group',
    templateUrl: 'clongroup.component.html'
})

export class ClonGroupComponent implements OnInit {
    dtOptions: any;
    neGroupName: string;
    static router: Router;
    clonGroupService: ClonGroupService;

    constructor(clonGroupService: ClonGroupService, router: Router) { 
        this.clonGroupService = clonGroupService;
        ClonGroupComponent.router = router;
    }

    ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#addGroupForm').hide();

        this.dtOptions = {
            ajax: environment.apiUrl + '/api/ClonGroup/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Name',
                data: 'Name'
            }, {
                title: 'Count pinned clones',
                data: 'CountPinnedClones'
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
                    text: 'Add Group',
                    key: '1',
                    action: function (e, dt, node, config) {
                        if ($('#addGroupForm').is(':visible')) {
                            $('#addGroupForm').hide(500);
                        }
                        else {
                            $('#addGroupForm').show(500);
                        }
                    }
                },
                {
                    text: 'View clon by group',
                    action: function (e, dt, node, config) {
                        ClonGroupComponent.router.navigate(["/clone/manager/" + dt.rows({ selected: true }).data()[0]["Id"]])
                    }
                }
            ]
        };
    }

    public async AddGroup() {
        var newGroup = new AddClonGroupDTO();
        newGroup.Name = this.neGroupName;

        (await this.clonGroupService.AddClonGroup(newGroup)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#successNot").delay(400).hide(200);
                    $('#addGroupForm').hide(500);
                }
                else {
                    $("#errorNot").show(200);
                    $("#errorNot").delay(400).hide(200);
                    $('#addGroupForm').hide(500);
                }
            },
            error: (error) => {
                $("#errorNot").val(error);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
                $('#addGroupForm').hide(500);
            }
        })
    }
}

export class AddClonGroupDTO {
    Name: string;
}