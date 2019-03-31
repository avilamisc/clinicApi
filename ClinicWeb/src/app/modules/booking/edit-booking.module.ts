import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { EditComponent } from 'src/app/components/booking/edit/edit.component';
import { EditDocumentComponent } from 'src/app/components/booking/edit-document/edit-document.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    EditComponent,
    EditDocumentComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  exports: [
    EditComponent,
    EditDocumentComponent
  ]
})
export class EditBookingModule { }
