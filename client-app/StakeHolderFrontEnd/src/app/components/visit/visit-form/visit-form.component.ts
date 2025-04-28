import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { VisitService, Visit } from '../../../services/visit.service';
import { ApiService } from '../../../services/api.service';
import { EnumService, EnumOption } from '../../../services/enum.service';

interface Stakeholder {
  id: string;
  name: string;
}

@Component({
  selector: 'app-visit-form',
  templateUrl: './visit-form.component.html',
  styleUrls: ['./visit-form.component.css']
})
export class VisitFormComponent implements OnInit {
  visitForm: FormGroup;
  isEditing = false;
  isViewOnly = false;
  visitId: string | null = null;
  isSubmitting = false;
  errorMessage = '';
  stakeholders: Stakeholder[] = [];
  isLoadingStakeholders = false;
  isLoadingVisit = false;
  pageTitle = 'Add New Visit';
  gifts: EnumOption[] = [];

  constructor(
    private fb: FormBuilder,
    private visitService: VisitService,
    private apiService: ApiService,
    private enumService: EnumService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.visitForm = this.fb.group({
      stakeholderId: ['', [Validators.required]],
      visitedPlace: ['', [Validators.required]],
      visitedTime: ['', [Validators.required]],
      visitedDate: ['', [Validators.required]],
      gift: [null],
      createdBy: [{value: '', disabled: true}],
      createdDate: [{value: '', disabled: true}]
    });
  }

  ngOnInit(): void {
    this.loadStakeholders();
    this.loadGifts();
    
    this.visitId = this.route.snapshot.paramMap.get('id');
    
    // Check the current route to determine mode (add, edit, or view)
    if (this.router.url.includes('/visit/add')) {
      this.pageTitle = 'Add New Visit';
      this.isEditing = false;
      this.isViewOnly = false;
    } else if (this.router.url.includes('/visit/edit/')) {
      this.pageTitle = 'Edit Visit';
      this.isEditing = true;
      this.isViewOnly = false;
    } else if (this.visitId) {
      this.pageTitle = 'View Visit';
      this.isViewOnly = true;
      this.visitForm.disable();
    }

    if (this.visitId) {
      this.loadVisit(this.visitId);
    }
  }

  loadGifts(): void {
    this.enumService.getGifts().subscribe({
      next: (gifts) => {
        this.gifts = gifts;
      },
      error: (error) => {
        console.error('Error loading gifts', error);
      }
    });
  }

  loadStakeholders(): void {
    this.isLoadingStakeholders = true;
    this.apiService.get<Stakeholder[]>('/api/stakeholder')
      .subscribe({
        next: (data) => {
          this.stakeholders = data;
          this.isLoadingStakeholders = false;
        },
        error: (error) => {
          this.errorMessage = 'Error loading stakeholders. Please try again.';
          this.isLoadingStakeholders = false;
          console.error('Error loading stakeholders', error);
        }
      });
  }

  loadVisit(id: string): void {
    this.isLoadingVisit = true;
    this.visitService.getVisit(id)
      .subscribe({
        next: (visit) => {
          this.visitForm.patchValue({
            stakeholderId: visit.stakeholderId,
            visitedPlace: visit.visitedPlace,
            visitedTime: visit.visitedTime,
            visitedDate: this.formatDateForInput(visit.visitedDate),
            gift: visit.gift,
            createdBy: visit.createdBy,
            createdDate: visit.createdDate ? new Date(visit.createdDate).toLocaleString() : ''
          });
          this.isLoadingVisit = false;
        },
        error: (error) => {
          this.errorMessage = 'Error loading visit. Please try again.';
          this.isLoadingVisit = false;
          console.error('Error loading visit', error);
        }
      });
  }

  // Format date from API to work with input[type="date"]
  formatDateForInput(date: any): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  onSubmit(): void {
    if (this.visitForm.invalid || this.isViewOnly) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formData = this.visitForm.value;
    const visit: Visit = {
      stakeholderId: formData.stakeholderId,
      visitedPlace: formData.visitedPlace,
      visitedTime: formData.visitedTime,
      visitedDate: formData.visitedDate,
      gift: formData.gift ? parseInt(formData.gift, 10) : 0
    };

    if (this.isEditing && this.visitId) {
      this.updateVisit(this.visitId, visit);
    } else {
      this.createVisit(visit);
    }
  }

  createVisit(visit: Visit): void {
    this.visitService.createVisit(visit)
      .subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.router.navigate(['/visit']);
        },
        error: (error) => {
          this.isSubmitting = false;
          this.errorMessage = error.error || 'Error creating visit. Please try again.';
          console.error('Error creating visit', error);
        }
      });
  }

  updateVisit(id: string, visit: Visit): void {
    this.visitService.updateVisit(id, visit)
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/visit']);
        },
        error: (error) => {
          this.isSubmitting = false;
          this.errorMessage = error.error || 'Error updating visit. Please try again.';
          console.error('Error updating visit', error);
        }
      });
  }

  cancel(): void {
    this.router.navigate(['/visit']);
  }
  
  // Get stakeholder name by id
  getStakeholderName(id: string): string {
    const stakeholder = this.stakeholders.find(s => s.id === id);
    return stakeholder ? stakeholder.name : 'Unknown';
  }
  
  // Helper method to get enum name for display
  getGiftName(id: number): string {
    const gift = this.gifts.find(g => g.id === id);
    return gift ? gift.name : 'None';
  }
}