import { environment } from 'src/environments/environment';

export const ApiRoutes = {
    authenticate: `${environment.serverUrl}/account/login`,
    registerPatient: `${environment.serverUrl}/account/register/patient`,
    registerClinician: `${environment.serverUrl}/account/register/clinician`,
    registerClinic: `${environment.serverUrl}/account/register/admin`,
    refreshToken: `${environment.serverUrl}/tokens/refresh`,
    revokeToken: `${environment.serverUrl}/tokens/revoke`,

    booking: `${environment.serverUrl}/bookings`,
    patientBookings: `${environment.serverUrl}/bookings/patient`,
    clinicianBookings: `${environment.serverUrl}/bookings/clinician`,

    clinics: `${environment.serverUrl}/clinics`,
    clinicsClinician: `${environment.serverUrl}/clinics/clinicians`,

    clinicians: `${environment.serverUrl}/clinicians`,

    documents: `${environment.serverUrl}/documents`,
    
    notifications: `${environment.serverUrl}/notifications`,

    patientProfile: `${environment.serverUrl}/account/profile/patient`,
    clinicProfile: `${environment.serverUrl}/account/profile/admin`,
    clinicianProfile: `${environment.serverUrl}/account/profile/clinician`
};
