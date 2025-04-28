import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ApiService } from './api.service';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  username: string;
  role: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser: AuthResponse | null = null;

  constructor(
    private http: HttpClient,
    private apiService: ApiService
  ) {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUser = JSON.parse(storedUser);
    }
  }

  login(username: string, password: string): Observable<AuthResponse> {
    return this.apiService.post<AuthResponse>('/api/auth/login', { username, password })
      .pipe(
        tap(response => {
          this.currentUser = response;
          localStorage.setItem('currentUser', JSON.stringify(response));
        }),
        catchError(error => {
          console.error('Login error:', error);
          throw error;
        })
      );
  }

  logout(): void {
    this.currentUser = null;
    localStorage.removeItem('currentUser');
  }

  isAuthenticated(): boolean {
    return !!this.currentUser;
  }

  getCurrentUser(): AuthResponse | null {
    return this.currentUser;
  }

  getToken(): string | null {
    return this.currentUser ? this.currentUser.token : null;
  }

  hasRole(role: string): boolean {
    return this.currentUser ? this.currentUser.role === role : false;
  }
} 