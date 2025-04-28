import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private authService: AuthService) {}

  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  getUserName(): string {
    return this.authService.getCurrentUser()?.username || '';
  }

  isAdmin(): boolean {
    return this.authService.hasRole('Admin');
  }
} 