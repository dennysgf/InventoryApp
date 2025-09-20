import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transaction } from '../models/transaction.model';
import { environment } from '../../../../environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private apiUrl = environment.apiUrlTransactions;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Transaction[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      map(transactions =>
        transactions.map(t => ({
          id: t.id ?? t.Id,
          date: t.date ?? t.Date,
          type: t.type ?? t.Type,
          productId: t.productId ?? t.ProductId,
          quantity: t.quantity ?? t.Quantity,
          unitPrice: t.unitPrice ?? t.UnitPrice,
          totalPrice: t.totalPrice ?? t.TotalPrice,
          detail: t.detail ?? t.Detail
        }))
      )
    );
  }
  getById(id: number): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.apiUrl}/${id}`);
  }

  create(transaction: Transaction): Observable<Transaction> {
    return this.http.post<Transaction>(this.apiUrl, transaction);
  }

  update(id: number, transaction: Transaction): Observable<Transaction> {
    return this.http.put<Transaction>(`${this.apiUrl}/${id}`, transaction);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getHistory(productId: number, from?: string, to?: string, type?: string): Observable<any[]> {
    let params = new HttpParams().set('productId', productId.toString());
    if (from) params = params.set('from', from);
    if (to) params = params.set('to', to);
    if (type) params = params.set('type', type);

    return this.http.get<any[]>(`${this.apiUrl}/history`, { params });
  }
}
