import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AuthenticationGuard } from './routing/authentication.guard';
import { AvatarGuard } from './routing/avatar.guard';
import { HomeComponent } from './components/home/home.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { LoadingPageComponent } from './components/loading-page/loading-page.component';
import { LandingpageComponent } from './components/landingpage/landingpage.component';
import { CollectionComponent } from './components/collection/collection.component';
import { CollectibleInfoComponent } from './components/collectible-info/collectible-info.component';
import { BiotopesComponent } from './components/biotopes/biotopes.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { AvatarInfoComponent } from './components/avatar-info/avatar-info.component';
import { ActivitiesComponent } from './components/activities/activities.component';
import { ActivityInfoComponent } from './components/activity-info/activity-info.component';
import { AccountConfirmedComponent } from './components/account-confirmed/account-confirmed.component';
import { ResetPasswordConfirmedComponent } from './components/reset-password-confirmed/reset-password-confirmed.component';
import { AboutComponent } from './components/about/about.component';
import { AvatarMineComponent } from './components/avatar-mine/avatar-mine.component';
import { AvatarMInfoComponent } from './components/avatar-m-info/avatar-m-info.component';

const routes: Routes = [
  { path: '', redirectTo: '/landingpage', pathMatch: 'full' },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegistrationComponent
  },
  {
    path: 'loading-page',
    component: LoadingPageComponent
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent
  },
  {
    path: 'account-confirmed',
    component: AccountConfirmedComponent
  },
  {
    path: 'reset-password-confirmed/:token',
    component: ResetPasswordConfirmedComponent
  },
  {
    path: 'avatar',
    canActivate: [AuthenticationGuard],
    component: AvatarComponent
  },
  {
    path: 'avatar-info/:index',
    canActivate: [AuthenticationGuard],
    component: AvatarInfoComponent
  },
  {
    path: 'mavatar-info/:index',
    canActivate: [AuthenticationGuard,AvatarGuard],
    component: AvatarMInfoComponent
  },
  {
    path: 'mijnavatars',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: AvatarMineComponent
  },
  {
    path: 'map',
    canActivate: [AuthenticationGuard, AvatarGuard],
    loadChildren: () => import('./game/game.module').then(m => m.GameModule)
  },
  {
    path: 'home',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: HomeComponent
  },
  {
    path: 'landingpage',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: LandingpageComponent
  },
  {
    path: 'collectible-info/:id',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: CollectibleInfoComponent
  },
  {
    path: 'collection',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: CollectionComponent
  },
  {
    path: 'biotopes',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: BiotopesComponent
  },
  {
    path: 'instructions',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: InstructionsComponent
  },
  {
    path: 'activities',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: ActivitiesComponent
  },
  {
    path: 'activity-info/:id',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: ActivityInfoComponent
  },
  {
    path: 'about',
    canActivate: [AuthenticationGuard, AvatarGuard],
    component: AboutComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
