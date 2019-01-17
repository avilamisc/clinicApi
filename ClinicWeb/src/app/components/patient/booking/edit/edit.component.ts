import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { PatientBookingModel } from 'src/app/core/models';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.styl']
})
export class EditComponent implements OnInit, OnChanges {
  public editForm: FormGroup;
  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();
  @Input('new') public isNewBooking = false;
  @Output('editCompleted') public onEditCompleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(private bookingService: BookingService) { }

  ngOnChanges(changes: SimpleChanges) {
      if (changes.model) {
          this.createForm();
      }
  }

  public ngOnInit(): void {
    this.createForm();
  }

  public onSubmit(): void {
    this.setValuesFromFormToModel();
    if (this.isNewBooking) {
      this.onEditCompleted.emit(this.model);
    } else {
      this.bookingService.updateBookings(this.model)
        .subscribe(result => {
          if (result.Result !== null) {
            this.onEditCompleted.emit(result.Result);
          }
        });
    }
  }

  private createForm(): void {
    this.editForm = new FormGroup({
      reciept: new FormControl(this.model.reciept, [Validators.required]),
      name: new FormControl(this.model.name, [Validators.required]),
      clinicId: new FormControl(this.model.clinicId, [Validators.required]),
      clinicianId: new FormControl(this.model.clinicianId, [Validators.required])
    });
  }

  private setValuesFromFormToModel(): void {
    const values = this.editForm.getRawValue();

    this.model.name = values.name;
    this.model.reciept = values.reciept;
    this.model.clinicId = values.clinicId;
    this.model.clinicianId = values.clinicianId;
  }
}
