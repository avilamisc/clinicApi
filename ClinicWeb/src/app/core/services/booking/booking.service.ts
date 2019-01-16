import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { TokenService } from '../auth/token.service';
import { ApiResponse, BookingModel, PatientBookingModel, ClinicianBookingModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/apiRouteConstats';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  constructor(
    private http: HttpClient) { }

  public GetPatientBookings(): Observable<ApiResponse<PatientBookingModel[]>> {
    return this.http.get<ApiResponse<PatientBookingModel[]>>(`${ApiRoutes.patientBookings}`);
  }

  public GetClinicianBookings(): Observable<ApiResponse<ClinicianBookingModel[]>> {
    return this.http.get<ApiResponse<ClinicianBookingModel[]>>(`${ApiRoutes.clinicianBookings}`);
  }
}
