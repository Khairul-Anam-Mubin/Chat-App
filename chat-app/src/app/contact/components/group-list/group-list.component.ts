import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommandService } from 'src/app/core/services/command-service';
import { AuthService } from 'src/app/identity/services/auth.service';
import { AddContactCommand } from '../../commands/add-contact-command';
import { UserService } from 'src/app/identity/services/user.service';
import { take } from 'rxjs';
import { ResponseStatus } from 'src/app/core/constants/response-status';
import { ContactQuery } from '../../queries/contact-query';
import { QueryService } from 'src/app/core/services/query-service';
import { AcceptOrRejectContactRequestCommand } from '../../commands/accept-or-reject-contact-request-command';
import { LastSeenQuery } from 'src/app/activity/queries/last-seen-query';
import { UserGroupsQuery } from '../../queries/user-groups-query';
import { ChatService } from 'src/app/chat/services/chat-service';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponent implements OnInit {

  items : any;
  tabs : any;
  activeTabIndex : any = 0;
  currentUserId : any;

  constructor(
    private userService : UserService,
    private commandService: CommandService,
    private queryService : QueryService,
    private chatService: ChatService,
    private router: Router) {}

  ngOnInit(): void {
    this.currentUserId = this.userService.getCurrentUserId();
    this.tabs = [
      {
        'id': 0,
        'key': "Groups",
        'title': "Groups",
        'isActive' : true
      }
    ];
    this.getGroups();
  }

  getGroups() {
    this.queryService.execute(this.getGroupQuery())
    .pipe(take(1))
    .subscribe(response => {
      this.items = response.value;
    });
  }

  onClickTab(idx : any) {
    this.activeTabIndex = idx;
    for (let i = 0; i < this.tabs.length; i++) {
      if (i === idx) {
        this.tabs[i].isActive = true;
        console.log("(onClickTab) selected index %d", idx, this.tabs[i]);
        continue;
      }
      this.tabs[i].isActive = false;
    }
  }

  onClickedItem(item : any) {
    console.log("clicked", item);
    this.chatService.openChat({
      id: item.id,
      type: 'GroupChat'
    });
  }

  getGroupQuery() {
    const groupQuery = new UserGroupsQuery();
    groupQuery.userId = this.userService.getCurrentUserId();
    return groupQuery;
  }
}
