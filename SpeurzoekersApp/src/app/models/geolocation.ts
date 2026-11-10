export class Geolocation {
  public longitude: number;
  public latitude: number;
  public accuracy: number;
  public heading: number;
  public speed: number;

  getCoords(): number[] {
    return [this.longitude, this.latitude, this.accuracy, this.heading, this.speed];
  }
}
