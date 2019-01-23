import { Component, OnInit } from '@angular/core';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicModel } from 'src/app/core/models';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html',
  styleUrls: ['./clinic.component.styl']
})
export class ClinicComponent implements OnInit {
  public data: any[] = [];

  constructor(private clinicService: ClinicService) { }

  public ngOnInit(): void {
    this.clinicService.getClosestClinicsWithClinician(30, 50)
      .subscribe(result => {
        if (result.Result !== null) {
          this.data = result.Result;
        }
      });
  }

}
