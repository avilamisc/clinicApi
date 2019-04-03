import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { AccountService } from 'src/app/core/services/auth/account.service';
import { LoginModel } from 'src/app/core/models';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { CommonConstants } from 'src/app/utilities/common-constants';
import { ToastNotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.styl']
})
export class AuthComponent implements OnInit {
  public loginForm: FormGroup;
  private model: LoginModel = new LoginModel();
  private returnUrl: string = null;
  // tslint:disable-next-line:max-line-length
  public googleAuthLink = 'https://accounts.google.com/o/oauth2/auth?redirect_uri=http://localhost:54865/api/account/google&response_type=code&client_id=433233257213-uoailm7olq0d7r1ds4pdlm0thqp4invk.apps.googleusercontent.com&scope=https://www.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private tokenService: TokenService,
    private accountService: AccountService,
    private notificationService: ToastNotificationService) { }

  public ngOnInit(): void {
    this.createForm();
    this.initializeReturnUrl();
  }

  public onSubmit(): void {
    this.setValuesFromFormToModel();
    this.tokenService.removeTokens();
    this.accountService.authenticate(this.model)
      .subscribe(result => {
        if (result && result.Data) {
          this.router.navigate([this.returnUrl || '/booking']);
          this.notificationService.successAuthentication();
        } else {
          this.notificationService.showApiErrorMessage(result);
        }
      });
  }

  public navigateToRegistration(): void {
    this.router.navigate(['registration']);
  }

  private createForm(): void {
    this.loginForm = new FormGroup({
      userEmail: new FormControl('', [Validators.email, Validators.required]),
      userPassword: new FormControl('', [Validators.required])
    });
  }

  private initializeReturnUrl(): void {
    this.returnUrl = this.route.snapshot.queryParams[CommonConstants.returnUrlSnapshot];
  }

  private setValuesFromFormToModel(): void {
    const values = this.loginForm.getRawValue();
    this.model.email = values.userEmail;
    this.model.password = values.userPassword;
  }
}
