import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { AdvertPostService } from '../../../../Services/AdvertPostService'

@Component({
    selector: 'advert-post-group',
    templateUrl: 'advertpostgroup.component.html'
})

export class AdvertPostGroupComponent implements OnInit {
    dtOptions: any;
    newGroup: string;
    private static router: Router;
    private static advertPostService: AdvertPostService;
    constructor(router: Router, advertPostService: AdvertPostService) {
        AdvertPostGroupComponent.router = router;
        AdvertPostGroupComponent.advertPostService = advertPostService;
    }

    ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#postGroupForm').hide();

        this.dtOptions = {
            ajax: environment.apiUrl + '/api/AdvertPostGroup/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Name',
                data: 'Name'
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
                    text: 'Add group',
                    key: '1',
                    action: function (e, dt, node, config) {
                        $('#postGroupForm').show(500);
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
                    text: 'View posts by Group',
                    action: function (e, dt, node, config) {
                        ViewPosts(dt.rows({ selected: true }).data()[0]["Id"]);
                    }
                }
            ]
        };

        function ViewPosts(id: number) {
            AdvertPostGroupComponent.router.navigate(['advertresourses/post/manager/' + id]);
        }

        async function Delete(data: string[], dt: any) {
            var deletedGroups = new Array<DeleteAdvertPostGroupDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new DeleteAdvertPostGroupDTO();
                newItem.Id = element["Id"]
                deletedGroups.push(newItem);
            }
            (await AdvertPostService.DeleteGroups(deletedGroups)).subscribe({
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
        var addedGroup = new AddAdvertPostGroupDTO();
        addedGroup.Name = this.newGroup;

        (await AdvertPostGroupComponent.advertPostService.AddGroups(addedGroup)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#successNot").delay(400).hide(200);
                    $("#postGroupForm").hide(500);
                }
                else {
                    $("#errorNot").show(200);
                    $("#errorNot").delay(400).hide(200);
                    $("#postGroupForm").hide(500);
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

export class AddAdvertPostGroupDTO {
    Name: string;
}

export class DeleteAdvertPostGroupDTO {
    Id: number;
}