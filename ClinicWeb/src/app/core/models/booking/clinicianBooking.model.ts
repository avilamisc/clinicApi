import { BookingModel } from './bookingModel.model';

export class ClinicianBookingModel extends BookingModel {
    clinicId: number;
    clinicName: string;
    patientId: number;
    patientName: string;
    patientSurname: string;
    patientRegion: string;
}
