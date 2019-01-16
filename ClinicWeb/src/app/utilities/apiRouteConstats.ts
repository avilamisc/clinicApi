import { environment } from 'src/environments/environment';

export const ApiRoutes = {
    authenticate: `${environment.serverUrl}/account/login`,
    patientBookings: `${environment.serverUrl}/booking/patient`,
    clinicianBookings: `${environment.serverUrl}/booking/clinician`,
    refreshToken: `${environment.serverUrl}/token/refresh`
};
