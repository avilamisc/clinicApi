import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import decode from 'jwt-decode';

import { AccountService } from 'src/app/core/services/auth/account.service';
import { LoginModel } from 'src/app/core/models';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { CommonConstants } from 'src/app/utilities/commonConstants';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.styl']
})
export class AuthComponent implements OnInit {
  public loginForm: FormGroup;
  private model: LoginModel = new LoginModel();
  private returnUrl: string = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private tokenService: TokenService) { }

  public ngOnInit(): void {
    this.CreateForm();
    this.InitializeReturnUrl();
  }

  public onSubmit(): void {
    this.setValuesFromFormToModel();
    this.accountService.authenticate(this.model)
      .subscribe(res => {
        const token = res.Result.AccessToken;
        this.tokenService.setAccessToken(token);
        this.tokenService.setRefreshToken(res.Result.RefreshToken);

        const role = decode(token).role;
        this.tokenService.setRole(role);

        const redirectTo = role === CommonConstants.patientRoleIdentifier
          ? '/patient/booking'
          : '/clinician/booking';

        console.log(redirectTo);

        this.router.navigate([this.returnUrl || redirectTo]);
      });
  }

  private CreateForm(): void {
    this.loginForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      userPassword: new FormControl('', [Validators.required])
    });
  }

  private InitializeReturnUrl(): void {
    this.returnUrl = this.route.snapshot.queryParams[CommonConstants.returnUrlSnapshot];
  }

  private setValuesFromFormToModel(): void {
    const values = this.loginForm.getRawValue();

    this.model.email = values.userName;
    this.model.password = values.userPassword;
  }
}
