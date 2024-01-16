import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostGroupService } from 'src/app/Services/PostGroupService';
import { PostService } from 'src/app/Services/PostService';
import { environment } from '../../../../../environments/environment';
import { PostCommentGroupService } from 'src/app/Services/PostCommentGroupService';

@Component({
    selector: 'overview-post',
    templateUrl: 'overviewpost.component.html'
})

export class OverviewPostComponent implements OnInit {
    postId: number;
    data: any;
    postData: CRUDPostDTO = new CRUDPostDTO();
    static imageBase64Data: string;

    postGroupList: string[] = Array<string>();
    commentGroupList: string[];

    postGroupService: PostGroupService;
    postService: PostService;
    postCommentGroupService: PostCommentGroupService;

    constructor(activateRoute: ActivatedRoute,
        postGroupService: PostGroupService,
        postService: PostService,
        postCommentGroupService: PostCommentGroupService) {
        this.postId = activateRoute.snapshot.params["postId"];
        this.postGroupService = postGroupService;
        this.postService = postService;
        this.postCommentGroupService = postCommentGroupService;
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();

        if (this.postId != undefined) {
            (await this.postService.GetPostById(this.postId)).subscribe(
                {
                    next: (data: CRUDPostDTO) => {
                        this.postData = data;
                        this.postData.ImagePath = environment.apiUrl + "/" + this.postData.ImagePath;
                    }
                }
            )
            $("#inputDescription").prop("disabled", true);
            $("#selectPostGroup").prop("disabled", true);
            $("#selectCommentGroup").prop("disabled", true);
            $("#inputPostName").prop("disabled", true);

            $("#addPostBtn").hide();
            $("#loadImageEl").hide();
            
        }
        else {
            $("#updatePostBtn").hide();
            $("#uploadedImage").hide();
            (await this.postGroupService.GetList()).subscribe({
                next: (data: string[]) => {
                    this.postGroupList = data;
                }
            })
            await (await this.postCommentGroupService.GetGroupNames()).subscribe({
                next: (data: string[]) => {
                    this.commentGroupList = data;
                }
            })
        }
    }

    UpdateImage() {
        let fileInput = document.createElement('input');
        fileInput.type = 'file';
        const target = event.target as HTMLInputElement;
        const selectedFile: File = target.files[0];
        var reader = new FileReader();
        reader.readAsDataURL(selectedFile);
        reader.onloadend = function (): string | ArrayBuffer {
            var base64data = reader.result;
            $("#uploadedImage").attr("src", base64data.toString());
            $("#uploadedImage").show(500);
            OverviewPostComponent.imageBase64Data = base64data.toString();
            return base64data;
        };
    }

    async AddPost() {
        var base64Data = OverviewPostComponent.imageBase64Data.split(",");
        var imageFormatBase64 = base64Data[0].split("/")[1];
        var imageFormat = imageFormatBase64.split(";")[0];
        this.postData.ImageBase64 = base64Data[1];
        this.postData.ImageFormat = imageFormat;
        this.postData.PostGroupName = $('#selectPostGroup').val().toString();
        this.postData.CommentGroupName = $("#selectCommentGroup").val().toString();
        (await this.postService.AddPost(this.postData)).subscribe({
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

    async UpdatePost() {
        var updatePostData: UpdatePostDTO = new UpdatePostDTO;
        updatePostData.PostStatus = this.postData.PostStatus;
        updatePostData.RequiredCountComments = this.postData.RequiredCountComments;
        updatePostData.RequiredCountLikes = this.postData.RequiredCountLikes;
        updatePostData.Id = this.postData.Id;

        (await this.postService.UpdatePost(updatePostData)).subscribe({
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
            error: (error) => {
                $("#errorNot").val(error);
                $("#errorNot").show(200);
                $("#errorNot").delay(2000).hide(200);
                $("#errorNot").val("Error");
            }
        })
    }
}

export class CRUDPostDTO {
    Id: number;
    Name: string;
    PostGroupName: string;
    PostStatus: string;
    RequiredCountLikes: number;
    RequiredCountComments: number;
    ImageBase64: string;
    ImageFormat: string;
    Description?: string;
    PostURI: string = "Added by user";
    ImagePath?: string;
    CommentGroupName: string;
}

export class UpdatePostDTO {
    Id: number;
    PostStatus: string;
    RequiredCountLikes: number;
    RequiredCountComments: number;
}