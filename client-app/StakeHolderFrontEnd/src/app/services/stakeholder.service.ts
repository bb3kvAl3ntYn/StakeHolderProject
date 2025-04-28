import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Stakeholder {
  id?: string;
  name: string;
  dateOfBirth: Date;
  phone: string;
  designation: string;
  organization: string;
  organizationType: number;
  joiningDate: Date;
  seniorityLevel: number;
  createdBy?: string;
  createdDate?: Date;
  status?: string;
}

@Injectable({
  providedIn: 'root'
})
export class StakeholderService {
  constructor(private apiService: ApiService) { }

  /**
   * Get all stakeholders
   */
  getStakeholders(): Observable<Stakeholder[]> {
    return this.apiService.get<Stakeholder[]>('/api/stakeholder');
  }

  /**
   * Get a stakeholder by ID
   * @param id Stakeholder ID
   */
  getStakeholder(id: string): Observable<Stakeholder> {
    return this.apiService.get<Stakeholder>(`/api/stakeholder/${id}`);
  }

  /**
   * Create a new stakeholder
   * @param stakeholder Stakeholder data
   */
  createStakeholder(stakeholder: Stakeholder): Observable<Stakeholder> {
    return this.apiService.post<Stakeholder>('/api/stakeholder', stakeholder);
  }

  /**
   * Update a stakeholder
   * @param id Stakeholder ID
   * @param stakeholder Stakeholder data
   */
  updateStakeholder(id: string, stakeholder: Stakeholder): Observable<any> {
    return this.apiService.put<any>(`/api/stakeholder/${id}`, stakeholder);
  }

  /**
   * Delete a stakeholder
   * @param id Stakeholder ID
   */
  deleteStakeholder(id: string): Observable<any> {
    return this.apiService.delete<any>(`/api/stakeholder/${id}`);
  }
} 