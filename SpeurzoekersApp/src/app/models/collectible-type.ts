export class collectibleTypeId {
  id: string;
}

export class CollectibleTypeMisc {
  id: string;
  pic: string;
  collectedCounter: number;
  questionAnswered: boolean;
  userCollectibleIdToAnswerQuestion: string;
  constructor(id: string) {
    this.id = id;
  }
}

export class CollectibleTypeTextInfo {
  name: string;
  collectibleTypeTexts: CollectibleTypeText[];
  questionText: string;
  collectibleTypeChoices: CollectibleTypeChoice[];
  points: number;
  questionPoints: number;
}

export class CollectibleTypeNameAndPoints {
  name: string;
  points: number;
}

export class CollectibleTypeText {
  textId: number;
  text: string;
}
export class CollectibleTypeChoice {
  text: string;
  isAnswer: boolean;
}
