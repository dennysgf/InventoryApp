using System;

namespace TransactionService.Models.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public string? Detail { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

