import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { MatMenuTrigger } from '@angular/material';


@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {


  constructor(private authenticationService: AuthenticationService,
    private toastr: ToastrService,
    private router: Router,
  ) {
    toastr.toastrConfig.timeOut = 15000;
  }

  ngOnInit() {
  }

  @ViewChild(MatMenuTrigger, { static: false }) trigger: MatMenuTrigger;
  closeMenu() {
    this.trigger.closeMenu();
  }

  logout() {
    this.authenticationService.logout()
    this.router.navigate(['/login']);
  }

}
