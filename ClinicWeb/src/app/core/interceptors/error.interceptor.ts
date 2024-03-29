import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, empty } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';

import { AccountService } from '../services/auth/account.service';
import { RefreshTokenModel, RevokeTokenModel } from '../models';
import { TokenService } from '../services/auth/token.service';
import { ToastNotificationService } from '../services/notification.service';
import { LoaderService } from '../services/loader/loader.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private tokenService: TokenService,
        private loaderService: LoaderService,
        private accountService: AccountService,
        private notificationService: ToastNotificationService) { }

    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
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
                        if (res.Data && res.Data.AccessToken && res.Data.RefreshToken) {
                            this.tokenService.setAccessToken(res.Data.AccessToken);
                            this.tokenService.setRefreshToken(res.Data.RefreshToken);
                            request = request.clone({
                                setHeaders: {
                                    Authorization: `Bearer ${res.Data.AccessToken}`
                                }
                            });

                            return next.handle(request);
                        } else {
                            const revokeModel: RevokeTokenModel = {
                                token: this.tokenService.getAccessToken(),
                                refreshToken: this.tokenService.getRefreshToken()
                            };
                            this.accountService.revokeToken(revokeModel)
                                .subscribe(result => {
                                    this.tokenService.removeTokens();
                                    if (result.Data) {
                                        this.accountService.logOut();
                                    } else {
                                        this.notificationService.showErrorMessage(result.ErrorMessage, result.StatusCode);
                                    }
                                    location.reload(true);
                                });

                            return empty();
                        }
                    }));
            } else {
                this.hideLoader();
                const errorMsg = err.error.message || err.statusText;
                this.notificationService.showErrorMessage(errorMsg, err.status);
            }
        }));
    }

    private hideLoader(): void {
        this.loaderService.hide();
    }
}
