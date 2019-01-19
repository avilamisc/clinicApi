import { environment } from 'src/environments/environment';

export const ApiRoutes = {
    authenticate: `${environment.serverUrl}/account/login`,
    refreshToken: `${environment.serverUrl}/token/refresh`,

    patientBookings: `${environment.serverUrl}/bookings/patient`,
    clinicianBookings: `${environment.serverUrl}/bookings/clinician`,
    booking: `${environment.serverUrl}/bookings`,
    clinics: `${environment.serverUrl}/clinics`,
    clinicians: `${environment.serverUrl}/clinicians`
};
