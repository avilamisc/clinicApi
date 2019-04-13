import { FormGroup, ValidatorFn } from '@angular/forms';

export function GreaterThen(controlName: string, matchingControlName: string): ValidatorFn {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const matchingControl = formGroup.controls[matchingControlName];

        if (matchingControl.errors && !matchingControl.errors.greaterThen) {
            return;
        }

        if (matchingControl.value
                && control.value
                && control.value > matchingControl.value) {
            matchingControl.setErrors({ greaterThen: true });
        } else {
            matchingControl.setErrors(null);
        }

        return null;
    };
}
