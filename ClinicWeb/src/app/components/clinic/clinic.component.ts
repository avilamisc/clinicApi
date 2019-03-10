import { Component, OnInit } from '@angular/core';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinic-distance.model';
import { LocationService } from 'src/app/core/services/location.service';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html',
  styleUrls: ['./clinic.component.styl']
})
export class ClinicComponent implements OnInit {
  public clinics: ClinicDistanceModel[] = [];
  public mapType = 'satellite';
  public latitude = 0;
  public longitude = 0;
  
  constructor(
    private clinicService: ClinicService,
    private locationService: LocationService) { }

  public ngOnInit(): void {
    this.initializeLocation();
    this.clinicService.getClosestClinicsWithClinician(this.longitude, this.latitude)
        .subscribe(result => {
          if (result.Data !== null) {
            this.clinics = result.Data;
          }
        });
  }

  private initializeLocation(): void {
    if (this.locationService.userLatitude === null
          || this.locationService.userLongitude === null) {
      this.locationService.initializeLocation();
    }

    this.longitude = this.locationService.userLongitude;
    this.latitude = this.locationService.userLatitude;
  }

  public getIconUrl(): string {
    return `assets/images/4.png`;
  }
}
