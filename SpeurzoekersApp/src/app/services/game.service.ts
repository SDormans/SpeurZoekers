import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Geolocation } from "../models/geolocation";
import { environment } from "../../environments/environment";
import { CollectiblePosition } from "../models/collectible-position";
import { SignalrService } from "./signalr.service";
import { ConfigurationService } from './configuration.service';

const endpoint = environment.endpoint;
const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json",
  }),
};

const collectiblePositionKey: string = "private_collectiblePositions";

@Injectable({
  providedIn: "root"
})
export class GameService {
  private spottingRange: number = 0.1;
  private referenceLocation: Geolocation;
  private userLocation: Geolocation;
  private hasSentCoords: boolean = false;
  private callback: Function;
  constructor(private http: HttpClient, 
    signalRService: SignalrService,
    configurationService: ConfigurationService) {
    signalRService.SubscribeForMapAlert(this.updateCollectibles.bind(this));
    configurationService.getSpottingDistance().then((obj) => {
      this.spottingRange = obj.spottingRange;
    });
  }

  public subscribeForChanges(callback: Function) {
    this.callback = callback;
  }

  public sendCoords() {
    if (!this.hasSentCoords) {
      this.hasSentCoords = true;
      this.http
        .post<any>(endpoint + "game/location", this.userLocation, httpOptions)
        .toPromise();
    }
  }

  public getCollectiblePositions(): Promise<CollectiblePosition[]> {
    if (sessionStorage.getItem(collectiblePositionKey)) {
      return new Promise<CollectiblePosition[]>((res) =>
        res(JSON.parse(sessionStorage.getItem(collectiblePositionKey)))
      );
    } else {
      return this.http
        .post<CollectiblePosition[]>(
          endpoint + "Game/map",
          this.userLocation,
          httpOptions
        )
        .toPromise()
        .then((c) => {
          sessionStorage.setItem(collectiblePositionKey, JSON.stringify(c));
          return c;
        });
    }
  }

  private async updateCollectibles() {
    sessionStorage.removeItem(collectiblePositionKey);
    await this.getCollectiblePositions();
    this.callback();
  }

  public setUserLocation(userLocation: Geolocation): void {
    this.userLocation = userLocation;
    if (!this.referenceLocation) {
      this.referenceLocation = userLocation
    } else {
      let difLat = Math.abs(this.userLocation.latitude - this.referenceLocation.latitude);
      let difLon = Math.abs(this.userLocation.longitude - this.referenceLocation.longitude);
      let distance = Math.sqrt(Math.pow(difLat, 2) + Math.pow(difLon, 2));
      if (distance > (this.spottingRange / 4)) {
        this.referenceLocation = this.userLocation;
        this.updateCollectibles();
      }
    }
  }

  public getUserLocation(): Geolocation {
    return this.userLocation;
  }
}
