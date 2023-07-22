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

  constructor(private activatedRoute: ActivatedRoute, private apiService : ApiService) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      if (params && params['response']) {
        this.pathResponse = JSON.parse(params['response']);
        console.log("Response:", this.pathResponse);
      }
    });
  }

}
