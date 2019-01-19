import { Column } from 'src/app/core/models/table/column.model';

export const PatientBookingTableConfiguration = {
    Id: new Column('Id', 'Id'),
    Reciept: new Column('Reciept', 'Reciept'),
    Name: new Column('Name', 'Name'),
    Documents: new Column('Documents', 'Documents'),
    ClinicName: new Column('ClinicName', 'ClinicName'),
    ClinicianSurname: new Column('ClinicianSurname', 'ClinicianSurname'),
    ClinicianRate: new Column('ClinicianRate', 'ClinicianRate'),
};

export const ClinicianBookingTableConfiguration = {
    Id: new Column('Id', 'Id'),
    Reciept: new Column('Reciept', 'Reciept'),
    Name: new Column('Name', 'Name'),
    Documents: new Column('Documents', 'Documents'),
    ClinicName: new Column('ClinicName', 'ClinicName'),
    PatientName: new Column('PatientName', 'PatientName'),
    PatientSurname: new Column('PatientSurname', 'PatientSurname'),
    PatientLocation: new Column('PatientLocation', 'PatientLocation')
};
