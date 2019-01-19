import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';

import { BookingService } from 'src/app/core/services/booking/booking.service';
import { BookingModel, PatientBookingModel } from 'src/app/core/models';
import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { UserRoles } from 'src/app/utilities/user-roles';
import { User } from 'src/app/core/models/user/user.model';
import { UserService } from 'src/app/core/services/user/user.service';
import { DocumentService } from 'src/app/core/services/document/document.service';
import { PatientBookingTableConfiguration,
         ClinicianBookingTableConfiguration } from './booking-table';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.styl']
})
export class BookingComponent implements OnInit {
  public bookings: BookingModel[] = [];
  public columns = [];
  public bookingToUpdate: UpdateBookingModel;
  public user: User;
  public isEditOpen = false;
  private editedBookingIndex: number;
  private isAddingNewBooking = false;
  private isPatient: boolean;

  @ViewChild('documetsColumn') documentsColumn: TemplateRef<any>;

  constructor(
    private userService: UserService,
    private tokenService: TokenService,
    private documentService: DocumentService,
    private bookingService: BookingService) { }

  public ngOnInit(): void {
    this.initializeBookings();
    this.initializeTableColumns();
  }

  public updateBoking(booking: BookingModel, index: number): void {
    this.editedBookingIndex = index;
    this.isAddingNewBooking = false;
    this.isEditOpen = true;
    this.bookingToUpdate = new UpdateBookingModel();
    this.bookingToUpdate.id = booking.Id;
    this.bookingToUpdate.reciept = booking.Reciept;
    this.bookingToUpdate.name = booking.Name;
    this.bookingToUpdate.documents = booking.Documents;
    this.bookingToUpdate.clinicId = (booking as PatientBookingModel).ClinicId;
    this.bookingToUpdate.clinicianId = (booking as PatientBookingModel).ClinicianId || this.user.Id;
  }

  public updateEditingModel(newBooking: PatientBookingModel): void {
    if (this.isAddingNewBooking) {
      this.bookingService.createBookings(newBooking)
        .subscribe(result => {
          if (result.Result !== null) {
            this.bookings.push(result.Result);
            this.bookingToUpdate = null;
          }
        });
    } else {
      this.bookings[this.editedBookingIndex] = newBooking;
    }
    this.closeEditWindow();
  }

  public crateNewBooking(): void {
    this.isAddingNewBooking = true;
    this.isEditOpen = true;
    this.bookingToUpdate = new UpdateBookingModel();
  }

  public initializeBookings(): void {
    this.user = this.userService.getUserFromLocalStorage();
    this.isPatient = this.tokenService.getUserRole() === UserRoles.Patient;
    this.isPatient
      ? this.bookingService.getPatientBookings()
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result;
          }
        })
      : this.bookingService.getClinicianBookings()
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result;
          }
        });
  }

  public initializeTableColumns(): void {
    const config = this.isPatient
    ? PatientBookingTableConfiguration
    : ClinicianBookingTableConfiguration;

  config.Documents.RowContent = this.documentsColumn;

  for (const key in config) {
    if (config.hasOwnProperty(key)) {
      this.columns.push(config[key]);
    }
  }
  }

  public loadDocument(event: any, id: number): void {
    this.documentService.downloadDocument(id).subscribe();
    event.stopPropagation();
  }

  public closeEditWindow(): void {
    this.isEditOpen = false;
  }
}
