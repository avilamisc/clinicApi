import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgmCoreModule } from '@agm/core';
import { AgmDirectionModule } from 'agm-direction';   // agm-direction

import { ClinicComponent } from 'src/app/components/clinic/clinic.component';
import { TableModule } from '../shared/table/table.module';
import { ClinicRoutingModule } from './clinic-routing.module';
import { ClinicInfoComponent } from 'src/app/components/clinic/clinic-info/clinic-info.component';
import { RaitingModule } from '../shared/raiting/star/star.module';
import { EditBookingModule } from '../booking/edit-booking.module';

@NgModule({
  declarations: [
    ClinicComponent,
    ClinicInfoComponent
  ],
  imports: [
    CommonModule,
    TableModule,
    ClinicRoutingModule,
    RaitingModule,
    EditBookingModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyDWSgvVWFJ0WkcWReqaAXOuCAn5pReZqCk'
      /* apiKey is required, unless you are a
      premium customer, in which case you can
      use clientId
      */
    }),
    AgmDirectionModule
  ]
})
export class ClinicModule { }
