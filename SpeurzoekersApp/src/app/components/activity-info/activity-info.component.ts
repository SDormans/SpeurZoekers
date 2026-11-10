import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PictureService } from 'src/app/services/picture.service';
import { ActivitiesService } from 'src/app/services/activities.service';

@Component({
  selector: 'app-activity-info',
  templateUrl: './activity-info.component.html',
  styleUrls: ['./activity-info.component.scss']
})
export class ActivityInfoComponent implements OnInit {
  public image: string;
  public imagetwo: string;
  public title: string;
  public text: string;

  constructor(
    private route: ActivatedRoute,
    private pictureService: PictureService,
    private activitiesService: ActivitiesService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      let id = params.get('id');
      this.pictureService.getActivityPicture(id, 0).then(res => this.image = res).catch(() => this.image="");
      this.pictureService.getActivityPicture(id, 1).then(res => this.imagetwo = res).catch(() => this.imagetwo="");
      this.activitiesService.getInfo(id).then(res => {
        this.title = res.name;
        document.getElementById("text").innerHTML = markdown.toHTML(res.text);
      });
      var markdown = require("markdown").markdown;
    })
  }
}
