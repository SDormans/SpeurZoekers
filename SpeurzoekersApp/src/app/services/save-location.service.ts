import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Geolocation } from '../models/geolocation';
import { BehaviorSubject } from 'rxjs';
const endpoint = environment.endpoint;
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class SaveLocationService {

  public longitude: BehaviorSubject<Geolocation>;
  public latitude: BehaviorSubject<Geolocation>;

  constructor(private http: HttpClient) {
  }

  savelocationexe(latitude: number, longitude: number) {
    return this.http.post<any>(endpoint + 'game/location', { latitude, longitude }, httpOptions)
  }
};