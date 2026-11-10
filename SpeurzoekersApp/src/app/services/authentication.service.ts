import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";

import { User } from "./../models/user";
import { environment } from "./../../environments/environment";
import { UserService } from './user.service';
import { Router } from '@angular/router';

const endpoint = environment.endpoint;
const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json",
  }),
};

@Injectable({
   providedIn: "root"
})

export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(
    private http: HttpClient,
    private userService: UserService,
    private router: Router
  ) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem("private_currentUser"))
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string) {
    return this.http
      .post<any>(
        endpoint + "User/authenticate",
        { username, password },
        httpOptions
      )
      .pipe(
        map((user) => {
          // login successful if there's a jwt token in the response
          if (user && user.token) {
            // store jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem("private_currentUser", JSON.stringify(user));
            this.currentUserSubject.next(user);
          }

          return user;
        })
      );
  }

  public async checkToken(): Promise<boolean> {
    try {
      let result = await this.http.get<any>(endpoint + "User/check", httpOptions).toPromise();
      return result.result;
    } catch {
      return false;
    }
  }

  logout() {
    // remove user from local storage to log user out
    // let index = 0;
    // while (index < localStorage.length) {
    //   let key = localStorage.key(index);
    //   if (key.startsWith("private_")) {
    //     localStorage.removeItem(key);
    //   } else {
    //     index++;
    //   }
    // }
    this.userService.setDistance().then(() => {
    })
    .finally(() => {
      localStorage.clear();
      sessionStorage.clear();
      this.currentUserSubject.next(null);
      this.router.navigate(['/login']);

    });
  }
}
