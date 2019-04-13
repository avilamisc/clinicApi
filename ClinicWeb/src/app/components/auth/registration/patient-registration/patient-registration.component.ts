import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { PatientRegistrationModel } from 'src/app/core/models/auth/registration/patient-registration.model';
import { AccountService } from 'src/app/core/services/auth/account.service';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';

@Component({
  selector: 'app-patient-registration',
  templateUrl: './patient-registration.component.html',
  styleUrls: ['./patient-registration.component.styl']
})
export class PatientRegistrationComponent implements OnInit {
  @Input('model') public patientModel: PatientRegistrationModel;
  @Input('returnUrl') public returnUrl: string = null;
  @Output('cancel') public cancelRegistrtion = new EventEmitter<any>();
  public registerForm: FormGroup;
  public formErrors = {};
  public submitTouched = false;
  public minDateFrom = new Date(1919, 1, 1);
  public maxDateFrom = new Date(Date.now());

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private accountService: AccountService,
    private formValidationService: FormValidationService,
    private notificationService: ToastNotificationService) { }

  public ngOnInit(): void {
    this.createForm();
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.registerForm.valid) {
      this.setValuesFromFormToModel();
      this.tokenService.removeTokens();
      this.accountService.registerPatient(this.patientModel)
        .subscribe(result => {
          if (result.Data) {
            this.router.navigate([this.returnUrl || '/booking']);
            this.notificationService.successRegistration();
          } else {
            this.notificationService.showErrorMessage(result.ErrorMessage, result.StatusCode);
          }
        });
    } else {
      this.formValidationService.markFormGroupTouched();
      this.notificationService.validationWarning();
      this.validateForm();
    }
  }

  public movePreviousStep(): void {
    this.cancelRegistrtion.emit();
  }
  
  public onSelectBornDate(bornDate: Date): void {
    if (bornDate) {
      this.registerForm.get('bornDate').patchValue(bornDate, { emitEvent: false });
    }
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
  }

  private createForm(): void {
    this.registerForm = new FormGroup({
      bornDate: new FormControl(this.patientModel.BornDate as Date, [Validators.required])
    });
    this.formValidationService.setFormData(this.registerForm, ValidationMessages.RegistrationPatient);
    this.registerForm.valueChanges.subscribe(() => this.validateForm());
  }

  private setValuesFromFormToModel(): void {
    const values = this.registerForm.getRawValue();

    this.patientModel.BornDate = values.bornDate;
  }
}
