import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.authService.getToken();
    
    if (token) {
      const authRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
      return next.handle(authRequest).pipe(
        catchError(error => this.handleError(error))
      );
    }
    
    return next.handle(request).pipe(
      catchError(error => this.handleError(error))
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      switch (error.status) {
        case 400:
          errorMessage = error.error || 'Bad request';
          break;
        case 401:
          errorMessage = 'Unauthorized. Please log in again.';
          this.authService.logout();
          this.router.navigate(['/login']);
          break;
        case 403:
          errorMessage = 'You do not have permission to perform this action';
          break;
        case 404:
          errorMessage = 'Resource not found';
          break;
        case 405:
          errorMessage = 'Method not allowed';
          break;
        case 500:
          errorMessage = 'Server error. Please try again later.';
          break;
        default:
          errorMessage = error.error || `Error Code: ${error.status}`;
          break;
      }
    }
    
    // Pass the error with our enhanced message
    return throwError(() => new Error(errorMessage));
  }
} 