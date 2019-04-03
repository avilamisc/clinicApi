import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { PatientRegistrationModel } from 'src/app/core/models/auth/registration/patient-registration.model';
import { AccountService } from 'src/app/core/services/auth/account.service';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { ToastNotificationService } from 'src/app/core/services/notification.service';

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

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private accountService: AccountService,
    private notificationService: ToastNotificationService) { }

  public ngOnInit(): void {
    this.createForm();
  }

  public onSubmit(): void {
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
      this.notificationService.validationWarning();
    }
  }

  public movePreviousStep(): void {
    this.cancelRegistrtion.emit();
  }

  private createForm(): void {
    this.registerForm = new FormGroup({
      location: new FormControl(this.patientModel.Location, [Validators.required])
    });
  }

  private setValuesFromFormToModel(): void {
    const values = this.registerForm.getRawValue();

    this.patientModel.Location = values.location;
  }
}
