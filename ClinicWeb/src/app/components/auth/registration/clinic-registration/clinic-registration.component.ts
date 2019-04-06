import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AccountService } from 'src/app/core/services/auth/account.service';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { ClinicRegistrationModel } from 'src/app/core/models';
import { LocationService } from 'src/app/core/services/location.service';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';

@Component({
  selector: 'app-clinic-registration',
  templateUrl: './clinic-registration.component.html',
  styleUrls: ['./clinic-registration.component.styl']
})
export class ClinicRegistrationComponent implements OnInit {
  @Input('model') public clinicModel: ClinicRegistrationModel;
  @Input('returnUrl') public returnUrl: string = null;
  @Output('cancel') public cancelRegistrtion = new EventEmitter<any>();
  public registerForm: FormGroup;
  public latitude = 0;
  public longitude = 0;
  public markerIconUrl: string;
  public submitTouched = false;
  public formErrors = {};

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private accountService: AccountService,
    private locationService: LocationService,
    private formValidationService: FormValidationService,
    private notificationService: ToastNotificationService) { }

  public ngOnInit(): void {
    this.initializeLocation()
    this.createForm();
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.registerForm.valid) {
      this.setValuesFromFormToModel();
      this.tokenService.removeTokens();
      this.accountService.registerClinic(this.clinicModel)
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
      this.validateForm();
      this.notificationService.validationWarning();
    }
  }

  public movePreviousStep(): void {
    this.cancelRegistrtion.emit();
  }

  public updateLocation(lat: number, long: number): void {
    this.longitude = long;
    this.latitude = lat;
  }

  private createForm(): void {
    this.registerForm = new FormGroup({
      name: new FormControl(this.clinicModel.Name, [Validators.required]),
      city: new FormControl(this.clinicModel.City, [Validators.required])
    });
    this.formValidationService.setFormData(this.registerForm, ValidationMessages.RegistrationClinic);
    this.registerForm.valueChanges.subscribe(() => this.validateForm());
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
  }

  private setValuesFromFormToModel(): void {
    const values = this.registerForm.getRawValue();

    this.clinicModel.Name = values.name;
    this.clinicModel.City = values.city;
    this.clinicModel.Lat = this.latitude;
    this.clinicModel.Long = this.longitude;
  }

  private initializeLocation(): void {
    this.markerIconUrl = this.locationService.getMapIconUrl();

    if (this.locationService.userLatitude === null
      || this.locationService.userLongitude === null) {
        this.locationService.initializeLocation();
    }

    this.longitude = this.locationService.userLongitude;
    this.latitude = this.locationService.userLatitude;
  }
}
