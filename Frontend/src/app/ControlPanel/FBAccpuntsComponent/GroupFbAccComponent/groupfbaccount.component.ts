import { Component, OnInit } from '@angular/core';
import { FbAccountService } from 'src/app/Services/FBAccountService'
import { DataService } from 'src/app/Services/DataService'
import * as $ from 'jquery';

@Component({
    selector: 'fbacc-group',
    templateUrl: 'groupfbaccount.component.html'
})

export class AddGroupFBACCComponent implements OnInit {
    FBAccountService: FbAccountService;
    groupData: string;
    DataService: DataService
    constructor(DataService: DataService) { 
        this.DataService = DataService;
    }

    ngOnInit() {
        $('#succcesAddFBAccGroup').hide();
        $('#errorAddFBAccGroup').hide();
    }

    AddFBAccGroup() {
        var data = new Array<FBAccGroupDTO>;
        data.push(new FBAccGroupDTO(this.groupData));
        FbAccountService.AddFBAccGroup(data).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $('#succcesAddFBAccGroup').show(200).delay(400).hide(200);
                    this.DataService.UpdateGroupData();
                }
                else {
                    $('#errorAddFBAccGroup').show(200).delay(400).hide(200);
                }
            },
            error: (data: any) => {
                $('#errorAddFBAccGroup').show(200).delay(400).hide(200);
            }
        })
    }
}

export class FBAccGroupDTO {
    constructor(name: string) {
        this.GroupName = name;
    }
    GroupName: string;
}