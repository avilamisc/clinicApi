import { DocumentModel } from './document.model';

export class BookingModel {
    Id: number;
    Name: string;
    Documents: DocumentModel[];
    HeartRate: number;
    Weight: number;
    Height: number;
    PatientDescription: string;
}

export class BookingModelResult extends BookingModel {
    BookingRate: number;
    ClinicId: number;
    ClinicName: string;
    PatientId?: number;
    PatientName?: string;
    PatientAge?: string;
    ClinicianId?: number;
    ClinicianName?: string;
    ClinicianRate?: number;
}
