import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Activity } from '../models/activity';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PictureService } from './picture.service';

@Injectable({
  providedIn: 'root'
})
export class ActivitiesService {

  constructor(
    private http: HttpClient,
    private pictureService: PictureService
  ) {}

  public getActivities(): Promise<Activity[]> {
    return this.http
      .get<Activity[]>(environment.endpoint + "activities/nameandseason", {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        }),
      })
      .toPromise();
  }

  getInfo(id: string): Promise<Activity> {
    return this.http.get<Activity>(
      `${environment.endpoint}Activities/info?id=${id}`,
        {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        }
      ).toPromise();
  }
}
