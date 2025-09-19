import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Transaction } from '../../../../core/models/transaction.model';
import { TransactionService } from '../../../../core/services/transaction.service';
import { Product } from '../../../../core/models/product.model';
import { DropdownModule } from 'primeng/dropdown';
import { ProductService } from '../../../../core/services/product.service';

@Component({
  selector: 'app-transactions-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DialogModule,
    ToastModule,
    DropdownModule
  ],
  providers: [MessageService],
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.scss']
})
export class TransactionsListComponent implements OnInit {
  transactions: Transaction[] = [];
  loading = true;
  products: Product[] = [];
  transactionDialog = false;
  transactionForm!: FormGroup;
  isEdit = false;
  selectedTransaction?: Transaction;

  constructor(
    private transactionService: TransactionService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private productService: ProductService,
  ) { }

  ngOnInit(): void {
    this.loadTransactions();
    this.initForm();
    this.loadProducts();
  }
  loadProducts() {
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: (err) => console.error(err)
    });
  }
  initForm() {
    this.transactionForm = this.fb.group({
      type: ['', Validators.required],
      productId: [null, Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      detail: ['']
    });
  }

  loadTransactions(): void {
    this.loading = true;
    this.transactionService.getAll().subscribe({
      next: (data) => {
        this.transactions = data;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar las transacciones'
        });
        this.loading = false;
      }
    });
  }

  openNew() {
    this.transactionForm.reset();
    this.isEdit = false;
    this.transactionDialog = true;
  }

  editTransaction(transaction: Transaction) {
    this.selectedTransaction = transaction;

    this.transactionForm.patchValue({
      id: transaction.id,
      type: transaction.type,
      productId: transaction.productId,
      quantity: transaction.quantity,
      unitPrice: transaction.unitPrice,
      detail: transaction.detail
    });

    this.transactionDialog = true;
  }


  saveTransaction() {
    if (this.transactionForm.invalid) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Validación',
        detail: 'Por favor completa todos los campos requeridos'
      });
      return;
    }

    const transactionData: Transaction = this.transactionForm.value;

    if (this.isEdit && this.selectedTransaction) {
      this.transactionService.update(this.selectedTransaction.id, transactionData).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Transacción actualizada' });
          this.loadTransactions();
          this.transactionDialog = false;
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo actualizar' });
        }
      });
    } else {
      this.transactionService.create(transactionData).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Transacción creada' });
          this.loadTransactions();
          this.transactionDialog = false;
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo crear' });
        }
      });
    }
  }

  deleteTransaction(transaction: Transaction) {
    this.transactionService.delete(transaction.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Transacción eliminada' });
        this.loadTransactions();
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo eliminar' });
      }
    });
  }
}
