import { Injectable } from "@angular/core";
import { Geolocation } from "src/app/models/geolocation";

const positionOptions: PositionOptions = {
  enableHighAccuracy: true,
  maximumAge: Infinity,
};

@Injectable({
  providedIn: "root"
})

export class GeoLocationService {
  constructor() {}
  private geolocationId;
  public getGeolocation(): Promise<Geolocation> {
    return new Promise<Geolocation>((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(
        (pos: Position) => {
          resolve(this.mapPositionToGeolocation(pos));
        },
        (posErr: PositionError) => {
          reject(posErr);
        },
        positionOptions
      );
    });
  }

  subscribe(callBack: Function) {
    this.geolocationId = navigator.geolocation.watchPosition(
      (pos: Position) => {
        callBack(this.mapPositionToGeolocation(pos));
      },
      (error) => {
        alert("LET OP! Zet je locatie aan om het spel te spelen...");
      },
      positionOptions
    );
  }

  unsubscribe() {
    navigator.geolocation.clearWatch(this.geolocationId);
  }

  private mapPositionToGeolocation(pos: Position): Geolocation {
    const coords = new Geolocation();
    coords.longitude = pos.coords.longitude;
    coords.latitude = pos.coords.latitude;
    coords.accuracy = pos.coords.accuracy;
    coords.heading = pos.coords.heading;
    coords.speed = pos.coords.speed;
    return coords;
  }
}
