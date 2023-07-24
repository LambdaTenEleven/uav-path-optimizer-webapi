import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {ApiService} from "../api.service";

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {
  pathResponse: any;
  selectedUavModels: any[] = [];
  schedules: any[] = [];

  departureTime: string = '';
  monitoringTime: string = '';
  chargingTime: string = '';
  constructor(private activatedRoute: ActivatedRoute, private apiService : ApiService) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      if (params && params['response']) {
        this.pathResponse = JSON.parse(params['response']);
        console.log("Response:", this.pathResponse);
        this.selectedUavModels = new Array(this.pathResponse.uavPaths.length);
        this.schedules = new Array(this.pathResponse.uavPaths.length);
      }
    });
  }

  selectedUavModelEvent(event: any, index: number) {
    this.selectedUavModels[index] = event;
    console.log("Selected UAV models:", this.selectedUavModels);
  }

  getSchedules() {
    for (let i = 0; i < this.pathResponse.uavPaths.length; i++) {
      this.getSchedule(i);
    }
  }

  getSchedule(id: number) {
    console.log("Selected UAV Model:", this.selectedUavModels[id])
    console.log("Selected UAV Path:", this.pathResponse.uavPaths[id].path)
    this.apiService.getSchedule(this.selectedUavModels[id], this.pathResponse.uavPaths[id].path, this.departureTime, this.monitoringTime, this.chargingTime, 0).subscribe((response: any) => {
      this.schedules[id] = response;
      console.log("Schedule:", this.schedules[id]);
    });
  }
}
