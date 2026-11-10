import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { UserRegistration } from '../models/user-registration';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root',
})

export class RegistrationService {
  constructor(private http: HttpClient) {}
  readonly BaseURI = environment.endpoint;

  register(user: UserRegistration) {
    const ep = `${this.BaseURI}User/register`;
    return this.http.post(ep, user, httpOptions);
  }
}
