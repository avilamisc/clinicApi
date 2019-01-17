import { Component, OnInit } from '@angular/core';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { PatientBookingModel } from 'src/app/core/models';
import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';

@Component({
  selector: 'app-patient-booking',
  templateUrl: './patientBooking.component.html',
  styleUrls: ['./patientBooking.component.styl']
})
export class PatientBookingComponent implements OnInit {
  public bookings: PatientBookingModel[] = [];
  public bookingToUpdate: UpdateBookingModel;
  private editedBookingIndex: number;
  private isAddingNewBooking = false;

  constructor(private bookingService: BookingService) { }

  public ngOnInit(): void {
      this.bookingService.getPatientBookings()
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result;
          }
        });
  }

  public updateBoking(booking: PatientBookingModel, index: number): void {
    this.editedBookingIndex = index;
    this.isAddingNewBooking = false;
    this.bookingToUpdate = new UpdateBookingModel();
    this.bookingToUpdate.id = booking.Id;
    this.bookingToUpdate.reciept = booking.Reciept;
    this.bookingToUpdate.name = booking.Name;
    this.bookingToUpdate.clinicId = booking.ClinicId;
    this.bookingToUpdate.clinicianId = booking.ClinicianId;
  }

  public updateEditingModel(newBooking: PatientBookingModel): void {
    if (this.isAddingNewBooking) {
      this.bookingService.createBookings(newBooking)
        .subscribe(result => {
          if (result.Result !== null) {
            this.bookings.push(newBooking);
            this.bookings = [...this.bookings];
            this.bookingToUpdate = null;
          }
        });
    } else {
      this.bookings[this.editedBookingIndex] = newBooking;
    }
  }

  public crateNewBooking(): void {
    this.isAddingNewBooking = true;
    this.bookingToUpdate = new UpdateBookingModel();
  }

}
