import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BookingComponent } from 'src/app/components/booking/booking.component';
import { EditComponent } from 'src/app/components/booking/edit/edit.component';
import { BookingRoutingModule } from './booking-routing.module';
import { EditDocumentComponent } from 'src/app/components/booking/edit-document/edit-document.component';

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
    BookingRoutingModule
  ]
})
export class BookingModule { }
