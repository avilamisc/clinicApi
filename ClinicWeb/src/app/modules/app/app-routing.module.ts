import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/booking'
  },
  {
    path: 'booking',
    loadChildren: '../booking/booking.module#BookingModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'clinic',
    loadChildren: '../clinic/clinic.module#ClinicModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'login',
    loadChildren: '../auth/auth.module#AuthModule'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
