import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { RegistrationModel } from 'src/app/core/models/auth/registration/registration.model';
import { CommonConstants, ValidatorLengths, CommonRegEx } from 'src/app/utilities/common-constants';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { UserRoles } from 'src/app/utilities/user-roles';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.styl']
})
export class RegistrationComponent implements OnInit {
  public registrationModel = new RegistrationModel();
  public registerForm: FormGroup;
  public isMainInfoSubmitted = false;
  public userRole = 'Patient';
  public returnUrl: string = null;
  public formErrors = {};
  public submitTouched = false;
  public userRoles = UserRoles;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private formValidationService: FormValidationService,
    private notificationService: ToastNotificationService) { }

  public ngOnInit(): void {
    this.createForm();
    this.initializeReturnUrl();
  }

  public get isPatient(): boolean {
    return this.userRole === this.userRoles.Patient;
  }

  public get isClinician(): boolean {
    return this.userRole === this.userRoles.Clinician;
  }

  public get isClinic(): boolean {
    return this.userRole === this.userRoles.Clinic;
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.registerForm.valid) {
      this.setValuesFromFormToModel();
      this.isMainInfoSubmitted = true;
    } else {
      this.formValidationService.markFormGroupTouched();
      this.validateForm();
      this.notificationService.validationWarning();
    }
  }

  public showMainRegistration(): void {
    this.isMainInfoSubmitted = false;
  }

  public cancelRegistration(): void {
    this.router.navigate(['login']);
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
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
      userMail: new FormControl(this.registrationModel.UserMail,
        [
          Validators.required,
          Validators.email
        ]),
      password: new FormControl(this.registrationModel.Password,
        [
          Validators.required,
          Validators.minLength(ValidatorLengths.passwordMin),
          Validators.pattern(CommonRegEx.passwordAlphaNumber)
        ]),
      userRole: new FormControl(this.userRole)
    });
    this.formValidationService.setFormData(this.registerForm, ValidationMessages.RegistrationBase);
    this.registerForm.valueChanges.subscribe(() => this.validateForm());
  }

  private setValuesFromFormToModel(): void {
    const values = this.registerForm.getRawValue();
    this.registrationModel.UserName = values.userName + ' ' + values.userSurname;
    this.registrationModel.UserMail = values.userMail;
    this.registrationModel.Password = values.password;
    this.userRole = values.userRole;
  }

  private initializeReturnUrl(): void {
    this.returnUrl = this.route.snapshot.queryParams[CommonConstants.returnUrlSnapshot];
  }
}
