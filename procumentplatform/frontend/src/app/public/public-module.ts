import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { TenderListComponent } from './tender-list/tender-list';
import { TenderDetailComponent } from './tender-detail/tender-detail';



const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'tenders', component: TenderListComponent },
  { path: 'tenders/:id', component: TenderDetailComponent }
];

@NgModule({
  declarations: [HomeComponent, TenderListComponent, TenderDetailComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class PublicModule { }
