import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { LoginUserDTO } from '../Authorization/Login/login.component'
import { RegisterUserDTO } from '../Authorization/Registration/registration.component'

@Injectable({ providedIn: 'root' })
export class AuthService {
    private httpClient: HttpClient;
    private address: string = environment.apiUrl + "/api/UserAuthorization/";
    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
    }

    public async CheckFreeLogin(login: string) {
        return this.httpClient.post(this.address + "CheckFreeLogin", {Login: login});
    }

    public async LoginUser(user: LoginUserDTO) {
        return this.httpClient.post(this.address + "Login", user);
    }

    public async RegisterUser(user: RegisterUserDTO) {
        return this.httpClient.post(this.address + "Registration", user);
    }

    public async SignOut()
    {
        return this.httpClient.get(this.address + "SignOut");
    }
}

export class CheckLoginDTO {
    Login: string;
}