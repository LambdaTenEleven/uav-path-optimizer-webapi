import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ApiService} from "../api.service";
import {UavModel} from "../../models/UavModel";
import {ScheduleResponse, UavScheduleEntry} from "../../models/ScheduleResponse";

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {
  pathResponse: any;
  selectedUavModelsIds: string[] = [];
  schedules: ScheduleResponse[] = [];

  departureTime: string = '';
  monitoringTime: string = '';
  chargingTime: string = '';

  scheduleLoading: boolean = false;

  constructor(private activatedRoute: ActivatedRoute, private apiService: ApiService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      if (params && params['response']) {
        this.pathResponse = JSON.parse(params['response']);
        console.log("Response:", this.pathResponse);
        this.selectedUavModelsIds = new Array(this.pathResponse.uavPaths.length);
        this.schedules = new Array(this.pathResponse.uavPaths.length);
      }
    });
  }

  selectedUavModelEvent(event: any, index: number) {
    this.selectedUavModelsIds[index] = event;
    console.log("Selected UAV models:", this.selectedUavModelsIds);
  }

  getSchedules() {
    for (let i = 0; i < this.pathResponse.uavPaths.length; i++) {
      this.getSchedule(i);
    }
  }

  getSchedule(id: number) {
    console.log("Selected UAV Model:", this.selectedUavModelsIds[id])
    console.log("Selected UAV Path:", this.pathResponse.uavPaths[id].path)
    this.scheduleLoading = true;
    this.apiService.getSchedule(this.selectedUavModelsIds[id], this.pathResponse.uavPaths[id].path, this.departureTime, this.monitoringTime, this.chargingTime, 0).subscribe((response: ScheduleResponse) => {
      // for some reason, the response is not parsed as a ScheduleResponse object
      let uavScheduleEntries: UavScheduleEntry[] = new Array(response.uavScheduleEntries.length);
      for(let i = 0; i < response.uavScheduleEntries.length; i++) {
        uavScheduleEntries[i] = new UavScheduleEntry(
          response.uavScheduleEntries[i].location,
          response.uavScheduleEntries[i].isPBR,
          response.uavScheduleEntries[i].arrivalTime,
          response.uavScheduleEntries[i].departureTime,
          response.uavScheduleEntries[i].timeSpent,
          response.uavScheduleEntries[i].batteryTimeLeft);
      }
      this.schedules[id] = new ScheduleResponse(response.uavModelId, uavScheduleEntries);
      this.scheduleLoading = false;
      console.log("Schedule:", this.schedules[id]);
    });
  }
}
