import { Column } from 'src/app/core/models/table/column.model';

export const PatientBookingTableConfiguration = new Map([
    ['Reciept', new Column('Reciept', 'Reciept')],
    ['Name', new Column('Name', 'Name')],
    ['Documents', new Column('Documents', 'Documents')],
    ['ClinicName', new Column('Clinic Name', 'ClinicName')],
    ['ClinicianName', new Column('Clinician', 'ClinicianName')],
    ['ClinicianRate', new Column('Clinician Rate', 'ClinicianRate')],
    ['Actions', new Column('Actions', 'Id')],
]);

export const ClinicianBookingTableConfiguration = new Map([
    ['Reciept', new Column('Reciept', 'Reciept')],
    ['Name', new Column('Name', 'Name')],
    ['Documents', new Column('Documents', 'Documents')],
    ['ClinicName', new Column('Clinic', 'ClinicName')],
    ['PatientName', new Column('Patient', 'PatientName')],
    ['PatientLocation', new Column('Patient Location', 'PatientLocation')],
    ['Actions', new Column('Actions', 'Id')],
]);
