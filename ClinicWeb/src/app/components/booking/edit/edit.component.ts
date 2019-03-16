import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { ClinicModel, ClinicianModel } from 'src/app/core/models';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicianService } from 'src/app/core/services/clinician/clinician.service';
import { Pagination } from 'src/app/core/models/table/pagination.model';
import { LocationService } from 'src/app/core/services/location.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.styl']
})
export class EditComponent implements OnInit, OnChanges {
  public editForm: FormGroup;
  public clinics: ClinicModel[];
  public clinicsPaging: Pagination = new Pagination();
  public clinicians: ClinicianModel[] = [];
  public loadClinicsOptionId = 'loadClinics';
  private currentClinic: ClinicModel;
  private clinicsPageSize = 10;
  private clientLongitude= 0;
  private clientLatitude = 0;

  @Input('visibility') visibility = false;
  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();
  @Input('new') public isNewBooking = false;
  @Input('isPatient') public isPatient = true;
  @Output('closeEditWindow') public closeEditWindow = new EventEmitter<any>();
  @Output('editCompleted') public onEditCompleted = new EventEmitter<any>();

  constructor(
    private clinicianService: ClinicianService,
    private clinicService: ClinicService,
    private bookingService: BookingService,
    private locationService: LocationService) { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.model) {
      this.createForm();
    }
  }

  public ngOnInit(): void {
    this.initializeLocation();
    this.initializeForm();
  }

  public onSubmit(): void {
    if (this.canSubmit()) {
      if (this.isPatient) {
        this.setValuesFromFormToModel();
      }
      this.isNewBooking
        ? this.onEditCompleted.emit(this.model)
        : this.bookingService.updateBookings(this.model)
          .subscribe(result => {
            if (result.Data !== null) {
              this.currentClinic = {
                Id : result.Data.ClinicId,
                Name : result.Data.ClinicName,
              };
              this.onEditCompleted.emit(result.Data);
            }
          });
    }
  }

  public canSubmit(): boolean {
    return !this.isPatient || this.editForm.valid;
  }

  public filterClinicians(clinicId: any): void {
    clinicId !== this.loadClinicsOptionId
      ? this.clinicianService.getAllClinic(clinicId)
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
    this.model.reciept = values.reciept;
    this.model.clinicId = values.clinic;
    this.model.clinicianId = values.clinician;
  }

  private initializeForm(): void {
    this.clinicsPaging.pageNumber = 0;
    this.clinicsPaging.pageCount = this.clinicsPageSize;
    
    this.updateClinics(this.clientLongitude, this.clientLatitude);

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

  private createForm(): void {
    const bookingClinic = this.clinics
        ? this.clinics.find(c => c.Id === this.model.clinicId)
        : null;
    const bookingClinician = this.clinicians
      ? this.clinicians.find(c => c.Id === this.model.clinicianId)
      : null;

    this.editForm = new FormGroup({
      reciept: new FormControl(this.model.reciept, [Validators.required]),
      name: new FormControl(this.model.name, [Validators.required]),
      clinic: new FormControl(bookingClinic ? bookingClinic.Id : null, [Validators.required]),
      clinician: new FormControl(bookingClinician ? bookingClinician.Id : null, [Validators.required])
    });
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
          this.clinicianService.getAllClinic(this.model.clinicId)
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
