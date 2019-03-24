import { Component, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';

import { AccountService } from 'src/app/core/services/auth/account.service';
import { TokenService } from 'src/app/core/services/auth/token.service';
import { LocationService } from 'src/app/core/services/location.service';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { NotificationModel, Pagination } from 'src/app/core/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.styl']
})
export class AppComponent implements OnInit {
  public notifications: NotificationModel[] = [];
  public totalNotificationsCount = 0;
  private notificationPagination: Pagination;
  private defaultPaginationAmount = 2;

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private accountService: AccountService,
    private locationService: LocationService,
    private notificationService: NotificationService) { }

  public ngOnInit(): void {
    this.locationService.initializeLocation();
    this.setUpdateNotificationEvent();
  }

  public logOutUser(): void {
    this.accountService.logOut();
    this.router.navigate(['/login']);
  }

  public get isAuthenticated(): boolean {
    return this.tokenService.getAccessToken() !== null;
  }

  public initializeNotifications(): void {
    this.notificationPagination = {
      pageNumber: 0,
      pageCount: this.defaultPaginationAmount
    };
    this.notificationService.getNotifications(this.notificationPagination)
      .subscribe(result => {
        if (result && result.Data) {
          this.notifications = [...result.Data.DataCollection];
          this.totalNotificationsCount = result.Data.TotalCount;
        }
      });
  }

  public uploadMoreNotification(): void {
    this.notificationPagination.pageNumber++;
    this.notificationService.getNotifications(this.notificationPagination)
      .subscribe(result => {
        if (result && result.Data) {
          this.notifications.push(...result.Data.DataCollection);
          this.totalNotificationsCount = result.Data.TotalCount;
        }
      })
  }

  public markNotificationAsRead(id: number): void {
    this.notificationService.setNotificationReadState({Id: id, Value: true})
      .subscribe(result => {
        const notificationIndex = this.notifications.findIndex(n => n.Id == id);
        if (notificationIndex === -1) {
          return;
        }
        const updatedNotification = {
          ...this.notifications[notificationIndex],
          IsRead: result.Data
        }
        this.notifications = [
          ...this.notifications.slice(0, notificationIndex),
          updatedNotification,
          ...this.notifications.slice(notificationIndex + 1)
        ];
        this.totalNotificationsCount--;
      })
  }

  public removeNotification(id: number): void {
    this.notificationService.removeNotification(id)
      .subscribe(result => {
        if (result && result.Data) {
          const notificatioIndex = this.notifications.findIndex(n => n.Id == result.Data.Value.Id);
          this.notifications.splice(notificatioIndex, 1);
        }
      })
  }

  private setUpdateNotificationEvent(): void {
    this.router.events
      .subscribe((event: NavigationStart ) => {
        if (event instanceof NavigationEnd) {
          this.initializeNotifications();
        }
      });
  }
}
