import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse, PatientBookingModel, ClinicianBookingModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { UpdateBookingModel } from '../../models/booking/update-booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  constructor(
    private http: HttpClient) { }

  public getPatientBookings(): Observable<ApiResponse<PatientBookingModel[]>> {
    return this.http.get<ApiResponse<PatientBookingModel[]>>(`${ApiRoutes.patientBookings}`);
  }

  public getClinicianBookings(): Observable<ApiResponse<ClinicianBookingModel[]>> {
    return this.http.get<ApiResponse<ClinicianBookingModel[]>>(`${ApiRoutes.clinicianBookings}`);
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

  private getMultipartData(model: any): FormData {
    const formData = new FormData();

    for (const property in model) {
        if (model.hasOwnProperty(property)) {
            if (model[property] instanceof Array) {
                formData.append(property, JSON.stringify(model[property]));
            } else {
                if (model[property] != null) {
                    formData.append(property, model[property]);
                }
            }
        }
    }

    return formData;
  }
}
