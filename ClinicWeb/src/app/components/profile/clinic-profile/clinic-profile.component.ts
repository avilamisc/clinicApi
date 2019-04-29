import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { UpdateClinicModel } from 'src/app/core/models';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';

@Component({
  selector: 'app-clinic-profile',
  templateUrl: './clinic-profile.component.html',
  styleUrls: ['./clinic-profile.component.styl']
})
export class ClinicProfileComponent implements OnInit {
  public formErrors = {};
  public submitTouched = false;
  public updateForm: FormGroup;

  @Input('model') model: UpdateClinicModel;

  @Output('update') onUpdate = new EventEmitter<boolean>();

  constructor(
    private toastNotification: ToastNotificationService,
    private formValidationService: FormValidationService
  ) { }

  public ngOnInit(): void {
    this.createForm();
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

  private setValuesFromFormToModel(): void {
    const values = this.updateForm.getRawValue();
    this.model.ClinicName = values.clinicName;
  }

  private createForm(): void {
    this.updateForm = new FormGroup({
      clinicName: new FormControl(this.model.ClinicName, [Validators.required])
    });
    this.formValidationService.setFormData(this.updateForm, ValidationMessages.UpdateProfile);
    this.updateForm.valueChanges.subscribe(() => this.validateForm());
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
  }
}
