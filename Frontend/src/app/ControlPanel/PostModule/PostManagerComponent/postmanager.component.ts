import { Component, OnInit } from '@angular/core';
import { PostService } from '../../../Services/PostService'
import { environment } from '../../../../environments/environment';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'postmanager',
    templateUrl: 'postmanager.component.html'
})

export class PostManagerComponent implements OnInit {
    dtOptions: any;
    PostService: PostService;
    URIPath: string;
    postName: string;
    constructor(PostService: PostService, activateRoute: ActivatedRoute) {
        this.PostService = PostService;
        var groupId = activateRoute.snapshot.params["groupId"];
        if (groupId == undefined) {
            this.URIPath = environment.apiUrl + '/api/post/get'
        }
        else {
            this.URIPath = environment.apiUrl + '/api/postgroup/get/' + groupId
        }
    }

    ngOnInit() {
        $('#postForm').hide();
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
                title: 'Post URI',
                data: 'PostURI'
            }, {
                title: 'Group name',
                data: 'GroupName'
            }, {
                title: 'Post status',
                data: 'PostStatus'
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
                    text: 'View or update selected post',
                    action: function (e, dt, node, config) {
                        ViewOrUpdate(dt.rows({selected: true}).data()[0]["Id"]);
                    }
                }
            ]
        };
        async function Delete(data: string[], dt: any) {
            var deletedpost = new Array<PostDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];

                var newItem = new PostDTO();
                newItem.Id = element["Id"]
                newItem.GroupName = element["GroupName"]
                newItem.PostStatus = element["PostStatus"]
                newItem.PostURI = element["PostURI"]


                deletedpost.push(newItem);
            }
            (await PostService.DeletePost(deletedpost)).subscribe({
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

        function Add()
        {
            $(location).attr('href', window.location.origin.toString() + "/post/postmanager/add");
        }

        function ViewOrUpdate(postId: number)
        {
            $(location).attr('href', window.location.origin.toString() + "/post/postmanager/update/" + postId);
        }
    }
}

export class PostDTO {
    Id: number;
    PostURI: string;
    GroupName: string;
    PostStatus: string;
}