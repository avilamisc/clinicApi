import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClinicComponent } from 'src/app/components/clinic/clinic.component';
import { TableModule } from '../shared/table/table.module';
import { ClinicRoutingModule } from './clinic-routing.module';

@NgModule({
  declarations: [
    ClinicComponent
  ],
  imports: [
    CommonModule,
    TableModule,
    ClinicRoutingModule
  ]
})
export class ClinicModule { }
