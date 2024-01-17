import { Component, OnInit } from '@angular/core';
import { db } from './Services/DBStoreService';
declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.css']
})
export class AppComponent implements OnInit{
  title = 'app';

  async ngOnInit()
  {
    if(await db.userData.count() == 0 && location.href != window.location.origin.toString() + "/auth")
    {
        $(location).attr('href', window.location.origin.toString() + "/auth");
        return false;
    }
  }
}