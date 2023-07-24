export class UavModel {
  id: string;
  name: string;
  maxSpeed: number;
  maxFlightTime: string;

  constructor(id: string, name: string, maxSpeed: number, maxFlightTime: string) {
    this.id = id;
    this.name = name;
    this.maxSpeed = maxSpeed;
    this.maxFlightTime = maxFlightTime;
  }
}
