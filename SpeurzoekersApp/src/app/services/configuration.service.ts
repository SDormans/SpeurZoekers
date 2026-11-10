import { Injectable } from "@angular/core";
import { SignalrService } from "./signalr.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "src/environments/environment";

const catchRangeKey: string = "catchRange";
const endpoint = environment.endpoint;

@Injectable({
  providedIn: "root",
})
export class ConfigurationService {
  private callback: Function;

  constructor(
    signalRService: SignalrService,
    private http: HttpClient
  ) {
    localStorage.setItem(catchRangeKey, "75"); //default
    signalRService.SubscribeForCatchRange(this.setCatchRange.bind(this));
    this.getCatchDistance().then((obj) => {
      this.setCatchRange(obj.catchRange);
    });
  }

  public getSpottingDistance(): Promise<any> {
    return this.http
      .get(endpoint + "configuration/spotrange", {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        }),
      })
      .toPromise();
  }

  private getCatchDistance(): Promise<any> {
    return this.http
      .get(endpoint + "configuration/catchrange", {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        }),
      })
      .toPromise();
  }

  public subscribeChanges(callback: Function): void {
    this.callback = callback;
  }

  public getCatchRange(): number {
    return +localStorage.getItem(catchRangeKey);
  }

  private setCatchRange(catchRange: number) {
    localStorage.setItem(catchRangeKey, catchRange.toString());
    if (this.callback) {
      this.callback();
    }
  }
}
