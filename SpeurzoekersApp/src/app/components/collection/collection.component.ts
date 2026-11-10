import { Component, OnInit } from "@angular/core";
import { CollectibleTypeMisc } from '../../models/collectible-type';
import { PictureService } from "src/app/services/picture.service";
import { UserCollectible } from 'src/app/models/user-collectible';
import { CollectibleTypeService } from 'src/app/services/collectible-type.service';
import { UserCollectibleService } from 'src/app/services/user-collectible.service';
import { ActivatedRoute } from '@angular/router';

const numberOfColumns = 4;

@Component({
  selector: "app-collection",
  templateUrl: "./collection.component.html",
  styleUrls: ["./collection.component.scss"],
  providers: [UserCollectibleService]
})
export class CollectionComponent implements OnInit {
  public allCollectibles: Array<CollectibleTypeMisc> = new Array<CollectibleTypeMisc>();
  public collectibleColumns: Array<Array<CollectibleTypeMisc>> = new Array<Array<CollectibleTypeMisc>>();
  public biot: string;

  constructor(
    private pictureService: PictureService,
    private collectibleTypeService: CollectibleTypeService,
    private userCollectibleService: UserCollectibleService,
    private route: ActivatedRoute
  ) {
    for (let i = 0; i < numberOfColumns; i++) {
      this.collectibleColumns.push(new Array<CollectibleTypeMisc>());
    }
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      var biotope: number = params['biotope'];
      if(biotope == 1) { this.biot = "stad"; }
      if(biotope == 2) { this.biot = "park"; }
      if(biotope == 4) { this.biot = "bos"; }
      if(biotope == 8) { this.biot = "weide"; }
      if(biotope == 16) { this.biot = "water"; }
      if(biotope == 32) { this.biot = "kust"; }

      this.collectibleTypeService.getCollectibleTypeIdsAtBiotope(biotope).then(result => {
        for(let i=0; i<result.length; i++) {
          this.allCollectibles.push(new CollectibleTypeMisc(result[i].id));
        }
        this.userCollectibleService.getUserCollectibles().then(res => {
          var myCollection: UserCollectible[] = res;
          for (let i = 0; i < this.allCollectibles.length; i++) {
            let x = i;
            this.pictureService
              .getPicture(this.allCollectibles[i].id)
              .then(result => this.allCollectibles[x].pic = result);
            this.allCollectibles[i].collectedCounter = 0;
            this.allCollectibles[i].questionAnswered = true;
            for (let jjj = 0; jjj < myCollection.length; jjj++) {
              if (this.allCollectibles[i].id == myCollection[jjj].collectibleTypeId && myCollection[jjj].biotope == biotope) {
                this.allCollectibles[i].collectedCounter++;
                if(myCollection[jjj].answerPoints === null) {
                  this.allCollectibles[i].questionAnswered = false;
                  this.allCollectibles[i].userCollectibleIdToAnswerQuestion = myCollection[jjj].id;
                }
              }
            }
            if(this.allCollectibles[i].userCollectibleIdToAnswerQuestion == undefined) {
              for (let jjj = 0; jjj < myCollection.length; jjj++) {
                if(this.allCollectibles[i].id == myCollection[jjj].collectibleTypeId) {
                  this.allCollectibles[i].userCollectibleIdToAnswerQuestion = myCollection[jjj].id;
                  break;
                }
              }
            }
          }

          for (let index = 0; index < this.allCollectibles.length; index++) {
            this.collectibleColumns[index % numberOfColumns].push(this.allCollectibles[index]);
          }
          if(this.collectibleColumns[1].length > this.collectibleColumns[2].length) {
            this.collectibleColumns[2].push(this.collectibleColumns[1][this.collectibleColumns[1].length-1]);
            this.collectibleColumns[1].pop();
          }
        });
      });
    });
  }
}
