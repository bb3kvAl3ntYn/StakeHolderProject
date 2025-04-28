import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Visit {
  id?: string;
  stakeholderId: string;
  visitedPlace: string;
  visitedTime: string;
  visitedDate: Date;
  gift: number;
  createdBy?: string;
  createdDate?: Date;
}

@Injectable({
  providedIn: 'root'
})
export class VisitService {
  constructor(private apiService: ApiService) { }

  /**
   * Get all visits
   */
  getVisits(): Observable<Visit[]> {
    return this.apiService.get<Visit[]>('/api/visit');
  }

  /**
   * Get a visit by ID
   * @param id Visit ID
   */
  getVisit(id: string): Observable<Visit> {
    return this.apiService.get<Visit>(`/api/visit/${id}`);
  }

  /**
   * Create a new visit
   * @param visit Visit data
   */
  createVisit(visit: Visit): Observable<Visit> {
    return this.apiService.post<Visit>('/api/visit', visit);
  }

  /**
   * Update a visit
   * @param id Visit ID
   * @param visit Visit data
   */
  updateVisit(id: string, visit: Visit): Observable<any> {
    return this.apiService.put<any>(`/api/visit/${id}`, visit);
  }

  /**
   * Delete a visit
   * @param id Visit ID
   */
  deleteVisit(id: string): Observable<any> {
    return this.apiService.delete<any>(`/api/visit/${id}`);
  }
} 