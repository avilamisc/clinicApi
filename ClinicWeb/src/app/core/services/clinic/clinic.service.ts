import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse, ClinicModel } from '../../models';
import { ApiRoutes, ApiRoutesV3 } from 'src/app/utilities/api-routes';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {

  constructor(
    private http: HttpClient) { }

    public getAllClinic(): Observable<ApiResponse<ClinicModel[]>> {
      return this.http.get<ApiResponse<ClinicModel[]>>(`${ApiRoutes.clinics}`);
    }

    public getClosestClinicsWithClinician(long: number, lat: number): Observable<ApiResponse<any[]>> {
      return this.http.get<ApiResponse<any[]>>(`${ApiRoutesV3.clinics}/${long}/${lat}`);
    }
}
