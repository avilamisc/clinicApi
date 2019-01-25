import { ClinicClinicianBaseModel } from './clinic-clinician-base.model';

export class ClinicDistanceModel extends ClinicClinicianBaseModel {
    ClinicName: string;
    City: string;
    Lat: number;
    Long: number;
    Distance: number;
}
