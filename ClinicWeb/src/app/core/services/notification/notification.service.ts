import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiRoutes } from 'src/app/utilities/api-routes';
import { BaseService } from '../base.service';
import {
    ApiResponse,
    Pagination,
    UpdateNotificationModel,
    CreateNotificationModel,
    NotificationModel,
    RemoveResult, 
    PagingResult} from '../../models';

@Injectable({
  providedIn: 'root'
})
export class NotificationService extends BaseService {

  constructor(private http: HttpClient) { 
      super();
  }

  public getNotifications(paging: Pagination): Observable<ApiResponse<PagingResult<NotificationModel>>> {
    return this.http.get<ApiResponse<PagingResult<NotificationModel>>>
          (`${ApiRoutes.notifications}/?PageNumber=${paging.pageNumber}&PageSize=${paging.pageCount}`);
  }

  public updateNotification(model: UpdateNotificationModel): Observable<ApiResponse<NotificationModel>> {
    return this.http.put<ApiResponse<NotificationModel>>(`${ApiRoutes.notifications}`, model);
  }

  public setNotificationReadState(updateModel: any): Observable<ApiResponse<boolean>> {
    return this.http.patch<ApiResponse<boolean>>(`${ApiRoutes.notifications}/readstate`, updateModel);
  }

  public createNotification(model: CreateNotificationModel): Observable<ApiResponse<NotificationModel>> {
    return this.http.post<ApiResponse<NotificationModel>>(`${ApiRoutes.notifications}`, model);
  }

  public removeNotification(id: number): Observable<ApiResponse<RemoveResult<NotificationModel>>> {
    return this.http.delete<ApiResponse<RemoveResult<NotificationModel>>>(`${ApiRoutes.notifications}?id=${id}`);
  }
}
