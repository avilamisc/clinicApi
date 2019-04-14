import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: 'star.component.html',
  styleUrls: ['star.component.styl']
})
export class StarRatingComponent implements OnInit {
  @Input('rating') public rating: number;
  @Input('subRating') public subRating: number;
  @Input('itemId') public itemId: number;
  @Input('disabled') public disabled: boolean = false;
  @Output('ratingClick') public ratingClick: EventEmitter<any> = new EventEmitter<any>();

  public inputName: string;

  public ngOnInit(): void {
    this.inputName = this.itemId + '_rating';
  }

  public onClick(event: any, rating: number): void {
    event.stopPropagation();
    if (!this.disabled) {
      this.rating = rating;
      this.ratingClick.emit({
        itemId: this.itemId,
        rating: rating
      });
    }
  }

  public hasSubRaiting(startCount: number): boolean {
    return this.subRating >= startCount && this.subRating >= this.rating;
  }

  public hasSubRaitingMainRaiting(startCount: number): boolean {
    return this.subRating >= startCount && this.subRating < this.rating;
  }
}
