import { Injectable } from '@angular/core';
import { ToastrManager } from 'ng6-toastr-notifications';
import { ApiResponse } from '../models';

@Injectable({
    providedIn: 'root'
})
export class ToastNotificationService {
    constructor(private toastr: ToastrManager) { }

    public showErrorMessage(errorMsg: string, statsCode: number): void {
        this.toastr.errorToastr(errorMsg, `Oops - !${statsCode}`);
    }

    public showApiErrorMessage(response: ApiResponse<any>): void {
        this.toastr.errorToastr(response.ErrorMessage, `Oops - !${response.StatusCode}`);
    }

    public successAuthentication(): void {
        this.toastr.successToastr('You have successfuly authenticate.', 'Welcome!');
    }

    public successRegistration(): void {
        this.toastr.successToastr('You have successfully create account', 'Congratulations!');
    }

    public validationWarning(): void {
        this.toastr.warningToastr('Please correctly set up all necessary fields.');
    }

    public successMessage(msg: string): void {
        this.toastr.successToastr(msg);
    }

    public cannotDelete(msg: string): void {
        this.toastr.errorToastr(msg, 'Cannot delete!');
    }

    public createBooking(): void {
        this.toastr.successToastr('You have successfuly create new booking.', 'Success!');
    }
}
