import { Component, OnInit } from '@angular/core';
import { ProxyService } from 'src/app/Services/ProxyService';
import { DataService } from 'src/app/Services/DataService'
@Component({
    selector: 'select-proxy-group',
    templateUrl: 'selectproxygroup.component.html'
})

export class SelectProxyGroupComponent implements OnInit {
    ProxyService: ProxyService;
    DataService: DataService;
    data;
    constructor(ProxyService: ProxyService, DataService: DataService) {
        this.ProxyService = ProxyService;
        this.DataService = DataService;
    }
    proxyGroups: string[];

    ngOnInit() {
        ProxyService.GetProxyGroups().subscribe((data: any) => {
            this.proxyGroups = data;
        })
    }

    UpdateData() {
        this.DataService.UpdateProxyGroup(<string>$('#selectProxyGroup option:selected').val());
    }
}