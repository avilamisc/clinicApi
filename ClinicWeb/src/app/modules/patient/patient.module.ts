import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientComponent } from 'src/app/components/patient/patient.component';
import { PatientRoutingModule } from './patient-routing.module';

@NgModule({
  declarations: [
    PatientComponent
  ],
  imports: [
    CommonModule,
    PatientRoutingModule
  ]
})
export class PatientModule { }
