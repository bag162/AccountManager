import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ProxyDTO } from "../ControlPanel/ProxyComponent/proxy.component"
import { environment } from 'src/environments/environment';
import { ProxyGroupDTO } from '../ControlPanel/ProxyComponent/GroupProxyComponent/addgroupproxy.component';

@Injectable({ providedIn: 'root' })
export class ProxyService {
    static address: string = environment.apiUrl + "/api/proxy/";
    static proxyGroupAddress: string = environment.apiUrl + "/api/proxygroup/";
    static httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        ProxyService.httpClient = httpClient;
    }

    public async AddProxy(proxy: ProxyDTO[])
    {
        return ProxyService.httpClient.post(ProxyService.address + "post", proxy);
    }

    static UpdateProxy(proxy: ProxyDTO[])
    {
        return ProxyService.httpClient.put(ProxyService.address + "put", proxy);
    }
    
    static DeleteProxy(proxy: ProxyDTO[]) {
        return ProxyService.httpClient.delete(this.address + "delete", { 
            body: proxy
        });
    }

    static GetProxyGroups()
    {
        return ProxyService.httpClient.get(this.proxyGroupAddress + "get");
    }

    static AddProxyGroup(newGroup: ProxyGroupDTO[])
    {
        return ProxyService.httpClient.post(this.proxyGroupAddress + "post", newGroup);
    }
}