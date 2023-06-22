import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { latLng, MapOptions, tileLayer, polyline } from 'leaflet';
import { Marker, marker } from 'leaflet';
import { icon } from 'leaflet';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  mapOptions: MapOptions = {};
  layers: (Marker | any)[] = [];
  uavCount: number = 1;
  coordinates: { latitude: number; longitude: number }[] = [];
  response: any;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.mapOptions = {
      layers: [
        tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          attribution: '&copy; OpenStreetMap contributors'
        })
      ],
      zoom: 2,
      center: latLng(0, 0)
    };
  }

  addMarker(latitude: number, longitude: number): void {
    const newMarker = marker([latitude, longitude], {
      icon: icon({
        iconUrl: 'assets/images/marker-icon.png',
        iconSize: [25, 41],
        iconAnchor: [12.5, 41],
        popupAnchor: [0, -41],
        iconRetinaUrl: 'assets/images/marker-icon-2x.png',
      })
    });
    this.layers.push(newMarker);
    this.coordinates.push({ latitude, longitude });
  }

  onMapClick(event: any): void {
    this.addMarker(event.latlng.lat, event.latlng.lng);
  }

  optimizePath(): void {
    this.apiService.optimizePath(this.uavCount, this.coordinates).subscribe(response => {
      this.response = response;
      this.drawPaths();
    });
  }

  drawPaths(): void {
    this.response.uavPaths.forEach((path: any) => {
      const latLngs = path.path.map((point: any) => [point.latitude, point.longitude]);
      const polylineOptions = {
        color: getRandomColor(),
        weight: 8
      };
      const polylineLayer = polyline(latLngs, polylineOptions);
      this.layers.push(polylineLayer);
    });
  }
}

function getRandomColor(): string {
  const letters = '0123456789ABCDEF';
  let color = '#';
  for (let i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}
