import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from 'src/services/alertify.service';
import { AuthService } from 'src/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private urlrouter: Router){
  }
  canActivate(): boolean {
    if (localStorage.token){
      console.log('user is Authorize');
      return true;
    }
    // this.auth.logout();
    return false;
  }
}
