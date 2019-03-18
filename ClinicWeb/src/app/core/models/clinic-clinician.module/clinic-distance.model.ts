import { ClinicianDistanceModel } from './clinician-distance.model';

export class ClinicDistanceModel {
    Id: number;
    ClinicName: string;
    City: string;
    Lat: number;
    Long: number;
    Distance: number;
    Clinicians: ClinicianDistanceModel[];
}
