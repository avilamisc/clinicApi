import { DocumentModel } from './document.model';

export class UpdateBookingModel {
    id: number;
    reciept: string;
    name: string;
    documents: DocumentModel[];
    clinicId: number;
    clinicianId: number;
    newFiles: any[] = [];
    deletedDocuments: DocumentModel[] = [];
    rate: number;
}
