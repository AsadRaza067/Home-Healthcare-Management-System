import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }

    const expectedRoles = route.data['roles'] as string[] | undefined;
    if (expectedRoles && expectedRoles.length > 0) {
      const currentRole = this.authService.getCurrentUser()?.role;
      if (!currentRole || !expectedRoles.includes(currentRole)) {
        this.router.navigate(['/login']);
        return false;
      }
    }

    return true;
  }
}
