import { BookingModel } from './booking.model';

export class ClinicianBookingModel extends BookingModel {
    ClinicId: number;
    ClinicName: string;
    PatientId: number;
    PatientName: string;
    PatientLocation: string;
}
