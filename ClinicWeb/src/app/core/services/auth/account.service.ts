import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import decode from 'jwt-decode';

import { ApiResponse,
         LoginModel,
         LoginResultModel,
         RefreshTokenModel,
         ClinicRegistrationModel,
         RevokeTokenModel, 
         ResetPasswordModel} from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { TokenService } from './token.service';
import { User } from '../../models/user/user.model';
import { UserService } from '../user/user.service';
import { BaseService } from '../base.service';
import { PatientRegistrationModel } from '../../models/auth/registration/patient-registration.model';
import { ClinicianRegistrationModel } from '../../models/auth/registration/clinician-registration.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService extends BaseService {
  public user: User;

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private userService: UserService
  ) {
    super();
  }

  public authenticate(model: LoginModel): Observable<ApiResponse<LoginResultModel>> {
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.authenticate, model)
      .pipe(map(result => {
        this.updateLoginationData(result.Data);
        return result;
      }));
  }

  public resetPassword(model: ResetPasswordModel): Observable<ApiResponse<LoginResultModel>> {
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.resetPassword, model)
      .pipe(map(result => {
        this.updateLoginationData(result.Data);
        return result;
      }));
  }

  public registerPatient(model: PatientRegistrationModel): Observable<ApiResponse<LoginResultModel>> {
    const multipartData = this.getMultipartData(model);
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.registerPatient, multipartData)
      .pipe(map(result => {
        this.updateLoginationData(result.Data);
        return result;
      }));
  }

  public registerClinic(model: ClinicRegistrationModel): Observable<ApiResponse<LoginResultModel>> {
    const multipartData = this.getMultipartData(model);
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.registerClinic, multipartData)
      .pipe(map(result => {
        this.updateLoginationData(result.Data);
        return result;
      }));
  }

  public registerClinician(model: ClinicianRegistrationModel): Observable<ApiResponse<LoginResultModel>> {
    const multipartData = this.getMultipartData(model);
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.registerClinician, multipartData)
      .pipe(map(result => {
        this.updateLoginationData(result.Data);
        return result;
      }));
  }

  public logOut(): void {
    this.user = null;
    this.tokenService.removeTokens();
  }

  public refreshToken(model: RefreshTokenModel): Observable<ApiResponse<LoginResultModel>> {
    return this.http.post<ApiResponse<LoginResultModel>>(`${ApiRoutes.refreshToken}`, model);
  }

  public revokeToken(model: RevokeTokenModel): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${ApiRoutes.revokeToken}`, model);
  }

  private updateLoginationData(loginResult: LoginResultModel): void {
    if (loginResult !== null) {
      const token = loginResult.AccessToken;
      this.tokenService.setAccessToken(token);
      this.tokenService.setRefreshToken(loginResult.RefreshToken);

      const user = {
        UserRole: decode(token).role,
        Id: loginResult.UserId,
        UserName: loginResult.UserName
      } as User;
      this.user = user;
      this.tokenService.setUserRole(user.UserRole);
      this.userService.setUserInLocalStorage(user);
    }
  }
}
