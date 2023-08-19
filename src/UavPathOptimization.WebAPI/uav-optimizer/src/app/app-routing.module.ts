import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapComponent } from './map/map.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { HealthCheckGuard } from './health-check.guard';
import {ErrorComponent} from "./error/error.component";

const routes: Routes = [
  { path: '', redirectTo: 'map', pathMatch: 'full'},
  { path: 'map', component: MapComponent, canActivate: [HealthCheckGuard] },
  { path: 'schedule', component: ScheduleComponent, canActivate: [HealthCheckGuard] },
  {path: 'error', component: ErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
