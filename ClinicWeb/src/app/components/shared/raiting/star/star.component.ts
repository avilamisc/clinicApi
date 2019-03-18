import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: 'star.component.html',
  styleUrls: ['star.component.styl']
})
export class StarRatingComponent implements OnInit {
  @Input() public rating: number;
  @Input() public subRating: number;
  @Input() public itemId: number;
  @Output() public ratingClick: EventEmitter<any> = new EventEmitter<any>();

  public inputName: string;

  ngOnInit() {
    this.inputName = this.itemId + '_rating';
  }

  public onClick(event: any, rating: number): void {
    this.rating = rating;
    this.ratingClick.emit({
      itemId: this.itemId,
      rating: rating
    });
    event.stopPropagation();
  }

  public hasSubRaiting(startCount: number): boolean {
    return this.subRating >= startCount && this.subRating >= this.rating;
  }

  public hasSubRaitingMainRaiting(startCount: number): boolean {
    return this.subRating >= startCount && this.subRating < this.rating;
  }
}
