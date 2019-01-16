import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ApiResponse, LoginModel, LoginResultModel, RefreshTokenModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/apiRouteConstats';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    private http: HttpClient
  ) { }

  public authenticate(model: LoginModel): Observable<ApiResponse<LoginResultModel>> {
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.authenticate, model);
  }

  public refreshToken(model: RefreshTokenModel): Observable<ApiResponse<LoginResultModel>> {
    return this.http.post<ApiResponse<LoginResultModel>>(`${ApiRoutes.refreshToken}`, model);
  }

}
