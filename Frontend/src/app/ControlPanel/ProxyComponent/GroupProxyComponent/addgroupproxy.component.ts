import { Component, OnInit } from '@angular/core';
import { ProxyService } from "src/app/Services/ProxyService"
import * as $ from 'jquery';
import { DataService } from 'src/app/Services/DataService';

@Component({
    selector: 'proxy-group',
    templateUrl: 'addgroup.proxy.component.html'
})

export class AddGroupProxyComponent implements OnInit {
    proxyService: ProxyService;
    groupData: string;
    DataService: DataService
    constructor(DataService: DataService) {
        this.DataService = DataService;
     }

    ngOnInit() {
        $('#succcesAddProxyGroup').hide();
        $('#errorAddProxyGroup').hide();
    }

    AddProxyGroup() {
        var data = new Array<ProxyGroupDTO>;
        data.push(new ProxyGroupDTO(this.groupData));
        ProxyService.AddProxyGroup(data).subscribe({
            next: (data: boolean) => {
                if (data) {
                    $('#succcesAddProxyGroup').show(200).delay(400).hide(200);
                    this.DataService.UpdateGroupData();
                }
                else {
                    $('#errorAddProxyGroup').show(200).delay(400).hide(200);
                }
            },
            error: (data:any) => {
                $('#errorAddProxyGroup').show(200).delay(400).hide(200);
            }
        })
    }
}

export class ProxyGroupDTO {
    constructor(name: string) {
        this.GroupName = name;
    }
    GroupName: string;
}