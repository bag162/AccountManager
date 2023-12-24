import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/Services/DataService';
import { InstAccountService } from 'src/app/Services/InstAccountService'
@Component({
    selector: 'select-account-group',
    templateUrl: 'selectaccountgroup.component.html'
})

export class SelectAccountGroupComponent implements OnInit {
    InstAccountService: InstAccountService;
    DataService: DataService;
    accountGroups: string[];
    data;

    constructor(InstAccountService: InstAccountService, DataService: DataService) {
        this.InstAccountService = InstAccountService;
        this.DataService = DataService;
    }

    ngOnInit() {
        InstAccountService.GetInstAccGroups().subscribe((data: string[]) => {
            this.accountGroups = data;
        })
    }

    UpdateData() {
        this.DataService.UpdateAccountGroup(<string>$('#selectAccountGroup option:selected').val());
    }
}