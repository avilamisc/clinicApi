import { FormGroup, ValidatorFn } from '@angular/forms';

export function GreaterThenNow(controlName: string): ValidatorFn {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];

        if (control === null || control.value > Date.now()) {
            control.setErrors({ greaterThenNow: true });
        } else {
            control.setErrors(null);
        }

        return null;
    };
}
