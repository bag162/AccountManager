import { Component, OnInit } from '@angular/core';
import { PostCommentGroupService } from "../../../../Services/PostCommentGroupService"
import { environment } from 'src/environments/environment';


@Component({
    selector: 'post-comment-group',
    templateUrl: 'postcommentgroup.component.html'
})

export class PostCommentGroupComponent implements OnInit {
    postGroupService: PostCommentGroupService;
    dtOptions: any;
    newGroup: string;

    constructor(postGroupService: PostCommentGroupService) {
        this.postGroupService = postGroupService;
    }

    ngOnInit() {
        $("#addGroupForm").hide();
        $('#successNot').hide();
        $('#errorNot').hide();

        this.dtOptions = {
            ajax: environment.apiUrl + '/api/PostCommentGroup/get',
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
                title: 'Count comments',
                data: 'CountComments'
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
                        if ($('#addGroupForm').is(':visible')) {
                            $('#addGroupForm').hide(500);
                        }
                        else {
                            $('#addGroupForm').show(500);
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
                    text: 'Reload data',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                },
                {
                    text: 'View posts by selected Group',
                    action: function (e, dt, node, config) {
                        RedirectToCommentManagerByGroup(dt.rows({ selected: true }).data()[0]);
                    }
                }
            ]
        };
        function RedirectToCommentManagerByGroup(group: any)
        {
            $(location).attr('href', window.location.origin.toString() + "/post/comment/manager/" + group["Id"]);
        }

        async function Delete(data: string[], dt: any) {
            var deletedGroups = new Array<CRUDCommentGroupDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new CRUDCommentGroupDTO();
                newItem.id = element["Id"];
                newItem.Name = element["Name"];
                deletedGroups.push(newItem);
            }

            (await PostCommentGroupService.DeleteGroup(deletedGroups)).subscribe({
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
    }

    public async AddGroup() {
        var newGroup = new CRUDCommentGroupDTO();
        newGroup.Name = this.newGroup;
        (await this.postGroupService.AddGroup(newGroup)).subscribe({
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
}

export class CRUDCommentGroupDTO {
    id?: string;
    Name: string;
}