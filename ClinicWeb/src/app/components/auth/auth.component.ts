import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { AccountService } from 'src/app/core/services/auth/account.service';
import { LoginModel } from 'src/app/core/models';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { CommonConstants } from 'src/app/utilities/common-constants';

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
    private tokenService: TokenService,
    private accountService: AccountService) { }

  public ngOnInit(): void {
    this.createForm();
    this.initializeReturnUrl();
  }

  public onSubmit(): void {
    this.setValuesFromFormToModel();
    this.tokenService.removeTokens();
    this.accountService.authenticate(this.model)
      .subscribe(result => {
        this.router.navigate([this.returnUrl || '/booking']);
      });
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
