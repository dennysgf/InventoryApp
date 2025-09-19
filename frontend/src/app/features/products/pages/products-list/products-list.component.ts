import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../../../core/services/product.service';
import { Product } from '../../../../core/models/product.model';

// PrimeNG
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DialogModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.scss']
})
export class ProductsListComponent implements OnInit {
  products: Product[] = [];
  loading = true;

  productDialog = false;
  productForm!: FormGroup;
  isEdit = false;
  selectedProduct?: Product;

  constructor(
    private productService: ProductService,
    private fb: FormBuilder,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
    this.initForm();
  }

  initForm() {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: ['', Validators.required],
      imageUrl: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]]
    });
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar los productos'
        });
        this.loading = false;
      }
    });
  }

  openNew() {
    this.productForm.reset();
    this.isEdit = false;
    this.productDialog = true;
  }

  editProduct(product: Product) {
    this.isEdit = true;
    this.selectedProduct = product;
    this.productForm.patchValue(product);
    this.productDialog = true;
  }

  saveProduct() {
    if (this.productForm.invalid) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Validación',
        detail: 'Por favor completa todos los campos requeridos'
      });
      return;
    }

    const productData: Product = this.productForm.value;

    if (this.isEdit && this.selectedProduct) {
      this.productService.update(this.selectedProduct.id, productData).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Producto actualizado' });
          this.loadProducts();
          this.productDialog = false;
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo actualizar' });
        }
      });
    } else {
      this.productService.create(productData).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Producto creado' });
          this.loadProducts();
          this.productDialog = false;
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo crear' });
        }
      });
    }
  }

  deleteProduct(product: Product) {
    this.productService.delete(product.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Producto eliminado' });
        this.loadProducts();
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo eliminar' });
      }
    });
  }
}
