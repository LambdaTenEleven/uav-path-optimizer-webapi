import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7040/api';

  constructor(private http: HttpClient) {}

  optimizePath(uavCount: number, coordinates: any[]): Observable<any> {
    const payload = { uavCount, coordinates };
    return this.http.post<any>(`${this.baseUrl}/optimizePath`, payload);
  }
}
