import { Component, OnInit, Input } from '@angular/core';
import { ClinicDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinic-distance.model';
import { ClinicianDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinician-distance.model';

@Component({
  selector: 'app-clinic-info',
  templateUrl: './clinic-info.component.html',
  styleUrls: ['./clinic-info.component.styl']
})
export class ClinicInfoComponent implements OnInit {
  @Input() public clinic: ClinicDistanceModel;

  constructor() { }

  ngOnInit() {
  }

  public getClinicianRate(clinician: ClinicianDistanceModel): number {
    return Math.floor(clinician.Rate);
  }

}
