import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/AuthServices';
import { UserData, db } from 'src/app/Services/DBStoreService';

@Component({
    selector: 'registration',
    templateUrl: 'registration.component.html'
})

export class RegistrationComponent implements OnInit {
    loginIsFree: boolean = true;
    login: string;
    password: string;
    repeatPassword: string;

    private authService: AuthService;
    private router: Router

    constructor(authService: AuthService,
        router: Router) {
        this.authService = authService;
        this.router = router;
    }

    async ngOnInit() {
        $("#successNot").hide();
        $("#errorNot").hide();
    }

    async Registration() {
        if (this.password != this.repeatPassword) {
            $("#errorNot").text('Password mismatch');
            $("#errorNot").show(200);
            $("#errorNot").delay(1000).hide(200);
        }
        if (!this.loginIsFree) {
            $("#errorNot").text('Login is taken by another user');
            $("#errorNot").show(200);
            $("#errorNot").delay(1000).hide(200);
        }

        var registerData = new RegisterUserDTO();
        registerData.Login = this.login;
        registerData.Password = this.password;

        (await this.authService.RegisterUser(registerData)).subscribe({
            next: (data: RegistrationUserDataDTO) => {
                if (data.Error == false) {
                    // Добавляем информацию о пользователе в хранилище
                    var userData: UserData = { Login: data.Login };
                    db.userData.clear();
                    db.accessData.clear();
                    db.userData.add(userData);
                    this.router.navigate([''])
                }
                else {
                    $("#errorNot").text(data.ErrorMessage);
                    $("#errorNot").show(200);
                    $("#errorNot").delay(1000).hide(200);
                }
            },
            error: (data: string) => {
                $("#errorNot").text(data);
                $("#errorNot").show(200);
                $("#errorNot").delay(1000).hide(200);
            }
        })
    }

    async ChangeUserLogin() {
        (await this.authService.CheckFreeLogin(this.login)).subscribe({
            next: (data: boolean) => {
                this.loginIsFree = data;
            }
        })
    }
}

export class RegisterUserDTO {
    Login: string;
    Password: string;
}

export class RegistrationUserDataDTO {
    Login: string;
    ErrorMessage: string;
    Error: boolean;
}