import { Injectable } from '@angular/core';

const AccessTokenIdentifier = 'accessToken';
const RefreshTokenIdentifier = 'refreshToken';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor() { }

  public getRefreshToken(): string {
    return localStorage.getItem(RefreshTokenIdentifier);
  }

  public setRefreshToken(token: string): void {
    return localStorage.setItem(RefreshTokenIdentifier, token);
  }

  public getAccessToken(): string {
    return localStorage.getItem(AccessTokenIdentifier);
  }

  public setAccessToken(token: string): void {
    return localStorage.setItem(AccessTokenIdentifier, token);
  }

  public removeTokens(): void {
    localStorage.removeItem(AccessTokenIdentifier);
    localStorage.removeItem(RefreshTokenIdentifier);
  }
}
