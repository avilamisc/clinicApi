import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse, PatientBookingModel, ClinicianBookingModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { UpdateBookingModel } from '../../models/booking/update-booking.model';
import { Pagination } from '../../models/table/pagination.model';
import { PagingResult } from '../../models/paging-result.model';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  public getPatientBookings(paging: Pagination): Observable<ApiResponse<PagingResult<PatientBookingModel>>> {
    return this.http.get<ApiResponse<PagingResult<PatientBookingModel>>>
          (`${ApiRoutes.patientBookings}/?PageNumber=${paging.pageNumber}&PageSize=${paging.pageCount}`);
  }

  public getClinicianBookings(paging: Pagination): Observable<ApiResponse<PagingResult<ClinicianBookingModel>>> {
    return this.http.get<ApiResponse<PagingResult<ClinicianBookingModel>>>
          (`${ApiRoutes.clinicianBookings}/?PageNumber=${paging.pageNumber}&PageSize=${paging.pageCount}`);
  }

  public updateBookings(model: UpdateBookingModel): Observable<ApiResponse<PatientBookingModel>> {
    const multipartData = this.getMultipartWithFiles(model);
    return this.http.put<ApiResponse<PatientBookingModel>>(`${ApiRoutes.booking}`, multipartData);
  }

  public createBookings(model: PatientBookingModel): Observable<ApiResponse<PatientBookingModel>> {
    const multipartData = this.getMultipartWithFiles(model);
    return this.http.post<ApiResponse<PatientBookingModel>>(`${ApiRoutes.booking}`, multipartData);
  }

  private getMultipartWithFiles(model: any): FormData {
    const formData = this.getMultipartData(model);

    model.newFiles.forEach(file => {
      formData.append(file.name, file);
    });

    return formData;
  }
}
