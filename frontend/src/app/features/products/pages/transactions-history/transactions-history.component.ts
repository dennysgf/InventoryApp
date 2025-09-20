import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { TransactionService } from '../../../../core/services/transaction.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { TransactionHistory } from '../../../../core/models/transaction-history.model';
import {Product} from "../../../../core/models/product.model";
import {ProductService} from "../../../../core/services/product.service";

@Component({
  selector: 'app-transactions-history',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    CalendarModule,
    DropdownModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './transactions-history.component.html',
  styleUrls: ['./transactions-history.component.scss']
})
export class TransactionsHistoryComponent implements OnInit {
  history: TransactionHistory[] = [];
  loading = false;
  products: Product[] = [];
  filterForm: FormGroup;

  transactionTypes = [
    { label: 'Compra', value: 'Compra' },
    { label: 'Venta', value: 'Venta' }
  ];

  constructor(
    private fb: FormBuilder,
    private transactionService: TransactionService,
    private messageService: MessageService,
    private productService: ProductService
  ) {
    this.filterForm = this.fb.group({
      productId: [null],
      from: [null],
      to: [null],
      type: ['']
    });
  }
  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: (err) => console.error('Error cargando productos', err)
    });
  }


  searchHistory() {
    const { productId, from, to, type } = this.filterForm.value;

    if (!productId) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Validación',
        detail: 'Debes ingresar un ProductId'
      });
      return;
    }

    this.loading = true;

    this.transactionService.getHistory(
      productId,
      from ? this.formatDate(from) : undefined,
      to ? this.formatDate(to) : undefined,
      type || undefined
    ).subscribe({
      next: (data) => {
        this.history = data;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se encontró historial para los filtros aplicados'
        });
        this.loading = false;
      }
    });
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }
}
