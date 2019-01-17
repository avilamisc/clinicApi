import { Injectable } from '@angular/core';
import { UserRole } from '../../enums/UserRoles.enum';

const AccessTokenIdentifier = 'accessToken';
const RefreshTokenIdentifier = 'refreshToken';
const UserRoleIdentifier = 'userRole';

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

  public getRole(): string {
    return localStorage.getItem(UserRoleIdentifier);
  }

  public setRole(role: string): void {
    localStorage.setItem(UserRoleIdentifier, role);
  }

  public removeTokens(): void {
    localStorage.removeItem(AccessTokenIdentifier);
    localStorage.removeItem(RefreshTokenIdentifier);
  }
}
