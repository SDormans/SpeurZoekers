import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { UserRegistration } from '../../models/user-registration';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent implements OnInit {
  public formModel: any;
  public isSubmitted = false;

  constructor(
    private fb: FormBuilder,
    public service: RegistrationService,
    private toastr: ToastrService,
    private router: Router
  ) {
    toastr.toastrConfig.timeOut = 15000;
    this.formModel = this.fb.group({
      name: [Validators.required],
      postcode: [
        '',
        [
          Validators.required,
          Validators.maxLength(4),
          Validators.minLength(4),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
      email: ['', [Validators.required, Validators.email]],
      passwords: this.fb.group(
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
  }

  ngOnInit() {
    this.formModel.reset();
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.formModel.valid) {
      const userRegistration = new UserRegistration();
      userRegistration.name = this.formModel.value.name;
      userRegistration.email = this.formModel.value.email;
      userRegistration.postcode = this.formModel.value.postcode;
      userRegistration.password = this.formModel.value.passwords.password;
      this.service
        .register(userRegistration)
        .toPromise()
        .then((r) => {
          this.toastr.success(
            'Bevestig je account door te klikken op de link in de e-mail. Als je hem niet ziet, controleer dan je spamfolder.'
          );
          this.router.navigate(['/login']);
        })

        .catch((e) => {
          this.toastr.error(
            e.error
          );
          console.log(e);
        });
    }
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
