import { environment } from 'src/environments/environment';

export const ApiRoutes = {
    authenticate: `${environment.serverUrl}/account/login`,
    refreshToken: `${environment.serverUrl}/token/refresh`,

    patientBookings: `${environment.serverUrl}/booking/patient`,
    clinicianBookings: `${environment.serverUrl}/booking/clinician`,
    booking: `${environment.serverUrl}/booking`,
};
