import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StarRatingComponent } from 'src/app/components/shared/raiting/star/star.component';

@NgModule({
  declarations: [
    StarRatingComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    StarRatingComponent
  ]
})
export class RaitingModule { }
