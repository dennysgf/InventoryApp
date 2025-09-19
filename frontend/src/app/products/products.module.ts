import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsRoutingModule } from './products-routing.module';
import {ProductsListComponent} from "./pages/products-list/products-list.component";
import {ProductFormComponent} from "./pages/product-form/product-form.component";
import {TableModule} from "primeng/table";

@NgModule({
  declarations: [
    ProductsListComponent,
    ProductFormComponent
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    TableModule
  ]
})
export class ProductsModule {}
