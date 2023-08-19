import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ApiService } from './api.service'; // Replace with the actual path to your ApiService

@Injectable({
  providedIn: 'root'
})
export class HealthCheckGuard implements CanActivate {

  constructor(private apiService: ApiService, private router: Router) {}

  canActivate(): Promise<boolean> {
    return this.apiService.getHealth()
      .toPromise()
      .then(response => {
        if (response.status === 'Healthy') {
          return true; // Allow navigation
        } else {
          this.router.navigate(['/error']); // Redirect to error page
          return false; // Prevent navigation
        }
      })
      .catch(() => {
        this.router.navigate(['/error']);
        return false;
      });
  }
}
