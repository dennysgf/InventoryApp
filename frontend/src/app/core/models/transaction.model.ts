export interface Transaction {
  id: number;
  date: string;
  type: string;
  productId: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  detail: string;
}
