import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ProductsListComponent } from './pages/products-list/products-list.component';
import { ProductFormComponent } from './pages/product-form/product-form.component';
import { ProductsRoutingModule } from './products-routing.module';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    ProductsListComponent,
    ProductFormComponent
  ],
  imports: [
   CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ProductsRoutingModule,
    HttpClientModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    InputTextareaModule
  ]
})
export class ProductsModule {}
