import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, ClinicianModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';

@Injectable({
  providedIn: 'root'
})
export class ClinicianService {

  constructor(
    private http: HttpClient) { }

    public getAllClinic(clinicId: number): Observable<ApiResponse<ClinicianModel[]>> {
      return this.http.get<ApiResponse<ClinicianModel[]>>(`${ApiRoutes.clinicians}?clinicId=${clinicId}`);
    }
}
