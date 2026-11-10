import { Component, OnInit } from '@angular/core';
import * as M from 'src/assets/materialize/js/materialize.min.js';
import { Avatar, AvatarTypeText } from 'src/app/models/avatar';
import { AvatarService } from 'src/app/services/avatar.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-avatar-info',
  templateUrl: './avatar-info.component.html',
  styleUrls: ['./avatar-info.component.scss'],
})
export class AvatarInfoComponent implements OnInit {
  title = 'swipe';
  rUrl = '/' + this.route.snapshot.queryParams['returnUrl'];
  options = { fullWidth: true, indicators: true };
  avatar: Avatar = new Avatar();
  avatarTypeText: AvatarTypeText[];
  avatarPic: string;

  constructor(
    private avatarService: AvatarService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      let avatarIndex = Number(params.get('index'));
      this.avatarService.getPicture(avatarIndex).then((result) => {
        this.avatarPic = result;
      });
      this.avatarService.getAvatarTypeInfo(avatarIndex).then((result) => {
        this.avatar = result;
        this.avatarTypeText = this.avatar.avatarTypeTexts;
        if (this.avatar.avatarTypeTexts.length <= 3) {
          document.getElementById('fourth').remove();
        }
        if (this.avatar.avatarTypeTexts.length <= 2) {
          document.getElementById('third').remove();
        }
        if (this.avatar.avatarTypeTexts.length <= 1) {
          document.getElementById('second').remove();
        }
        this.avatarTypeText = this.avatarTypeText.sort((a, b) => a.textId - b.textId);
        var markdown = require('markdown').markdown;
        for (let i = 0; i < document.getElementsByClassName('infoText').length; i++) {
          document.getElementsByClassName('infoText')[i].innerHTML = markdown.toHTML(this.avatarTypeText[i].text);
        }
        var elems = document.querySelectorAll('.carousel');
        // initiate carousel
        M.Carousel.init(elems, this.options);
      });
    });
  }

  public confirmAvatar() {
    let avatar = this.avatar;

    this.avatarService.chooseFirstAvatar(avatar.index).then(() => {
      this.router.navigate(['/instructions']);
    });
  }
}
