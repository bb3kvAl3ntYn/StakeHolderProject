import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StakeholderService, Stakeholder } from '../../services/stakeholder.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-stakeholder',
  templateUrl: './stakeholder.component.html',
  styleUrls: ['./stakeholder.component.css']
})
export class StakeholderComponent implements OnInit {
  stakeholders: Stakeholder[] = [];
  isLoading = false;
  errorMessage = '';
  deleteMessage = '';
  stakeholderToDelete: Stakeholder | null = null;
  showDeleteModal = false;
  userRole: string | null = null;

  constructor(
    private stakeholderService: StakeholderService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const currentUser = this.authService.getCurrentUser();
    this.userRole = currentUser ? currentUser.role : null;
    this.loadStakeholders();
  }

  loadStakeholders(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.stakeholderService.getStakeholders()
      .subscribe({
        next: (data) => {
          this.stakeholders = data;
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = err.message;
          this.isLoading = false;
        }
      });
  }

  viewStakeholder(id: string): void {
    this.router.navigate(['/stakeholder', id]);
  }

  addStakeholder(): void {
    this.router.navigate(['/stakeholder/add']);
  }

  editStakeholder(id: string): void {
    this.router.navigate(['/stakeholder/edit', id]);
  }

  confirmDelete(stakeholder: Stakeholder): void {
    this.stakeholderToDelete = stakeholder;
    this.showDeleteModal = true;
    this.deleteMessage = '';
  }

  cancelDelete(): void {
    this.stakeholderToDelete = null;
    this.showDeleteModal = false;
  }

  deleteStakeholder(): void {
    if (!this.stakeholderToDelete) return;
    
    this.isLoading = true;
    this.deleteMessage = '';
    
    this.stakeholderService.deleteStakeholder(this.stakeholderToDelete.id!)
      .subscribe({
        next: () => {
          this.loadStakeholders();
          this.showDeleteModal = false;
          this.stakeholderToDelete = null;
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