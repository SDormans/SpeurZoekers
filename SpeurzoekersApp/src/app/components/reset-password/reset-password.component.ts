import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  resetRequestForm: FormGroup;
  public loading = false;
  submitted = false;
  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    toastr.toastrConfig.timeOut = 15000;
  }

  ngOnInit() {
    this.resetRequestForm = this.formBuilder.group({
      username: [, [Validators.required, Validators.email]],
    });
  }
  get f() {
    return this.resetRequestForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.resetRequestForm.invalid) {
      return;
    }

    this.loading = true;

    this.userService
      .requestPasswordReset(this.resetRequestForm.value.username)
      .toPromise()
      .then((r) => {
        this.toastr.success('Link voor wachtwoordherstel is verzonden naar je e-mailadres. Als je het niet ziet, controleer dan je spamfolder.');
        this.router.navigate(['/login']);
      })

      .catch((e) => {
        this.toastr.error('Er is iets misgegaan, neem contact op met De Speurzoekers');
        console.log(e);
      });
  }
}
