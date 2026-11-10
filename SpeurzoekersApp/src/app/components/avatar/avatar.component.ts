import { Component, OnInit } from "@angular/core";
import { AvatarService } from "src/app/services/avatar.service";
import { Avatar } from "src/app/models/avatar";
import { Router } from '@angular/router';

const numberOfColumns = 3;
const avatarKey = "private_avatarImage";

@Component({
  selector: "app-avatar",
  templateUrl: "./avatar.component.html",
  styleUrls: ["./avatar.component.scss"],
})
export class AvatarComponent implements OnInit {
  avatar: Avatar = new Avatar();

  public allAvatars: Array<Avatar> = new Array<Avatar>();
  public avatarColumns: Array<Array<Avatar>> = new Array<Array<Avatar>>();
 
  constructor(private avatarService: AvatarService, private router: Router) {
    for (let i = 0; i < numberOfColumns; i++) {
      this.avatarColumns.push(new Array<Avatar>());
    }
  }

  ngOnInit() {
    this.avatarService.getAvatars().then((avatars) => {
      this.allAvatars = avatars;
      for (let i = 0; i < this.allAvatars.length; i++) {
        let x = i;
        this.avatarService
          .getPicture(this.allAvatars[i].index)
          .then((result) => (this.allAvatars[x].pic = result));
      }

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

  public selectAvatar(index: number) {
    this.allAvatars.forEach((a) => {
      a.selected = a.index === index;
    });
  }

}