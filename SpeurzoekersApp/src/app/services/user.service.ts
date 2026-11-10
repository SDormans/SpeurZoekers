import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { PasswordResetDto } from '../models/passwordResetDto';
const endpoint = environment.endpoint;
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};
@Injectable({
  providedIn: 'root',
})
export class UserService {
  public dist: number = -1;

  constructor(
    private http: HttpClient,
  ) { }

  getDisplayName() {
    return this.http
      .get(endpoint + 'User/displayname', {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        responseType: 'text',
      })
      .toPromise();
  }

  confirmEmail(userName: string, token: string) {
    var encodedToken = encodeURIComponent(token);
    return this.http.get(
      endpoint + 'User/ConfirmEmail?email=' + userName + '&token=' + encodedToken,
      httpOptions
    );
  }

  resetPassword(passwordResetDto: PasswordResetDto) {
    return this.http.post(
      endpoint + 'User/PasswordReset',
      passwordResetDto,
      httpOptions
    );
  }

  requestPasswordReset(userName: string) {
    return this.http.get(
      endpoint + 'User/RequestPasswordReset?username=' + userName,
      httpOptions
    );
  }

  getDistance() {
    return new Promise<number>((resolve, reject) => {
      if(this.dist === -1) {
        this.http.get(endpoint + "user/totaldistance",
          { headers: new HttpHeaders({'Content-Type': 'application/json'}),
            responseType: 'text'
          })
        .toPromise()
        .then(res => {
          this.dist = +res;
          resolve(+res);
        })
        .catch(res => {
          console.log(res);
          reject()
        });
      } else {
        resolve(this.dist)
      }
    });
  }

  setDistance() {
    return this.http.post<any>(`${endpoint}User/settotaldistance?dist=${Math.floor(this.dist)}`,
      { headers: new HttpHeaders({'Content-Type': 'text'}) }).toPromise();
  }

}
