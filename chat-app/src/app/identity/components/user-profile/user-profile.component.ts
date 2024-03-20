import { Component, OnInit } from '@angular/core';
import { QueryService } from 'src/app/core/services/query-service';
import { UserProfileQuery } from '../../queries/user-profile-query';
import { take } from 'rxjs';
import { ResponseStatus } from 'src/app/core/constants/response-status';
import { UserProfile } from '../../models/user-profile';
import { CommandService } from 'src/app/core/services/command-service';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { FileService } from 'src/app/core/services/file-service';
import { UpdateUserProfileCommand } from '../../commands/update-user-profile-command';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit{

  isCurrentUser: boolean = false;
  currentUserId: any;
  isEditable : boolean = false;
  editButtonText : string = "Edit Profile";
  userProfile : UserProfile = new UserProfile();
  profilePictureDetails : any;
  userBlobImageUrl : any = '';
  pictureUploadEnable: boolean = false;

  userProfileFormControl = this.fb.group({
    firstName : ['', [Validators.required, Validators.pattern('[a-zA-z ]*')]],
    lastName : ['', [Validators.required, Validators.pattern('[a-zA-z ]*')]],
    birthDay : ['', Validators.required],
    about : ['', Validators.maxLength(200)],
    email : ['', [Validators.required, Validators.email]]
  });

  constructor(
    private queryService : QueryService,
    private commandService: CommandService,
    private userService: UserService,
    private router: Router,
    private fileService : FileService,
    private fb: FormBuilder) {}

  ngOnInit(): void {
    console.log("[UserProfileComponent] ngOnInit");
    this.currentUserId = this.userService.getCurrentUserId();
    if (this.router.url.includes(this.currentUserId)) {
      this.isCurrentUser = true;
    } else {
      this.isCurrentUser = false;
      this.currentUserId = this.userService.getCurrentOpenedProfileUserId();
    }
    this.getUserProfile(this.currentUserId, false);
  }

  getUserProfile(userId : any, saveToStore: boolean) {
    this.userService.getUserProfileById(userId)
    .pipe(take(1))
    .subscribe(response => {
      this.userProfile = response.value.items[0];
      this.setFormData();
      if (this.userProfile.profilePictureId) {
        this.fileService.getFileModelByFileId(this.userProfile.profilePictureId)
        .pipe(take(1))
        .subscribe(response => {
          console.log(response);
          this.profilePictureDetails = response.value.items[0];
          this.fileService.downloadFile(this.userProfile.profilePictureId)
          .pipe(take(1))
          .subscribe(response => {
            this.userBlobImageUrl = response;
          });
        });
      }
      if (saveToStore) {
        this.userService.setUserProfileToStore(this.userProfile);
      }
    });
  }

  onSubmit() {
    console.log("[UserProfileComponent] onSubmit");
    const userProfile = this.getUserProfileFromFornControl();
    this.updateUserProfileCommand(userProfile);
  }

  onClickEditProfile() {
    console.log("[UserProfileComponent] onClickEditProfile");
    this.isEditable = !this.isEditable;
    if (this.isEditable) this.editButtonText = "Cancel";
    else this.editButtonText = "Edit Profile";
  }

  onClickUpdateProfilePicture() {
    this.pictureUploadEnable = true;
  }

  setFormData() {
    console.log("[UserProfileComponent] setFormData");
    this.userProfileFormControl.setValue({
      firstName: this.userProfile.firstName,
      lastName: this.userProfile.lastName,
      birthDay: this.userProfile.birthDay?  this.userProfile.birthDay.split('T')[0] : '',
      about: this.userProfile.about,
      email: this.userProfile.email
    });
  }

  getUserProfileFromFornControl() {
    let userProfile : any = {};
    userProfile.firstName = this.userProfileFormControl.getRawValue().firstName;
    userProfile.lastName = this.userProfileFormControl.getRawValue().lastName;
    userProfile.birthDay = this.userProfileFormControl.getRawValue().birthDay;
    userProfile.about = this.userProfileFormControl.getRawValue().about;
    userProfile.email = this.userProfileFormControl.getRawValue().email;
    return userProfile;
  }

  updateUserProfileCommand(userProfile : any) {
    const updateUserProfileCommand = new UpdateUserProfileCommand();
    
    updateUserProfileCommand.about = userProfile.about;
    updateUserProfileCommand.birthDay = userProfile.birthDay;
    updateUserProfileCommand.firstName = userProfile.firstName;
    updateUserProfileCommand.lastName = userProfile.lastName;
    updateUserProfileCommand.profilePictureId = userProfile.profilePictureId;
    
    this.commandService.execute(updateUserProfileCommand)
    .pipe(take(1))
    .subscribe(response => {
      if (response && response.status === ResponseStatus.success) {
        this.getUserProfile(this.userProfile.id, true);
      }
    });
  }

  onFileSelected($event: any) {
    console.log($event);
    const file:File = $event.target.files[0];
    this.fileService.uploadFile(file)
    .pipe(take(1))
    .subscribe(response => {
      console.log(response);
      const fileId = response.value;
      const userProfile = new UserProfile();
      userProfile.email = this.userProfile.email;
      userProfile.profilePictureId = fileId;
      this.updateUserProfileCommand(userProfile);
    });
  }
}
