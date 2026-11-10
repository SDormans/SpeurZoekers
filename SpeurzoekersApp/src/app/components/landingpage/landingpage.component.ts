import { Component, OnInit } from "@angular/core";
import { ScoreService } from "src/app/services/score.service";
import { Geolocation } from "src/app/models/geolocation";
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: "app-landingpage",
  templateUrl: "./landingpage.component.html",
  styleUrls: ["./landingpage.component.scss"],
})
export class LandingpageComponent implements OnInit {
  public totalAnswered: number = 0;
  public userLocation: Geolocation = new Geolocation();
  public distance: number = 0;

  constructor(
    private scoreService: ScoreService,
    private userService: UserService
  ) {
    this.getUserPointsTotal().then( p => this.totalAnswered = p);
  }

  ngOnInit() {
    this.userService.getDistance().then(res => {
      this.distance = Math.floor(res);
    });
  }

  public async getUserPointsTotal(): Promise<number> {
    if (this.scoreService.userPoints === undefined) {
      await this.scoreService.refreshUserPoints();
    }
    return ((this.scoreService.userPoints.totalAnswerPoints + this.scoreService.userPoints.totalCatchPoints) - this.scoreService.userPoints.spendPoints);
  }
} 
