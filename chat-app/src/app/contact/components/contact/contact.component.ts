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

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  addContactFormControl = this.fb.group({
    email : ['', [Validators.required, Validators.email]],
  });

  items : any;
  contactTabs : any;
  activeTabIndex : any = 0;
  currentUserId : any;

  constructor(
    private userService : UserService,
    private commandService: CommandService,
    private queryService : QueryService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder) {}

  ngOnInit(): void {
    this.currentUserId = this.userService.getCurrentUserId();
    this.contactTabs = [
      {
        'id': 0,
        'key': "Contacts",
        'text': "Contacts",
        'isActive' : true
      },
      {
        'id' : 1,
        'key': "Requests",
        'text': "Requests",
        'isActive' : false
      },
      {
        'id' : 2,
        'key': "Pendings",
        'text': "Pendings",
        'isActive' : false
      }
    ];
    this.getContacts();
  }

  getContacts() {
    this.queryService.execute(this.getContactQuery())
    .pipe(take(1))
    .subscribe(response => {
      console.log(response);
      this.items = response.items;
      const contactUserIds = [];
      for (let item of this.items) {
        contactUserIds.push(item.contactUserId);
      }
      this.getLastSeenStatus(contactUserIds);

      if (contactUserIds !== null && contactUserIds.length > 0) {
        this.userService.getUserProfilesByIds(contactUserIds)
          .pipe(take(1))
          .subscribe(response => {
            this.syncContactsWithProfiles(response.items);
          });
      }
    });
  }

  private syncContactsWithProfiles(userProfiles: any) {
    for (let i = 0; i < this.items.length; i++) {
      for (let user of userProfiles) {
        if (user.id === this.items[i].contactUserId) {
          this.items[i].email = user.email;
          // can take others properties
          break;
        }
      }
    }
  }

  getLastSeenStatus(userIds : any) {
    if (userIds === null || userIds.length <= 0) return;
    const lastSeenQuery = new LastSeenQuery();
    lastSeenQuery.userIds = userIds;
    this.queryService.execute(lastSeenQuery)
    .pipe(take(1))
    .subscribe(response => {
      // TODO : need to refactor here
      for (let i = 0; i < this.items.length; i++) {
        for (let item of response.items) {
          if (item.userId === this.items[i].contactUserId) {
            this.items[i].status = item.status;
            this.items[i].isActive = item.isActive;
            break;
          }
        }
      }
    });
  }

  onSubmitAddContact() {
    this.commandService.execute(this.getAddContactCommand())
    .pipe(take(1))
    .subscribe(response => {

    });
  }

  onClickTab(idx : any) {
    this.activeTabIndex = idx;
    for (let i = 0; i < this.contactTabs.length; i++) {
      if (i === idx) {
        this.contactTabs[i].isActive = true;
        this.getContacts();
        console.log("[ContactComponent] (onClickTab) selected index %d", idx, this.contactTabs[i]);
        continue;
      }
      this.contactTabs[i].isActive = false;
    }
  }

  onClickContact(id : any) {
    this.commandService.execute(this.getAcceptOrRejectContactRequestCommand(id))
    .pipe(take(1))
    .subscribe(response => {
      if (response.status === ResponseStatus.success) {
        this.router.navigateByUrl('contact');
      }
    });
  }

  onClickedContact(contact : any) {
    console.log("clicked", contact);
    this.router.navigateByUrl('chat/' + this.getContactUserId(contact));
  }

  getContactUserId(contact: any) {
    return contact.contactUserId;
  }

  getAcceptOrRejectContactRequestCommand(id : any) {
    const acceptOrRejectContactRequestCommand = new AcceptOrRejectContactRequestCommand();
    acceptOrRejectContactRequestCommand.contactId = this.items[id].contactId;
    acceptOrRejectContactRequestCommand.isAcceptRequest = this.activeTabIndex === 1;
    return acceptOrRejectContactRequestCommand;
  }

  getAddContactCommand() {
    const addContactCommand = new AddContactCommand();
    addContactCommand.contactEmail = this.getFormValue("email");
    addContactCommand.userId = this.userService.getCurrentUserId();
    return addContactCommand;
  }

  getContactQuery() {
    const contactQuery = new ContactQuery();
    contactQuery.isPendingContacts = this.activeTabIndex === 2;
    contactQuery.isRequestContacts = this.activeTabIndex === 1;
    contactQuery.userId = this.userService.getCurrentUserId();
    return contactQuery;
  }

  getFormValue(key : string) {
    return this.addContactFormControl.get(key)?.value?.toString();
  }
}
