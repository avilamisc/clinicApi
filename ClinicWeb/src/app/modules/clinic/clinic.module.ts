import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgmCoreModule } from '@agm/core';

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
    ClinicRoutingModule,
    AgmCoreModule.forRoot({
      apiKey: '4587487875898598598'
      /* apiKey is required, unless you are a 
      premium customer, in which case you can 
      use clientId 
      */
    })
  ]
})
export class ClinicModule { }
