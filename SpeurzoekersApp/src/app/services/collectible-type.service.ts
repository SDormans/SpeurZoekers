import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { CollectibleTypeTextInfo, collectibleTypeId, CollectibleTypeNameAndPoints } from "../models/collectible-type";
import { environment } from "src/environments/environment";
import { PictureService } from './picture.service';

const endpoint = environment.endpoint;

@Injectable({
  providedIn: "root"
})
export class CollectibleTypeService {
  constructor(
    private http: HttpClient,
    private pictureService: PictureService
  ) {}

  getCollectibleInfo(id: string): Promise<CollectibleTypeTextInfo> {
    return this.http.get<CollectibleTypeTextInfo>(
      `${environment.endpoint}Collectible/collectibletypeinfo?collectibleTypeId=${id}`,
        {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        }
      ).toPromise();
  }

  getCollectibleTypeNameAndPoints(id: string): Promise<CollectibleTypeNameAndPoints> {
    return this.http.get<CollectibleTypeNameAndPoints>(
      `${environment.endpoint}Collectible/collectibletypenameandpoints?collectibleTypeId=${id}`,
        {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        }
      ).toPromise();
  }


  public getCollectibleTypeIds() {
    return new Promise<collectibleTypeId[]>((resolve, reject) => {
      this.http
        .get<collectibleTypeId[]>(endpoint + "Collectible/collectibletypes", {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        })
        .toPromise()
        .then(result => {
          resolve(result);
        })
        .catch(result => {
          reject();
        });
    });
  }

  public getCollectibleTypeIdsAtBiotope(biotope: number) {
    return new Promise<collectibleTypeId[]>((resolve, reject) => {
      this.http
        .get<collectibleTypeId[]>(endpoint + "Collectible/collectibletypesatbiotope?biotope=" + biotope, {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        })
        .toPromise()
        .then(result => {
          resolve(result);
        })
        .catch(result => {
          reject();
        });
    });
  }

  public getAllCollectibleImages() {
    this.getCollectibleTypeIds().then(a => {
      var b = Object.values(a);
      b.forEach(el => this.pictureService.getPicture(el.id));
    });
  }


}
