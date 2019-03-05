import { Component, OnInit } from '@angular/core';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicClinicianBaseModel } from 'src/app/core/models/clinic-clinician.module/clinic-clinician-base.model';
import { ClinicDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinic-distance.model';
import { ClinicianDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinician-distance.model';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html',
  styleUrls: ['./clinic.component.styl']
})
export class ClinicComponent implements OnInit {
  public data: ClinicClinicianBaseModel[] = [];

  private mapType = 'satellite';
  private latitude = -28.68352;
  private longitude = -147.20785;
  
  constructor(private clinicService: ClinicService) { }

  public ngOnInit(): void {
    window.navigator.geolocation.getCurrentPosition(location => {
      const long = location.coords.longitude;
      const lat = location.coords.latitude;
      this.clinicService.getClosestClinicsWithClinician(long, lat)
        .subscribe(result => {
          if (result.Data !== null) {
            this.data = result.Data;
          }
        });
    });
  }

  public instanceOfClinician(model: ClinicianDistanceModel | ClinicDistanceModel): boolean {
    return 'Id' in model && 'Name' in model && 'Rate' in model;
  }
}
