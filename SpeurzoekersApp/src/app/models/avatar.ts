export class Avatar {
  index: number;
  name: string;
  avatarTypeTexts: AvatarTypeText[];
  points: number;
  isCollected: boolean;
  pic: string;
  selected: boolean;
}

export class AvatarTypeText {
  textId: number;
  text: string;
}
