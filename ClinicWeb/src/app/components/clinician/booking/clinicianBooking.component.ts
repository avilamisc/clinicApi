import { Component, OnInit } from '@angular/core';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { PatientBookingModel, ClinicianBookingModel } from 'src/app/core/models';

@Component({
  selector: 'app-clinician-booking',
  templateUrl: './clinicianBooking.component.html',
  styleUrls: ['./clinicianBooking.component.styl']
})
export class ClinicianBookingComponent implements OnInit {
  public bookings: ClinicianBookingModel[] = [];

  constructor(private bookingService: BookingService) { }

  ngOnInit() {
      this.bookingService.getClinicianBookings()
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result;
          }
        });
  }

}
