import { Column } from 'src/app/core/models/table/column.model';

export const PatientBookingTableConfiguration = new Map([
    ['ClinicName', new Column('Reciept', 'Reciept')],
    ['Name', new Column('Name', 'Name')],
    ['Documents', new Column('Documents', 'Documents')],
    ['ClinicName', new Column('Clinic Name', 'ClinicName')],
    ['ClinicianName', new Column('Clinician', 'ClinicianName')],
    ['ClinicianRate', new Column('Clinician Rate', 'ClinicianRate')],
]);
