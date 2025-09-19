export interface TransactionHistory {
  transactionId: number;
  date: string;
  type: string;
  productId: number;
  productName: string;
  currentStock: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  detail: string;
}
