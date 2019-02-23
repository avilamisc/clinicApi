import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse, ClinicModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { ClinicClinicianBaseModel } from '../../models/clinic-clinician.module/clinic-clinician-base.model';
import { Pagination } from '../../models/table/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {

  constructor(
    private http: HttpClient) { }

    public getAllClinic(paging: Pagination, long: number, lat: number): Observable<ApiResponse<ClinicModel[]>> {
      console.log('lat', lat);
      return this.http.get<ApiResponse<ClinicModel[]>>
        (`${ApiRoutes.clinics}?PageNumber=${paging.pageNumber}&PageSize=${paging.pageCount}&longitude=${long}&latitude=${lat}`);
    }

    public getClinicById(id: number): Observable<ApiResponse<ClinicModel>> {
      return this.http.get<ApiResponse<ClinicModel>>(`${ApiRoutes.clinics}/${id}`);
    }

    public getClosestClinicsWithClinician(long: number, lat: number): Observable<ApiResponse<ClinicClinicianBaseModel[]>> {
      return this.http.get<ApiResponse<ClinicClinicianBaseModel[]>>
        (`${ApiRoutes.clinicsClinician}?longitude=${long}&latitude=${lat}&v=1`);
    }
}
