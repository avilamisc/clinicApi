import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { ClinicModel, ClinicianModel, Stage } from 'src/app/core/models';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicianService } from 'src/app/core/services/clinician/clinician.service';
import { Pagination } from 'src/app/core/models/table/pagination.model';
import { LocationService } from 'src/app/core/services/location.service';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';
import { ToastNotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.styl']
})
export class EditComponent implements OnInit, OnChanges {
  public editForm: FormGroup;
  public clinicsPaging: Pagination = new Pagination();
  public loadClinicsOptionId = 'loadClinics';
  public fixedClinicList = false;
  public formErrors: any = {};
  public submitTouched = false;
  private currentClinic: ClinicModel;
  private clinicsPageSize = 10;
  private clientLongitude = 0;
  private clientLatitude = 0;

  @Input('clinics') public clinics: ClinicModel[];
  @Input('clinicians') public clinicians: ClinicianModel[] = [];
  @Input('visibility') visibility = false;
  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();
  @Input('new') public isNewBooking = false;
  @Input('isPatient') public isPatient = true;
  @Input('isClinician') public isClinician;
  @Input('isClinic') public isClinic;
  @Output('closeEditWindow') public closeEditWindow = new EventEmitter<any>();
  @Output('editCompleted') public onEditCompleted = new EventEmitter<any>();

  constructor(
    private clinicService: ClinicService,
    private bookingService: BookingService,
    private locationService: LocationService,
    private clinicianService: ClinicianService,
    private formValidationService: FormValidationService,
    private notificationService: ToastNotificationService) { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.model) {
      this.createForm();
    }
  }

  public ngOnInit(): void {
    this.initializeLocation();
    this.initializeForm();
  }

  public get canEditName(): boolean {
    return this.isNewBooking || (this.isPatient && this.model.stage === Stage.Send);
  }

  public get canEditHealthInfo(): boolean {
    return this.isClinician &&
            (this.model.stage === Stage.Send ||
             this.model.stage === Stage.Confirmed ||
             this.model.stage === Stage.InProgress);
  }

  public get canViewHealthInfo(): boolean {
    return !this.isPatient ||
              (this.isPatient &&
              !this.isNewBooking &&
               this.model.stage !== Stage.Send &&
               this.model.stage !== Stage.Confirmed);
  }

  public get canEditDescription(): boolean {
    return this.isNewBooking || (this.isPatient && this.model.stage === Stage.Send);
  }

  public get canEditClinicClinician(): boolean {
    return this.isNewBooking && this.isPatient;
  }

  public get canEditDocuments(): boolean {
    return this.isNewBooking ||
            (this.model.stage === Stage.Send ||
             this.model.stage === Stage.InProgress ||
             this.model.stage === Stage.Confirmed);
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.canSubmit()) {
      this.setValuesFromFormToModel();
      this.isNewBooking
        ? this.onEditCompleted.emit(this.model)
        : this.bookingService.updateBookings(this.model)
          .subscribe(result => {
            if (result.Data !== null) {
              this.notificationService.successMessage(
                `You have successfuly update booking ${this.model.name}`);
              this.currentClinic = {
                Id : result.Data.ClinicId,
                Name : result.Data.ClinicName,
              };
              this.onEditCompleted.emit(result.Data);
            } else {
              this.notificationService.showApiErrorMessage(result);
            }
          });
    } else {
      this.formValidationService.markFormGroupTouched();
      this.notificationService.validationWarning();
      this.validateForm();
    }
  }

  public canSubmit(): boolean {
    return !this.isPatient || this.editForm.valid;
  }

  public filterClinicians(clinicId: any): void {
    clinicId !== this.loadClinicsOptionId
      ? this.clinicianService.getAllClinician(clinicId)
          .subscribe(result => {
            if (result.Data !== null) {
              this.clinicians = result.Data;
              this.editForm.get('clinician').markAsUntouched();
            }
          })
      : this.uploadClinics();
  }

  public changeVisibility(visibility: boolean): void {
    this.visibility = visibility;
    if (this.visibility === false) {
      this.closeEditWindow.emit();
    }
  }

  public uploadClinics(): void {
    this.clinicsPaging.pageNumber += 1;
    this.clinicService.getAllClinic(this.clinicsPaging, this.clientLongitude, this.clientLatitude)
      .subscribe(result => {
        if (result.Data != null) {
          this.clinics.push(...result.Data);
        }
      });
  }

  private setValuesFromFormToModel(): void {
    const values = this.editForm.getRawValue();
    this.model.name = values.name;
    this.model.HeartRate = values.heartRate;
    this.model.Weight = values.weight;
    this.model.Height = values.height;
    this.model.PatientDescription = values.patientDescription;
    this.model.clinicId = values.clinic;
    this.model.clinicianId = values.clinician;
  }

  private initializeForm(): void {
    if (this.clinics && this.clinics.length >= 1) {
      this.fixedClinicList = true;
      if (this.clinics.length === 1) {
        const clinicId = this.clinics[0].Id;
        this.editForm.controls['clinic'].setValue(clinicId);
        this.filterClinicians(clinicId);
      }
    } else {
      this.fixedClinicList = false;
      this.clinicsPaging.pageNumber = 0;
      this.clinicsPaging.pageCount = this.clinicsPageSize;

      this.updateClinics(this.clientLongitude, this.clientLatitude);

      if (this.model.clinicId) {
        this.clinicService.getClinicById(this.model.clinicId)
          .subscribe(result => {
            if (result.Data !== null) {
              this.currentClinic = result.Data;
              if (this.clinics) {
                this.clinics.push(this.currentClinic);
              }
              this.createForm();
            }
          });
      }
    }
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
  }

  private createForm(): void {
    const bookingClinic = this.clinics
        ? this.clinics.find(c => c.Id === this.model.clinicId)
        : null;
    const bookingClinician = this.clinicians
      ? this.clinicians.find(c => c.Id === this.model.clinicianId)
      : null;

    this.editForm = new FormGroup({
      heartRate: new FormControl(this.model.HeartRate),
      height: new FormControl(this.model.Height),
      weight: new FormControl(this.model.Weight),
      patientDescription: new FormControl(this.model.PatientDescription),
      name: new FormControl(this.model.name, [Validators.required]),
      clinic: new FormControl(bookingClinic ? bookingClinic.Id : null, [Validators.required]),
      clinician: new FormControl(bookingClinician ? bookingClinician.Id : null, [Validators.required])
    });
    this.formValidationService.setFormData(this.editForm, ValidationMessages.Booking);
    this.editForm.valueChanges.subscribe(() => this.validateForm());
  }

  private updateClinics(long = 0, lat = 0): void {
    this.clinicService.getAllClinic(this.clinicsPaging, long, lat)
      .subscribe(result => {
        if (result.Data !== null) {
          this.clinics = result.Data;
          if (this.currentClinic) {
            this.clinics.push(this.currentClinic);
          }
        }
        if (this.model.clinicId) {
          this.clinicianService.getAllClinician(this.model.clinicId)
            .subscribe(clinicianResult => {
                if (clinicianResult.Data !== null) {
                  this.clinicians = clinicianResult.Data;
                }
                this.createForm();
              });
        } else {
          this.createForm();
        }
      });
  }

  private initializeLocation(): void {
    if (this.locationService.userLatitude === null
          || this.locationService.userLongitude === null) {
      this.locationService.initializeLocation();
    }

    this.clientLongitude = this.locationService.userLongitude;
    this.clientLatitude = this.locationService.userLatitude;
  }
}
