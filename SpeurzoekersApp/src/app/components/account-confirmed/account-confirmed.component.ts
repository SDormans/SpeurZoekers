import { Component, OnInit } from '@angular/core';
import { ToastrService } from "ngx-toastr";
import { Router, ActivatedRoute } from "@angular/router";
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-account-confirmed',
  templateUrl: './account-confirmed.component.html',
  styleUrls: ['./account-confirmed.component.css']
})
export class AccountConfirmedComponent implements OnInit {

  constructor(
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {
    toastr.toastrConfig.timeOut = 15000;
  }

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      var userName = params.get('email');
      var token = params.get('token');
      if(!userName || !token) {
        this.toastr.error('Er is iets misgegaan, neem contact op met De Speurzoekers');
      } else {
        this.userService
        .confirmEmail(userName, token)
        .toPromise()
        .then( (res) => {
          this.toastr.success("Welkom bij de Speurzoekers app, je account is bevestigd");
        })
        .catch( (err) => {
          this.toastr.error(err.error);
          console.log(err);
        });
      }

      this.router.navigate(["/login"]);
    });
  }
}
