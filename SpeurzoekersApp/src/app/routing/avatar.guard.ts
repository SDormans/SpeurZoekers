import { Injectable } from '@angular/core';
import {
  CanActivate,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  Router,
} from '@angular/router';
import { AvatarService } from '../services/avatar.service';

@Injectable({ providedIn: 'root' })
export class AvatarGuard implements CanActivate {
  constructor(private avatarService: AvatarService, private router: Router) {}

  async canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> {
    return this.avatarService
      .getAvatarIndex()
      .then((a) => {
        if (a === null) {
          return this.navigateToAvatar();
        } else {
          return true;
        }
      })
      .catch(() => this.navigateToAvatar());
  }

  private navigateToAvatar(): boolean {
    this.router.navigate(['/avatar']);
    return false;
  }
}
