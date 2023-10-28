import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MapComponent } from './map/map.component';
import { HttpClientModule } from '@angular/common/http';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { ScheduleComponent } from './schedule/schedule.component';
import { UavModelDropdownComponent } from './uav-model-dropdown/uav-model-dropdown.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ErrorComponent } from './error/error.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatTabsModule} from "@angular/material/tabs";
import {GoogleChartsModule} from "angular-google-charts";

@NgModule({
  declarations: [
    AppComponent,
    MapComponent,
    ScheduleComponent,
    UavModelDropdownComponent,
    ErrorComponent,
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        LeafletModule,
        NgSelectModule,
        BrowserAnimationsModule,
        MatTabsModule,
        GoogleChartsModule,
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
