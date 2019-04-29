import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UpdateClinicianModel } from 'src/app/core/models';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-clinician-profile',
  templateUrl: './clinician-profile.component.html',
  styleUrls: ['./clinician-profile.component.styl']
})
export class ClinicianProfileComponent implements OnInit {
  public formErrors = {};
  public submitTouched = false;
  public updateForm: FormGroup;

  @Input('model') model: UpdateClinicianModel;

  @Output('update') onUpdate = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  public onSubmit(): void {
    console.log('submit patient');
    this.submitTouched = true;
    this.onUpdate.emit(true);
  }
}
