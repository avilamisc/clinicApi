import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProfileComponent } from 'src/app/components/profile/profile.component';
import { ClinicProfileComponent } from 'src/app/components/profile/clinic-profile/clinic-profile.component';
import { PatientProfileComponent } from 'src/app/components/profile/patient-profile/patient-profile.component';
import { ClinicianProfileComponent } from 'src/app/components/profile/clinician-profile/clinician-profile.component';

const routes: Routes = [
  {
    path: '',
    component: ProfileComponent
  },
  {
    path: 'clinic',
    component: ClinicProfileComponent
  },
  {
    path: 'clinician',
    component: ClinicianProfileComponent
  },
  {
    path: 'patient',
    component: PatientProfileComponent
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ProfileRoutingModule { }
