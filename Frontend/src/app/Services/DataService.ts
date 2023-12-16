import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({providedIn: 'root'})
export class DataService {
  groupData = new Subject();
  public subscriber$ = this.groupData.asObservable();

  UpdateGroupData() {
    this.groupData.next(0);
  }
}