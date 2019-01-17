import { DocumentModel } from './documentModel.model';

export class UpdateBookingModel {
    id: number;
    reciept: string;
    name: string;
    documents: DocumentModel[];
    clinicId: number;
    clinicianId: number;
}
