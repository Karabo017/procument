import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'public',
    loadChildren: () => import('./public/public-module').then(m => m.PublicModule)
  },
  {
    path: 'buyer',
    loadChildren: () => import('./buyer/buyer-module').then(m => m.BuyerModule)
  },
  {
    path: 'platform-manager',
    loadChildren: () => import('./platform-manager/platform-manager-module').then(m => m.PlatformManagerModule)
  },
  {
    path: '',
    redirectTo: 'public',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'public'
  }
];
