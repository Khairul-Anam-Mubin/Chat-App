import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, Pipe, PipeTransform } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/identity/services/user.service';
import { SendMessageCommand } from '../../commands/send-message-command';
import { ChatModel } from '../../models/chat-model';
import { CommandService } from 'src/app/core/services/command-service';
import { take, takeLast, takeUntil, timer } from 'rxjs';
import { QueryService } from 'src/app/core/services/query-service';
import { ChatQuery } from '../../queries/chat-query';
import { ResponseStatus } from 'src/app/core/constants/response-status';
import { LastSeenQuery } from 'src/app/activity/queries/last-seen-query';
import { FileService } from 'src/app/core/services/file-service';
import { SecurtiyService } from 'src/app/core/services/security-service';
import { EncrytptionDecryptionFactory, IEncryptionDecryption } from 'src/app/core/helpers/encryption-decryption-helper';
import { ChatProcessor } from '../../helpers/chat-processor';
import { SocketService } from 'src/app/core/services/socket-service';
import { ChatByIdsQuery } from '../../queries/chat-by-ids-query';
import { ChatService } from '../../services/chat-service';
import { GroupService } from 'src/app/contact/services/group-service';
import { GroupsQuery } from 'src/app/contact/queries/groups-query';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy{

  groupId : string | undefined;
  isGroup : boolean = false;
  chatTitle : any = "";
  lastSeen : any = "";
  inputMessage : any = "";
  currentUserId : any = "";
  currentUserProfile : any;
  sendToUserProfile : any;
  sendToUserId : any = "";
  chats : any;
  isActive : any;
  query: ChatQuery = new ChatQuery();
  totalChats:any;
  canExecuteChatQuery: any = true;
  sendToUserBlobImageUrl: any = '';
  sharedSecret: any;
  encryptionDecryptionHelper: IEncryptionDecryption | undefined;

  constructor(
    private elementRef : ElementRef,
    private userService : UserService,
    private commandService : CommandService,
    private queryService : QueryService,
    private socketService: SocketService,
    private fileService: FileService,
    private router : Router,
    private securityService: SecurtiyService,
    private chatService: ChatService,
    private groupService: GroupService) {}

  ngOnInit(): void {
    // this.encryptionDecryptionHelper = EncrytptionDecryptionFactory.getEncryptionDecryption();
    this.chats = [];

    this.isGroup = this.groupService.getCurrentOpenedGroupId() ? true: false;
    this.currentUserId = this.userService.getCurrentUserId();
    this.currentUserProfile = this.userService.getCurrentUserProfile();

    if (this.isGroup) {
     this.initiateGroupChat();
    } else {
      this.initiateUserChat();
    }
  }

  initiateGroupChat() {
    if (!this.isGroup) return;
    this.groupId = this.groupService.getCurrentOpenedGroupId();
    this.sendToUserId = this.groupId;

    const groupQuery = new GroupsQuery();
    groupQuery.groupIds = [this.groupId];
    
    this.queryService.execute(groupQuery).pipe(take(1)).subscribe(response => {
      const group = response.value[0];
      this.chatTitle = group.name;
    });

    this.query.sendTo = this.sendToUserId;
    this.query.userId = this.currentUserId;

    this.getChats(this.query);
    
    this.socketService.subscribeToTopic(this.getGroupChatTopic()).subscribe(notification => {
      if (notification && notification.content && notification.contentType === 'ChatId') {
        const query = new ChatByIdsQuery();
        query.chatIds = [notification.content];
        this.queryService.execute(query).pipe(take(1)).subscribe(response => {
          let message = ChatProcessor.process(response.value[0], this.sharedSecret);
          this.chats = [message].concat(this.chats);
          this.setChatScrollStartFromBottom();
        });
      }
    });
  }

  initiateUserChat() {
    if (this.isGroup) return;
    this.sendToUserId = this.userService.getCurrentOpenedChatUserId();
    
    this.query.sendTo = this.sendToUserId;
    this.query.userId = this.currentUserId;

    this.userService.getUserProfileById(this.sendToUserId)
    .pipe(take(1))
    .subscribe(res => {
      if (res.status === ResponseStatus.success) {
        this.sendToUserProfile = res.value.items.find((item: any) => item.id === this.sendToUserId);
        // this.sharedSecret = this.securityService.getSharedSecretKey(this.sendToUserProfile.publicKey);
        this.chatTitle = this.sendToUserProfile.firstName + " " + this.sendToUserProfile.lastName;
        this.getChats(this.query);
        if (this.sendToUserProfile.profilePictureId){
          this.fileService.downloadFile(this.sendToUserProfile.profilePictureId)
          .pipe(take(1))
          .subscribe(response => {
            this.sendToUserBlobImageUrl = response;
            console.log("Blob url", response);
          });
        }
      }
    });
    this.getLastSeenStatus();
    this.socketService.subscribeToTopic(this.getUserChatTopic()).subscribe(notification => {
      console.log('UserChat Received : ', notification);
      if (notification && notification.content) {
        let message = ChatProcessor.process(notification.content, this.sharedSecret);
        // let message = notification.content;
        this.chats = [message].concat(this.chats);
        this.setChatScrollStartFromBottom();
      }
    });
  }

  ngOnDestroy(): void {
    if (this.isGroup) {
      this.socketService.unsubscribeTopic(this.getGroupChatTopic());
    } else {
      this.socketService.unsubscribeTopic('UserChat');
    }
  }

  getChats(query : ChatQuery) {
    this.query.isGroupMessage = this.isGroup;
    this.queryService.execute(query)
    .pipe(take(1))
    .subscribe(res => {
      if (res.status === ResponseStatus.success) {
        if (res.value.items.length === 0) {
          this.canExecuteChatQuery = false;
        } else {
          this.chats = this.chats.concat(this.processChats(res.value.items));
          if (query.Offset === 0) {
            this.setChatScrollStartFromBottom();
            this.totalChats = res.totalCount;
          }
        }
      }
    });
  }

  processChats(chats : any) {
    for (let index = 0; index < chats.length; index++) {
      chats[index] = ChatProcessor.process(chats[index], this.sharedSecret);
    }
    return chats;
  }
  
  setChatScrollStartFromBottom() {
    timer(1).subscribe(res => {
      const chatContainer = this.elementRef.nativeElement.querySelector('.chat-container');
      chatContainer.scrollTop = chatContainer.scrollHeight - chatContainer.clientHeight;
    });
  }


  onChatScroll(event: any): void {
    console.log("clientHeight : " + event.target.clientHeight + "\nscrolltop : " + event.target.scrollTop + "\nscrollheight : " + event.target.scrollHeight);
    if( event.target.scrollTop < event.target.clientHeight && this.canExecuteChatQuery) {
      this.getChats(this.query.getNextPaginationQuery());
    }
  }

  onClickSendMessage() {
    console.log(this.inputMessage);
    const sendMessageCommand = new SendMessageCommand();
    sendMessageCommand.message = this.inputMessage;
    sendMessageCommand.sendTo = this.sendToUserId;
    sendMessageCommand.isGroupMessage = this.isGroup;
    this.inputMessage = '';
    this.commandService.execute(sendMessageCommand)
    .pipe(take(1))
    .subscribe(response => {
      console.log('Message Received', response);
    });
  }

  getLastSeenStatus() {
    const lastSeenQuery = new LastSeenQuery();
    lastSeenQuery.userIds = [this.sendToUserId];
    this.queryService.execute(lastSeenQuery)
    .pipe(take(1))
    .subscribe(response => {
      this.lastSeen = response.value[0].status;
      this.isActive = response.value[0].isActive;
    });
  }

  onClickUserProfile() {
    const userId = this.sendToUserId;
    this.router.navigateByUrl('/user/' + userId);
  }

  getGroupChatTopic() {
    return "GroupChat-" + this.groupId;
  }

  getUserChatTopic() {
    const ids = [this.currentUserId, this.sendToUserId];
    const sortedIds = ids.sort((n1,n2) => {
      if (n1 > n2) {
        return 1;
      }
      if (n1 < n2) {
        return -1;
      }
      return 0;
    });
    return "UserChat-" + sortedIds[0] + "-" + sortedIds[1];
  }
}
