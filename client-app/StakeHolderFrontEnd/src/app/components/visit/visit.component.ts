import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { VisitService, Visit } from '../../services/visit.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-visit',
  templateUrl: './visit.component.html',
  styleUrls: ['./visit.component.css']
})
export class VisitComponent implements OnInit {
  visits: Visit[] = [];
  isLoading = false;
  errorMessage = '';
  deleteMessage = '';
  visitToDelete: Visit | null = null;
  showDeleteModal = false;
  userRole: string | null = null;

  constructor(
    private visitService: VisitService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const currentUser = this.authService.getCurrentUser();
    this.userRole = currentUser ? currentUser.role : null;
    this.loadVisits();
  }

  loadVisits(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.visitService.getVisits()
      .subscribe({
        next: (data) => {
          this.visits = data;
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = err.message;
          this.isLoading = false;
        }
      });
  }

  viewVisit(id: string): void {
    this.router.navigate(['/visit', id]);
  }

  addVisit(): void {
    this.router.navigate(['/visit/add']);
  }

  editVisit(id: string): void {
    this.router.navigate(['/visit/edit', id]);
  }

  confirmDelete(visit: Visit): void {
    this.visitToDelete = visit;
    this.showDeleteModal = true;
    this.deleteMessage = '';
  }

  cancelDelete(): void {
    this.visitToDelete = null;
    this.showDeleteModal = false;
  }

  deleteVisit(): void {
    if (!this.visitToDelete) return;
    
    this.isLoading = true;
    this.deleteMessage = '';
    
    this.visitService.deleteVisit(this.visitToDelete.id!)
      .subscribe({
        next: () => {
          this.loadVisits();
          this.showDeleteModal = false;
          this.visitToDelete = null;
        },
        error: (err) => {
          this.deleteMessage = err.message;
          this.isLoading = false;
        }
      });
  }

  // Helper function to check if user has admin role
  isAdmin(): boolean {
    return this.userRole === 'Admin';
  }
} 