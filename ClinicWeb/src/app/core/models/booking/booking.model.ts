import { DocumentModel } from './document.model';

export class BookingModel {
    Id: number;
    Reciept: string;
    Name: string;
    Documents: DocumentModel[];
}
