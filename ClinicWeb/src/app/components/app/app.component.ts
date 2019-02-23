import { Component } from '@angular/core';
import { AccountService } from 'src/app/core/services/auth/account.service';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.styl']
})
export class AppComponent {

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private accountService: AccountService) { }

  public logOutUser(): void {
    this.accountService.logOut();
    this.router.navigate(['/login']);
  }

  public get isAuthenticated(): boolean {
    return this.tokenService.getAccessToken() !== null;
  }
}
