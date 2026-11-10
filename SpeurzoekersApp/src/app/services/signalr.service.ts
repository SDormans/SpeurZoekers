import { Injectable } from "@angular/core";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { environment } from "src/environments/environment";
import { formatDate } from "@angular/common";

@Injectable({
  providedIn: "root"
})
export class SignalrService {
  private _hubConnection: HubConnection;
  private CatchRangeCallback: Function;
  private MapAlertCallback: Function;

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public SubscribeForCatchRange(callback: Function) {
    this.CatchRangeCallback = callback;
  }

  public SubscribeForMapAlert(callback: Function) {
    this.MapAlertCallback = callback;
  }  

  // create connection
  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubEndpoint + "CollectibleHub", {
        accessTokenFactory: () =>
          JSON.parse(localStorage.getItem("private_currentUser")).token,
        logger: 5
      })
      .build();
  }

  // register on server events
  private registerOnServerEvents(): void {
    this._hubConnection.on("RefreshMapAlert", () => {
      if (this.MapAlertCallback) {
        this.MapAlertCallback();
      }
    });
    this._hubConnection.on("CatchRangeToClient", (data: number) => {
      if (this.CatchRangeCallback) {
        this.CatchRangeCallback(data);
      }
    });
  }

  // start connection
  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        // time indication + feedback
        console.log(
          formatDate(new Date(), "[h:mm:ss a d-M-yyyy]", "en-US") +
            " Hub connection started"
        );
      })
      .catch(() => {
        console.log("Error while establishing connection, retrying...");
        setTimeout(() => {
          this.startConnection();
        }, 5000);
      });
  }
}
