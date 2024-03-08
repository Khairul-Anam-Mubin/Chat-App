import { Component, OnInit } from '@angular/core';
import { RegisterCommand } from '../../commands/register-command';
import { CommandService } from 'src/app/core/services/command-service';
import { take } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { ResponseStatus } from 'src/app/core/constants/response-status';
import { Router } from '@angular/router';
import { SecurtiyService } from 'src/app/core/services/security-service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{

  registerFormControl = this.fb.group({
    firstName : ['', [Validators.required, Validators.pattern('[a-zA-z ]*')]],
    lastName : ['', [Validators.required, Validators.pattern('[a-zA-z ]*')]],
    birthDay : ['', Validators.required],
    email : ['', [Validators.required, Validators.email]],
    password : ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword : ['', [Validators.required, Validators.minLength(6)]],
  });

  constructor(
    private commandService: CommandService,
    private router: Router,
    private fb: FormBuilder,
    private securityService: SecurtiyService) {}

  ngOnInit() {

  }

  async onSubmit() {
    this.securityService.createAndSavePrivateKey(this.getFormValue('password'));
    const registerCommand = this.getRegisterCommand();
    this.commandService.execute(registerCommand).pipe(take(1)).subscribe(response => {
      console.log(response);
      if (response.status === ResponseStatus.success) {
        this.router.navigateByUrl("log-in");
      }
    });
  }

  getRegisterCommand() {
    const registerCommand = new RegisterCommand();
    registerCommand.firstName = this.getFormValue('firstName');
    registerCommand.lastName = this.getFormValue('lastName');
    registerCommand.birthDay = this.getFormValue('birthDay');
    registerCommand.email = this.getFormValue('email');
    registerCommand.password = this.getFormValue('password');
    return registerCommand;
  }

  getFormValue(key : string) {
    return this.registerFormControl.get(key)?.value?.toString();
  }
}
