import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {GameVersionsSelectorComponent} from './game-versions-selector/game-versions-selector.component';


@NgModule({
  declarations: [GameVersionsSelectorComponent],
  imports: [
    CommonModule
  ],
  exports: [
    GameVersionsSelectorComponent
  ]
})
export class ComponentsModule { }
