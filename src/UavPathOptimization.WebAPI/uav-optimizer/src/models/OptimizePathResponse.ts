import {GeoCoordinate} from "./GeoCoordinate";

export class OptimizePathResponse {
  uavPaths: UavPath[];
  constructor(uavPaths: UavPath[]) {
    this.uavPaths = uavPaths;
  }
}

export class UavPath {
  vehicleId: number;
  path: GeoCoordinate[];
  distance: number;
  constructor(vehicleId: number, path: GeoCoordinate[], distance: number) {
    this.vehicleId = vehicleId;
    this.path = path;
    this.distance = distance;
  }
}
