import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { UserCollectibleService } from 'src/app/services/user-collectible.service';
import { ScoreService } from 'src/app/services/score.service';
import { first } from 'rxjs/operators';
import { UserService } from 'src/app/services/user.service';
import { PasswordResetDto } from 'src/app/models/passwordResetDto';

@Component({
  selector: 'app-reset-password-confirmed',
  templateUrl: './reset-password-confirmed.component.html',
  styleUrls: ['./reset-password-confirmed.component.scss'],
})
export class ResetPasswordConfirmedComponent implements OnInit {
  resetForm: FormGroup;
  public loading = false;
  submitted = false;
  returnUrl: string;
  error = '';
  token: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private userService: UserService
  ) {
    toastr.toastrConfig.timeOut = 15000;
  }

  ngOnInit() {
    this.token = this.route.snapshot.paramMap.get('token');
    this.resetForm = this.formBuilder.group({
      token: [this.token],
      username: [, Validators.required],
      passwords: this.formBuilder.group(
        {
          password: [
            '',
            [
              Validators.required,
              Validators.pattern(
                '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$'
              ),
            ],
          ],
          confirmPassword: ['', Validators.required],
        },
        { validator: this.comparePasswords }
      ),
    });

    // get return url from route parameters or default to '/'

    console.log(this.token);
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.resetForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.resetForm.invalid) {
      return;
    }

    this.loading = true;
    const passwordResetDto = new PasswordResetDto();
    passwordResetDto.username = this.resetForm.value.username;
    passwordResetDto.token = this.resetForm.value.token;
    passwordResetDto.newPassword = this.resetForm.value.passwords.password;
    this.userService
      .resetPassword(passwordResetDto)
      .toPromise()
      .then((r) => {
        this.toastr.success('Wachtwoord reset!');
        this.router.navigate(['/login']);
      })

      .catch((e) => {
        this.toastr.error('Er is iets misgegaan, neem contact op met de Speurzoekers');
        console.log(e);
      });
  }
  comparePasswords(fb: FormGroup) {
    const confirmPasswordCrtl = fb.get('confirmPassword');
    // passwordMismatch
    // confirmPasswordCtrl.errors={passwordMismatch:true}
    if (
      confirmPasswordCrtl.errors === null ||
      'passwordMismatch' in confirmPasswordCrtl.errors
    ) {
      if (fb.get('password').value !== confirmPasswordCrtl.value) {
        confirmPasswordCrtl.setErrors({ passwordMismatch: true });
      } else {
        confirmPasswordCrtl.setErrors(null);
      }
    }
  }
}
