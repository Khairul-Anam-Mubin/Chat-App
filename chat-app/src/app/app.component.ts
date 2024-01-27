import { Component, OnInit } from '@angular/core';
import { CommandService } from './core/services/command-service';
import { AuthService } from './identity/services/auth.service';
import { take } from 'rxjs';
import { Router } from '@angular/router';
import { SignalRService } from './core/services/signalr-service';
import { UserService } from './identity/services/user.service';
import { ActivityService } from './activity/services/activity-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  title = 'chat-app';
  isLoggedIn : boolean = false;
  currentOpenedNavItem: any = '';

  constructor(
    private commandService : CommandService,
    private userService: UserService,
    private authService: AuthService,
    private signlaRService: SignalRService,
    private activityService: ActivityService,
    private router: Router) {}

  ngOnInit(): void {
    this.setCurrentOpenedNavItem();
    this.isLoggedIn = this.authService.isLoggedIn();
    if (this.isLoggedIn) {
      this.signlaRService.startConnection();
    }
    this.activityService.startRecurringTrackActivity();
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
