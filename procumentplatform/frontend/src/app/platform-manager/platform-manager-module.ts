import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { PlatformManagerDashboardComponent } from './dashboard/dashboard';
import { SettingsComponent } from './settings/settings';
import { IntegrationSetupComponent } from './integration-setup/integration-setup';



const routes: Routes = [
  { path: '', component: PlatformManagerDashboardComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'integration-setup', component: IntegrationSetupComponent }
];

@NgModule({
  declarations: [PlatformManagerDashboardComponent, SettingsComponent, IntegrationSetupComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class PlatformManagerModule { }
