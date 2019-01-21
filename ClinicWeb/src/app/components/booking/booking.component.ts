import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';

import { BookingService } from 'src/app/core/services/booking/booking.service';
import { BookingModel, PatientBookingModel, DocumentModel } from 'src/app/core/models';
import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { UserRoles } from 'src/app/utilities/user-roles';
import { User } from 'src/app/core/models/user/user.model';
import { UserService } from 'src/app/core/services/user/user.service';
import { DocumentService } from 'src/app/core/services/document/document.service';
import { PatientBookingTableConfiguration,
         ClinicianBookingTableConfiguration } from './table-config';
import { Pagination } from 'src/app/core/models/table/pagination.model';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.styl']
})
export class BookingComponent implements OnInit {
  public bookings: BookingModel[] = [];
  public totalAmount = 0;
  public columns = [];
  public bookingToUpdate: UpdateBookingModel;
  public user: User;
  public isEditOpen = false;
  private editedBookingIndex: number;
  private isAddingNewBooking = false;
  private isPatient: boolean;
  private rowAmount = 10;

  @ViewChild('documetsColumn') documentsColumn: TemplateRef<any>;

  constructor(
    private userService: UserService,
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
    this.isPatient = this.user.UserRole === UserRoles.Patient;
    this.uploadBookings({
        pageNumber: 0,
        pageCount: this.rowAmount
      });
  }

  public initializeTableColumns(): void {
    const config = this.isPatient
      ? PatientBookingTableConfiguration
      : ClinicianBookingTableConfiguration;

    const docConfig = config.get('Documents');
    docConfig.RowContent = this.documentsColumn;
    config.set('Documents', docConfig);

    this.columns = Array.from(config.values());
  }

  public loadDocument(event: any, doc: DocumentModel): void {
    this.documentService.downloadDocument(doc.Id, doc.Name).subscribe();
    event.stopPropagation();
  }

  public closeEditWindow(): void {
    this.isEditOpen = false;
  }

  private uploadBookings(pagination: Pagination): void {
    this.isPatient
      ? this.bookingService.getPatientBookings(pagination)
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result.Result;
            this.totalAmount = res.Result.TotalAmount;
          }
        })
      : this.bookingService.getClinicianBookings(pagination)
        .subscribe(res => {
          if (res.Result !== null) {
            this.bookings = res.Result.Result;
            this.totalAmount = res.Result.TotalAmount;
          }
        });
  }
}
