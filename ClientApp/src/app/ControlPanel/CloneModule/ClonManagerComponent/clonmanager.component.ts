import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment'
import { ActivatedRoute, Router } from '@angular/router';
import { ClonService } from 'src/app/Services/ClonService';

@Component({
    selector: 'clon-manager',
    templateUrl: 'clonmanager.component.html'
})

export class ClonManagerComponent implements OnInit {
    dtOptions: any;
    URIPath: string;
    static router: Router;
    ClonService: ClonService;
    constructor(activateRoute: ActivatedRoute, router: Router, ClonService: ClonService) {
        var groupId = activateRoute.snapshot.params["groupId"];
        if (groupId == undefined) {
            this.URIPath = environment.apiUrl + '/api/clon/get'
        }
        else {
            this.URIPath = environment.apiUrl + '/api/clon/get/' + groupId
        }
        ClonManagerComponent.router = router;
        this.ClonService = ClonService;
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: this.URIPath,
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Clon status',
                data: 'ClonStatus'
            }, {
                title: 'Group name',
                data: 'GroupName'
            }, {
                title: 'Count pinned account',
                data: 'CountPinnedAccount'
            }, {
                title: 'Count pinned posts',
                data: 'CountPinnedPosts'
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
                    text: 'View clon data',
                    action: function (e, dt, node, config) {
                        ClonManagerComponent.router.navigate(["/clone/manager/view/" + dt.rows({ selected: true }).data()[0]["Id"]])
                    }
                },
                {
                    text: 'Delete selected',
                    action: async function (e, dt, node, config) {
                        var ids = new Array<number>();
                        for (let index = 0; index < dt.rows({ selected: true }).data().length; index++) {
                            const element = dt.rows({ selected: true }).data()[index];
                            ids.push(element["Id"])
                        }
                        (await ClonService.DeleteClones(ids)).subscribe({
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
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                }
            ]
        };
    }
}