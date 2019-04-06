import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { RegistrationRoutingModule } from './registration-routing.module';
import { RegistrationComponent } from 'src/app/components/auth/registration/registration.component';
import {
  PatientRegistrationComponent
} from 'src/app/components/auth/registration/patient-registration/patient-registration.component';
import {
  ClinicianRegistrationComponent
} from 'src/app/components/auth/registration/clinician-registration/clinician-registration.component';
import { ClinicRegistrationComponent } from 'src/app/components/auth/registration/clinic-registration/clinic-registration.component';
import { AgmCoreModule } from '@agm/core';

@NgModule({
  declarations: [
    RegistrationComponent,
    PatientRegistrationComponent,
    ClinicianRegistrationComponent,
    ClinicRegistrationComponent
  ],
  imports: [
    CommonModule,
    RegistrationRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AgmCoreModule.forRoot({
      apiKey: ''
      /* apiKey is required, unless you are a
      premium customer, in which case you can
      use clientId
      */
    })
  ]
})
export class RegistrationModule { }
