import {GeoCoordinate} from "./GeoCoordinate";


export class ScheduleResponse {
    uavPathSchedules: SchedulePath[];
    abrasSchedule: AbrasSchedule;

    constructor(uavPathSchedules: SchedulePath[], abrasSchedule: AbrasSchedule) {
        this.uavPathSchedules = uavPathSchedules;
        this.abrasSchedule = abrasSchedule;
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

export class AbrasSchedule {
    abrasScheduleEntries: ScheduleEntry[];

    constructor(abrasScheduleEntries: ScheduleEntry[]) {
        this.abrasScheduleEntries = abrasScheduleEntries;
    }
}

export class ScheduleEntry {
    location: GeoCoordinate;
    arrivalTime: string | null;
    departureTime: string | null;
    timeSpent: string;

    constructor(location: GeoCoordinate, arrivalTime: string | null, departureTime: string | null, timeSpent: string) {
        this.location = location;
        this.timeSpent = timeSpent;
        this.arrivalTime = arrivalTime;
        this.departureTime = departureTime;
    }

    getArrivalTimeFormatted(): string {
        if (this.arrivalTime == null) {
            return '';
        }
        return new Date(this.arrivalTime).toLocaleString('uk-UA', this.dateFormatOptions);
    }

    getDepartureTimeFormatted(): string {
        if (this.departureTime == null) {
            return '';
        }
        return new Date(this.departureTime).toLocaleString('uk-UA', this.dateFormatOptions);
    }

    private readonly dateFormatOptions: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
    };
}

export class UavScheduleEntry extends ScheduleEntry {
    isPBR: boolean;
    batteryTimeLeft: string;

    constructor(location: GeoCoordinate, arrivalTime: string | null, departureTime: string | null, timeSpent: string, batteryTimeLeft: string, isPBR: boolean) {
        super(location, arrivalTime, departureTime, timeSpent);
        this.isPBR = isPBR;
        this.batteryTimeLeft = batteryTimeLeft;
    }
}
