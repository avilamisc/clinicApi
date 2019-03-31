import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ClinicDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinic-distance.model';
import { ClinicianDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinician-distance.model';

@Component({
  selector: 'app-clinic-info',
  templateUrl: './clinic-info.component.html',
  styleUrls: ['./clinic-info.component.styl']
})
export class ClinicInfoComponent {
  @Input('clinic') public clinic: ClinicDistanceModel;
  @Input('patient') public isPatient: boolean;
  @Output('createbooking') public createBoking = new EventEmitter<ClinicDistanceModel>();

  public getClinicianRate(clinician: ClinicianDistanceModel): number {
    return Math.floor(clinician.Rate);
  }

  public crateNewBooking(): void {
    this.createBoking.emit(this.clinic);
  }

}
