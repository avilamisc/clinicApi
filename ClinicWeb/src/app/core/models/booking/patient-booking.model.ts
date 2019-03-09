import { BookingModel } from './booking.model';

export class PatientBookingModel extends BookingModel {
    ClinicId: number;
    ClinicName: string;
    ClinicianId: number;
    ClinicianName: string;
    ClinicianRate: number;
    BookingRate: number;
    newFiles: any[] = [];
}
