import { Injectable, EventEmitter } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Avatar } from "../models/avatar";
import { Price } from '../models/price';
const endpoint = environment.endpoint;
const avatarIndexKey: string = "private_avatarindex";

@Injectable({
  providedIn: "root",
})
export class AvatarService {
  [x: string]: any;
  public price: Price;

  constructor(private http: HttpClient) {
    //this.getAvatarIndex().then(x => this.avatarIndex = x);
  }

  getAvatarIndex() {
    return new Promise<number>((resolve, reject) => {
      var avatarIndex = localStorage.getItem(avatarIndexKey);
      if (avatarIndex == null) {
        this.http.get(endpoint + "User/avatarindex", {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          }),
          responseType: "text"
        })
          .toPromise()
          .then(result => {
            localStorage.setItem(avatarIndexKey, result);
            resolve(+result)
          })
          .catch(result => {
            console.log(result);
            reject();
          })
      } else {
        resolve(+avatarIndex);
      }
    });
  }

  public getAvatars(): Promise<Avatar[]> {
    return this.http
      .get<Avatar[]>(endpoint + "Avatars", {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        }),
      })
      .toPromise();
  }

  getAvatarTypeInfo(index: number): Promise<Avatar> {
    return this.http.get<Avatar>(
      `${environment.endpoint}Avatars/avatartypeinfo?index=${index}`,
      {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }
    ).toPromise();
  }

  public async getPicture(id: number): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      return this.http
        .get(endpoint + "Avatars/image?index=" + id, {
          headers: new HttpHeaders({
            "Content-Type": "application/json",
          }),
          responseType: "text",
        })
        .toPromise()
        .then((result) => {
          result = `data:image/png;base64,${result}`;
          resolve(result);
        });
    });
  }

  public chooseFirstAvatar(Id: number): Promise<Avatar> {
    localStorage.setItem(avatarIndexKey, Id.toString());
    return this.http
      .post<Avatar>(`${environment.endpoint}Avatars/first?index=${Id}`, {
        headers: new HttpHeaders({ "Content-Type": "application/json" }),
      })
      .toPromise();
  }

  public buyAvatar(Id: number, selectAsCurrent: boolean): Promise<Avatar> {
    localStorage.setItem(avatarIndexKey, Id.toString());
    return this.http
      .post<Avatar>(`${environment.endpoint}Avatars/collect?index=${Id}&setAsCurrent=${selectAsCurrent}`, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        }),
      })
      .toPromise();
  }

  public getAvatarsWithCollected(): Promise<Avatar[]> {
    return this.http
      .get<Avatar[]>(endpoint + "Avatars/myCollection", {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        }),
      })
      .toPromise();
  }

  public async getAvatarPrice() {
    const m = await this.http.get<Price>(endpoint + "Configuration/avatarPrice", {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).toPromise();
    return this.price = m;
  }

  public onAvatarChange = new EventEmitter();


}
