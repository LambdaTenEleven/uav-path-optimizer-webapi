import {GeoCoordinate} from "./GeoCoordinate";

export class OptimizePathResponse {
  uavPaths: UavPath[];
  constructor(uavPaths: UavPath[]) {
    this.uavPaths = uavPaths;
  }
}

export class UavPath {
  uavId: number;
  path: GeoCoordinate[];
  distance: number;
  constructor(uavId: number, path: GeoCoordinate[], distance: number) {
    this.uavId = uavId;
    this.path = path;
    this.distance = distance;
  }
}
