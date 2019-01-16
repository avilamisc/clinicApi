import { BookingModel } from './bookingModel.model';

export class PatientBookingModel extends BookingModel {
    ClinicId: number;
    ClinicName: string;
    ClinicianId: number;
    ClinicianName: string;
    ClinicianSurname: string;
    ClinicianRate: number;
}
