import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { ClinicianRegistrationModel } from 'src/app/core/models/auth/registration/clinician-registration.model';
import { ClinicModel } from 'src/app/core/models';
import { Pagination } from 'src/app/core/models/table/pagination.model';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { AccountService } from 'src/app/core/services/auth/account.service';
import { ToastNotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-clinician-registration',
  templateUrl: './clinician-registration.component.html',
  styleUrls: ['./clinician-registration.component.styl']
})
export class ClinicianRegistrationComponent implements OnInit {
  @Input('model') clinicianModel: ClinicianRegistrationModel;
  @Input('returnUrl') public returnUrl: string = null;
  @Output('cancel') public cancelRegistrtion = new EventEmitter<any>();
  public selectedClinics: Array<ClinicModel> = [];
  public clinics: Array<ClinicModel> = [];
  public clinicsPaging: Pagination = new Pagination();
  private clinicsPageSize = 10;

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private clinicService: ClinicService,
    private accountService: AccountService,
    private notificationService: ToastNotificationService) { }

  public ngOnInit(): void {
    this.initializeClinics();
  }

  public selectClinic(clinic: ClinicModel): void {
    if (!this.selectedClinics.find(c => c.Id === clinic.Id)) {
      this.selectedClinics.push(clinic);
    }
  }

  public onSubmit(): void {
    if (this.selectedClinics.length >= 1) {
      this.clinicianModel.ClinicsId = this.selectedClinics.map(cl => cl.Id);
      this.tokenService.removeTokens();
      this.accountService.registerClinician(this.clinicianModel)
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

  public uploadClinics(): void {
    this.clinicsPaging.pageNumber += 1;
    window.navigator.geolocation.getCurrentPosition(location => {
      this.clinicService.getAllClinic(this.clinicsPaging, location.coords.longitude, location.coords.longitude)
        .subscribe(result => {
          if (result.Data != null) {
            this.clinics.push(...result.Data);
          }
        });
    });
  }

  public removeSelectedClinic(index: number): void {
    this.selectedClinics.splice(index, 1);
  }

  public movePreviousStep(): void {
    this.cancelRegistrtion.emit();
  }

  private initializeClinics(): void {
    this.clinicsPaging.pageNumber = 0;
    this.clinicsPaging.pageCount = this.clinicsPageSize;

    window.navigator.geolocation.getCurrentPosition(location => {
        this.updateClinics(location.coords.longitude, location.coords.latitude);
      },
      () => {
        this.updateClinics();
      }
    );
  }

  private updateClinics(long = 0, lat = 0): void {
    this.clinicService.getAllClinic(this.clinicsPaging, long, lat)
      .subscribe(result => {
        if (result.Data !== null) {
          this.clinics = result.Data;
        }
      });
  }
}
