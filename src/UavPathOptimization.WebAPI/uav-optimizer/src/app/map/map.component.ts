import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import {latLng, MapOptions, tileLayer, polyline, Layer, LayerGroup, Control, Zoom, LatLng} from 'leaflet';
import { Marker, marker, icon, LatLngExpression, Icon } from 'leaflet';
import { Router } from '@angular/router';
import ZoomOptions = Control.ZoomOptions;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  mapOptions: MapOptions = {};
  mapZoom = 2;
  mapCenter: LatLng = new LatLng(0, 0);
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
      zoom: this.mapZoom,
      center: latLng(0, 0)
    };

    console.log("Map options:", this.mapOptions);

    this.loadMapState();
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

    this.markerLayer.addLayer(newMarker);
    this.coordinates.push({ latitude, longitude });
    this.updateStartingMarker();

    this.attachMarkerEventListeners(newMarker);
  }

  attachMarkerEventListeners(marker: Marker): void {
    // Attach event listeners only when the marker is created
    marker.on('dragend', (event: any) => {
      const marker = event.target;
      const position = marker.getLatLng();
      const index = this.markerLayer.getLayers().indexOf(marker);
      this.coordinates[index] = { latitude: position.lat, longitude: position.lng };
    });

    marker.on('click', (event: any) => {
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
    this.saveMapState();
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
    this.saveMapState();
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
    this.saveMapState();
  }

  goToSchedule(): void {
    this.saveMapState();
    this.router.navigate(['/schedule'], { queryParams: { response: JSON.stringify(this.response) } });
  }

  saveMapState(): void {
    const mapState = {
      coordinates: this.coordinates,
      response: this.response,
      zoom: this.mapZoom,
      center: this.mapCenter,
    };
    localStorage.setItem('mapState', JSON.stringify(mapState));
  }

  loadMapState(): void {
    const savedMapState = localStorage.getItem('mapState');
    if (savedMapState) {
      const mapState = JSON.parse(savedMapState);
      console.log("Loading map state:", mapState);

      this.mapZoom = mapState.zoom;
      this.mapCenter = mapState.center;

      for(const coordinate of mapState.coordinates) {
        console.log("Adding marker:", coordinate.latitude, coordinate.longitude);
        this.addMarker(coordinate.latitude, coordinate.longitude);
      }
      this.response = mapState.response;
      this.drawPaths();
    }
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
