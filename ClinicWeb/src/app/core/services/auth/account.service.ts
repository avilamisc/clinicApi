import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import decode from 'jwt-decode';

import { ApiResponse, LoginModel, LoginResultModel, RefreshTokenModel } from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { TokenService } from './token.service';
import { User } from '../../models/user/user.model';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public user: User;

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private userService: UserService
  ) { }

  public authenticate(model: LoginModel): Observable<ApiResponse<LoginResultModel>> {
    return this.http.post<ApiResponse<LoginResultModel>>(ApiRoutes.authenticate, model)
      .pipe(map(result => {
        if (result.Data !== null) {
          const token = result.Data.AccessToken;
          this.tokenService.setAccessToken(token);
          this.tokenService.setRefreshToken(result.Data.RefreshToken);

          const user = {
            UserRole: decode(token).role,
            Id: result.Data.UserId,
            UserName: result.Data.UserName
          } as User;
          this.user = user;
          this.userService.setUserInLocalStorage(user);
        }

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

}
