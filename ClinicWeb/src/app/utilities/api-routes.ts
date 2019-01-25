import { environment } from 'src/environments/environment';

export const ApiRoutes = {
    authenticate: `${environment.serverUrl}/account/login`,
    refreshToken: `${environment.serverUrl}/tokens/refresh`,

    patientBookings: `${environment.serverUrl}/bookings/patient`,
    clinicianBookings: `${environment.serverUrl}/bookings/clinician`,
    booking: `${environment.serverUrl}/bookings`,
    clinics: `${environment.serverUrl}/clinics`,
    clinicsClinician: `${environment.serverUrl}/clinics/clinician`,
    clinicians: `${environment.serverUrl}/clinicians`,
    documents: `${environment.serverUrl}/documents`
};
