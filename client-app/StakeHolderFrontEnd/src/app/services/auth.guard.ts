import { Injectable } from '@angular/core';
import { 
  CanActivate, 
  ActivatedRouteSnapshot, 
  RouterStateSnapshot, 
  Router
} from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.authService.isAuthenticated()) {
      const requiredRole = route.data['role'] as string;
      if (requiredRole && !this.authService.hasRole(requiredRole)) {
        this.router.navigate(['/']);
        return false;
      }
      return true;
    }
    
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
} 