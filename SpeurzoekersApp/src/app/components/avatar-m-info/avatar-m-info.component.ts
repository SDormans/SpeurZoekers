import { Component, OnInit } from '@angular/core';
import * as M from 'src/assets/materialize/js/materialize.min.js';
import { Avatar, AvatarTypeText } from 'src/app/models/avatar';
import { AvatarService } from 'src/app/services/avatar.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ScoreService } from 'src/app/services/score.service';

@Component({
  selector: 'app-avatar-m-info',
  templateUrl: './avatar-m-info.component.html',
  styleUrls: ['./avatar-m-info.component.scss'],
})
export class AvatarMInfoComponent implements OnInit {
  title = 'swipe';
  rUrl = '/' + this.route.snapshot.queryParams['returnUrl'];
  options = { fullWidth: true, indicators: true };
  avatar: Avatar = new Avatar();
  avatarTypeText: AvatarTypeText[];
  avatarPic: string;
  public attachOutsideOnClick: boolean = true;
  public allAvatars: Array<Avatar> = new Array<Avatar>();
  public pointsSufficient: boolean = false;
  public isUserAvatar: boolean = false;
  public showBadge: boolean = false;
  currentAvatarIndex: number;

  constructor(
    private avatarService: AvatarService,
    private route: ActivatedRoute,
    private router: Router,
    private scoreService: ScoreService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      let avatarIndex = Number(params.get('index'));

      this.avatarService.getAvatarIndex().then((res) => {
        this.currentAvatarIndex = res;
      });

      this.avatarService.getAvatarsWithCollected().then((res) => {
        this.allAvatars = res;
        for (let i = 0; i < this.allAvatars.length; i++) {
          if (this.allAvatars[i].index === avatarIndex && this.allAvatars[i].isCollected === true) {
            this.isUserAvatar = true;
          }
        }
        this.getUserPointsTotal().then(() => {
          if ( (this.pointsSufficient === true || this.isUserAvatar === true) && this.currentAvatarIndex !== avatarIndex) {
            this.showBadge = true;
          }
        });
      });

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

        this.avatarTypeText = this.avatarTypeText.sort(
          (a, b) => a.textId - b.textId
        );
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

  public async getUserPointsTotal(): Promise<void> {
    if (this.scoreService.userPoints === undefined || this.avatarService.price === undefined) {
      await this.scoreService.refreshUserPoints();
      await this.avatarService.getAvatarPrice();
      if (this.scoreService.userPoints.totalAnswerPoints
          + this.scoreService.userPoints.totalCatchPoints
          - this.scoreService.userPoints.spendPoints >= this.avatarService.price.avatarPrice
      ) {
        this.pointsSufficient = true;
      } else {
        this.pointsSufficient = false;
      }
    } else {
      if (this.scoreService.userPoints.totalAnswerPoints
          + this.scoreService.userPoints.totalCatchPoints
          - this.scoreService.userPoints.spendPoints >= this.avatarService.price.avatarPrice
      ) {
        this.pointsSufficient = true;
      } else {
        this.pointsSufficient = false;
      }
    }
  }

  public popUpAvatar() {
    this.setPopup();
  }

  private setPopup() {
    document.getElementById('popup').setAttribute('class', 'show');
    document.getElementById('border').setAttribute('class', 'show');
    setTimeout(() => (this.attachOutsideOnClick = false), 100);
  }

  public notThisAvatar() {
    this.router.navigate(['/mijnavatars']);
  }

  public confirmAvatar() {
    let avatar = this.avatar;
    let pick = true;
    this.avatarService.buyAvatar(avatar.index, pick).then(() => {
      this.avatarService.onAvatarChange.emit();
      this.scoreService.refreshUserPoints().then(() => {
        this.router.navigate(['/landingpage']);
      });
    });
  }

  hide() {
    document.getElementById('popup').setAttribute('class', 'hide');
    document.getElementById('border').setAttribute('class', 'hide');
    this.attachOutsideOnClick = true;
  }
}
