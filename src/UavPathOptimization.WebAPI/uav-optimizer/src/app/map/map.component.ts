import {Component, OnInit} from '@angular/core';
import {ApiService} from '../api.service';
import {icon, Icon, latLng, LatLng, LayerGroup, MapOptions, Marker, marker, polyline, tileLayer} from 'leaflet';
import {Router} from '@angular/router';
import {GeoCoordinate} from "../../models/GeoCoordinate";
import {OptimizePathResponse} from "../../models/OptimizePathResponse";
import {MarkerType} from "./MarkerType";

// Warning: the code below is kinda bad, but it works ¯\_(ツ)_/¯
// Probably should refactor it later
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
    pathLayer: LayerGroup = new LayerGroup();
    uavCount = 1;
    coordinates: GeoCoordinate[] = [];
    response: OptimizePathResponse | null = null;
    pathColors: string[] = [];
    errorMessage = '';
    startingMarker: Marker | null = null;
    abrasMarkerLayer: LayerGroup = new LayerGroup();
    abrasMarker: Marker | null = null;
    abrasMarkerIsAdding = false;

    pathLoading = false;

    constructor(private apiService: ApiService, private router: Router) {
    }

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

    createMarkerIcon(type: MarkerType): Icon {
        // choose different icon url based on the type of marker
        let iconUrl = "assets/images/marker-icon.png";
        let iconRetinaUrl = "assets/images/marker-icon-2x.png";

        if (type === MarkerType.Start) {
            iconUrl = "assets/images/marker-icon-start.png";
            iconRetinaUrl = "assets/images/marker-icon-start-2x.png";
        } else if (type === MarkerType.Abras) {
            iconUrl = "assets/images/marker-icon-abras.png";
            iconRetinaUrl = "assets/images/marker-icon-abras-2x.png";
        }

        return icon({
            iconUrl: iconUrl,
            iconSize: [25, 41],
            iconAnchor: [12.5, 41],
            popupAnchor: [0, -41],
            iconRetinaUrl: iconRetinaUrl
        });
    }

    addMarker(latitude: number, longitude: number, type: MarkerType): void {
        const newMarker = marker([latitude, longitude], {
            icon: this.createMarkerIcon(type),
            draggable: true,
            // Name the marker after it's position index in the marker layer
            title: type === MarkerType.Abras ? "ABRAS" : "Waypoint " + (this.markerLayer.getLayers().length + 1).toString(),
        });

        if (type === MarkerType.Abras) {
            if (this.abrasMarker) {
                this.abrasMarkerLayer.removeLayer(this.abrasMarker);
                console.log("Removing old abras marker:", this.abrasMarker);
            }
            console.log("Adding new abras marker:", newMarker)
            this.abrasMarker = newMarker;
            this.abrasMarkerLayer.addLayer(newMarker);
            this.abrasMarker.on('click', (event: any) => {
                const marker = event.target;
                const confirmed = confirm('Are you sure you want to delete this marker?');
                if (confirmed) {
                    this.abrasMarkerLayer.removeLayer(marker);
                    this.abrasMarker = null;
                }
            });
            this.abrasMarker.on('dragend', (_: any) => {
            });
        } else {
            this.markerLayer.addLayer(newMarker);
            this.coordinates.push(new GeoCoordinate(latitude, longitude));
            this.updateStartingMarker();
            this.attachMarkerEventListeners(newMarker);
        }
    }

    attachMarkerEventListeners(marker: Marker): void {
        // Attach event listeners only when the marker is created
        marker.on('dragend', (event: any) => {
            const marker = event.target;
            const position = marker.getLatLng();
            const index = this.markerLayer.getLayers().indexOf(marker);
            this.coordinates[index] = new GeoCoordinate(position.lat, position.lng);
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
            if (!this.startingMarker || !this.markerLayer.hasLayer(this.startingMarker)) {
                // Set the first marker as the starting marker
                this.startingMarker = markerLayers[0] as Marker;
                this.startingMarker.setIcon(this.createMarkerIcon(MarkerType.Start));
            }
        } else {
            this.startingMarker = null;
        }

        // Update the title of the markers
        for (let i = 0; i < markerLayers.length; i++) {
            const marker = markerLayers[i] as Marker;
            marker.options.title = "Waypoint " + (i + 1).toString();
        }
    }

    onMapClick(event: any): void {
        if (this.abrasMarkerIsAdding) {
            this.addMarker(event.latlng.lat, event.latlng.lng, MarkerType.Abras);
            this.setAbrasMarkerAddingMode(false);
        } else {
            this.addMarker(event.latlng.lat, event.latlng.lng, MarkerType.Waypoint);
        }

        this.saveMapState();
    }

    optimizePath(): void {
        this.pathLoading = true;
        this.clearPaths();
        this.apiService.optimizePath(this.uavCount, this.coordinates).subscribe(
            (response) => {
                this.response = response;
                this.drawPaths();
                this.pathLoading = false;
            },
            (error) => {
                for (const key in error.error.errors) {
                    this.errorMessage += error.error.errors[key] + '\n';
                }
                alert(this.errorMessage);
                this.errorMessage = '';
                this.pathLoading = false;
            }
        );

        console.log("Paths optimized:", this.response)

        this.saveMapState();
    }

    drawPaths(): void {
        this.response?.uavPaths.forEach((path: any) => {
            const latLngs = path.path.map((point: any) => [point.latitude, point.longitude]);
            const color = getRandomColor();
            this.pathColors.push(color);
            const polylineOptions = {
                color: color,
                weight: 8
            };
            const polylineLayer = polyline(latLngs, polylineOptions);
            this.pathLayer.addLayer(polylineLayer);
        });
    }

    clearPaths(): void {
        this.pathColors = [];
        this.pathLayer.clearLayers();
    }

    clearMap(): void {
        this.pathColors = [];
        this.coordinates = [];
        this.response = null;
        this.markerLayer.clearLayers();
        this.pathLayer.clearLayers();
        this.startingMarker = null;
        this.abrasMarkerLayer.clearLayers();
        this.abrasMarker = null;
        this.saveMapState();
    }

    goToSchedule(): void {
        this.saveMapState();
        // throw error if path is not optimized
        if (!this.response) {
            alert("Please optimize the path first!");
            return;
        }
        //throw error if abras marker is not set
        if (!this.abrasMarker) {
            alert("Please set the Abras marker first!");
            return;
        }

        this.router.navigate(['/schedule'], {
            queryParams: {
                response: JSON.stringify(this.response),
                abrasCoordinates: JSON.stringify(
                    new GeoCoordinate(this.abrasMarker.getLatLng().lat, this.abrasMarker.getLatLng().lng)
                )
            }
        });
    }

    saveMapState(): void {
        const mapState = {
            coordinates: this.coordinates,
            response: this.response,
            zoom: this.mapZoom,
            center: this.mapCenter,
            abrasCoordinates: this.abrasMarker ? [this.abrasMarker.getLatLng().lat, this.abrasMarker.getLatLng().lng] : null,
            uavCount: this.uavCount
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

            for (let i = 0; i < mapState.coordinates.length; i++) {
                const coordinate = mapState.coordinates[i];
                console.log("Adding marker:", coordinate.latitude, coordinate.longitude);
                this.addMarker(coordinate.latitude, coordinate.longitude, i === 0 ? MarkerType.Start : MarkerType.Waypoint);
            }

            this.uavCount = mapState.uavCount;
            if (mapState.abrasCoordinates) {
                this.addMarker(mapState.abrasCoordinates[0], mapState.abrasCoordinates[1], MarkerType.Abras);
            }

            this.response = mapState.response;
            this.drawPaths();
        }
    }

    setAbrasMarkerAddingMode(isAdding: boolean): void {
        this.abrasMarkerIsAdding = isAdding;
        console.log("Abras marker adding mode:", this.abrasMarkerIsAdding);
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
