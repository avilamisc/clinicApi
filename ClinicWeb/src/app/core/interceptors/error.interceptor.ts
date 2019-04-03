import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError, empty } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';

import { AccountService } from '../services/auth/account.service';
import { RefreshTokenModel } from '../models';
import { TokenService } from '../services/auth/token.service';
import { ToastNotificationService } from '../services/notification.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private tokenService: TokenService,
        private accountService: AccountService,
        private notificationService: ToastNotificationService) { }

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
                        if (res.Data !== null) {
                            this.tokenService.setAccessToken(res.Data.AccessToken);
                            this.tokenService.setRefreshToken(res.Data.RefreshToken);
                            request = request.clone({
                                setHeaders: {
                                    Authorization: `Bearer ${res.Data.AccessToken}`
                                }
                            });

                            return next.handle(request);
                        } else {
                            this.tokenService.removeTokens();
                            location.reload(true);
                            return empty();
                        }
                    }));
            } else {
                const errorMsg = err.error.message || err.statusText;
                this.notificationService.showErrorMessage(errorMsg, err.status);
            }
        }));
    }
}
