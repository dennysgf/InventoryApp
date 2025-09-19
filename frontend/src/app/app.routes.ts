import { Routes } from '@angular/router';
import { DashboardComponent } from './layout/dashboard/dashboard.component';
import { ProductsListComponent } from './features/products/pages/products-list/products-list.component';

export const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      { path: 'products', component: ProductsListComponent },
      { path: '', redirectTo: 'products', pathMatch: 'full' }
    ]
  }
];
