import { Component, OnInit } from '@angular/core';
import { InstAccountService } from 'src/app/Services/InstAccountService'
import { DataService } from 'src/app/Services/DataService'
import * as $ from 'jquery';

@Component({
    selector: 'instacc-group',
    templateUrl: 'groupInstaccount.component.html'
})

export class AddGroupInstComponent implements OnInit {
    InstAccountService: InstAccountService;
    groupData: string;
    DataService: DataService
    constructor(DataService: DataService) { 
        this.DataService = DataService;
    }

    ngOnInit() {
        $('#succcesAddInstAccGroup').hide();
        $('#errorAddInstAccGroup').hide();
    }

    AddInstAccGroup() {
        var data = new Array<InstAccGroupDTO>;
        data.push(new InstAccGroupDTO(this.groupData));
        InstAccountService.AddInstAccGroup(data).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $('#succcesAddInstAccGroup').show(200).delay(400).hide(200);
                    this.DataService.UpdateGroupData();
                }
                else {
                    $('#errorAddInstAccGroup').show(200).delay(400).hide(200);
                }
            },
            error: (data: any) => {
                $('#errorAddInstAccGroup').show(200).delay(400).hide(200);
            }
        })
    }
}

export class InstAccGroupDTO {
    constructor(name: string) {
        this.GroupName = name;
    }
    GroupName: string;
}