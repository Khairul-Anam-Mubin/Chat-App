import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AuthService } from 'src/app/identity/services/auth.service';
import { Configuration } from './configuration';
import { HubConnection } from '@microsoft/signalr';
import { NotificationData } from '../models/notification-data';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class SignalRService {

  private hubConnection: HubConnection | undefined;
  private notificationSubject: Subject<NotificationData> = new Subject<NotificationData>();

  constructor(
    private authService: AuthService) { 

    this.authService.authStateObservable().subscribe(state => {
      if (state) {
        if (state.action === 'LoggedIn') {
          this.startConnection();
        } else if (state.action === 'LoggedOut') {
          this.stopConnection();
        } else if (state.action === 'Refreshed') {
          this.stopConnection();
          this.startConnection();
        }
      }
    });
  }

  startConnection(): void {
    if (this.isConnected()) {
      return;
    }
    
    const token = this.authService.getAccessToken();

    console.log(token);
    
    const options : any = { accessTokenFactory: () => token }
    
    const url = Configuration.notificationHub;
    
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, options)
      .withAutomaticReconnect([100, 400, 800, 1600])
      .build();

    this.hubConnection.start().catch((err: any) => console.log(err));

    this.hubConnection.on("notificationReceived",  (notification: NotificationData) => {
      console.log("Received message with signalR", notification);
      this.notificationSubject.next(notification);
    });
  }

  stopConnection(): void {
    if (this.isConnected()) {
      this.hubConnection?.stop().catch((err: any) => console.log(err));
    }
  }

  isConnected(): boolean {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      return true;
    }
    return false;
  }

  notificationObservable(): Observable<NotificationData> {
    return this.notificationSubject.asObservable();
  }

  getCurrentConnectionId() {
    return this.hubConnection?.connectionId;
  }

}
