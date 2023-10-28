import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ApiService} from "../api.service";
import {UavModel} from "../../models/UavModel";
import {AbrasSchedule, ScheduleEntry, ScheduleResponse, UavScheduleEntry} from "../../models/ScheduleResponse";
import {GeoCoordinate} from "../../models/GeoCoordinate";
import {ChartType} from "angular-google-charts";
import * as moment from 'moment';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {
  pathResponse: any;
  selectedUavModelsIds: string[] = [];
  schedule: ScheduleResponse | null = null;

  departureTime: string = '';
  monitoringTime: string = '';
  chargingTime: string = '';
  abrasCoordinates: GeoCoordinate | null = null;
  abrasSpeed: number = 0;

  scheduleLoading: boolean = false;

  chartData: any[] = [];
  chartColumns: any[] = [
    {type: 'string', role: 'domain'},
    {type: 'string', role: 'data'},
    {type: 'string', role: 'data'},
    {type: 'date', role: 'data'},
    {type: 'date', role: 'data'},
    {type: 'number', role: 'data'},
    {type: 'number', role: 'data'},
    {type: 'string', role: 'data'},
  ];

  chartOptions: any = {
    height: 600,
    width: 1000,
  };

  constructor(private activatedRoute: ActivatedRoute, private apiService: ApiService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      if (params && params['response']) {
        this.pathResponse = JSON.parse(params['response']);
        console.log("Response:", this.pathResponse);
        this.selectedUavModelsIds = new Array(this.pathResponse.uavPaths.length);
      }
      if (params && params['abrasCoordinates']) {
        const abrasCoordinates = JSON.parse(params['abrasCoordinates']);
        this.abrasCoordinates = new GeoCoordinate(abrasCoordinates.latitude, abrasCoordinates.longitude);
        console.log("ABRAS coordinates:", this.abrasCoordinates);
      }
    });
  }

  selectedUavModelEvent(event: any, index: number) {
    this.selectedUavModelsIds[index] = event;
    console.log("Selected UAV models:", this.selectedUavModelsIds);
  }

  getSchedules() {
    this.scheduleLoading = true;

    const uavPaths = this.pathResponse.uavPaths.map((uavPath: any, index: number) => {
      return {
        UavModelId: this.selectedUavModelsIds[index],
        Coordinates: uavPath.path
      };
    });

    this.apiService.getSchedule(uavPaths, this.departureTime, this.monitoringTime, this.chargingTime, this.abrasSpeed, this.abrasCoordinates!).subscribe((response: any) => {
      this.schedule = new ScheduleResponse(response.uavPathSchedules.map((schedulePath: any) => {
          return {
            uavModelId: schedulePath.uavModelId,
            uavScheduleEntries: schedulePath.uavScheduleEntries.map((uavScheduleEntry: any) => {
              return new UavScheduleEntry(uavScheduleEntry.location, uavScheduleEntry.arrivalTime, uavScheduleEntry.departureTime, uavScheduleEntry.timeSpent, uavScheduleEntry.batteryTimeLeft, uavScheduleEntry.isPBR);
            })
          };
        }),
        new AbrasSchedule(response.abrasSchedule.abrasScheduleEntries.map((abrasScheduleEntry: any) => {
          return new ScheduleEntry(abrasScheduleEntry.location, abrasScheduleEntry.arrivalTime, abrasScheduleEntry.departureTime, abrasScheduleEntry.timeSpent);
        }))
      );

      // Clear the chartData array
      this.chartData = [];

      // for each UAV, generate a chart
      this.schedule.uavPathSchedules.forEach((schedulePath: any) => {
        this.chartData.push(this.generateChartData(schedulePath.uavScheduleEntries));
      });
    });

    this.scheduleLoading = false;
  }

  generateChartData(uavScheduleEntries: UavScheduleEntry[]): any {
    const rows: any[][] = [];

    if (this.departureTime) {
      const initialDate = new Date(this.departureTime);
      const firstDepartureDate = uavScheduleEntries[0].departureTime
        ? new Date(uavScheduleEntries[0].departureTime as string)
        : null;

      if (firstDepartureDate && firstDepartureDate.getTime() > initialDate.getTime()) {
        const waitingDuration = firstDepartureDate.getTime() - initialDate.getTime();
        // Add a waiting task
        rows.push([
          'waiting-task',
          'Waiting',
          null,
          initialDate,
          firstDepartureDate,
          waitingDuration,
          100,
          null,
        ]);
      }
    }

    uavScheduleEntries.forEach((entry, index) => {
      // Parse arrival and departure times as Date objects
      const startDate = entry.arrivalTime ? new Date(entry.arrivalTime as string) : null;
      const endDate = entry.departureTime ? new Date(entry.departureTime as string) : null;

      if (index > 0) {
        const moveStartDate = uavScheduleEntries[index - 1].departureTime
          ? new Date(uavScheduleEntries[index - 1].departureTime as string)
          : null;
        const moveDuration = startDate && moveStartDate ? startDate.getTime() - moveStartDate.getTime() : 0;

        // Add a moving task
        rows.push([
          'move-task-' + index,
          'Moving',
          null,
          moveStartDate,
          startDate,
          moveDuration,
          100,
          null,
        ]);
      }

      let taskName = 'Monitoring';
      if (entry.isPBR) {
        taskName = 'Monitoring+Charging';
      }

      const monitoringDuration =
        startDate && endDate ? endDate.getTime() - startDate.getTime() : 0;

      // Add a monitoring or charging task only if endDate is not null
      if (endDate !== null && monitoringDuration > 0) {
        rows.push([
          'monitor-task-' + index,
          taskName,
          null,
          startDate,
          endDate,
          monitoringDuration,
          100,
          null,
        ]);
      }
    });

    return rows;
  }

  readonly ChartType = ChartType;
}
