import { Component, OnInit } from '@angular/core';
import 'ol/ol.css';
import { Map, View } from 'ol';
import TileLayer from 'ol/layer/Tile';
import OSMSource from 'ol/source/OSM';
import { fromLonLat } from 'ol/proj';
import VectorLayer from 'ol/layer/Vector';
import VectorSource from 'ol/source/Vector';
import Feature from 'ol/Feature';
import { circular } from 'ol/geom/Polygon';
import Point from 'ol/geom/Point';
import { Style, Icon, Fill, Stroke } from 'ol/style';
import { GeoLocationService } from 'src/app/services/geo-location.service';
import { Geolocation } from '../models/geolocation';
import { GameService } from '../services/game.service';
import { AvatarService } from '../services/avatar.service';
import { CollectiblePosition } from '../models/collectible-position';
import { PictureService } from 'src/app/services/picture.service';
import { Router } from '@angular/router';
import { UserCollectibleService } from 'src/app/services/user-collectible.service';
import { CollectibleTypeNameAndPoints } from 'src/app/models/collectible-type';
import { CollectibleTypeService } from 'src/app/services/collectible-type.service';
import { ConfigurationService } from '../services/configuration.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
  providers: [UserCollectibleService],
})
export class MapComponent implements OnInit {
  private source: VectorSource;
  private map: Map;
  private posStyle: Style;
  private layer: VectorLayer = new VectorLayer();
  private avatarImage: string = 'assets/GeoData/empty.svg'; // placeholder
  public collectibleImage: string;
  private userCollectibleId: string;
  public collectibleNameAndPoints: CollectibleTypeNameAndPoints = new CollectibleTypeNameAndPoints();
  public attachOutsideOnClick: boolean = true;
  private firstRender: boolean = true;
  private timer: any;
  private userLocation: Geolocation = new Geolocation();
  private timerDistance: any;

  constructor(
    private router: Router,
    private pictureService: PictureService,
    public geolocationService: GeoLocationService,
    public gameService: GameService,
    public avatarservice: AvatarService,
    private userCollectibleService: UserCollectibleService,
    private collectibleTypeService: CollectibleTypeService,
    private configurationService: ConfigurationService,
    private userService: UserService
  ) {
    this.avatarservice.getAvatarIndex().then((result) => {
      this.avatarservice.getPicture(result).then((result) => {
        this.geolocationService.getGeolocation().then((geo) => {
          this.gameService.setUserLocation(geo);
          this.avatarImage = result.toString();
          this.userService.getDistance().then((res) => {
            this.geolocationService.subscribe(
              this.updateUserLocation.bind(this)
            );
          });
          this.userLocation = geo;
          this.drawUser(true);
        });
      });
    });
    this.subscribeToEvents();
  }

  ngOnInit() {
    this.map = new Map({
      target: 'map',
      layers: [
        new TileLayer({
          source: new OSMSource(),
        }),
      ],
      view: new View({
        enableRotation: false,
        center: fromLonLat([5.2794, 52.213]),
        minZoom: 7.0,
        zoom: 7.0,
      }),
    });

    this.source = new VectorSource();
    this.layer = new VectorLayer({
      source: this.source,
    });
    this.layer.setZIndex(99); // AVATAR ALWAYS ON TOP
    this.map.addLayer(this.layer);

    // the clicking of the features:
    this.map.on('click', (evt) => {
      var num = 0;
      this.map.forEachFeatureAtPixel(evt.pixel, (feature: Feature) => {
        var userLoc = this.gameService.getUserLocation();
        var dist = this.calcDist(
          feature.get('latitude'),
          feature.get('longitude'),
          userLoc.latitude,
          userLoc.longitude
        );

        if (feature.get('id')) {
          // protect the avatar!
          if (
            num == 0 /* first collectible? */ &&
            dist < this.configurationService.getCatchRange()
          ) {
            feature.setGeometry(null);
            this.userCollectibleService
              .catchCollectible(feature.get('id'))
              .then((userCollectible) => {
                this.setPopup(userCollectible.id);
              });
          }
        }
        num++;
      });
    });
    document
      .getElementsByClassName('ol-viewport')[0]
      .setAttribute('touch-action', 'none');
    document
      .getElementsByClassName('ol-viewport')[0]
      .setAttribute(
        'style',
        'position: relative; overflow: hidden; width: 100%; height: 100%; touch-action: none;'
      );
    document
      .getElementsByClassName('ol-overlaycontainer-stopevent')[0]
      .setAttribute(
        'style',
        'position: absolute; z-index: 0; width: 0%; height: 0%;'
      );
    this.timer = setInterval(() => {
      this.RedrawNewCollectiblePositions();
    }, 60000);
  }

  ngOnDestroy() {
    clearInterval(this.timer);
    this.geolocationService.unsubscribe();
    this.userService.setDistance();
  }

  private drawUser(shouldZoom: boolean) {
    let userLocation = this.gameService.getUserLocation();
    if (!userLocation) {
      return;
    }
    let radius = this.configurationService.getCatchRange();
    const accuracy = circular(userLocation.getCoords(), radius, 200);
    this.source.clear(true);
    this.source.addFeatures([
      new Feature(
        accuracy.transform('EPSG:4326', this.map.getView().getProjection())
      ),
      new Feature(new Point(fromLonLat(userLocation.getCoords()))),
    ]);

    if (shouldZoom) {
      this.map.getView().fit(this.source.getExtent(), {
        maxZoom: 18,
        duration: 1500,
      });
    }
    this.posStyle = new Style({
      stroke: new Stroke({
        color: 'rgba(0, 255, 0, 0.5)',
        width: 3,
      }),
      image: new Icon({
        src: this.avatarImage,
        scale: 0.2,
      }),
    });
    this.layer.setStyle(this.posStyle);
  }

  GetFeature(label: string): Promise<Style> {
    return new Promise<Style>((resolve) => {
      var pic: string = 'assets/GeoData/empty.svg';
      this.pictureService.getPicture(label).then((result) => {
        pic = result;
        resolve(
          new Style({
            image: new Icon({
              src: pic,
              scale: 0.25,
            }),
          })
        );
      });
    });
  }

  updateUserLocation(geolocation: Geolocation) {
    this.gameService.setUserLocation(geolocation);
    this.setNewDistance(geolocation);
    this.RedrawNewCollectiblePositions();

    this.drawUser(false);
    this.gameService.sendCoords();
  }

  private calcDist(
    lat1: number,
    lon1: number,
    lat2: number,
    lon2: number
  ): number {
    if (lat1 != lat2 || lon1 != lon2) {
      var deltaLatRad = ((lat2 - lat1) * Math.PI) / 180;
      var deltaLonRad = ((lon2 - lon1) * Math.PI) / 180;
      var dist =
        12742000 *
        Math.asin(
          Math.sqrt(
            Math.pow(Math.sin(deltaLatRad / 2), 2) +
              Math.cos((lat1 * Math.PI) / 180) *
                Math.cos((lat2 * Math.PI) / 180) *
                Math.pow(Math.sin(deltaLonRad / 2), 2)
          )
        );
      if (dist !== NaN) {
        return dist;
      } else {
        return 0;
      }
    } else {
      return 0;
    }
  }

  private setNewDistance(geolocation: Geolocation): void {
    var oldUserLocation = this.userLocation;
    this.userLocation = geolocation;
    var dist = this.calcDist(
      oldUserLocation.latitude,
      oldUserLocation.longitude,
      this.userLocation.latitude,
      this.userLocation.longitude
    );
    if (dist > 0.2) {
      this.userService.dist += dist;
      console.log(this.userService.dist);
    }
  }

  private updateCollectiblePositions(cols: CollectiblePosition[]) {
    // create new features, according to BE-generated data
    var newFeatureArray = new Array<Feature>();
    var stylePromises = new Array<Promise<Style>>();
    var now = new Date();
    var now_utc = new Date(
      now.getUTCFullYear(),
      now.getUTCMonth(),
      now.getUTCDate(),
      now.getUTCHours(),
      now.getUTCMinutes(),
      now.getUTCSeconds()
    );
    for (let inx = 0; inx < cols.length; inx++) {
      var beginDate = new Date(cols[inx].beginDate);
      var endDate = new Date(cols[inx].endDate);
      if (
        cols[inx].beginDate == null ||
        cols[inx].endDate == null ||
        (now_utc > beginDate && now_utc < endDate)
      ) {
        // is the collectible already collected?
        if (
          !this.userCollectibleService.hasCollectiblePositionId(cols[inx].id)
        ) {
          let newF = new Feature({
            geometry: new Point(
              fromLonLat([cols[inx].longitude, cols[inx].latitude])
            ),
            id: cols[inx].id,
            collectibleTypeId: cols[inx].collectibleTypeId,
            longitude: cols[inx].longitude,
            latitude: cols[inx].latitude,
          });
          let promise = this.GetFeature(cols[inx].collectibleTypeId);
          promise.then((style) => {
            newF.setStyle(style);
            newFeatureArray.push(newF);
            return style;
          });
          stylePromises.push(promise);
        }
      }
    }

    Promise.all(stylePromises).then(() => {
      // create layer for new features
      var newVectorLayer = new VectorLayer({
        source: new VectorSource({
          features: newFeatureArray,
        }),
      });
      // replace old feature layer with new one
      this.map.getLayers().setAt(2, newVectorLayer);
      if (this.firstRender) {
        this.updateCollectiblePositions(cols);
        this.firstRender = false;
      }
    });
  }

  private subscribeToEvents(): void {
    this.gameService.subscribeForChanges(() => {
      this.RedrawNewCollectiblePositions();
    });
    this.configurationService.subscribeChanges(() => {
      this.drawUser(true);
    });
  }

  private RedrawNewCollectiblePositions(): void {
    this.gameService.getCollectiblePositions().then((cols) => {
      this.updateCollectiblePositions(cols);
    });
  }

  private setPopup(userCollectibleId: string) {
    this.userCollectibleId = userCollectibleId;
    let collectibleTypeId = this.userCollectibleService.getCollectibleTypeId(
      userCollectibleId
    );
    this.pictureService.getPicture(collectibleTypeId).then((result) => {
      this.collectibleImage = result;
      this.collectibleNameAndPoints.name = '';
      this.collectibleNameAndPoints.points = null;
      document.getElementById('popup').setAttribute('class', 'show');
      document.getElementById('border').setAttribute('class', 'show');
      this.collectibleTypeService
        .getCollectibleTypeNameAndPoints(collectibleTypeId)
        .then((res) => {
          this.collectibleNameAndPoints = res;
        });
    });
    this.attachOutsideOnClick = false;
  }

  public gotoCollectibleInfo() {
    this.router.navigate(['/collectible-info/' + this.userCollectibleId], {
      queryParams: { returnUrl: 'map' },
    });
  }

  hide() {
    document.getElementById('popup').setAttribute('class', 'hide');
    document.getElementById('border').setAttribute('class', 'hide');
    this.attachOutsideOnClick = true;
  }
}
