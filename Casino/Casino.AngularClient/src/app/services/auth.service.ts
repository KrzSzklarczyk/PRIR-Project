import { Injectable } from '@angular/core';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtHelper: JwtHelperService) { }

  isUserAuthenticated(): boolean {
    const token = localStorage.getItem("accessToken");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

  logOut = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
  }
}
