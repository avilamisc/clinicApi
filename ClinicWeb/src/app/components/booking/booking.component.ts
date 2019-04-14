import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { SelectItem } from 'primeng/components/common/selectitem';

import { BookingService } from 'src/app/core/services/booking/booking.service';
import { BookingModel, PatientBookingModel, DocumentModel, Stage } from 'src/app/core/models';
import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { UserRoles } from 'src/app/utilities/user-roles';
import { User } from 'src/app/core/models/user/user.model';
import { UserService } from 'src/app/core/services/user/user.service';
import { DocumentService } from 'src/app/core/services/document/document.service';
import { PatientBookingTableConfiguration,
         ClinicianBookingTableConfiguration } from './table-config';
import { Pagination } from 'src/app/core/models/table/pagination.model';
import { Column } from 'src/app/core/models/table/column.model';
import { ToastNotificationService } from 'src/app/core/services/notification.service';
import { NotificationMessages } from 'src/app/utilities/notification-message';

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
  public tableRowAmount = 5;
  public isPatient: boolean;
  public isClinician: boolean;
  public isClinic: boolean;
  public stage = Stage;
  public stageForFlter: Stage = null;
  public stages: SelectItem[];
  public completedNotificationMessage = NotificationMessages.Booking.UpdateToCompleteStage;
  public confirmedNotificationMessage = NotificationMessages.Booking.UpdateToConfirmedStage;
  private editedBookingIndex: number;
  private isAddingNewBooking = false;
  private currentPage = 0;

  @ViewChild('updateDateColumn') updateDateColumn: TemplateRef<any>;
  @ViewChild('documetsColumn') documentsColumn: TemplateRef<any>;
  @ViewChild('rateColumn') rateColumn: TemplateRef<any>;
  @ViewChild('patientColumn') patientColumn: TemplateRef<any>;
  @ViewChild('actionsColumn') actionsColumn: TemplateRef<any>;
  @ViewChild('actionsHeaderColumn') actionsHeaderColumn: TemplateRef<any>;

  constructor(
    private userService: UserService,
    private bookingService: BookingService,
    private documentService: DocumentService,
    private notificationService: ToastNotificationService) {
      this.stages = [
        { label: 'All stages', value: null },
        { label: 'Send', value: Stage.Send },
        { label: 'Rejected', value: Stage.Rejected },
        { label: 'InProgress', value: Stage.InProgress },
        { label: 'Confirmed', value: Stage.Confirmed },
        { label: 'Completed', value: Stage.Completed },
        { label: 'Canceled', value: Stage.Canceled }
      ];
    }

  public ngOnInit(): void {
    this.initializeBookings();
    this.initializeTableColumns();
  }

  public onFilterStageChanged(): void {
    this.uploadBookings({
      pageNumber: 0,
      pageCount: this.tableRowAmount
    });
  }

  public canReject(booking: BookingModel): boolean {
    return this.isClinician && booking.Stage === Stage.Send;
  }

  public canConfirm(booking: BookingModel): boolean {
    return this.isClinician && booking.Stage === Stage.Send;
  }

  public canComplete(booking: BookingModel): boolean {
    return this.isClinician && booking.Stage === Stage.InProgress;
  }

  public canCancel(booking: BookingModel): boolean {
    return (this.isPatient || this.isClinic) &&
           (booking.Stage === Stage.InProgress || booking.Stage === Stage.Confirmed);
  }

  public canRateBooking(booking: BookingModel): boolean {
    return this.isPatient && booking.Stage === Stage.Completed;
  }

  public updateStage($event: any, bookingId: number, stage: Stage, notificationMsg: string): void {
    $event.stopPropagation();
    const updateModel = {
      id: bookingId,
      value: stage
    };
    this.bookingService.updateBookingStage(updateModel)
      .subscribe((result) => {
        if (result.Data) {
          const updatedBooking = this.bookings.find(b => b.Id === bookingId);
          if (updatedBooking) {
            updatedBooking.Stage = result.Data;
            this.notificationService.successMessage(notificationMsg);
          }
        } else {
          this.notificationService.showApiErrorMessage(result);
        }
      });
  }

  public openEditWindow(booking: BookingModel, index: number): void {
    this.editedBookingIndex = index;
    this.bookingToUpdate = new UpdateBookingModel();
    this.bookingToUpdate.id = booking.Id;
    this.bookingToUpdate.HeartRate = booking.HeartRate;
    this.bookingToUpdate.Weight = booking.Weight;
    this.bookingToUpdate.Height = booking.Height;
    this.bookingToUpdate.PatientDescription = booking.PatientDescription;
    this.bookingToUpdate.name = booking.Name;
    this.bookingToUpdate.documents = booking.Documents;
    this.bookingToUpdate.stage = booking.Stage;
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
            this.notificationService.createBooking();
          } else {
            this.notificationService.showApiErrorMessage(result);
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
    this.isClinic = this.user.UserRole === UserRoles.Clinic;
    this.isClinician = this.user.UserRole === UserRoles.Clinician;
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

    const updateDateConfig = config.get('UpdateDate');
    updateDateConfig.RowContent = this.updateDateColumn;
    config.set('UpdateDate', updateDateConfig);

    const actionsConfig = config.get('Actions');
    actionsConfig.RowContent = this.actionsColumn;
    actionsConfig.HeaderContent = this.actionsHeaderColumn;
    config.set('Actions', actionsConfig);

    if (this.isPatient) {
      const rateConfig = config.get('ClinicianRate');
      rateConfig.RowContent = this.rateColumn;
      config.set('ClinicianRate', rateConfig);
    }

    if (this.isClinician) {
      const patientConfig = config.get('PatientName');
      patientConfig.RowContent = this.patientColumn;
      config.set('PatientName', patientConfig);
    }

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
    this.currentPage = pagination.pageNumber;
    this.isPatient
      ? this.bookingService.getPatientBookings(pagination, this.stageForFlter)
          .subscribe(res => {
            if (res.Data !== null) {
              this.bookings = res.Data.DataCollection;
              this.totalDataAmount = res.Data.TotalCount;
            }
          })
      : this.bookingService.getClinicianBookings(pagination, this.stageForFlter)
          .subscribe(res => {
            if (res.Data !== null) {
              this.bookings = res.Data.DataCollection;
              this.totalDataAmount = res.Data.TotalCount;
            }
          });
  }

  public updateRate(bookingId: number, newRate: number): void {
    const updateModel = {
      id: bookingId,
      value: newRate
    };
    this.bookingService.updateBookingRate(updateModel)
      .subscribe((result) => {
        if (result.Data) {
          this.uploadBookings({
            pageNumber: this.currentPage,
            pageCount: this.tableRowAmount
          });
        } else {
          this.notificationService.showApiErrorMessage(result);
        }
      });
  }

  public getCountOfStart(raitingValue: number, item): number {
    return item.BookingRate !== null ? Math.floor(raitingValue) : 0;
  }

  public removeBooking($event: any, id: number): void {
    $event.stopPropagation();
    this.bookingService.removeBookings(id)
      .subscribe(result => {
        if (!result.Data) {
          this.notificationService.showApiErrorMessage(result);
        } else if (!result.Data.IsRemoved) {
          this.notificationService.cannotDelete(result.Data.Description);
        } else {
          const bookingIndex = this.bookings.findIndex(b => b.Id === id);
          if (bookingIndex !== -1) {
            this.notificationService.successMessage(
              `Successfuly remove booking - ${this.bookings[bookingIndex].Name}`);
            this.bookings.splice(bookingIndex, 1);
          } else {
            this.notificationService.successMessage('Successfuly remove booking');
          }
        }
      });
  }

  public getStage(booking: BookingModel): string {
    switch (booking.Stage) {
      case Stage.Send: return 'Send';
      case Stage.InProgress: return 'InProgress';
      case Stage.Canceled: return 'Canceled';
      case Stage.Confirmed: return 'Confirmed';
      case Stage.Rejected: return 'Rejected';
      case Stage.Completed: return 'Completed';
      default: return '';
    }
  }
}
