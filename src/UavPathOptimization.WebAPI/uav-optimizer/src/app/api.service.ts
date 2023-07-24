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
    return this.http.post<any>(`${this.baseUrl}/optimize_path`, payload);
  }

  getUavModels(pageNumber: number, pageSize: number, keyword: string, sortField: string, sortDirection: number) {
    const params = {
      Page: pageNumber.toString(),
      Size: pageSize.toString(),
      Keyword: keyword,
      SortField: sortField,
      SortDirection: sortDirection
    };

    return this.http.get<any>(this.baseUrl + '/uav_model', { params });
  }

  getSchedule(uavModelId: string, path: [], departureTimeStart: string, monitoringTime: string, chargingTime: string, abrasSpeed : number) {
    const payload = {
      UavModelId: uavModelId,
      Path: path,
      DepartureTimeStart: departureTimeStart,
      MonitoringTime: monitoringTime,
      ChargingTime: chargingTime,
      AbrasSpeed: abrasSpeed
    };

    return this.http.post<any>(this.baseUrl + '/schedule', payload);
  }
}
