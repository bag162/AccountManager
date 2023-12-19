import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DataService {
  groupData = new Subject();
  accountGroup = new Subject();
  proxyGroup = new Subject();
  smsservice = new Subject();

  public subscriber$ = this.groupData.asObservable();
  public subscriberAccountGroup$ = this.accountGroup.asObservable();
  public subscriberProxyGroup$ = this.proxyGroup.asObservable();
  public subscriberSMSService$ = this.smsservice.asObservable();

  UpdateGroupData() {
    this.groupData.next(0);
  }

  UpdateAccountGroup(data: string) {
    this.accountGroup.next(data);
  }

  UpdateProxyGroup(data: string) {
    this.proxyGroup.next(data);
  }

  UpdateSMSService(data: string) {
    this.smsservice.next(data);
  }
}