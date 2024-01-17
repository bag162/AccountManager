import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { db } from '../Services/DBStoreService'
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";

@Injectable({ providedIn: 'root' })
export class GuardService {
    private static HttpClient: HttpClient;
    constructor(HttpClient: HttpClient, router: Router) {
        GuardService.HttpClient = HttpClient;
    }

    public static async CanAccess(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
        var accessData = await db.accessData.toArray();
        var accessResult: boolean = false;

        accessData.forEach(element => {
            var checlUrl = state.url;
            var splitedEl = state.url.split('/');


            if (splitedEl[2] == "workertaskdata") {
                checlUrl = splitedEl[0] + '/' + splitedEl[1] + '/' + splitedEl[2];
            }
            if (splitedEl[2] == "manager" && splitedEl[1] == "post") {
                switch (splitedEl[3]) {
                    case "add":
                        checlUrl = '/' + splitedEl[1] + '/' + splitedEl[2] + '/' + splitedEl[3];
                        break;
                    case "update":
                        checlUrl = '/' + splitedEl[1] + '/' + splitedEl[2] + '/' + splitedEl[3];
                        break;
                    default:
                        checlUrl = '/' + splitedEl[1] + '/' + splitedEl[2];
                        break;
                }
            }
            if (splitedEl[3] == 'manager' && splitedEl[2] == 'comment' && splitedEl[1] == 'post') {
                checlUrl =  '/' + splitedEl[1] + '/' + splitedEl[2] + '/' + splitedEl[3];
            }

            if (element.Role == "admin") {
                accessResult = true;
            }
            if (element.Role == checlUrl) {
                accessResult = true;
            }
        });
        if (accessResult) {
            return true;
        }
        else {
            return false;
        }
    }
}