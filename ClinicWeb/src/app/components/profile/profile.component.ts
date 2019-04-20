import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';

import { TokenService } from 'src/app/core/services/auth/token.service';
import { UserRoles } from 'src/app/utilities/user-roles';
import { ProfileModel, UpdateProfileModel, UpdatePatientModel, UpdateClinicModel, UpdateClinicianModel } from 'src/app/core/models';
import { ProfileService } from 'src/app/core/services/profile/profile.service';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { FormValidationService } from 'src/app/core/services/validation.service';
import { ValidationMessages } from 'src/app/utilities/validation-messages';
import { InputFileAccepts } from 'src/app/utilities/common-constants';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.styl']
})
export class ProfileComponent implements OnInit {
  public userModel: ProfileModel;
  public updateUserModel = new UpdateProfileModel();
  public userRole: string;
  public isClinic = false;
  public isClinician = false;
  public isPatient = false;
  public isLoaded = false;
  public formErrors = {};
  public submitTouched = false;
  public updateForm: FormGroup;
  public inputFileAcceptImageTypes: string;

  constructor(
    private tokenService: TokenService,
    private profileService: ProfileService,
    private toastNotification: ToastNotificationService,
    private formValidationService: FormValidationService) { }

  public ngOnInit(): void {
    this.setConstatns();
    this.setUserRole();
    this.uploadUser();
  }

  public onSubmit(): void {
    this.submitTouched = true;
    if (this.updateForm.valid) {
      this.setValuesFromFormToModel();
      this.updateProfile();
    } else {
      this.formValidationService.markFormGroupTouched();
      this.validateForm();
      this.toastNotification.validationWarning();
    }
  }

  private uploadUser(): void {
    switch (this.userRole)
    {
        case UserRoles.Patient:
            this.uploadPatient(); break;
        case UserRoles.Clinic:
            this.uploadClinic(); break;
        case UserRoles.Clinician:
            this.uploadClinician(); break;
    }
  }

  private uploadPatient(): void {
    this.profileService.getPatientProfle()
      .subscribe(result => {
        if (result.Data) {
          this.isPatient = true;

          this.userModel = result.Data;
          this.setUpdatedModel(result.Data);

          (this.updateUserModel as UpdatePatientModel).BornDate = result.Data.BornDate;

          this.createForm();
          this.isLoaded = true;
        } else {
          this.toastNotification.showErrorMessage(result.ErrorMessage, result.StatusCode);
        }
      });
  }

  private uploadClinic(): void {
    this.profileService.getClinicProfle()
      .subscribe(result => {
        if (result.Data) {
          this.isClinic = true;

          this.userModel = result.Data;
          this.setUpdatedModel(result.Data);

          (this.updateUserModel as UpdateClinicModel).ClinicName = result.Data.ClinicName;

          this.createForm();
          this.isLoaded = true;
        } else {
          this.toastNotification.showErrorMessage(result.ErrorMessage, result.StatusCode);
        }
      });
  }

  private uploadClinician(): void {
    this.profileService.getClinicianProfle()
      .subscribe(result => {
        if (result.Data) {
          this.isClinician = true;

          this.userModel = result.Data;
          this.setUpdatedModel(result.Data);

          this.createForm();
          this.isLoaded = true;
        } else {
          this.toastNotification.showErrorMessage(result.ErrorMessage, result.StatusCode);
        }
      });
  }

  private setUserRole(): void {
    this.userRole = this.tokenService.getUserRole();
  }

  private setUpdatedModel(model: ProfileModel): void {
    this.updateUserModel.Mail = model.Mail;
    this.updateUserModel.Name = model.Name;
    this.updateUserModel.UserImageUrl = model.UserImageUrl;
  }

  private validateForm(): void {
    this.formErrors = this.formValidationService.validateForm();
  }

  private createForm(): void {
    this.updateForm = new FormGroup({
      userName: new FormControl(this.userModel.Name, [Validators.required]),
      userMail: new FormControl(this.userModel.Mail,
        [
          Validators.required,
          Validators.email
        ])
    });
    this.formValidationService.setFormData(this.updateForm, ValidationMessages.UpdateProfile);
    this.updateForm.valueChanges.subscribe(() => this.validateForm());
  }

  private setValuesFromFormToModel(): void {
    const values = this.updateForm.getRawValue();
    this.updateUserModel.Name = values.userName;
    this.updateUserModel.Mail = values.userMail;
  }

  private updateProfile(): void {
    switch (this.userRole)
    {
        case UserRoles.Patient:
            this.updatePatient(); break;
        case UserRoles.Clinic:
            this.updateClinic(); break;
        case UserRoles.Clinician:
            this.updateClinician(); break;
    }
  }

  private updatePatient(): void {
    this.profileService.updatePatientProfle(this.updateUserModel as UpdatePatientModel)
      .subscribe(result => {
        if (result.Data) {
          this.userModel.Name = result.Data.Name;
          this.userModel.Mail = result.Data.Mail;
          this.userModel.RegistrationDate = result.Data.RegistrationDate;
          this.userModel.UserImageUrl = result.Data.UserImageUrl;

          this.toastNotification.successMessage('');
        } else {
          this.toastNotification.showErrorMessage(result.ErrorMessage, result.StatusCode);
        }
      })
  }

  private updateClinic(): void {
    this.profileService.updateClinicProfle(this.updateUserModel as UpdateClinicModel)
      .subscribe(result => {
        if (result.Data) {
          this.userModel.Name = result.Data.Name;
          this.userModel.Mail = result.Data.Mail;
          this.userModel.RegistrationDate = result.Data.RegistrationDate;
          this.userModel.UserImageUrl = result.Data.UserImageUrl;

          this.toastNotification.successMessage('');
        } else {
          this.toastNotification.showErrorMessage(result.ErrorMessage, result.StatusCode);
        }
      })
  }

  private updateClinician(): void {
    this.profileService.updateClinicianProfle(this.updateUserModel as UpdateClinicianModel)
      .subscribe(result => {
        if (result.Data) {
          this.userModel.Name = result.Data.Name;
          this.userModel.Mail = result.Data.Mail;
          this.userModel.RegistrationDate = result.Data.RegistrationDate;
          this.userModel.UserImageUrl = result.Data.UserImageUrl;

          this.toastNotification.successMessage('');
        } else {
          this.toastNotification.showErrorMessage(result.ErrorMessage, result.StatusCode);
        }
      })
  }

  private setConstatns(): void {
    this.inputFileAcceptImageTypes = InputFileAccepts.imageTypes;
  }

  public uploadNewCompanyLogo(event: any): void {
    if (event.target.files.length > 0) {
        const file = event.target.files[0];

        const reader = new FileReader();
        reader.onload = () => this.userModel.UserImageUrl = reader.result;
        reader.readAsDataURL(file);

        this.updateUserModel.UserImage = file;
    }
  }
}
