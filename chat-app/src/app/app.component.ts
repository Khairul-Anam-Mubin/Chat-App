import { Component, ElementRef, OnInit } from '@angular/core';
import { CommandService } from './core/services/command-service';
import { AuthService } from './identity/services/auth.service';
import { take, takeUntil } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from './core/services/alert-service';
import { SignalRService } from './core/services/signalr-service';
import { UserService } from './identity/services/user.service';
import { MathHelper } from './core/helpers/math-helper';
import { DiffieHellmanKeyExchange } from './core/cryptography/diffie-hellman-key-exchange';
import { EncrytptionDecryptionFactory, IEncryptionDecryption } from './core/helpers/encryption-decryption-helper';
import { SecurtiyService } from './core/services/security-service';
import { Subject } from '@microsoft/signalr';
import { ChatSocketService } from './chat/services/chat-socket-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  title = 'chat-app';
  isLoggedIn : boolean = false;
  currentOpenedNavItem: any = '';
  val : any = [];
  encryptionDecryption: IEncryptionDecryption;

  constructor(
    private commandService : CommandService,
    private alertService : AlertService,
    private userService: UserService,
    private authService: AuthService,
    private signlaRService: SignalRService,
    private router: Router,
    private securityService : SecurtiyService) {
      this.encryptionDecryption = EncrytptionDecryptionFactory.getEncryptionDecryption();
    }

  ngOnInit(): void {
    
    this.setCurrentOpenedNavItem();

    this.isLoggedIn = this.authService.isLoggedIn();
    if (this.isLoggedIn) {
      this.signlaRService.startConnection();
    }

    this.authService.authStateObservable().subscribe(state => {
      this.isLoggedIn = this.authService.isLoggedIn();
    });
  }

  onClickLogOut() {
    this.commandService.execute(this.authService.getLogOutCommand())
    .pipe(take(1))
    .subscribe(response => {
      if (response.status) {
        this.authService.logOut(); 
        this.router.navigateByUrl("");
      }
    });
  }

  setCurrentOpenedNavItem() {
    if (this.router.url.includes('chat')) {
      this.currentOpenedNavItem = 'chat';
    }
    else if (this.router.url.includes('contact')) {
      this.currentOpenedNavItem = 'contact';
    }
    else if (this.router.url.includes('home')) {
      this.currentOpenedNavItem = 'home';
    }
    else {
      this.currentOpenedNavItem =  '';
    }
  }

  onClickNavItem(item: any) {
    this.currentOpenedNavItem = item;
    this.router.navigateByUrl(item);
  }

  onClickUserProfile() {
    const userId = this.userService.getCurrentUserId();
    this.router.navigateByUrl('/user/' + userId);
  }
}
