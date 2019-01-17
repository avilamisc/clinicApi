import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { UserRole } from './core/enums/UserRoles.enum';
import { AppComponent } from './app.component';
import { CommonConstants } from './utilities/commonConstants';

const routes: Routes = [
  {
    path: '',
    component: AppComponent
  },
  {
    path: 'patient',
    loadChildren: './modules/patient/patient.module#PatientModule',
    canActivate: [AuthGuard],
    data: {
      expectedRole: CommonConstants.patientRoleIdentifier
    }
  },
  {
    path: 'clinician',
    loadChildren: './modules/clinician/clinician.module#ClinicianModule',
    canActivate: [AuthGuard],
    data: {
      expectedRole: CommonConstants.clinicianRoleIdentifier
    }
  },
  {
    path: 'login',
    loadChildren: './modules/auth/auth.module#AuthModule'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
