import {GeoCoordinate} from "./GeoCoordinate";


export class ScheduleResponse {
  uavPathSchedules: SchedulePath[];
  abrasScheduleEntries: UavScheduleEntry[];

  constructor(uavPathSchedules: SchedulePath[], abrasScheduleEntries: UavScheduleEntry[]) {
    this.uavPathSchedules = uavPathSchedules;
    this.abrasScheduleEntries = abrasScheduleEntries;
  }
}

export class SchedulePath {
  uavModelId: string;
  uavScheduleEntries: UavScheduleEntry[];

  constructor(uavModelId: string, uavScheduleEntries: UavScheduleEntry[]) {
    this.uavModelId = uavModelId;
    this.uavScheduleEntries = uavScheduleEntries;
  }
}

export class UavScheduleEntry {
  location: GeoCoordinate;
  isPBR: boolean;
  arrivalTime: string | null;
  departureTime: string | null;
  timeSpent: string;
  batteryTimeLeft: string;

  constructor(location: GeoCoordinate, isPBR: boolean, arrivalTime: string | null, departureTime: string | null, timeSpent: string, batteryTimeLeft: string) {
    this.location = location;
    this.isPBR = isPBR;
    this.timeSpent = timeSpent;
    this.batteryTimeLeft = batteryTimeLeft;

    this.arrivalTime = arrivalTime;
    this.departureTime = departureTime;
  }

  private readonly dateFormatOptions: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
  };

  getArrivalTimeFormatted(): string {
    if(this.arrivalTime == null) {
      return '';
    }
    return new Date(this.arrivalTime).toLocaleString('uk-UA', this.dateFormatOptions);
  }

  getDepartureTimeFormatted(): string {
    if(this.departureTime == null) {
      return '';
    }
    return new Date(this.departureTime).toLocaleString('uk-UA', this.dateFormatOptions);
  }
}
