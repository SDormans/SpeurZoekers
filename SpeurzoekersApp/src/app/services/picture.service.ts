import { Injectable } from "@angular/core";

import { environment } from "src/environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
const endpoint = environment.endpoint;

@Injectable({
  providedIn: "root"
})

export class PictureService {
  private promises: Map<string, Promise<string>>;

  constructor(private http: HttpClient) {
    this.promises = new Map<string, Promise<string>>();
  }

  public getPicture(id: string): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      if (this.promises.has(id)) {
        this.promises.get(id).then(image => {
          image = `data:image/png;base64,${image}`;
          resolve(image);
        });
        return;
      }

      var promise = this.http
        .get(
          endpoint +
            "Collectible/collectibletypeimage?collectibleTypeId=" +
            id,
          {
            headers: new HttpHeaders({
              "Content-Type": "application/json"
            }),
            responseType: "text"
          }
        )
        .toPromise();

      this.promises.set(id, promise);

      promise
        .then(result => {
          result = `data:image/png;base64,${result}`;
          resolve(result);
        })
        .catch(result => {
          reject();
        })
        .finally(() => {
          this.promises.delete(id);
        });
    });
  }

  getbigpicture(id: string) {
    return new Promise<string>((resolve, reject) => {
      this.http
        .get(
          endpoint +
            "Collectible/collectibletypeimagebig?collectibleTypeId=" +
            id,
          {
            headers: new HttpHeaders({
              "Content-Type": "application/json"
            }),
            responseType: "text"
          }
        )
        .toPromise()
        .then(result => {
          result = `data:image/png;base64,${result}`;
          resolve(result);
        })
        .catch(result => {
          console.log(result);
          reject();
        });
    });
  }

  public getActivityPicture(id: string, index: number): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      if (this.promises.has(id+index)) {
        this.promises.get(id+index).then(image => {
          image = `data:image/png;base64,${image}`;
          resolve(image);
        });
        return;
      }

      var promise = this.http
        .get(
          endpoint +
            "activities/image?id=" + id + "&index=" + index,
          {
            headers: new HttpHeaders({
              "Content-Type": "application/json"
            }),
            responseType: "text"
          }
        )
        .toPromise();

      this.promises.set(id+index, promise);

      promise
        .then(result => {
          result = `data:image/png;base64,${result}`;
          localStorage.setItem(id+index, result);
          resolve(result);
        })
        .catch(result => {
          reject();
        })
        .finally(() => {
          this.promises.delete(id+index);
        });
    });
  }

}
