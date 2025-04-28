import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { StakeholderService, Stakeholder } from '../../../services/stakeholder.service';
import { AuthService } from '../../../services/auth.service';
import { EnumService, EnumOption } from '../../../services/enum.service';

@Component({
  selector: 'app-stakeholder-form',
  templateUrl: './stakeholder-form.component.html',
  styleUrls: ['./stakeholder-form.component.css']
})
export class StakeholderFormComponent implements OnInit {
  stakeholderForm: FormGroup;
  isEditing = false;
  isViewOnly = false;
  stakeholderId: string | null = null;
  isSubmitting = false;
  errorMessage = '';
  isLoadingStakeholder = false;
  pageTitle = 'Add New Stakeholder';

  organizationTypes: EnumOption[] = [];
  seniorityLevels: EnumOption[] = [];

  constructor(
    private fb: FormBuilder,
    private stakeholderService: StakeholderService,
    private authService: AuthService,
    private enumService: EnumService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.stakeholderForm = this.fb.group({
      name: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      designation: ['', [Validators.required]],
      organization: ['', [Validators.required]],
      organizationType: [null, [Validators.required]],
      joiningDate: ['', [Validators.required]],
      seniorityLevel: [null, [Validators.required]],
      createdBy: [{value: '', disabled: true}],
      createdDate: [{value: '', disabled: true}]
    });
  }

  ngOnInit(): void {
    // Load enum values from API
    this.loadEnumValues();
    
    this.stakeholderId = this.route.snapshot.paramMap.get('id');
    
    // Check the current route to determine mode (add, edit, or view)
    if (this.router.url.includes('/stakeholder/add')) {
      this.pageTitle = 'Add New Stakeholder';
      this.isEditing = false;
      this.isViewOnly = false;
    } else if (this.router.url.includes('/stakeholder/edit/')) {
      this.pageTitle = 'Edit Stakeholder';
      this.isEditing = true;
      this.isViewOnly = false;
    } else if (this.stakeholderId) {
      this.pageTitle = 'View Stakeholder';
      this.isViewOnly = true;
      this.stakeholderForm.disable();
    }

    if (this.isEditing || this.isViewOnly) {
      if (this.stakeholderId) {
        this.loadStakeholder(this.stakeholderId);
      }
    }
  }

  loadEnumValues(): void {
    this.enumService.getOrganizationTypes().subscribe({
      next: (types) => {
        this.organizationTypes = types;
      },
      error: (error) => {
        console.error('Error loading organization types', error);
      }
    });

    this.enumService.getSeniorityLevels().subscribe({
      next: (levels) => {
        this.seniorityLevels = levels;
      },
      error: (error) => {
        console.error('Error loading seniority levels', error);
      }
    });
  }

  loadStakeholder(id: string): void {
    this.isLoadingStakeholder = true;
    this.stakeholderService.getStakeholder(id)
      .subscribe({
        next: (stakeholder) => {
          this.stakeholderForm.patchValue({
            name: stakeholder.name,
            dateOfBirth: this.formatDateForInput(stakeholder.dateOfBirth),
            phone: stakeholder.phone,
            designation: stakeholder.designation,
            organization: stakeholder.organization,
            organizationType: stakeholder.organizationType,
            joiningDate: this.formatDateForInput(stakeholder.joiningDate),
            seniorityLevel: stakeholder.seniorityLevel,
            createdBy: stakeholder.createdBy,
            createdDate: stakeholder.createdDate ? new Date(stakeholder.createdDate).toLocaleString() : ''
          });
          this.isLoadingStakeholder = false;
        },
        error: (error) => {
          this.errorMessage = 'Error loading stakeholder. Please try again.';
          this.isLoadingStakeholder = false;
          console.error('Error loading stakeholder', error);
        }
      });
  }

  // Format date from API to work with input[type="date"]
  formatDateForInput(date: any): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  // Format date for display in view-only mode
  formatDate(dateStr: string): string {
    if (!dateStr) return '';
    const date = new Date(dateStr);
    
    const options: Intl.DateTimeFormatOptions = { 
      year: 'numeric', 
      month: 'long', 
      day: 'numeric' 
    };
    
    return date.toLocaleDateString(undefined, options);
  }

  // Navigate to edit mode
  editStakeholder(): void {
    if (this.stakeholderId) {
      this.router.navigate(['/stakeholder/edit', this.stakeholderId]);
    }
  }

  onSubmit(): void {
    if (this.stakeholderForm.invalid || this.isViewOnly) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formData = this.stakeholderForm.value;
    const stakeholder: Stakeholder = {
      name: formData.name,
      dateOfBirth: formData.dateOfBirth,
      phone: formData.phone,
      designation: formData.designation,
      organization: formData.organization,
      organizationType: parseInt(formData.organizationType, 10),
      joiningDate: formData.joiningDate,
      seniorityLevel: parseInt(formData.seniorityLevel, 10)
    };

    if (this.isEditing && this.stakeholderId) {
      this.updateStakeholder(this.stakeholderId, stakeholder);
    } else {
      this.createStakeholder(stakeholder);
    }
  }

  createStakeholder(stakeholder: Stakeholder): void {
    this.stakeholderService.createStakeholder(stakeholder)
      .subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.router.navigate(['/stakeholder']);
        },
        error: (error) => {
          this.isSubmitting = false;
          this.errorMessage = error.error || 'Error creating stakeholder. Please try again.';
          console.error('Error creating stakeholder', error);
        }
      });
  }

  updateStakeholder(id: string, stakeholder: Stakeholder): void {
    this.stakeholderService.updateStakeholder(id, stakeholder)
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/stakeholder']);
        },
        error: (error) => {
          this.isSubmitting = false;
          this.errorMessage = error.error || 'Error updating stakeholder. Please try again.';
          console.error('Error updating stakeholder', error);
        }
      });
  }

  cancel(): void {
    this.router.navigate(['/stakeholder']);
  }
  
  // Helper method to get enum name for display
  getEnumName(enumArray: EnumOption[], id: number): string {
    const item = enumArray.find(item => item.id === id);
    return item ? item.name : '';
  }
} 