import { DocumentModel } from './documentModel.model';

export class BookingModel {
    Id: number;
    Reciept: string;
    Name: string;
    Documents: DocumentModel[];
}
