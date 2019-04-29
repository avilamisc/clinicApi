import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';

import { FormValidationService } from 'src/app/core/services/validation.service';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { ResetPasswordModel } from 'src/app/core/models';
import { ValidationMessages } from 'src/app/utilities/validation-messages';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { AccountService } from 'src/app/core/services/auth/account.service';
import { MustMatch } from 'src/app/utilities/validators/must-match.validator';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.styl']
})
export class ResetPasswordComponent implements OnInit {
  public formErrors = {};
  public submitTouched = false;
  public updateForm: FormGroup;
  public resetModel = new ResetPasswordModel();
  private formValidationService = new FormValidationService();

  constructor(
    private tokenService: TokenService,
    private accountService: AccountService,
    private toastNotification: ToastNotificationService,
  ) { }

  public ngOnInit(): void {
    this.createForm();
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.updateForm.valid) {
      this.setValuesFromFormToModel();
      this.resetModel.RefreshToken = this.tokenService.getRefreshToken();
      this.accountService.resetPassword(this.resetModel)
        .subscribe(result => {
          if (result.Data) {
            this.toastNotification.successMessage('You`ve successfully update your password!');
          } else {
            this.toastNotification.showApiErrorMessage(result);
          }
        });
    } else {
      console.log('ERRORS: ', this.formErrors);
      this.formValidationService.markFormGroupTouched();
      this.toastNotification.validationWarning();
      this.validateForm();
      console.log('ERRORS: ', this.formErrors);
    }
  }

  private validateForm(): void {
    console.log('validate form');
    this.formErrors = this.formValidationService.validateForm();
    console.log(this.formErrors);
  }

  private createForm(): void {
    this.updateForm = new FormGroup({
      oldPassword: new FormControl('', [Validators.required]),
      newPassword: new FormControl('', [Validators.required]),
      repeatPassword: new FormControl('', [Validators.required]),
    }, { validators: MustMatch('newPassword', 'repeatPassword') });
    this.formValidationService.setFormData(this.updateForm, ValidationMessages.ResetPassword);
    this.updateForm.valueChanges.subscribe(() => this.validateForm());
  }

  private setValuesFromFormToModel(): void {
    const values = this.updateForm.getRawValue();
    this.resetModel.OldPassword = values.oldPassword;
    this.resetModel.NewPassword = values.newPassword;
  }
}
