import { Component, OnInit } from '@angular/core';
import { AvatarService } from 'src/app/services/avatar.service';
import { Avatar } from 'src/app/models/avatar';
import { Router } from '@angular/router';
import { ScoreService } from 'src/app/services/score.service';

const numberOfColumns = 3;

@Component({
  selector: 'app-avatar-mine',
  templateUrl: './avatar-mine.component.html',
  styleUrls: ['./avatar-mine.component.scss'],
})
export class AvatarMineComponent implements OnInit {
  avatar: Avatar = new Avatar();
  public allAvatars: Array<Avatar> = new Array<Avatar>();
  public avatarColumns: Array<Array<Avatar>> = new Array<Array<Avatar>>();
  public uPoints: number = 0;
  public needPoints: number = 0;
  public pointsSufficient: boolean = false;
  public currentAvatarIndex: number;
  public currentAvatar: boolean = false;

  constructor(
    private avatarService: AvatarService,
    private router: Router,
    private scoreService: ScoreService
  ) {
    this.getUserPointsTotal().then((p) => (this.uPoints = p));
    this.getPointsNeeded().then((p) => (this.needPoints = p));
    for (let i = 0; i < numberOfColumns; i++) {
      this.avatarColumns.push(new Array<Avatar>());
    }
  }

  ngOnInit() {
    this.avatarService.getAvatarsWithCollected().then((avatars) => {
      this.allAvatars = avatars;

      for (let i = 0; i < this.allAvatars.length; i++) {
        let x = i;
        this.avatarService
          .getPicture(this.allAvatars[i].index)
          .then((result) => (this.allAvatars[x].pic = result));
      }

      this.avatarService.getAvatarIndex().then((res) => {
        this.currentAvatarIndex = res;
      });

      for (let i = 0; i < this.allAvatars.length; i++) {
        this.avatarColumns[i % numberOfColumns].push(this.allAvatars[i]);
      }
      if (this.avatarColumns[1].length > this.avatarColumns[2].length) {
        this.avatarColumns[2].push(
          this.avatarColumns[1][this.avatarColumns[1].length - 1]
        );
        this.avatarColumns[1].pop();
      }
    });
  }

  public async getUserPointsTotal(): Promise<number> {
    try {
      if (this.scoreService.userPoints === undefined || this.avatarService.price === undefined) {
        await this.scoreService.refreshUserPoints();
        try {
          await this.avatarService.getAvatarPrice();
        } catch (error) {
          console.error(error);
        }

        if (this.scoreService.userPoints.totalAnswerPoints
            + this.scoreService.userPoints.totalCatchPoints
            - this.scoreService.userPoints.spendPoints >= this.avatarService.price.avatarPrice
        ) {
          this.pointsSufficient = true;
        } else {
          this.pointsSufficient = false;
        }
        return (this.scoreService.userPoints.totalAnswerPoints
          + this.scoreService.userPoints.totalCatchPoints
          - this.scoreService.userPoints.spendPoints
        );
      } else {
        if (this.scoreService.userPoints.totalAnswerPoints
            + this.scoreService.userPoints.totalCatchPoints
            - this.scoreService.userPoints.spendPoints >= this.avatarService.price.avatarPrice
        ) {
          this.pointsSufficient = true;
        } else {
          this.pointsSufficient = false;
        }
        return (this.scoreService.userPoints.totalAnswerPoints
          + this.scoreService.userPoints.totalCatchPoints
          - this.scoreService.userPoints.spendPoints
        );
      }
    } catch (error) {
      console.error(error);
    }
  }

  public async getPointsNeeded(): Promise<number> {
    if (this.scoreService.userPoints === undefined) {
      await this.scoreService.refreshUserPoints();
    }

    try {
      await this.avatarService.getAvatarPrice();
      return (this.avatarService.price.avatarPrice
              - (this.scoreService.userPoints.totalAnswerPoints
                 + this.scoreService.userPoints.totalCatchPoints
                 - this.scoreService.userPoints.spendPoints)
      );
    } catch (error) {
      console.error(error);
    }
  }

  public selectAvatar(index: number) {
    this.allAvatars.forEach((a) => {
      a.selected = a.index === index;
    });
  }
}
