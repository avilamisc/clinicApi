import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.styl scss']
})
export class ProfileComponent implements OnInit {
  public userModel: any;

  constructor() { }

  ngOnInit() {
  }

}
