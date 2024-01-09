import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostCommentService } from 'src/app/Services/PostCommentService';
import { PostCommentGroupService } from 'src/app/Services/PostCommentGroupService';
import { environment } from 'src/environments/environment';


@Component({
    selector: 'post-comment-manager',
    templateUrl: 'postcomment.component.html'
})

export class PostCommentComponent implements OnInit {
    dtOptions: any;
    URIPath: string;
    groupNames: string[];
    newCommentsData: string;
    PostCommentGroupService: PostCommentGroupService;
    PostCommentService: PostCommentService;

    constructor(activateRoute: ActivatedRoute, PostCommentGroupService: PostCommentGroupService, PostCommentService: PostCommentService) {
        this.PostCommentService = PostCommentService;
        this.PostCommentGroupService = PostCommentGroupService;
        var groupId = activateRoute.snapshot.params["groupId"];
        if (groupId == undefined) {
            this.URIPath = environment.apiUrl + '/api/PostComment/get'
        }
        else {
            this.URIPath = environment.apiUrl + '/api/PostComment/get/' + groupId
        }
    }

    async ngOnInit() {
        $("#addCommentsFormGroup").hide();
        $('#successNot').hide();
        $('#errorNot').hide();

        this.dtOptions = {
            ajax: this.URIPath,
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            // autoFill: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Message',
                data: 'Message'
            }, {
                title: 'Comment group name',
                data: 'CommentGroupName'
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
                    text: 'Add comments',
                    key: '1',
                    action: function (e, dt, node, config) {
                        if ($('#addCommentsFormGroup').is(':visible')) {
                            $('#addCommentsFormGroup').hide(500);
                        }
                        else {
                            $('#addCommentsFormGroup').show(500);
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
                }
            ]
        };

        (await this.PostCommentGroupService.GetGroupNames()).subscribe({
            next: (data: string[]) => {
                this.groupNames = data;
            }
        })

        async function Delete(data: string[], dt: any) {
            var deletedComments: CRUDCommentGroupDTO[] = new Array<CRUDCommentGroupDTO>;

            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var pushedData = new CRUDCommentGroupDTO;
                pushedData.Id = element["Id"];
                pushedData.Message = element["Message"];
                pushedData.CommentGroupName = element["CommentGroupName"];
                deletedComments.push(pushedData);
            }

            (await PostCommentService.Delete(deletedComments)).subscribe({
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

    public async AddComments() {
        var commentsArray = this.newCommentsData.split('\n');
        var newCommetsArray = new Array<CRUDCommentGroupDTO>();
        var groupName = $("#commentSelect").val().toString();
        commentsArray.forEach(commentMessage => {
            var pushedElement = new CRUDCommentGroupDTO();
            pushedElement.Message = commentMessage;
            pushedElement.CommentGroupName = groupName;
            newCommetsArray.push(pushedElement);
        });

        (await this.PostCommentService.Add(newCommetsArray)).subscribe({
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
            error: error => {
                $("#errorNot").val(error);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
            }
        })
    }
}

export class CRUDCommentGroupDTO {
    Id: number;
    Message: string;
    CommentGroupName: string;
}