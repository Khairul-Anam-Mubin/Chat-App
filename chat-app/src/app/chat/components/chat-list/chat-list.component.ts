import { Component, OnInit } from '@angular/core';
import { QueryService } from 'src/app/core/services/query-service';
import { ChatListQuery } from '../../queries/chat-list-query';
import { UserService } from 'src/app/identity/services/user.service';
import { take } from 'rxjs';
import { Router } from '@angular/router';
import { UpdateChatsStatusCommand } from '../../commands/update-chats-status-command';
import { CommandService } from 'src/app/core/services/command-service';
import { SecurtiyService } from 'src/app/core/services/security-service';
import { ChatProcessor } from '../../helpers/chat-processor';
import { GroupService } from 'src/app/contact/services/group-service';

@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.css']
})
export class ChatListComponent implements OnInit{

  items : any;
  chatList : any;
  chatUsers : any;
  groups: any[] = [];

  constructor(
    private commandService: CommandService,
    private queryService : QueryService,
    private userService : UserService,
    private router : Router,
    private securityService : SecurtiyService,
    private groupService: GroupService) {

  }

  ngOnInit(): void {
    var chatLstQuery = new ChatListQuery();
    chatLstQuery.userId = this.userService.getCurrentUserId();
   this.queryService.execute(chatLstQuery)
   .pipe(take(1))
   .subscribe(response => {
    this.items = response.value.items;
    let userIds = [];
    let groupIds = [];
    for (let i = 0; i < this.items.length; i++) {
      if (this.items[i].isGroupMessage)
        groupIds.push(this.items[i].userId);
      else
        userIds.push(this.items[i].userId);
    }
    this.userService.getUserProfilesByIds(userIds)
    .pipe(take(1))
    .subscribe(response => {
      this.chatUsers = response.value.items;
      this.setChatList();
    });
    this.groupService.getGroupsByIds(groupIds).pipe(take(1)).subscribe(response => {
      this.groups = response.value;
      this.setChatList();
    });
   });
  }

  setChatList() {
    if (!this.items || this.items.length === 0) return;
    this.chatList = [];
    for (let i = 0; i < this.items.length; i++) {
      if (this.items[i].isGroupMessage) {
        const group = this.getGroup(this.items[i].userId);
        const sharedSecret = this.securityService.getSharedSecretKey(123);
        this.items[i] = ChatProcessor.process(this.items[i], sharedSecret);
        let chat = {
          'latestMessage' : this.items[i].content,
          'durationDisplayTime' : this.items[i].durationDisplayTime,
          'chatName' : group?.name,
          'userId' : this.items[i].userId,
          'isReceiver' : this.items[i].isReceiver,
          'occurrence' : this.items[i].occurrence,
          'isGroupMessage' : true
        };
        this.chatList.push(chat);
      } else {
        const userProfile = this.getChatUser(this.items[i].userId);
        const sharedSecret = this.securityService.getSharedSecretKey(userProfile?.publicKey);
        this.items[i] = ChatProcessor.process(this.items[i], sharedSecret);
        let chat = {
          'latestMessage' : this.items[i].content,
          'durationDisplayTime' : this.items[i].durationDisplayTime,
          'chatName' : userProfile.firstName + " " + userProfile.lastName,
          'userId' : this.items[i].userId,
          'isReceiver' : this.items[i].isReceiver,
          'occurrence' : this.items[i].occurrence
        };
        this.chatList.push(chat);
      }
    }
    console.log(this.chatList);
  }

  onClickChat(chat: any) {
    console.log("clicked", chat);
    var url = "chat/" + chat.userId;
    if (chat.isGroupMessage) {
      url = "group/chat/" + chat.userId;
    }
    if (chat.occurrence === 0) {
      this.router.navigateByUrl(url);
      return;
    }
    var updateChatsStatusCommand = new UpdateChatsStatusCommand();
    updateChatsStatusCommand.userId = this.userService.getCurrentUserId();
    updateChatsStatusCommand.openedChatUserId = chat.userId;
    this.commandService.execute(updateChatsStatusCommand)
    .pipe(take(1))
    .subscribe(response => {
      this.router.navigateByUrl(url);
    });
  }

  getChatUser(userId : any) {
    for (let i = 0; i < this.chatUsers.length; i++) {
      if (this.chatUsers[i].id === userId) return this.chatUsers[i];
    }
    return null;
  }

  getGroup(id: any) {
    for (let i = 0; i < this.groups.length; i++) {
      if (this.groups[i].id === id) return this.groups[i];
    }
    return null;
  }

  onChatListScroll($event : any) {
    console.log($event);
  }
}
