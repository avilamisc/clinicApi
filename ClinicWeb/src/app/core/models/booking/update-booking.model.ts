import { DocumentModel } from './document.model';
import { Stage } from '..';

export class UpdateBookingModel {
    id: number;
    name: string;
    documents: DocumentModel[];
    clinicId: number;
    clinicianId: number;
    newFiles: any[] = [];
    deletedDocuments: DocumentModel[] = [];
    rate: number;
    HeartRate: number;
    Weight: number;
    Height: number;
    PatientDescription: string;
    stage: Stage;
}
