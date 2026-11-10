import { Component, OnInit } from '@angular/core';
import { AvatarService } from 'src/app/services/avatar.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  public avatarImage; //string = "assets/GeoData/empty.svg"; // placeholder
  public displayName: string;

  constructor(private avatarservice: AvatarService, private sanitizer: DomSanitizer, private userService: UserService) {
    this.avatarservice.getAvatarIndex().then(result => {
      this.avatarservice.getPicture(result).then(result => {
        this.avatarImage = this.sanitizer.bypassSecurityTrustUrl(`${result.toString()}`)
      });
    });
    this.userService.getDisplayName().then(d => this.displayName = d);
  }

  ngOnInit() {
    this.avatarservice.onAvatarChange.subscribe(() => { 
      this.avatarservice.getAvatarIndex().then(result => {
        this.avatarservice.getPicture(result).then(result => {
          this.avatarImage = this.sanitizer.bypassSecurityTrustUrl(`${result.toString()}`)
        });
      });
    });
  }

}
