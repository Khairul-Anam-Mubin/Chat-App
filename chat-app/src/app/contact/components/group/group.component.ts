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
import { GroupService } from '../../services/group-service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {
  groupId: any;
  group: any;
  contacts: any[] = [];
  groupMembers: any[] = [];

  constructor(private queryService: QueryService,
    private userService: UserService,
    private groupService: GroupService,
    private router: Router) {}

  ngOnInit(): void {
    this.groupId = this.groupService.getCurrentOpenedGroupId();
    this.groupService.getGroupsByIds([this.groupId]).pipe(take(1)).subscribe(response => {
      this.group = response.value[0];
    });

    this.queryService.execute(this.getContactQuery()).pipe(take(1)).subscribe(response => {
      this.contacts = response.value.items;
      const userIds = [];
      for (let i = 0; i < this.contacts.length; i++) {
        userIds.push(this.contacts[i].contactUserId);
      }
      this.userService.getUserProfilesByIds(userIds)
      .pipe(take(1))
      .subscribe(response => {
        const userProfiles = response.value.items;
        for (let i = 0; i < this.contacts.length; i++) {
          for (let j = 0; j < userProfiles.length; j++) {
            if (this.contacts[i].contactUserId === userProfiles[i].id) {
              this.contacts[i].email = userProfiles[i].email;
              this.contacts[i].name = userProfiles[i].firstName + " " + userProfiles[i].lastName;
              break;
            }
          }
        }
      });
    });

    this.groupService.getGroupMembers(this.groupId).pipe(take(1)).subscribe(response => {
      this.groupMembers = response.value;
      const userIds = [];
      for (let i = 0; i < this.groupMembers.length; i++) {
        userIds.push(this.groupMembers[i].memberId);
      }
      this.userService.getUserProfilesByIds(userIds)
      .pipe(take(1))
      .subscribe(response => {
        const userProfiles = response.value.items;
        for (let i = 0; i < this.groupMembers.length; i++) {
          for (let j = 0; j < userProfiles.length; j++) {
            if (this.groupMembers[i].memberId === userProfiles[i].id) {
              this.groupMembers[i].email = userProfiles[i].email;
              this.groupMembers[i].name = userProfiles[i].firstName + " " + userProfiles[i].lastName;
              break;
            }
          }
        }
      });
    });
  }

  getContactQuery() {
    const contactQuery = new ContactQuery();
    contactQuery.isPendingContacts = false;
    contactQuery.isRequestContacts = false;
    contactQuery.userId = this.userService.getCurrentUserId();
    return contactQuery;
  }

  onClickContact(contact: any) {
    console.log('Selected To Add Member: ', contact);
    this.groupService.addMemberToGroup(this.groupId, contact.contactUserId, this.userService.getCurrentUserId())
    .pipe(take(1))
    .subscribe(response => {

    });
  }

  onClickStartChat() {
    this.groupService.openGroupChat(this.group);
  }

  onClickGroupMember(groupMember : any) {
    this.router.navigateByUrl('/user/' + groupMember.memberId);
  }
}
