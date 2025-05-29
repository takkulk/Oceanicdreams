import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { YachtsListsComponent } from './yachts-lists/yachts-lists.component';

const routes: Routes = [
    {path:"", component:HomeComponent},
  {path:"yachts", component:YachtsListsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
