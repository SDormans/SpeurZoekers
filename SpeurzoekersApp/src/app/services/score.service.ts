import { Injectable } from '@angular/core';
import { UserPoints } from '../models/UserPoints';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
const endpoint = environment.endpoint;

@Injectable({
  providedIn: 'root'
})
export class ScoreService {
  public userPoints: UserPoints;

  constructor(private http: HttpClient) { }
  
  public async refreshUserPoints() {
    const u = await this.http.get<UserPoints>(endpoint + "User/userpoints", {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).toPromise();
    return this.userPoints = u; 
  } 
}
