import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse, ClinicModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { Pagination } from '../../models/table/pagination.model';
import { ClinicDistanceModel } from '../../models/clinic-clinician.module/clinic-distance.model';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {

  constructor(
    private http: HttpClient) { }

    public getAllClinic(paging: Pagination, long: number, lat: number): Observable<ApiResponse<ClinicModel[]>> {
      return this.http.get<ApiResponse<ClinicModel[]>>
        (`${ApiRoutes.clinics}?PageNumber=${paging.pageNumber}&PageSize=${paging.pageCount}&longitude=${long}&latitude=${lat}`);
    }

    public getClinicById(id: number): Observable<ApiResponse<ClinicModel>> {
      return this.http.get<ApiResponse<ClinicModel>>(`${ApiRoutes.clinics}/${id}`);
    }

    public getClosestClinicsWithClinician(long: number, lat: number): Observable<ApiResponse<ClinicDistanceModel[]>> {
      return this.http.get<ApiResponse<ClinicDistanceModel[]>>
        (`${ApiRoutes.clinicsClinician}?longitude=${long}&latitude=${lat}&v=3`);
    }
}
