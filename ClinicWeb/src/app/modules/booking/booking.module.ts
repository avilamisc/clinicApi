import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StarRatingModule } from 'angular-star-rating';

import { BookingComponent } from 'src/app/components/booking/booking.component';
import { EditComponent } from 'src/app/components/booking/edit/edit.component';
import { BookingRoutingModule } from './booking-routing.module';
import { EditDocumentComponent } from 'src/app/components/booking/edit-document/edit-document.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    BookingComponent,
    EditComponent,
    EditDocumentComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BookingRoutingModule,
    SharedModule,
    StarRatingModule.forRoot()
  ]
})
export class BookingModule { }
