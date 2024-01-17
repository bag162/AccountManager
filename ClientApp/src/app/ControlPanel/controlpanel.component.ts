import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { db } from '../Services/DBStoreService';
import { AuthService } from '../Services/AuthServices';

@Component({
    selector: 'control-panel',
    templateUrl: 'controlpanel.component.html'
})

export class ControlPanelComponent implements OnInit {
    private router: Router;
    private authService: AuthService;
    public UserLogin: string;
    constructor(authService: AuthService, router: Router) { 
        this.authService = authService;
        this.router = router;
    }

    async ngOnInit() { 
        var userDatas = await db.userData.toArray();
        this.UserLogin = userDatas[0].Login;
    }

    async SignOut()
    {
        await db.accessData.clear();
        await db.userData.clear();
        (await this.authService.SignOut()).subscribe({
            next: () => this.router.navigate(['auth'])
        })
        
    }
}