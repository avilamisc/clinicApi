import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { UpdatePatientModel } from 'src/app/core/models';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';
import { ToastNotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-patient-profile',
  templateUrl: './patient-profile.component.html',
  styleUrls: ['./patient-profile.component.styl']
})
export class PatientProfileComponent implements OnInit {
  public formErrors = {};
  public submitTouched = false;
  public updateForm: FormGroup;
  public minDateFrom = new Date(1919, 1, 1);
  public maxDateFrom = new Date(Date.now());

  @Input('model') public model: UpdatePatientModel;

  @Output('update') public onUpdate = new EventEmitter<boolean>();

  constructor(
    private toastNotification: ToastNotificationService,
    private formValidationService: FormValidationService
  ) { }

  public ngOnInit(): void {
    this.createForm();
  }

  public defaultBornDate(): Date {
    return this.model ? new Date(this.model.BornDate) : null;
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.updateForm.valid) {
      this.setValuesFromFormToModel();
      this.onUpdate.emit(true);
    } else {
      this.formValidationService.markFormGroupTouched();
      this.validateForm();
      this.toastNotification.validationWarning();
      this.onUpdate.emit(false);
    }
  }

  public onSelectBornDate(bornDate: Date): void {
    if (bornDate) {
      this.updateForm.get('bornDate').patchValue(bornDate, { emitEvent: false });
    }
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
  }

  private createForm(): void {
    this.updateForm = new FormGroup({
      bornDate: new FormControl(this.model.BornDate, [Validators.required])
    });
    this.formValidationService.setFormData(this.updateForm, ValidationMessages.UpdateProfile);
    this.updateForm.valueChanges.subscribe(() => this.validateForm());
  }

  private setValuesFromFormToModel(): void {
    const values = this.updateForm.getRawValue();
    this.model.BornDate = values.bornDate;
  }
}
