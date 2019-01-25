import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { ClinicModel, ClinicianModel } from 'src/app/core/models';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicianService } from 'src/app/core/services/clinician/clinician.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.styl']
})
export class EditComponent implements OnInit, OnChanges {
  public editForm: FormGroup;
  public clinics: ClinicModel[];
  public clinicians: ClinicianModel[] = [];
  private currentClinic: ClinicModel;

  @Input('visibility') visibility = false;
  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();
  @Input('new') public isNewBooking = false;
  @Input('isPatient') public isPatient = true;
  @Output('closeEditWindow') public closeEditWindow = new EventEmitter<any>();
  @Output('editCompleted') public onEditCompleted = new EventEmitter<any>();

  constructor(
    private clinicianService: ClinicianService,
    private clinicService: ClinicService,
    private bookingService: BookingService) { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.model) {
      this.createForm();
    }
  }

  public ngOnInit(): void {
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
            if (result.Result !== null) {
              this.currentClinic = {
                Id : result.Result.ClinicId,
                Name : result.Result.ClinicName,
              };
              this.onEditCompleted.emit(result.Result);
            }
          });
    }
  }

  public canSubmit(): boolean {
    return !this.isPatient || this.editForm.valid;
  }

  public filterClinicians(clinicId: number): void {
    this.clinicianService.getAllClinic(clinicId)
      .subscribe(result => {
        if (result.Result !== null) {
          this.clinicians = result.Result;
          this.editForm.get('clinician').markAsUntouched();
        }
      });
  }

  public changeVisibility(visibility: boolean): void {
    this.visibility = visibility;
    if (this.visibility === false) {
      this.closeEditWindow.emit();
    }
  }

  private setValuesFromFormToModel(): void {
    const values = this.editForm.getRawValue();
    this.model.name = values.name;
    this.model.reciept = values.reciept;
    this.model.clinicId = values.clinic;
    this.model.clinicianId = values.clinician;
  }

  private initializeForm(): void {
    window.navigator.geolocation.getCurrentPosition(location => {
      this.clinicService.getAllClinic(location.coords.longitude, location.coords.longitude)
        .subscribe(result => {
          if (result.Result !== null) {
            this.clinics = result.Result;
            if (this.currentClinic) {
              this.clinics.push(this.currentClinic);
            }
          }
          if (this.model.clinicId) {
            this.clinicianService.getAllClinic(this.model.clinicId)
              .subscribe(clinicianResult => {
                  if (clinicianResult.Result !== null) {
                    this.clinicians = clinicianResult.Result;
                  }
                  this.createForm();
                });
          } else {
            this.createForm();
          }
        });
      });

      this.clinicService.getClinicById(this.model.clinicId)
        .subscribe(result => {
          if (result.Result !== null) {
            this.currentClinic = result.Result;
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
}
