import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { latLng, MapOptions, tileLayer, polyline, Layer, LayerGroup } from 'leaflet';
import { Marker, marker } from 'leaflet';
import { icon } from 'leaflet';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  mapOptions: MapOptions = {};
  layers: (Marker | LayerGroup)[] = [];
  uavCount: number = 1;
  coordinates: { latitude: number; longitude: number }[] = [];
  response: any;
  pathColors: string[] = [];
  errorMessage: string = '';

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
    this.clearPaths();
    this.apiService.optimizePath(this.uavCount, this.coordinates).subscribe(
      response => {
        this.response = response;
        this.drawPaths();
      },
      error => {
        //this.errorMessage = error.error.errors;
        for (var key in error.error.errors) {
          this.errorMessage += error.error.errors[key] + "\n";
        }
        alert(this.errorMessage)
        this.errorMessage = '';
      }
    );
  }

  drawPaths(): void {
    const pathLayers: Layer[] = [];
    this.response.uavPaths.forEach((path: any) => {
      const latLngs = path.path.map((point: any) => [point.latitude, point.longitude]);
      const color = getRandomColor();
      this.pathColors.push(color);
      const polylineOptions = {
        color: color,
        weight: 8
      };
      const polylineLayer = polyline(latLngs, polylineOptions);
      pathLayers.push(polylineLayer);
    });
    const pathsLayerGroup = new LayerGroup(pathLayers);
    this.layers.push(pathsLayerGroup);
  }

  clearPaths(): void {
    // Filter out the path layers from the layers array
    this.pathColors = [];
    this.layers = this.layers.filter(layer => !(layer instanceof LayerGroup));
  }

  getPathColor(path: any): string {
    const index = this.response.uavPaths.indexOf(path);
    return this.pathColors[index];
  }

  clearMap(): void {
    this.pathColors = [];
    this.layers = [];
    this.coordinates = [];
    this.response = null;
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
