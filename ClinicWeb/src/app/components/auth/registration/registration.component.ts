import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { RegistrationModel } from 'src/app/core/models/auth/registration/registration.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.styl']
})
export class RegistrationComponent implements OnInit {
  public registrationModel = new RegistrationModel();
  public registerForm: FormGroup;

  private isPatient = true;

  constructor(private router: Router) { }

  ngOnInit() {
    this.createForm();
  }

  public onSubmit(): void {
    if (this.registerForm.valid) {
      this.setValuesFromFormToModel();
      console.log('move (is patient: ', this.isPatient, ')');
      const nextStepRoute = this.isPatient ? '/patient' : '/clinician';
      this.router.navigate([nextStepRoute]);
    } else {
      console.log('denied');
    }
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
      userRole: new FormControl(this.isPatient)
    })
  }

  private setValuesFromFormToModel(): void {
    const values = this.registerForm.getRawValue();
    this.registrationModel.UserName = values.userName + ' ' + values.userSurname;
    this.registrationModel.UserMail = values.userMail;
    this.registrationModel.Password = values.password;
    this.isPatient = values.userRole === 'Patient';
  }
}
