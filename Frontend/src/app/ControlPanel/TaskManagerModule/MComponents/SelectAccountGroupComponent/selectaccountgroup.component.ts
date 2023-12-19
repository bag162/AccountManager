import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { FbAccountService } from 'src/app/Services/FBAccountService'
@Component({
    selector: 'select-account-group',
    templateUrl: 'selectaccountgroup.component.html'
})

export class SelectAccountGroupComponent implements OnInit {
    FbAccountService: FbAccountService;
    DataService: DataService;
    accountGroups: string[];
    data;

    constructor(FbAccountService: FbAccountService, DataService: DataService) {
        this.FbAccountService = FbAccountService;
        this.DataService = DataService;
    }

    ngOnInit() {
        FbAccountService.GetFBAccGroups().subscribe((data: string[]) => {
            this.accountGroups = data;
        })
    }

    UpdateData() {
        this.DataService.UpdateAccountGroup(<string>$('#selectAccountGroup option:selected').val());
    }
}