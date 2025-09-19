import { Routes } from '@angular/router';
import { DashboardComponent } from './layout/dashboard/dashboard.component';

export const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: 'products',
        loadChildren: () =>
          import('./products/products.module').then(m => m.ProductsModule),
      },
      {
        path: 'transactions',
        loadChildren: () =>
          import('./transactions/transactions.module').then(m => m.TransactionsModule),
      },
      { path: '', redirectTo: 'products', pathMatch: 'full' }
    ]
  }
];
