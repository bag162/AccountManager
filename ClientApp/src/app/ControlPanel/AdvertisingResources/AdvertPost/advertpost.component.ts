import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { AdvertPostService } from 'src/app/Services/AdvertPostService';

@Component({
    selector: 'advert-post',
    templateUrl: 'advertpost.component.html'
})

export class AdvertPostComponent implements OnInit {
    dtOptions: any;
    newGroup: string;
    URIPath: string;
    postData: string;
    groupNames: string[];

    private static router: Router;
    private static AdvertPostService: AdvertPostService;
    constructor(router: Router, advertPostService: AdvertPostService, activateRoute: ActivatedRoute) {
        AdvertPostComponent.router = router;
        AdvertPostComponent.AdvertPostService = advertPostService;
        var groupId = activateRoute.snapshot.params["groupId"];
        if (groupId == undefined) {
            this.URIPath = environment.apiUrl + '/api/AdvertPost/get'
        }
        else {
            this.URIPath = environment.apiUrl + '/api/AdvertPost/get/' + groupId
        }
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#addAdvertPostForm').hide();

        this.dtOptions = {
            ajax: this.URIPath,
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Post URL',
                data: 'PostURL'
            }, {
                title: 'Like status',
                data: 'AdvertPostLikeStatus'
            }, {
                title: 'Comment status',
                data: 'AdvertPostCommentStatus'
            }, {
                title: 'Group',
                data: 'AdvertPostGroupName'
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
                    text: 'Add Post',
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
            $("#addAdvertPostForm").show(500);
        }

        async function Delete(data: string[], dt: any) {
            var deletedPosts = new Array<DeleteAdvertPostDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                var newItem = new DeleteAdvertPostDTO();
                newItem.Id = element["Id"]
                deletedPosts.push(newItem);
            }
            (await AdvertPostService.DeletePosts(deletedPosts)).subscribe({
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

        (await AdvertPostComponent.AdvertPostService.GetGroupNames()).subscribe({
            next: (data: string[]) => {
                this.groupNames = data;
            }
        })
    }

    public async AddPost() {
        var PostArray = this.postData.split('\n');
        var PostToAdd = new Array<AddAdvertPostDTO>();
        PostArray.forEach(element => {
            var newPost = new AddAdvertPostDTO();
            newPost.PostURL = element;
            newPost.AdvertPostGroupName = $("#selectAdvertGroup").val().toString()
            PostToAdd.push(newPost);
        });

        (await AdvertPostComponent.AdvertPostService.AddPosts(PostToAdd)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(200);
                    $("#successNot").delay(400).hide(200);
                    $("#addAdvertPostForm").hide(500);
                }
                else {
                    $("#errorNot").show(200);
                    $("#errorNot").delay(400).hide(200);
                    $("#addAdvertPostForm").hide(500);
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

export class AddAdvertPostDTO
{
    PostURL: string;
    AdvertPostGroupName: string;
}

export class DeleteAdvertPostDTO
{
    Id: number;
}