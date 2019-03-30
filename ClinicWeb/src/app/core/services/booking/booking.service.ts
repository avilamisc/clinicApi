import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  ApiResponse,
  PatientBookingModel,
  ClinicianBookingModel,
  RemoveResult,
  UpdateBookingModel,
  Pagination,
  PagingResult,
  CreateNotificationModel,
  BookingModelResult} from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { BaseService } from '../base.service';
import { NotificationService } from '../notification/notification.service';
import { map } from 'rxjs/operators';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService extends BaseService {
  constructor(
    private http: HttpClient,
    private userService: UserService,
    private notificationService: NotificationService) {
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

  public updateBookings(model: UpdateBookingModel): Observable<ApiResponse<BookingModelResult>> {
    const multipartData = this.getMultipartWithFiles(model);
    return this.http.put<ApiResponse<BookingModelResult>>(`${ApiRoutes.booking}`, multipartData)
      .pipe(map(result => {
        if (result && result.Data) {
          const user = this.userService.getUserFromLocalStorage();
          this.createNotification(
            `${user.UserName} has updated your booking ${result.Data.Name}.`,
            user.Id,
            result.Data
          );
        }
        return result;
      }));
  }

  public updateBookingRate(updateModel: any): Observable<ApiResponse<PatientBookingModel>> {
    return this.http.patch<ApiResponse<PatientBookingModel>>(`${ApiRoutes.booking}/rate`, updateModel);
  }

  public createBookings(model: PatientBookingModel): Observable<ApiResponse<BookingModelResult>> {
    const multipartData = this.getMultipartWithFiles(model);
    return this.http.post<ApiResponse<BookingModelResult>>(`${ApiRoutes.booking}`, multipartData)
      .pipe(map(result => {
        if (result && result.Data) {
          const user = this.userService.getUserFromLocalStorage();
          this.createNotification(
            `${user.UserName} created new booking for your (${result.Data.Name}).`,
            user.Id,
            result.Data
          );
        }
        return result;
      }));
  }

  public removeBookings(id: number): Observable<ApiResponse<RemoveResult<BookingModelResult>>> {
    return this.http.delete<ApiResponse<RemoveResult<BookingModelResult>>>(`${ApiRoutes.booking}?id=${id}`)
      .pipe(map(result => {
        if (result && result.Data) {
          const user = this.userService.getUserFromLocalStorage();
          this.createNotification(
            `${user.UserName} ${result.Data.Description} ${result.Data.Value.Name}.`,
            user.Id,
            result.Data.Value
          );
        }
        return result;
      }));
  }

  private getMultipartWithFiles(model: any): FormData {
    const formData = this.getMultipartData(model);

    model.newFiles.forEach(file => {
      formData.append(file.name, file);
    });

    return formData;
  }

  private createNotification(message: string, userId: number, booking: BookingModelResult): void {
    const notificationUserId =
      userId === booking.PatientId
        ? booking.ClinicianId
        : booking.PatientId;
    const newNotification: CreateNotificationModel = {
      Content: message,
      UserId: notificationUserId
    };
    this.notificationService.createNotification(newNotification)
      .subscribe();
  }
}
