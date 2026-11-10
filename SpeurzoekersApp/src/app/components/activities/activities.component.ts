import { Component, OnInit } from '@angular/core';
import { ActivitiesService } from 'src/app/services/activities.service';
import { Activity } from 'src/app/models/activity';
import { PictureService } from 'src/app/services/picture.service';


@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
})
export class ActivitiesComponent implements OnInit {
  title = 'my-tabs';
  seasons: string[] = [];
  currentSeason: string;
  filteredData: Activity[];
  activities: Activity[];
  selectedSeason = ''

  constructor(
    private activitiesService: ActivitiesService,
    private pictureService: PictureService
  ) { }

  ngOnInit() {
    const season = this.getCurrentSeason();
    this.selectedSeason = season;
    this.seasons = this.getSeasons(season);
    this.getActivities(season);
  }

  getCurrentSeason() {
    const currentDate = new Date();

    // winter 21 dec t/m 20 maart
    if (currentDate >= new Date(currentDate.getFullYear(), 11, 21) &&
      currentDate <= new Date(currentDate.getFullYear() + 1, 2, 20)) {
      return "Winter";
    }

    // lente 21 maart t/m 20 juni
    if (currentDate >= new Date(currentDate.getFullYear(), 2, 21) &&
      currentDate <= new Date(currentDate.getFullYear(), 5, 20)) {
      return "Spring";
    }

    // zomer 21 juni t/m, 20 september
    if (currentDate >= new Date(currentDate.getFullYear(), 5, 21) &&
      currentDate <= new Date(currentDate.getFullYear(), 8, 20)) {
      return "Summer";
    }

    // herfst 21 september t/m 20 december
    if (currentDate.getDate() >= new Date(currentDate.getFullYear(), 8, 21).getDate() &&
      currentDate.getDate() <= new Date(currentDate.getFullYear(), 11, 20).getDate()) {
      return "Autumn";
    }
  }

  getSeasons(currentSeason: string) {
    const seasonsOfYear = ['Winter', 'Spring', 'Summer', 'Autumn'];
    const seasons = [...seasonsOfYear, ...seasonsOfYear];
    const seasonIdx = seasons.indexOf(currentSeason);
    const sortedSeasons = []

    for (let index = seasonIdx; index < seasonIdx + 4; index++) {
      sortedSeasons.push(seasons[index])
    }

    return sortedSeasons;
  }

  async getActivities(season: string) {
    this.activities = await this.activitiesService.getActivities();
    this.activities.forEach(el => {
      this.pictureService.getActivityPicture(el.id, 0).then( res => { el.image = res} );
    });
    this.filteredData = this.activities.filter(
      (x) => x.season.includes(season)
    );
  }

  getSeasonName(season: string) {
    switch (season) {
      case "Winter":
        return "winter"
      case "Spring":
        return "lente"
      case "Summer":
        return "zomer"
      case "Autumn":
        return "herfst"
      default:
        return "winter"
    }
  }

  filter(event: MouseEvent, selectedSeason: string): void {
    event.preventDefault();
    this.selectedSeason = selectedSeason;
    this.filteredData = this.activities.filter(
      (x) => x.season.includes(selectedSeason)
    );
  }
}