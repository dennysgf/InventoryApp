namespace TransactionService.Models.DTOs
{
    public class TransactionHistoryDto
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int CurrentStock { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Detail { get; set; } = "";
    }
}