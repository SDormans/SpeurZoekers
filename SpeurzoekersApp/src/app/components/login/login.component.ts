import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthenticationService } from './../../services/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { ScoreService } from 'src/app/services/score.service';
import { UserCollectibleService } from 'src/app/services/user-collectible.service';
import { AvatarService } from 'src/app/services/avatar.service';

@Component({
  templateUrl: 'login.component.html',
  styleUrls: ['login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  public loading = false;
  submitted = false;
  returnUrl: string;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private toastr: ToastrService,
    private userCollectibleService: UserCollectibleService,
    private scoreService: ScoreService,
    private avatarService: AvatarService
  ) {
    toastr.toastrConfig.timeOut = 15000;
    // redirect to home if already logged in
    if (this.authenticationService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: [, Validators.required],
      password: ['', Validators.required],
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService
      .login(this.f.username.value, this.f.password.value)
      .pipe(first())
      .subscribe(
        () => {
          this.userCollectibleService.getUserCollectibles();
          this.scoreService.refreshUserPoints();
          this.avatarService.getAvatarPrice();
          this.avatarService.getAvatarIndex().then((result) => {
            this.avatarService
              .getPicture(result)
              .then(() => this.router.navigate([this.returnUrl]));
          });
        },
        (error) => {
          this.toastr.error(
            'Je inloggegevens kloppen niet, of je account is niet bevestigd'
          );
          this.error = error;
          this.loading = false;
        }
      );
  }
}
