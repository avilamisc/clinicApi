import { Component, OnInit } from '@angular/core';

import { User } from 'src/app/core/models/user/user.model';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.styl']
})
export class AppComponent implements OnInit {
  public UserName: string;

  constructor(private userService: UserService) { }

  public ngOnInit(): void {
    this.UserName = this.userService.getUserFromLocalStorage().UserName;
    console.log('res: ', this.userService.getUserFromLocalStorage(), this.UserName);
  }
}
