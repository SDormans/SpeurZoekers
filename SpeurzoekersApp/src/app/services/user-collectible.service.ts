import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import { UserCollectible } from "../models/user-collectible";
import { ScoreService } from "./score.service";

const endpoint = environment.endpoint;
const userCollectiblesKey = "private_userCollectibles";

@Injectable({
  providedIn: "root",
})
export class UserCollectibleService {
  private userCollectibles: UserCollectible[];

  constructor(
    private http: HttpClient,
    private scoreService: ScoreService
  ) {
    let store = localStorage.getItem(userCollectiblesKey);
    if (store == null || store == "[]") {
      this.userCollectibles = new Array<UserCollectible>();
    } else {
      this.userCollectibles = JSON.parse(store);
    }
  }

  public getUserCollectibles() {
    return new Promise<UserCollectible[]>(resolve => {
      var myCollection = JSON.parse(localStorage.getItem(userCollectiblesKey));
      if (!myCollection) {
        this.http.get<UserCollectible[]>(endpoint + "User/usercollectibles", {
          headers: new HttpHeaders({
            'Content-Type': 'application/json'
          })
        }).toPromise()
          .then(u => {
            localStorage.setItem(userCollectiblesKey, JSON.stringify(u));
            resolve(u)
          });
      } else {
        resolve(myCollection);
      }
    });
  }

  public catchCollectible(
    collectiblePositionId: number
  ): Promise<UserCollectible> {
    return this.http
      .post<UserCollectible>(
        `${environment.endpoint}Collectible/caughtcollectible?collectiblePositionId=${collectiblePositionId}`,
        {
          headers: new HttpHeaders({ "Content-Type": "application/json" }),
        }
      )
      .toPromise()
      .then<UserCollectible>((userCollectible) => {
        this.upsertUserCollectible(userCollectible);
        this.scoreService.refreshUserPoints();
        return userCollectible;
      });
  }

  public answerQuestion(
    userCollectibleId: string,
    answerPoints: number,
    answeredCorrectly: boolean
  ): Promise<any> {
    return this.http
      .post(`${environment.endpoint}Collectible/usercollectibleanswer`, {
        userCollectibleId: userCollectibleId,
        answeredCorrect: answeredCorrectly,
      })
      .toPromise()
      .then(() => {
        let uc = this.userCollectibles.find(
          (u) => u.id == userCollectibleId
        );
        uc.answerPoints = answeredCorrectly ? answerPoints : 0;
        this.scoreService.refreshUserPoints();
        this.upsertUserCollectible(uc);
      }).catch(err => console.log(err));
  }

  public getById(userCollectibleId: string): UserCollectible {
    let userCollectible = this.userCollectibles.find(u => u.id == userCollectibleId);
    if (userCollectible !== undefined) {
      return userCollectible;
    }
    throw Error("UserCollectible does not exist");
  }

  public getCollectibleTypeId(userCollectibleId: string): string {
    let userCollectible = this.userCollectibles.find(u => u.id == userCollectibleId);
    if (userCollectible !== undefined) {
      return userCollectible.collectibleTypeId;
    }
    throw Error("UserCollectible does not exist");
  }

  // update and insert
  private upsertUserCollectible(userCollectible: UserCollectible) {
    let existingUserCollectible = this.userCollectibles.find(u => u.id == userCollectible.id);
    if (existingUserCollectible === undefined) {
      this.userCollectibles.push(userCollectible);
    } else {
      this.userCollectibles[this.userCollectibles.indexOf(existingUserCollectible)] = userCollectible;
    }
    localStorage.setItem(
      userCollectiblesKey,
      JSON.stringify(this.userCollectibles)
    );
  }

  public hasCollectiblePositionId(collectiblePositionId: string): boolean {
    return this.userCollectibles.some(c => c.collectiblePositionId == collectiblePositionId);
  }
}
