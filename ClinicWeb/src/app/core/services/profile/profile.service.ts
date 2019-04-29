import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResponse,
         PatientProfileModel,
         UpdateClinicianModel,
         UpdatePatientModel,
         ClinicProfileModel,
         ClinicianProfileModel,
         UpdateClinicModel} from '../../models';
import { ApiRoutes } from 'src/app/utilities/api-routes';
import { BaseService } from '../base.service';
import { ImageFieldName } from 'src/app/utilities/common-constants';

@Injectable({
  providedIn: 'root'
})
export class ProfileService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }

  public getPatientProfle(): Observable<ApiResponse<PatientProfileModel>> {
    return this.http.get<ApiResponse<PatientProfileModel>>(ApiRoutes.patientProfile);
  }

  public getClinicProfle(): Observable<ApiResponse<ClinicProfileModel>> {
    return this.http.get<ApiResponse<ClinicProfileModel>>(ApiRoutes.clinicProfile);
  }

  public getClinicianProfle(): Observable<ApiResponse<ClinicianProfileModel>> {
    return this.http.get<ApiResponse<ClinicianProfileModel>>(ApiRoutes.clinicianProfile);
  }

  public updateClinicianProfle(updateModel: UpdateClinicianModel): Observable<ApiResponse<ClinicianProfileModel>> {
    var formData = this.getMultipartData(updateModel);
    if (updateModel.UserImage) {
        formData.append(ImageFieldName, updateModel.UserImage);
    }
    
    return this.http.put<ApiResponse<ClinicianProfileModel>>(ApiRoutes.clinicianProfile, formData);
  }

  public updateClinicProfle(updateModel: UpdateClinicModel): Observable<ApiResponse<ClinicProfileModel>> {
    console.log('update clinic', updateModel);
    var formData = this.getMultipartData(updateModel);
    console.log('update clinic', formData);
    if (updateModel.UserImage) {
        formData.append(ImageFieldName, updateModel.UserImage);
    }
    
    return this.http.put<ApiResponse<ClinicProfileModel>>(ApiRoutes.clinicProfile, formData);
  }

  public updatePatientProfle(updateModel: UpdatePatientModel): Observable<ApiResponse<PatientProfileModel>> {
    var formData = this.getMultipartData(updateModel);
    if (updateModel.UserImage) {
        formData.append(ImageFieldName, updateModel.UserImage);
    }
    
    return this.http.put<ApiResponse<PatientProfileModel>>(ApiRoutes.patientProfile, formData);
  }
}
