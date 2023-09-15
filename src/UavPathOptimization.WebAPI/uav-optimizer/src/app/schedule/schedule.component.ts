import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ApiService} from "../api.service";
import {UavModel} from "../../models/UavModel";
import {AbrasSchedule, ScheduleEntry, ScheduleResponse, UavScheduleEntry} from "../../models/ScheduleResponse";
import {GeoCoordinate} from "../../models/GeoCoordinate";

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
            console.log("Schedule response:", response);
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
        });

        this.scheduleLoading = false;
    }
}
