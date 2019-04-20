export const ValidationMessages = {
    RegistrationBase: {
        userMail: {
            required: 'Mail is required.',
            email: 'Email is in wrong format.'
        },
        userName: {
            required: 'Name is required.'
        },
        userSurname: {
            required: 'Surname is required.'
        },
        password: {
            required: 'Please, enter your password',
            minlength: 'Password should be at least 8 characters long',
            pattern: 'Password should contain numbers and lettes'
        }
    },
    RegistrationClinic: {
        name: {
            required: 'Name is required.',
        },
        city: {
            required: 'City is required.'
        },
    },
    RegistrationPatient: {
        bornDate: {
            required: 'Born date is required.',
        }
    },
    Booking: {
        reciept: {
            required: 'Receipt is required.'
        },
        name: {
            required: 'Name is required.'
        },
        clinic: {
            required: 'Please select some clinic.'
        },
        clinician: {
            required: 'Please select some clinician.'
        }
    },
    UpdateProfile: {
        userName: {
            required: 'Receipt is required.'
        },
        userMail: {
            required: 'Name is required.'
        }
    }
};
