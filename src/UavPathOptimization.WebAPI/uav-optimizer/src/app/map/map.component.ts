import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { latLng, MapOptions, tileLayer, polyline, Layer, LayerGroup } from 'leaflet';
import { Marker, marker, icon, LatLngExpression, Icon } from 'leaflet';
import { Router } from '@angular/router';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  mapOptions: MapOptions = {};
  markerLayer: LayerGroup = new LayerGroup();
  pathLayers: LayerGroup = new LayerGroup();
  uavCount = 1;
  coordinates: { latitude: number; longitude: number }[] = [];
  response: any;
  pathColors: string[] = [];
  errorMessage = '';
  startingMarker: Marker | null = null;

  constructor(private apiService: ApiService, private router: Router) {}

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

  createMarkerIcon(isStartingMarker: boolean): Icon {
    const iconUrl = isStartingMarker
      ? 'assets/images/marker-icon-start.png'
      : 'assets/images/marker-icon.png';
    const iconRetinaUrl = isStartingMarker
      ? 'assets/images/marker-icon-start-2x.png'
      : 'assets/images/marker-icon-2x.png';

    return icon({
      iconUrl: iconUrl,
      iconSize: [25, 41],
      iconAnchor: [12.5, 41],
      popupAnchor: [0, -41],
      iconRetinaUrl: iconRetinaUrl
    });
  }

  addMarker(latitude: number, longitude: number): void {
    const isStartingMarker = this.startingMarker === null;

    const newMarker = marker([latitude, longitude], {
      icon: this.createMarkerIcon(isStartingMarker),
      draggable: true
    });

    newMarker.on('dragend', (event: any) => {
      const marker = event.target;
      const position = marker.getLatLng();
      const index = this.markerLayer.getLayers().indexOf(marker);
      this.coordinates[index] = { latitude: position.lat, longitude: position.lng };
    });

    newMarker.on('click', (event: any) => {
      const marker = event.target;
      const index = this.markerLayer.getLayers().indexOf(marker);

      if (index !== -1) {
        const confirmed = confirm('Are you sure you want to delete this marker?');
        if (confirmed) {
          this.markerLayer.removeLayer(marker);
          this.coordinates.splice(index, 1);
          this.updateStartingMarker();
        }
      }
    });

    this.markerLayer.addLayer(newMarker);
    this.coordinates.push({ latitude, longitude });
    this.updateStartingMarker();
  }

  updateStartingMarker(): void {
    const markerLayers = this.markerLayer.getLayers();
    if (markerLayers.length > 0) {
      if (!this.startingMarker) {
        // Set the first marker as the starting marker
        this.startingMarker = markerLayers[0] as Marker;
        this.startingMarker.setIcon(this.createMarkerIcon(true));
      } else if (!this.markerLayer.hasLayer(this.startingMarker)) {
        // If the starting marker is deleted, set the next available marker as the starting marker
        this.startingMarker = markerLayers[0] as Marker;
        this.startingMarker.setIcon(this.createMarkerIcon(true));
      }
    } else {
      this.startingMarker = null;
    }
  }

  onMapClick(event: any): void {
    this.addMarker(event.latlng.lat, event.latlng.lng);
  }

  optimizePath(): void {
    this.clearPaths();
    this.apiService.optimizePath(this.uavCount, this.coordinates).subscribe(
      (response) => {
        this.response = response;
        this.drawPaths();
      },
      (error) => {
        for (const key in error.error.errors) {
          this.errorMessage += error.error.errors[key] + '\n';
        }
        alert(this.errorMessage);
        this.errorMessage = '';
      }
    );
  }

  drawPaths(): void {
    this.response.uavPaths.forEach((path: any) => {
      const latLngs = path.path.map((point: any) => [point.latitude, point.longitude]);
      const color = getRandomColor();
      this.pathColors.push(color);
      const polylineOptions = {
        color: color,
        weight: 8
      };
      const polylineLayer = polyline(latLngs, polylineOptions);
      this.pathLayers.addLayer(polylineLayer);
    });
  }

  clearPaths(): void {
    this.pathColors = [];
    this.pathLayers.clearLayers();
  }

  getPathColor(path: any): string {
    const index = this.response.uavPaths.indexOf(path);
    return this.pathColors[index];
  }

  clearMap(): void {
    this.pathColors = [];
    this.coordinates = [];
    this.response = null;
    this.markerLayer.clearLayers();
    this.pathLayers.clearLayers();
    this.startingMarker = null;
  }

  goToSchedule(): void {
    this.router.navigate(['/schedule'], { queryParams: { response: JSON.stringify(this.response) } });
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
