import { Component, OnInit, Input } from '@angular/core';

import { UpdateBookingModel } from 'src/app/core/models/booking/update-booking.model';
import { User } from 'src/app/core/models/user/user.model';
import { UserService } from 'src/app/core/services/user/user.service';
import { DocumentModel } from 'src/app/core/models';

@Component({
  selector: 'app-edit-document',
  templateUrl: './edit-document.component.html',
  styleUrls: ['./edit-document.component.styl']
})
export class EditDocumentComponent implements OnInit {
  public user: User;

  @Input('model') public model: UpdateBookingModel = new UpdateBookingModel();

  constructor(private userService: UserService) { }

  public ngOnInit(): void {
    this.initializeUser();
  }

  public removeDocument(id: number): void {
    const docIndex = this.model.documents.findIndex(d => d.Id !== id);
    this.model.documents.splice(docIndex, 1);
  }

  public removeFile(index: number): void {
    this.model.newFiles.splice(index, 1);
  }

  public uploadNewFile(event: any): void {
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
}
