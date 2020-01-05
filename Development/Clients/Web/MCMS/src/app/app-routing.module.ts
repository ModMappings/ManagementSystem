import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PageSearchComponent} from './page-search/page-search.component';


const routes: Routes = [
  { path: 'search', component: PageSearchComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
