import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomePageComponent } from './home-page/home-page.component';
import { GlobalSearchBoxComponent } from './global-search-box/global-search-box.component';
import {ComponentsModule} from '../../components/components.module';

@NgModule({
  declarations: [HomePageComponent, GlobalSearchBoxComponent],
  imports: [
    CommonModule,
    ComponentsModule
  ]
})
export class HomeModule { }
