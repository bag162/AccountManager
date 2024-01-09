import { Component, OnInit } from '@angular/core';
import { PostGroupService } from '../../../Services/PostGroupService';
import { environment } from '../../../../environments/environment';
import { InstAccountService } from 'src/app/Services/InstAccountService';

@Component({
    selector: 'post-group',
    templateUrl: 'postgroup.component.html'
})

export class PostGroupComponent implements OnInit {
    dtOptions: any;
    PostGroupService: PostGroupService;
    InstAccountService: InstAccountService;
    accountGroupNames: string[];
    PostGroupName: string;

    constructor(PostGroupService: PostGroupService,  InstAccountService: InstAccountService) {
        this.PostGroupService = PostGroupService;
        this.InstAccountService = InstAccountService;
    }

    async ngOnInit() {
        $('#postgroupForm').hide();
        $('#successNot').hide();
        $('#errorNot').hide();
        this.dtOptions = {
            ajax: environment.apiUrl + '/api/postgroup/get',
            lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
            serverSide: true,
            columns: [{
                title: 'ID',
                data: 'Id'
            }, {
                title: 'Name',
                data: 'Name'
            }, {
                title: 'Account group',
                data: 'AccountGroupName'
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
                    text: 'Add Group',
                    key: '1',
                    action: function (e, dt, node, config) {
                        if ($('#postgroupForm').is(':visible')) {
                            $('#postgroupForm').hide(500);
                        }
                        else {
                            $('#postgroupForm').show(500);
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
                        RedirectToPostManager(dt.rows({ selected: true }).data()[0]);
                    }
                }
            ]
        };

        await InstAccountService.GetInstAccGroups().subscribe({
            next: (data:string[]) => {
                this.accountGroupNames = data;
            }
        })
        async function RedirectToPostManager(data: string)
        {
            $(location).attr('href', window.location.origin.toString() + "/post/manager/" + data["Id"]);
        }

        async function Delete(data: string[], dt: any) {
            var deletedpostgroup = new Array<PostGroupDTO>;
            for (let index = 0; index < data.length; index++) {
                const element = data[index];
                
                var newItem = new PostGroupDTO();
                newItem.Id = element["Id"]
                newItem.Name = element["Name"]
                newItem.AccountGroupName = element["AccountGroupName"]
                deletedpostgroup.push(newItem);
            }
            (await PostGroupService.DeletePostGroup(deletedpostgroup)).subscribe({
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

    async AddPostGroup() {
        var addedpostgroup = new Array<PostGroupDTO>();
        var newpostgroup = new PostGroupDTO();

        newpostgroup.Name = this.PostGroupName;
        newpostgroup.AccountGroupName = $('#accountGroupName').val().toString();

        addedpostgroup.push(newpostgroup);
        (await this.PostGroupService.AddPostGroup(addedpostgroup)).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $("#successNot").show(500);
                    $("#postgroupForm").hide(500);
                    $("#successNot").delay(1500).hide(500);
                }
                else {
                    $("#errorNot").show(500);
                    $("#postgroupForm").hide(500);
                    $("#errorNot").delay(1500).hide(500);
                }
            },
            error: error => {
                $("#errorNot").val(error);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
            }
        });

    }
}

export class PostGroupDTO
{
    Id: number;
    Name: string;
    AccountGroupName: string;
    CountPinnedPosts: number;
}