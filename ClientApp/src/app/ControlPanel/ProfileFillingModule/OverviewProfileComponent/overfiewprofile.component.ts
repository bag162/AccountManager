import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProfileFillingService } from '../../../Services/ProfileFillingService'
@Component({
    selector: 'overview-profile',
    templateUrl: 'overfiewprofile.component.html'
})

export class OverfiewProfileFillingComponent implements OnInit {
    data: any;
    profileFillingDataId: number;
    profileFillingService: ProfileFillingService;
    profileData: ProfileFillingDataDTO = new ProfileFillingDataDTO();

    static base64Data: string;

    changeEditNameOrSurname: boolean = false;
    changeEditUsername: boolean = false;

    changeUsernameData: string = "{<ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><AnyDigit><AnyDigit>|<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow>_<ELowVow><ELowCons><ELowVow><ELowCons>|<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow>_<ELowVow><ELowCons><ELowVow><ELowCons><AnyDigit><AnyDigit><AnyDigit><AnyDigit>|<EFemNameLow>_<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow>|<EFemNameLow>_<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><AnyDigit><AnyDigit><AnyDigit><AnyDigit>}";
    changeNameOrUsernameData: string = "{<RMaleName>:<RSurname>|<EMaleName>:<ESurname>}";

    constructor(activateRoute: ActivatedRoute, profileFillingService: ProfileFillingService) {
        this.profileFillingDataId = activateRoute.snapshot.params["fillingDataId"];
        this.profileFillingService = profileFillingService;
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        $('#changeNameOrSurnameform').hide();
        $('#changeUsernameForm').hide();

        if (this.profileFillingDataId == undefined) {
            $('#uploadedImage').hide();
        }
        else {
            $('#loadImageEl').hide();
            $('#addFillingDataBtn').hide();
            $("#inputProfileName").prop("disabled", true);

            (await this.profileFillingService.GetProfileFillingData(this.profileFillingDataId)).subscribe({
                next: (data: ProfileFillingDataDTO) => {
                    this.profileData = data;
                }
            })
        }
    }

    async AddImage() {
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
            OverfiewProfileFillingComponent.base64Data = base64data.toString();
            return base64data;
        };
    }

    async AddFillingData() {
        var base64Data = OverfiewProfileFillingComponent.base64Data.split(",");
        var imageFormatBase64 = base64Data[0].split("/")[1];
        var imageFormat = imageFormatBase64.split(";")[0];
        this.profileData.AvatarBASE64 = base64Data[1];
        this.profileData.AvatarFormat = imageFormat;
        this.profileData.Gender = $('#selectGender').val().toString();
        if (this.changeEditNameOrSurname) {
            this.profileData.NameOrSurnameGenString = this.changeNameOrUsernameData;
        }
        if (this.changeEditUsername) {
            this.profileData.UsernameGenString = this.changeUsernameData;
        }
        if ($('#selectAccountRecomendation').val().toString() == "Enable") {
            this.profileData.EnableRecomendations = true;
        }
        else {
            this.profileData.EnableRecomendations = false;
        }

        if ($('#selectClosedAccount').val().toString() == "Enable") {
            this.profileData.ClosedAccount = true;
        }
        else {
            this.profileData.ClosedAccount = false;
        }

        (await this.profileFillingService.AddProfileFillingData(this.profileData)).subscribe({
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

    public ChangeVisibleEditNameOrSurname() {
        if (this.changeEditNameOrSurname) {
            $('#changeNameOrSurnameform').show(500);
        }
        else {
            $('#changeNameOrSurnameform').hide(500);
        }

    }

    public ChangeVisibleUsername() {
        if (this.changeEditUsername) {
            $('#changeUsernameForm').show(500);
        }
        else {
            $('#changeUsernameForm').hide(500);
        }

    }
}

export class ProfileFillingDataDTO {
    Id: number;
    Name: string;
    Gender: string;
    EnableRecomendations: boolean;
    ClosedAccount: boolean;
    AvatarPath: string;
    AboutMe: string;
    NameOrSurnameGenString?: string;
    UsernameGenString?: string;
    AvatarBASE64: string;
    AvatarFormat: string;
}