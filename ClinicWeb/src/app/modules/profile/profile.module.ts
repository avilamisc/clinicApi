import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ProfileComponent } from 'src/app/components/profile/profile.component';
import { ResetPasswordComponent } from 'src/app/components/profile/reset-password/reset-password.component';
import { ProfileRoutingModule } from './profile-routing.module';
import { ClinicProfileComponent } from 'src/app/components/profile/clinic-profile/clinic-profile.component';
import { PatientProfileComponent } from 'src/app/components/profile/patient-profile/patient-profile.component';
import { ClinicianProfileComponent } from 'src/app/components/profile/clinician-profile/clinician-profile.component';

@NgModule({
  declarations: [
    ProfileComponent,
    ClinicProfileComponent,
    PatientProfileComponent,
    ClinicianProfileComponent,
    ResetPasswordComponent
  ],
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
