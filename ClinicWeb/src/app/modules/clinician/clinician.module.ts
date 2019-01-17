import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClinicianComponent } from 'src/app/components/clinician/clinician.component';
import { ClinicianRoutingModule } from './clinician-routing.module';

@NgModule({
  declarations: [
    ClinicianComponent
  ],
  imports: [
    CommonModule,
    ClinicianRoutingModule
  ]
})
export class ClinicianModule { }
