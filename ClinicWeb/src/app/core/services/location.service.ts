import { Injectable, OnInit } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class LocationService implements OnInit {
    public userLatitude: number = null;
    public userLongitude: number = null;

    public ngOnInit(): void {
        this.initializeLocation();
    }

    public initializeLocation(): void {
        window.navigator.geolocation.getCurrentPosition(location => {
            this.userLongitude = location.coords.longitude;
            this.userLatitude = location.coords.latitude;
        });
    }
}

