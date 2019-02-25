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
import { Column } from 'src/app/core/models/table/column.model';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.styl']
})
export class BookingComponent implements OnInit {
  public bookings: BookingModel[] = [];
  public totalDataAmount = 0;
  public columns: Column[] = [];
  public bookingToUpdate: UpdateBookingModel;
  public user: User;
  public isEditWindowOpen = false;
  private editedBookingIndex: number;
  private isAddingNewBooking = false;
  private isPatient: boolean;
  private tableRowAmount = 5;

  @ViewChild('documetsColumn') documentsColumn: TemplateRef<any>;

  constructor(
    private userService: UserService,
    private documentService: DocumentService,
    private bookingService: BookingService) { }

  public ngOnInit(): void {
    this.initializeBookings();
    this.initializeTableColumns();
  }

  public openEditWindow(booking: BookingModel, index: number): void {
    this.editedBookingIndex = index;
    this.bookingToUpdate = new UpdateBookingModel();
    this.bookingToUpdate.id = booking.Id;
    this.bookingToUpdate.reciept = booking.Reciept;
    this.bookingToUpdate.name = booking.Name;
    this.bookingToUpdate.documents = booking.Documents;
    this.bookingToUpdate.clinicId = (booking as PatientBookingModel).ClinicId;
    this.bookingToUpdate.clinicianId = (booking as PatientBookingModel).ClinicianId || this.user.Id;
    this.isAddingNewBooking = false;
    this.isEditWindowOpen = true;
  }

  public editBooking(newBooking: PatientBookingModel): void {
    if (this.isAddingNewBooking) {
      this.bookingService.createBookings(newBooking)
        .subscribe(result => {
          if (result.Data !== null) {
            this.bookings.splice(0, 0, result.Data);
          }
        });
    } else {
      this.bookings[this.editedBookingIndex] = newBooking;
    }
    this.closeEditWindow();
  }

  public crateNewBooking(): void {
    this.isAddingNewBooking = true;
    this.isEditWindowOpen = true;
    this.bookingToUpdate = new UpdateBookingModel();
  }

  public initializeBookings(): void {
    this.user = this.userService.getUserFromLocalStorage();
    this.isPatient = this.user.UserRole === UserRoles.Patient;
    this.uploadBookings({
        pageNumber: 0,
        pageCount: this.tableRowAmount
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

  public downloadDocument(event: any, doc: DocumentModel): void {
    this.documentService.downloadDocument(doc.Id, doc.Name).subscribe();
    event.stopPropagation();
  }

  public closeEditWindow(): void {
    this.bookingToUpdate = null;
    this.isEditWindowOpen = false;
  }

  public uploadBookings(pagination: Pagination): void {
    this.isPatient
      ? this.bookingService.getPatientBookings(pagination)
        .subscribe(res => {
          if (res.Data !== null) {
            this.bookings = res.Data.DataCollection;
            this.totalDataAmount = res.Data.TotalCount;
          }
        })
      : this.bookingService.getClinicianBookings(pagination)
        .subscribe(res => {
          if (res.Data !== null) {
            this.bookings = res.Data.DataCollection;
            this.totalDataAmount = res.Data.TotalCount;
          }
        });
  }
}
