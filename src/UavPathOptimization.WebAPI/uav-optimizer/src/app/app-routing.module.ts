import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapComponent } from './map/map.component';
import { ScheduleComponent } from './schedule/schedule.component';

const routes: Routes = [
  { path: '', redirectTo: 'map', pathMatch: 'full' },
  { path: 'map', component: MapComponent },
  { path: 'schedule', component: ScheduleComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
