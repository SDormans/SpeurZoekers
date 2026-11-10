import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameRoutingModule } from './game-routing.module';
import { MapComponent } from './map.component';
import { ClickOutsideModule } from 'ng-click-outside';


@NgModule({
  declarations: [
    MapComponent
  ],
  imports: [
    CommonModule,
    GameRoutingModule,
    ClickOutsideModule
  ],
  exports: [
    MapComponent
  ]

})
export class GameModule { }
