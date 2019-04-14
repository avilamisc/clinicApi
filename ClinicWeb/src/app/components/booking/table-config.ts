import { Column } from 'src/app/core/models/table/column.model';

export const PatientBookingTableConfiguration = new Map([
    ['Name', new Column('Name', 'Name')],
    ['Documents', new Column('Documents', 'Documents')],
    ['ClinicName', new Column('Clinic Name', 'ClinicName')],
    ['ClinicianRate', new Column('Clinician Rate', 'ClinicianRate')],
    ['UpdateDate', new Column('Last Update', 'UpdateDate')],
    ['Actions', new Column('Actions/Stages', 'Id')],
]);

export const ClinicianBookingTableConfiguration = new Map([
    ['Name', new Column('Name', 'Name')],
    ['Documents', new Column('Documents', 'Documents')],
    ['ClinicName', new Column('Clinic', 'ClinicName')],
    ['PatientName', new Column('Patient', 'PatientName')],
    ['UpdateDate', new Column('Last Update', 'UpdateDate')],
    ['Actions', new Column('Actions/Stages', 'Id')],
]);
