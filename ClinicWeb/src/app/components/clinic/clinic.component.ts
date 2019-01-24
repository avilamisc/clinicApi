import { Component, OnInit } from '@angular/core';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html',
  styleUrls: ['./clinic.component.styl']
})
export class ClinicComponent implements OnInit {
  public data: any[] = [];

  constructor(private clinicService: ClinicService) { }

  public ngOnInit(): void {
    window.navigator.geolocation.getCurrentPosition(location => {
      console.log(location);
      const long = Math.floor(location.coords.longitude);
      const lat = Math.floor(location.coords.latitude);
      this.clinicService.getClosestClinicsWithClinician(long, lat)
        .subscribe(result => {
          if (result.Result !== null) {
            this.data = result.Result;
          }
        });
    });
  }

}
