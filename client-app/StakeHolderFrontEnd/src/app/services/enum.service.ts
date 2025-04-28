import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface EnumOption {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class EnumService {
  private apiUrl = environment.apiUrl + '/api/enums';

  constructor(private http: HttpClient) { }

  getSeniorityLevels(): Observable<EnumOption[]> {
    return this.http.get<EnumOption[]>(`${this.apiUrl}/SeniorityLevels`);
  }

  getOrganizationTypes(): Observable<EnumOption[]> {
    return this.http.get<EnumOption[]>(`${this.apiUrl}/OrganizationTypes`);
  }

  getGifts(): Observable<EnumOption[]> {
    return this.http.get<EnumOption[]>(`${this.apiUrl}/Gifts`);
  }
} 