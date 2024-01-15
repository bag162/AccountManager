import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/AuthServices'
import { db } from '../../Services/DBStoreService'
import { UserData, AccessData } from '../../Services/DBStoreService'
import { Router } from '@angular/router';


@Component({
    selector: 'login',
    templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
    login: string;
    password: string;

    private authService: AuthService;
    private router: Router
    constructor(authService: AuthService, router: Router) {
        this.authService = authService;
        this.router = router;
    }

    async ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();
    }

    async Login() {
        var user = new LoginUserDTO();
        user.Login = this.login;
        user.Password = this.password;

        (await this.authService.LoginUser(user)).subscribe({
            next: (data: LoginUserDataDTO) => {
                if (data.Error) {
                    $("#errorNot").text(data.ErrorMessage);
                    $("#errorNot").show(200);
                    $("#errorNot").delay(1000).hide(200);
                }
                else {
                    // Добавляем информацию о пользователе в хранилище
                    var userData: UserData = { Login: data.Login };
                    db.userData.clear();
                    db.accessData.clear();
                    db.userData.add(userData);
                    // TODO добавить выгрузки ролей
                    // for (let index = 0; index < data.Roles.length; index++) {
                    //     const element = data.Roles[index];
                    //     var addedItem: AccessData = {
                    //         Role: element
                    //     }
                    //     db.accessData.add(addedItem);
                    // }

                    this.router.navigate([''])
                }
            },
            error: (data: string) => {
                $("#errorNot").text(data);
                $("#errorNot").show(200);
                $("#errorNot").delay(1000).hide(200);
            }
        })
    }
}

export class LoginUserDTO {
    Login: string;
    Password: string;
}

export class LoginUserDataDTO {
    Login: string;
    Roles: string[];
    ErrorMessage: string;
    Error: boolean;
}