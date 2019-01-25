import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { User } from 'src/app/core/models/user/user.model';
import { UserService } from 'src/app/core/services/user/user.service';
import { DocumentModel } from 'src/app/core/models';

@Component({
  selector: 'app-edit-document',
  templateUrl: './edit-document.component.html',
  styleUrls: ['./edit-document.component.styl']
})
export class EditDocumentComponent implements OnInit, OnChanges {
  public user: User;
  public displayedDocuments: DocumentModel[];

  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();

  constructor(private userService: UserService) { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.model) {
      this.initializeDisplayedDocuments();
    }
  }

  public ngOnInit(): void {
    this.initializeUser();
  }

  public removeDocument(doc: DocumentModel): void {
    const index = this.displayedDocuments.findIndex(d => d.Id === doc.Id);
    this.displayedDocuments.splice(index, 1);
    this.model.deletedDocuments.push(doc);
  }

  public removeNewDocument(index: number): void {
    this.model.newFiles.splice(index, 1);
  }

  public uploadNewDocument(event: any): void {
    if (event.target.files.length > 0) {
      this.model.newFiles.push(event.target.files[0]);
    }
  }

  public canEditDocument(doc: DocumentModel): boolean {
    return this.user.Id === doc.UserId;
  }

  private initializeUser(): void {
    this.user = this.userService.getUserFromLocalStorage();
  }

  private initializeDisplayedDocuments(): void {
    this.displayedDocuments = this.model.documents
      ? [...this.model.documents]
      : [];
  }
}
