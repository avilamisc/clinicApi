import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { RegistrationModel } from 'src/app/core/models/auth/registration/registration.model';
import { CommonConstants } from 'src/app/utilities/common-constants';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.styl']
})
export class RegistrationComponent implements OnInit {
  public registrationModel = new RegistrationModel();
  public registerForm: FormGroup;
  public isMainInfoSubmitted = false;
  public isPatient = true;
  public returnUrl: string = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.createForm();
    this.initializeReturnUrl();
  }

  public onSubmit(): void {
    if (this.registerForm.valid) {
      this.setValuesFromFormToModel();
      this.isMainInfoSubmitted = true;
    }
  }

  public showMainRegistration(): void {
    this.isMainInfoSubmitted = false;
  }

  public cancelRegistration(): void {
    this.router.navigate(['login']);
  }

  private createForm(): void {
    const fullName = this.registrationModel.UserName
      ? this.registrationModel.UserName.split(' ') 
      : '';
    const userName = fullName.length === 2 ? fullName[0] : '';
    const userSurname = fullName.length === 2 ? fullName[1] : '';

    this.registerForm = new FormGroup({
      userName: new FormControl(userName, [Validators.required]),
      userSurname: new FormControl(userSurname, [Validators.required]),
      userMail: new FormControl(this.registrationModel.UserMail, [Validators.required]),
      password: new FormControl(this.registrationModel.Password, [Validators.required]),
      userRole: new FormControl(this.isPatient ? 'Patient' : 'Clinician')
    });
  }

  private setValuesFromFormToModel(): void {
    const values = this.registerForm.getRawValue();
    this.registrationModel.UserName = values.userName + ' ' + values.userSurname;
    this.registrationModel.UserMail = values.userMail;
    this.registrationModel.Password = values.password;
    this.isPatient = values.userRole === 'Patient';
  }

  private initializeReturnUrl(): void {
    this.returnUrl = this.route.snapshot.queryParams[CommonConstants.returnUrlSnapshot];
  }
}
