import { Component, OnInit } from '@angular/core';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinic-distance.model';
import { LocationService } from 'src/app/core/services/location.service';
import { LocationPagingModel } from 'src/app/core/models/location-paging.model';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html',
  styleUrls: ['./clinic.component.styl']
})
export class ClinicComponent implements OnInit {
  public clinics: ClinicDistanceModel[] = [];
  public mapType = 'satellite';
  public pagingModel = new LocationPagingModel();
  public latitude = 0;
  public longitude = 0;
  private displayedCountOfClinics = 10;

  constructor(
    private clinicService: ClinicService,
    private locationService: LocationService) { }

  public ngOnInit(): void {
    this.initializeLocation();
  }

  public mapReady(map): void {
    map.addListener('dragend', (event) => {
      this.pagingModel.Latitude = map.center.lat();
      this.pagingModel.Longitude = map.center.lng();
      this.updateClinics();
      this.latitude = this.pagingModel.Latitude;
      this.longitude = this.pagingModel.Longitude;
    });
  }

  public getIconUrl(): string {
    return `assets/images/4.png`;
  }

  private initializeLocation(): void {
    this.pagingModel.Count = this.displayedCountOfClinics;

    if (this.locationService.userLatitude === null
          || this.locationService.userLongitude === null) {
      this.locationService.initializeLocation();
      this.pagingModel.Longitude = this.locationService.userLongitude;
      this.pagingModel.Latitude = this.locationService.userLatitude;
      this.updateClinics();
      this.latitude = this.pagingModel.Latitude;
      this.longitude = this.pagingModel.Longitude;
    } else {
      this.pagingModel.Longitude = this.locationService.userLongitude;
      this.pagingModel.Latitude = this.locationService.userLatitude;
      this.updateClinics();
      this.latitude = this.pagingModel.Latitude;
      this.longitude = this.pagingModel.Longitude;
    }
  }

  private updateClinics(): void {
    this.clinicService.getClosestClinicsWithClinician(this.pagingModel)
        .subscribe(result => {
          if (result.Data !== null) {
            this.clinics = result.Data;
          }
        });
  }
}
