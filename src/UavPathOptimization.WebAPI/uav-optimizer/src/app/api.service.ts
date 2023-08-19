import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {GeoCoordinate} from "../models/GeoCoordinate";
import {OptimizePathResponse} from "../models/OptimizePathResponse";
import {Page} from "../models/Page";
import {UavModel} from "../models/UavModel";
import {SortDirection} from "../models/SortDirection";
import {ScheduleResponse} from "../models/ScheduleResponse";
import {UavPath} from "../models/UavPath";

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7040/api';

  constructor(private http: HttpClient) {}

  optimizePath(uavCount: number, coordinates: GeoCoordinate[]): Observable<OptimizePathResponse> {
    const payload = { uavCount, coordinates };
    return this.http.post<any>(`${this.baseUrl}/optimize_path`, payload);
  }

  getUavModels(pageNumber: number, pageSize: number, keyword: string, sortField: string, sortDirection: SortDirection) : Observable<Page<UavModel>> {
    const params = {
      Page: pageNumber.toString(),
      Size: pageSize.toString(),
      Keyword: keyword,
      SortField: sortField,
      SortDirection: sortDirection
    };

    return this.http.get<any>(this.baseUrl + '/uav_model', { params });
  }

  getSchedule(uavPaths: UavPath[], departureTimeStart: string, monitoringTime: string, chargingTime: string, abrasSpeed : number) : Observable<ScheduleResponse> {
    const payload = {
      Paths: uavPaths,
      DepartureTimeStart: departureTimeStart,
      MonitoringTime: monitoringTime,
      ChargingTime: chargingTime,
      AbrasSpeed: abrasSpeed
    };

    return this.http.post<any>(this.baseUrl + '/schedule', payload);
  }

  getHealth() : Observable<any> {
    return this.http.get<any>(this.baseUrl + '/_health');
  }
}
