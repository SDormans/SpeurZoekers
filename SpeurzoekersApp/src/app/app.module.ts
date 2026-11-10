import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';

import { MatCardModule, MatButtonModule, MatFormFieldModule, MatMenuModule, MatIconModule, MatToolbarModule, MatProgressSpinnerModule, MatInputModule, MatListModule, MatSidenavModule } from '@angular/material';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home/home.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { JwtInterceptor } from './routing/jwt.interceptor';
import { RegistrationComponent } from './components/registration/registration.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { LoadingPageComponent } from './components/loading-page/loading-page.component';
import { LandingpageComponent } from './components/landingpage/landingpage.component';
import { CollectionComponent } from './components/collection/collection.component';
import { CollectibleInfoComponent } from './components/collectible-info/collectible-info.component';
import { MenuComponent } from './components/menu/menu.component';
import { BiotopesComponent } from './components/biotopes/biotopes.component';
import { ClickOutsideModule } from 'ng-click-outside';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { AvatarInfoComponent } from './components/avatar-info/avatar-info.component';
import { ActivitiesComponent } from './components/activities/activities.component';
import { ActivityInfoComponent } from './components/activity-info/activity-info.component';
import { CachingInterceptor } from './routing/caching.interceptor';
import { AccountConfirmedComponent } from './components/account-confirmed/account-confirmed.component';
import { ResetPasswordConfirmedComponent } from './components/reset-password-confirmed/reset-password-confirmed.component';
import { AboutComponent } from './components/about/about.component';
import { AvatarMineComponent } from './components/avatar-mine/avatar-mine.component';
import { AvatarMInfoComponent } from './components/avatar-m-info/avatar-m-info.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegistrationComponent,
    ResetPasswordComponent,
    LoadingPageComponent,
    LandingpageComponent,
    CollectibleInfoComponent,
    CollectionComponent,
    MenuComponent,
    BiotopesComponent,
    HeaderComponent,
    FooterComponent,
    AvatarComponent,
    InstructionsComponent,
    AvatarInfoComponent,
    AvatarMineComponent,
    AvatarMInfoComponent,
    AvatarInfoComponent,
    ActivitiesComponent,
    ActivityInfoComponent,
    AccountConfirmedComponent,
    ResetPasswordConfirmedComponent,
    AboutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatMenuModule,
    ClickOutsideModule,
    MatIconModule,
    MatInputModule,
    MatToolbarModule,
    MatSidenavModule,
    MatProgressSpinnerModule,
    MatListModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: CachingInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
