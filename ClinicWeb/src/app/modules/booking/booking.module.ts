import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StarRatingModule } from 'angular-star-rating';
import { DropdownModule } from 'primeng/dropdown';

import { BookingComponent } from 'src/app/components/booking/booking.component';
import { BookingRoutingModule } from './booking-routing.module';
import { SharedModule } from '../shared/shared.module';
import { EditBookingModule } from './edit-booking.module';

@NgModule({
  declarations: [
    BookingComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BookingRoutingModule,
    SharedModule,
    DropdownModule,
    StarRatingModule.forRoot(),
    EditBookingModule
  ]
})
export class BookingModule { }
