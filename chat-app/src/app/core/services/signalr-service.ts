import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AuthService } from 'src/app/identity/services/auth.service';
import { AlertService } from './alert-service';
import { Subject } from 'rxjs';
import { ChatSocketService } from 'src/app/chat/services/chat-socket-service';
import { Configuration } from './configuration';

@Injectable()
export class SignalRService {

  private hubConnection: any;

  constructor(
    private authService: AuthService,
    private chatSocketService : ChatSocketService) { }

  startConnection(): void {
    const token = 'Bearer ' + this.authService.getAccessToken();
    console.log(token);
    const options : any = {
        accessTokenFactory : () => token
    };
    const url = Configuration.notificationHub;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, options)
      .withAutomaticReconnect([100, 400, 800, 1600])
      .build();

    this.hubConnection.start().catch((err: any) => console.log(err));

    // this.hubConnection.on("ReceivedChat",  (message: any) => {
    //     console.log("Received message with web socket", message);
    //     this.chatSocketService.chatSubject.next(message);
    // });
    this.hubConnection.on("UserChat",  (message: any) => {
      console.log("Received message with web socket", message);
      this.chatSocketService.chatSubject.next(message.content);
    });
  }

}
