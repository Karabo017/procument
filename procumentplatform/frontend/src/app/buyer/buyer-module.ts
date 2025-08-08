import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BuyerDashboardComponent } from './dashboard/dashboard';
import { TenderCreateComponent } from './tender-create/tender-create';
import { TenderEvaluateComponent } from './tender-evaluate/tender-evaluate';



const routes: Routes = [
  { path: '', component: BuyerDashboardComponent },
  { path: 'tender-create', component: TenderCreateComponent },
  { path: 'tender-evaluate', component: TenderEvaluateComponent }
];

@NgModule({
  declarations: [BuyerDashboardComponent, TenderCreateComponent, TenderEvaluateComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class BuyerModule { }
