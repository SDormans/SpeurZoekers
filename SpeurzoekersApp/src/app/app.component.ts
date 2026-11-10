import { Component, OnInit, HostListener } from '@angular/core';
import { AuthenticationService } from './services/authentication.service';
import { Router } from '@angular/router';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Speurzoekers';
  submitted = false;
  returnUrl: string;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private userService: UserService
  ) {
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.authenticationService.logout();
  }

  public isPublicScreen(): Boolean {
    return this.router.url === "/" 
            || this.router.url.startsWith("/login") 
            || this.router.url.startsWith("/register")
            || this.router.url.startsWith("/reset-password")
            || this.router.url.startsWith("/avatar"); //This is not public, however should not show menu
  }

  @HostListener('window:beforeunload', ['$event'])
  sendData($event) {
    this.userService.setDistance();
  }

}
