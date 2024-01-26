import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AuthService } from 'src/app/identity/services/auth.service';
import { ChatSocketService } from 'src/app/chat/services/chat-socket-service';
import { Configuration } from './configuration';
import { HubConnection } from '@microsoft/signalr';

@Injectable()
export class SignalRService {

  private hubConnection: HubConnection | undefined;

  constructor(
    private authService: AuthService,
    private chatSocketService : ChatSocketService) { 

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

    this.hubConnection.on("notificationReceived",  (message: any) => {
      console.log("Received message with web socket", message);
      this.chatSocketService.chatSubject.next(message.content);
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

}
