import { DocumentModel } from './document.model';

export class BookingModel {
    Id: number;
    Reciept: string;
    Name: string;
    Documents: DocumentModel[];
}

export class BookingModelResult extends BookingModel {
    BookingRate: number;
    ClinicId: number;
    ClinicName: string;
    PatientId?: number;
    PatientName?: string;
    PatientLocation?: string;
    ClinicianId?: number;
    ClinicianName?: string;
    ClinicianRate?: number;
}
