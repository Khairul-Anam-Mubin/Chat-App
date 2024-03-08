import { Injectable } from '@angular/core';
import { LoginCommand } from '../commands/login-command';
import { RefreshTokenCommand } from '../commands/refresh-token-command';
import { Token } from '../models/token';
import { LogOutCommand } from '../commands/logout-command';
import {Guid} from "guid-typescript";
import { Observable, Subject } from 'rxjs';
@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private subject: Subject<any> = new Subject<any>();
    
    authStateObservable(): Observable<any> {
        return this.subject.asObservable();
    }

    canActivate() {
      return this.isLoggedIn();
    }

    isTokenExpired(token: string) {
        const expiry = (JSON.parse(atob(token.split('.')[1]))).exp;
        return (Math.floor((new Date).getTime() / 1000)) >= expiry;
    }

    getAccessToken() {
        return localStorage.getItem("accessToken");
    }

    getRefreshToken() {
        return localStorage.getItem("refreshToken");
    }

    isLoggedIn() {
        const refreshToken = this.getRefreshToken();
        const accessToken = this.getAccessToken();
        if (refreshToken && accessToken) return true;
        return false;
    }

    logOut() {
        localStorage.clear();
        sessionStorage.clear();

        this.subject.next({
            action: 'LoggedOut',
            isLoggedIn: false
        });
    }

    loggedIn(token: any) {
        this.setTokenToStore(token);

        this.subject.next({
            action: 'LoggedIn',
            token: token,
            isLoggedIn: true
        });
    }

    tokenRefreshed(token: any) {
        this.setTokenToStore(token);

        this.subject.next({
            action: 'Refreshed',
            token: token,
            isLoggedIn: true
        });
    }

    setTokenToStore(token: any) {
        localStorage.setItem("accessToken", token.accessToken);
        localStorage.setItem("refreshToken", token.refreshToken);
    }

    getAppId() {
        const appId = localStorage.getItem("appId");
        if (appId) return appId;
        return this.setAppId();
    }

    getRefreshTokenCommand() {
        const refreshTokenCommand = new RefreshTokenCommand();
        refreshTokenCommand.appId = this.getAppId();
        refreshTokenCommand.accessToken = this.getAccessToken();
        refreshTokenCommand.refreshToken = this.getRefreshToken();
        return refreshTokenCommand;
    }

    getLogInCommand(email : string, password : string) {
        const logInCommand = new LoginCommand();
        logInCommand.email = email;
        logInCommand.password = password;
        logInCommand.appId = this.getAppId();
        return logInCommand;
    }

    getLogOutCommand() {
        const logOutCommand = new LogOutCommand();
        logOutCommand.appId = this.getAppId();
        return logOutCommand;
    }

    private setAppId() {
        const appId = Guid.create().toString();
        localStorage.setItem("appId", appId);
        return appId;
    }
}
