import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

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
  public loginForm: FormGroup = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    userPassword: new FormControl('', [Validators.required])
  });
  private model: LoginModel = new LoginModel();
  private returnUrl: string = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private tokenService: TokenService) { }

  public ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams[CommonConstants.returnUrlSnapshot];
  }

  public onSubmit(): void {
    this.setValuesFromFormToModel();
    this.accountService.authenticate(this.model)
      .subscribe(res => {
        this.tokenService.setAccessToken(res.Result.AccessToken);
        this.tokenService.setRefreshToken(res.Result.RefreshToken);

        console.log(this.returnUrl);
        this.router.navigate([this.returnUrl || '/booking']);
      });
  }

  private setValuesFromFormToModel(): void {
    const values = this.loginForm.getRawValue();

    this.model.email = values.userName;
    this.model.password = values.userPassword;
  }
}
