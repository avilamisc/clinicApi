import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError, empty } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';

import { AccountService } from '../services/auth/account.service';
import { RefreshTokenModel } from '../models';
import { TokenService } from '../services/auth/token.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private tokenService: TokenService,
        private accountService: AccountService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            if (err.status === 401) {
                if (this.tokenService.getRefreshToken() === null) {
                    return next.handle(request);
                }

                const refreshModel: RefreshTokenModel = {
                    token: this.tokenService.getAccessToken(),
                    refreshToken: this.tokenService.getRefreshToken()
                };
                return this.accountService.refreshToken(refreshModel)
                    .pipe(mergeMap(res => {
                        if (res.Result !== null) {
                            this.tokenService.setAccessToken(res.Result.AccessToken);
                            this.tokenService.setRefreshToken(res.Result.RefreshToken);
                            request = request.clone({
                                setHeaders: {
                                    Authorization: `Bearer ${res.Result.AccessToken}`
                                }
                            });

                            return next.handle(request);
                        } else {
                            this.tokenService.removeTokens();
                            location.reload(true);
                            return empty();
                        }
                    }));
            }

            const error = err.error.message || err.statusText;
            return throwError(error);
        }));
    }
}
