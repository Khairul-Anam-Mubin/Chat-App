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
import { CreateNewGroupCommand } from '../../commands/create-new-group-command';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  addContactFormControl = this.fb.group({
    email : ['', [Validators.required, Validators.email]],
  });
  
  createGroupFormControl = this.fb.group({
    groupName : ['', [Validators.required]],
  });

  items : any;
  contactTabs : any;
  activeTabIndex : any = 0;
  currentUserId : any;
  isContactSelected: boolean = true;
  isGroupSelected: boolean = false;

  constructor(
    private userService : UserService,
    private commandService: CommandService,
    private queryService : QueryService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder) {}

  ngOnInit(): void {
    this.currentUserId = this.userService.getCurrentUserId();
  }
  
  onClickContact() {
    this.isGroupSelected = false;
    this.isContactSelected = true;
  }

  onClickGroup() {
    this.isContactSelected = false;
    this.isGroupSelected = true;
  }

  onSubmitAddContact() {
    this.commandService.execute(this.getAddContactCommand())
    .pipe(take(1))
    .subscribe(response => {

    });
  }
  
  onSubmitCreateGroup() {
    this.commandService.execute(this.getCreateNewGroupCommand())
    .pipe(take(1))
    .subscribe(response => {

    });
  }

  getAddContactCommand() {
    const addContactCommand = new AddContactCommand();
    addContactCommand.contactEmail = this.getFormValue("email");
    addContactCommand.userId = this.userService.getCurrentUserId();
    return addContactCommand;
  }
  getCreateNewGroupCommand() {
    const createNewGroupCommand = new CreateNewGroupCommand();
    createNewGroupCommand.groupName = this.getGroupFormValue("groupName");
    createNewGroupCommand.userId = this.userService.getCurrentUserId();
    return createNewGroupCommand;
  }
  getFormValue(key : string) {
    return this.addContactFormControl.get(key)?.value?.toString();
  }
  getGroupFormValue(key : string) {
    return this.createGroupFormControl.get(key)?.value?.toString();
  }
}
