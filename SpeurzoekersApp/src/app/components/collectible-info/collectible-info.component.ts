import { Component, OnInit } from '@angular/core';
import * as M from 'src/assets/materialize/js/materialize.min.js';
import {
  CollectibleTypeTextInfo,
  CollectibleTypeChoice,
  CollectibleTypeText,
} from 'src/app/models/collectible-type';
import { ActivatedRoute } from '@angular/router';

import { PictureService } from 'src/app/services/picture.service';
import { UserCollectibleService } from 'src/app/services/user-collectible.service';
import { UserCollectible } from 'src/app/models/user-collectible';
import { CollectibleTypeService } from 'src/app/services/collectible-type.service';

@Component({
  selector: 'app-collectible-info',
  templateUrl: './collectible-info.component.html',
  styleUrls: ['./collectible-info.component.scss'],
  providers: [UserCollectibleService],
})
export class CollectibleInfoComponent implements OnInit {
  title = 'swipe';
  rUrl = '/' + this.route.snapshot.queryParams['returnUrl'];
  options = { fullWidth: true, indicators: true };
  public hasAnsweredCorrectly: boolean = null;
  hasAnswered: boolean;
  collectibleInfo: CollectibleTypeTextInfo = new CollectibleTypeTextInfo();
  public choices: CollectibleTypeChoice[];
  collectiblePic: string;
  userCollectible: UserCollectible;
  textCollectionOrdered: CollectibleTypeText[];

  constructor(
    private userCollectibleService: UserCollectibleService,
    private collectibleTypeService: CollectibleTypeService,
    private pictureService: PictureService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      let userCollectibleId = params.get('id');
      this.userCollectible = this.userCollectibleService.getById(userCollectibleId);
      this.hasAnswered = this.userCollectible.answerPoints == null ? false : true;
      let collectibleTypeId = this.userCollectible.collectibleTypeId;

      this.pictureService.getPicture(collectibleTypeId).then((result) => {
        this.collectiblePic = result;
      });
      this.collectibleTypeService
        .getCollectibleInfo(collectibleTypeId)
        .then((result) => {
          this.collectibleInfo = result;
          // avoid showing an empty fourth page
          if (this.collectibleInfo.collectibleTypeTexts.length <= 3) {
            document.getElementById('fourth').remove();
          }
          if (this.collectibleInfo.collectibleTypeTexts.length <= 2) {
            document.getElementById('third').remove();
          }
          if (this.collectibleInfo.collectibleTypeTexts.length <= 1) {
            document.getElementById('second').remove();
          }
          // sort pages
          this.textCollectionOrdered = this.collectibleInfo.collectibleTypeTexts.sort((a, b) => a.textId - b.textId);
          // randomize answers
          this.collectibleInfo.collectibleTypeChoices.sort(() => { return 0.5 - Math.random() });
          this.choices = [];
          for (var i = 0; i < this.collectibleInfo.collectibleTypeChoices.length; i++) {
            if (this.collectibleInfo.collectibleTypeChoices[i].text.trim() !== '') {
              this.choices.push(this.collectibleInfo.collectibleTypeChoices[i]);
            }
          }
          var markdown = require('markdown').markdown;
          for (let xx = 0; xx < document.getElementsByClassName('infoText').length; xx++) {
            document.getElementsByClassName('infoText')[xx].innerHTML = markdown.toHTML(this.textCollectionOrdered[xx].text);
          }
          // count the number of childnodes of .carousel
          var elems = document.querySelectorAll('.carousel');
          // initiate carousel
          M.Carousel.init(elems, this.options);
        });
    });
  }

  showCorrectness(answer: CollectibleTypeChoice) {
    this.hasAnsweredCorrectly = answer.isAnswer;
    this.userCollectibleService.answerQuestion(this.userCollectible.id, this.collectibleInfo.questionPoints, answer.isAnswer);
  }
}
