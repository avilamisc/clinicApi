import { ClinicClinicianBaseModel } from './clinic-clinician-base.model';
import { ClinicClinicianType } from './clinic-clinician.enum';

export class ClinicianDistanceModel extends ClinicClinicianBaseModel {
    Name: string;
    Rate: number;
}
