import { environment } from 'src/environments/environment';

export const ApiRoutes = {
    authenticate: `${environment.serverUrl}/account/login`,
    registerPatient: `${environment.serverUrl}/account/register/patient`,
    registerClinician: `${environment.serverUrl}/account/register/clinician`,
    refreshToken: `${environment.serverUrl}/tokens/refresh`,

    patientBookings: `${environment.serverUrl}/bookings/patient`,
    clinicianBookings: `${environment.serverUrl}/bookings/clinician`,
    booking: `${environment.serverUrl}/bookings`,
    clinics: `${environment.serverUrl}/clinics`,
    clinicsClinician: `${environment.serverUrl}/clinics/clinicians`,
    clinicians: `${environment.serverUrl}/clinicians`,
    documents: `${environment.serverUrl}/documents`,
    notifications: `${environment.serverUrl}/notifications`
};
