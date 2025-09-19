import { Routes } from '@angular/router';
import { DashboardComponent } from './layout/dashboard/dashboard.component';
import { ProductsListComponent } from './features/products/pages/products-list/products-list.component';
import { TransactionsListComponent } from './features/products/pages/transactions-list/transactions-list.component';
import { TransactionsHistoryComponent } from './features/products/pages/transactions-history/transactions-history.component';

export const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      { path: 'products', component: ProductsListComponent },
      { path: 'transactions', component: TransactionsListComponent },
      { path: 'transactions/history', component: TransactionsHistoryComponent },
      { path: '', redirectTo: 'products', pathMatch: 'full' }
    ]
  }
];
