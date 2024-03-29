import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './identity/components/register/register.component';
import { LogInComponent } from './identity/components/log-in/log-in.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CommandService } from './core/services/command-service';
import { AuthService } from './identity/services/auth.service';
import { AuthInterceptor } from './interceptors/auth-interceptor';
import { QueryService } from './core/services/query-service';
import { UserProfileComponent } from './identity/components/user-profile/user-profile.component';
import { ContactComponent } from './contact/components/contact/contact.component';
import { UserService } from './identity/services/user.service';
import { AlertComponent } from './core/components/alert/alert.component';
import { AlertService } from './core/services/alert-service';
import {MatSnackBar, MatSnackBarModule} from "@angular/material/snack-bar";
import {MatButton, MatButtonModule} from "@angular/material/button";
import { ChatComponent } from './chat/components/chat/chat.component';
import { ChatListComponent } from './chat/components/chat-list/chat-list.component';
import { SignalRService } from './core/services/signalr-service';
import { ReversePipe } from './core/pipes/reverse-pipe';
import { FileService } from './core/services/file-service';
import { SafeUrlPipe } from './core/pipes/safe-url-pipe';
import { SecurtiyService } from './core/services/security-service';
import { HomeComponent } from './identity/components/home/home.component';
import { SocketService } from './core/services/socket-service';
import { ActivityService } from './activity/services/activity-service';
import { ContactListComponent } from './contact/components/contact-list/contact-list.component';
import { GroupListComponent } from './contact/components/group-list/group-list.component';
import { ChatService } from './chat/services/chat-service';
import { GroupService } from './contact/services/group-service';
import { GroupComponent } from './contact/components/group/group.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LogInComponent,
    UserProfileComponent,
    ContactComponent,
    AlertComponent,
    ChatComponent,
    ChatListComponent,
    ReversePipe,
    SafeUrlPipe,
    HomeComponent,
    ContactListComponent,
    GroupListComponent,
    GroupComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSnackBarModule,
    MatButtonModule
  ],
  providers: [
    CommandService,
    QueryService,
    AuthService,
    UserService,
    AlertService,
    SignalRService,
    SocketService,
    FileService,
    SecurtiyService,
    ActivityService,
    ChatService,
    GroupService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
