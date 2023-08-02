import {GeoCoordinate} from "./GeoCoordinate";

export class UavPath {
    uavModelId: string;
    coordinates: GeoCoordinate[];
    constructor(uavModelId: string, path: GeoCoordinate[]) {
        this.uavModelId = uavModelId;
        this.coordinates = path;
    }
}