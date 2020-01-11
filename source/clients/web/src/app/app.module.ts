import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NavbarComponent} from './navbar/navbar.component';
import {HomeModule} from './pages/home/home.module';
import {SearchModule} from './pages/search/search.module';
import {GameVersionsService} from './services/game-versions.service';
import {ComponentsModule} from './components/components.module';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    ComponentsModule,
    HomeModule,
    SearchModule
  ],
  providers: [
    GameVersionsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
