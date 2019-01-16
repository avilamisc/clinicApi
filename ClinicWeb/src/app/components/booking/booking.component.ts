import { Component, OnInit } from '@angular/core';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { PatientBookingModel } from 'src/app/core/models';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.styl']
})
export class BookingComponent implements OnInit {
  public bookings: PatientBookingModel[] = [];

  constructor(private bookingService: BookingService) { }

  ngOnInit() {
      this.bookingService.GetPatientBookings()
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result;
          }
        });
  }

}
