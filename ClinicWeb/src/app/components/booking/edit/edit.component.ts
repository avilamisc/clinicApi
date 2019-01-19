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
  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();
  @Input('new') public isNewBooking = false;
  @Input('patient') public isPatient = true;
  @Output('editCompleted') public onEditCompleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private clinicianService: ClinicianService,
    private clinicService: ClinicService,
    private bookingService: BookingService) { }

  ngOnChanges(changes: SimpleChanges) {
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

  private setValuesFromFormToModel(): void {
    const values = this.editForm.getRawValue();
    this.model.name = values.name;
    this.model.reciept = values.reciept;
    this.model.clinicId = values.clinic;
    this.model.clinicianId = values.clinician;
  }

  private initializeForm(): void {
    if (this.model.clinicId !== null) {
      this.clinicService.getAllClinic()
        .subscribe(result => {
          if (result.Result !== null) {
            this.clinics = result.Result;
          }

          if (this.model.clinicId !== null) {
            this.clinicianService.getAllClinic(this.model.clinicId)
              .subscribe(clinicianResult => {
                if (clinicianResult.Result !== null) {
                  this.clinicians = clinicianResult.Result;
                }
                this.createForm();
              });
          }
        });
    } else {
      this.createForm();
    }
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
