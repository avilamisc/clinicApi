import { Component, OnInit } from '@angular/core';
import { ClinicService } from 'src/app/core/services/clinic/clinic.service';
import { ClinicDistanceModel } from 'src/app/core/models/clinic-clinician.module/clinic-distance.model';
import { LocationService } from 'src/app/core/services/location.service';
import { LocationPagingModel } from 'src/app/core/models/location-paging.model';
import { UpdateBookingModel, ClinicModel } from 'src/app/core/models';
import { BookingService } from 'src/app/core/services/booking/booking.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { User } from 'src/app/core/models/user/user.model';
import { UserRoles } from 'src/app/utilities/user-roles';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html',
  styleUrls: ['./clinic.component.styl']
})
export class ClinicComponent implements OnInit {
  public clinics: ClinicDistanceModel[] = [];
  public mapType = 'satellite';
  public pagingModel = new LocationPagingModel();
  public user: User;
  public latitude = 0;
  public longitude = 0;
  public isPatient: boolean;
  public isEditWindowOpen = false;
  public selectedClinic: ClinicModel;
  public bookingToUpdate: UpdateBookingModel;
  private displayedCountOfClinics = 10;

  constructor(
    private userService: UserService,
    private clinicService: ClinicService,
    private bookingService: BookingService,
    private locationService: LocationService) { }

  public ngOnInit(): void {
    this.initializeLocation();
  }

  public mapClick(res: any): void {
    this.pagingModel.Latitude = res.coords.lat;
    this.pagingModel.Longitude = res.coords.lng;
    this.updateClinics();
  }

  public getIconUrl(): string {
    return `assets/images/4.png`;
  }

  public closeEditWindow(): void {
    this.bookingToUpdate = null;
    this.isEditWindowOpen = false;
  }

  public crateNewBooking(clinic: ClinicDistanceModel): void {
    this.selectedClinic = {
      Id: clinic.Id,
      Name: clinic.ClinicName
    } as ClinicModel;
    this.isEditWindowOpen = true;
    this.bookingToUpdate = new UpdateBookingModel();
  }

  public aproveCreationBooking(newBooking): void {
    this.bookingService.createBookings(newBooking)
      .subscribe(result => {
        if (result.Data !== null) {
          // redirect or add toast notification
          console.log('added: ', result);
        }
      });

    this.closeEditWindow();
  }

  private initializeLocation(): void {
    this.user = this.userService.getUserFromLocalStorage();
    this.isPatient = this.user.UserRole === UserRoles.Patient;
    this.pagingModel.Count = this.displayedCountOfClinics;

    if (this.locationService.userLatitude === null
          || this.locationService.userLongitude === null) {
      this.locationService.initializeLocation();
      this.pagingModel.Longitude = this.locationService.userLongitude;
      this.pagingModel.Latitude = this.locationService.userLatitude;
      this.updateClinics();
      this.latitude = this.pagingModel.Latitude;
      this.longitude = this.pagingModel.Longitude;
    } else {
      this.pagingModel.Longitude = this.locationService.userLongitude;
      this.pagingModel.Latitude = this.locationService.userLatitude;
      this.updateClinics();
      this.latitude = this.pagingModel.Latitude;
      this.longitude = this.pagingModel.Longitude;
    }
  }

  private updateClinics(res = null): void {
    this.clinicService.getClosestClinicsWithClinician(this.pagingModel)
        .subscribe(result => {
          if (result.Data !== null) {
            this.clinics = result.Data;
          }
        });
  }
}
