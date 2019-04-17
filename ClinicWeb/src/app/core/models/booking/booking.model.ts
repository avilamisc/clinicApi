import { DocumentModel } from './document.model';

export enum Stage {
    Send = 1, // Patient send it but clinician didn`t approved it yet
    Confirmed, // Clinician confirm it but haven`t sart working
    InProgress, // Clinician set up all patients props
    Rejected, // Clinician reject it at send stage
    Canceled, // Clinician or patient has cancelled it
    Completed // Clinician finish and set up Clinician decission property
}

export class BookingModel {
    Id: number;
    Name: string;
    Documents: DocumentModel[];
    HeartRate: number;
    Weight: number;
    Height: number;
    PatientDescription: string;
    CreationDate: Date;
    UpdateDate: Date;
    Stage: Stage;
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
